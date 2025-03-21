using System.Buffers.Binary;

namespace Arrowgene.DJMaxOnline.Server;

public class DjMaxCrypto
{
    private class DjMaxCryptoState
    {
        private MersenneTwister _mt;
        private readonly byte[] _rngBuffer;
        private readonly byte[] _sumBuffer;
        private readonly uint _sumSeed;
        private readonly byte[] _mtSeed;

        public DjMaxCryptoState(byte[] mtSeed, uint sumSeed)
        {
            _sumSeed = sumSeed;
            _mtSeed = mtSeed;
            _sumBuffer = new byte[8];
            _rngBuffer = new byte[8];
            Reset();
        }

        public void Reset()
        {
            _mt = new MersenneTwister(_mtSeed);
            UpdateSumBuffer(_sumSeed, 0);
        }

        public ReadOnlySpan<byte> NextRngBuffer()
        {
            Span<byte> span = _rngBuffer;
            BinaryPrimitives.WriteUInt32LittleEndian(span, _mt.NextUInt32());
            BinaryPrimitives.WriteUInt32LittleEndian(span[4..], _mt.NextUInt32());
            return new ReadOnlySpan<byte>(_rngBuffer);
        }

        public void UpdateSumBuffer(uint first, uint second)
        {
            Span<byte> span = _sumBuffer;
            BinaryPrimitives.WriteUInt32LittleEndian(span, first);
            BinaryPrimitives.WriteUInt32LittleEndian(span[4..], second);
        }

        public ReadOnlySpan<byte> GetSumBuffer()
        {
            return new ReadOnlySpan<byte>(_sumBuffer);
        }
    }

    private DjMaxCryptoState _enc;
    private DjMaxCryptoState _dec;

    public DjMaxCrypto(byte[] mtSeed, uint delta)
    {
        _enc = new DjMaxCryptoState(mtSeed, delta);
        _dec = new DjMaxCryptoState(mtSeed, delta);
    }

    public void Decrypt(ref Span<byte> data)
    {
        Crypt(ref data, _dec);
    }

    public void Encrypt(ref Span<byte> data)
    {
        Crypt(ref data, _enc);
    }

    private void Crypt(ref Span<byte> data, DjMaxCryptoState state)
    {
        ReadOnlySpan<byte> rng = state.NextRngBuffer();
        ReadOnlySpan<byte> sum = state.GetSumBuffer();
        int idx = 0;
        
        uint ebp_10 = BinaryPrimitives.ReadUInt32LittleEndian(data[4..]);
        uint ebp_4 = BinaryPrimitives.ReadUInt32LittleEndian(data[8..]);
        
        for (int i = 0; i < data.Length; i++)
        {
            if (idx > 7)
            {
                uint edi = 0;
                uint eax = 0; //  delta sum
                uint esi = 0;
                uint edx = BinaryPrimitives.ReadUInt32LittleEndian(sum);
                uint ecx = BinaryPrimitives.ReadUInt32LittleEndian(sum[4..]);


                uint ebp_8 = ebp_10;
                uint ebp_c = ebp_4;

                for (int j = 0; j < 32; j++)
                {
                    eax = edx;
                    eax = eax >> 5;
                    eax = eax + ebp_10; // ebp+10 //07 00 00 00
                    edi = edx;
                    edi = edi << 4;
                    edi = edi + ebp_4; // ebp-4 // AB 64 AB C8
                    esi = esi - 0x61C88647;
                    eax = eax ^ edi;
                    edi = esi + edx; // [esi + edx]
                    eax = eax ^ edi;
                    ecx = ecx + eax;

                    eax = ecx;
                    eax = eax >> 5;
                    eax = eax + ebp_8; // ebp-8
                    edi = ecx;
                    edi = edi << 4;
                    edi = edi + ebp_c; // ebp-C
                    eax = eax ^ edi;
                    edi = esi + ecx;
                    eax = eax ^ edi;
                    edx = edx + eax;
                }
                
                idx = 0;
                state.UpdateSumBuffer(ecx, edx);
                rng = state.NextRngBuffer();
                
                ebp_10 = BinaryPrimitives.ReadUInt32LittleEndian(data[(i-4)..]);
                ebp_4 = BinaryPrimitives.ReadUInt32LittleEndian(data[(i-8)..]);
            }

            byte key = (byte)(sum[idx] ^ rng[idx]);
            data[i] = (byte)(data[i] ^ key);
            idx++;
        }
    }
}