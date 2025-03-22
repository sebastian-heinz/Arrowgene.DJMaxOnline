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
            BinaryPrimitives.WriteUInt32LittleEndian(_sumBuffer, _sumSeed);
            BinaryPrimitives.WriteUInt32LittleEndian(_sumBuffer[4..], 0);
        }

        public ReadOnlySpan<byte> NextRngBuffer()
        {
            Span<byte> span = _rngBuffer;
            BinaryPrimitives.WriteUInt32LittleEndian(span, _mt.NextUInt32());
            BinaryPrimitives.WriteUInt32LittleEndian(span[4..], _mt.NextUInt32());
            return new ReadOnlySpan<byte>(_rngBuffer);
        }

        public Span<byte> GetSumBuffer()
        {
            return new Span<byte>(_sumBuffer);
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
        ReadOnlySpan<byte> rng = _dec.NextRngBuffer();
        Span<byte> sum = _dec.GetSumBuffer();
        int idx = 0;

        for (int i = 0; i < data.Length; i++)
        {
            if (idx > 7)
            {
                uint clearBlock2 = BinaryPrimitives.ReadUInt32LittleEndian(data[(i - 4)..]);
                uint clearBlock1 = BinaryPrimitives.ReadUInt32LittleEndian(data[(i - 8)..]);
                Crypt(ref sum, clearBlock2, clearBlock1);
                idx = 0;
                rng = _dec.NextRngBuffer();
            }

            byte key = (byte)(sum[idx] ^ rng[idx]);
            data[i] = (byte)(data[i] ^ key);
            idx++;
        }
    }

    public void Encrypt(ref Span<byte> data)
    {
        ReadOnlySpan<byte> rng = _enc.NextRngBuffer();
        Span<byte> sum = _enc.GetSumBuffer();
        int idx = 0;

        uint clearBlock2 = BinaryPrimitives.ReadUInt32LittleEndian(data[4..]);
        uint clearBlock1 = BinaryPrimitives.ReadUInt32LittleEndian(data);

        for (int i = 0; i < data.Length; i++)
        {
            if (idx > 7)
            {
                Crypt(ref sum, clearBlock2, clearBlock1);
                idx = 0;
                rng = _enc.NextRngBuffer();
                if (i + 4 + 4 < data.Length)
                {
                    clearBlock2 = BinaryPrimitives.ReadUInt32LittleEndian(data[(i + 4)..]);
                    clearBlock1 = BinaryPrimitives.ReadUInt32LittleEndian(data[i..]);
                }
            }

            byte key = (byte)(sum[idx] ^ rng[idx]);
            data[i] = (byte)(data[i] ^ key);
            idx++;
        }
    }

    private void Crypt(ref Span<byte> sum, uint clearBlock2, uint clearBlock1)
    {
        uint edi = 0;
        uint eax = 0;
        uint esi = 0;
        uint edx = BinaryPrimitives.ReadUInt32LittleEndian(sum[4..]);
        uint ecx = BinaryPrimitives.ReadUInt32LittleEndian(sum);

        // Console.WriteLine($"clearBlock2:{clearBlock2}");
        // Console.WriteLine($"clearBlock1:{clearBlock1}");
        // Console.WriteLine($"edx:{edx}");
        // Console.WriteLine($"ecx:{ecx}");
        // Console.WriteLine($"---------");

        for (int j = 0; j < 32; j++)
        {
            eax = edx;
            eax = eax >> 5;
            eax = eax + clearBlock2;
            edi = edx;
            edi = edi << 4;
            edi = edi + clearBlock1;
            esi = esi - 0x61C88647;
            eax = eax ^ edi;
            edi = esi + edx;
            eax = eax ^ edi;
            ecx = ecx + eax;

            eax = ecx;
            eax = eax >> 5;
            eax = eax + clearBlock2;
            edi = ecx;
            edi = edi << 4;
            edi = edi + clearBlock1;
            eax = eax ^ edi;
            edi = esi + ecx;
            eax = eax ^ edi;
            edx = edx + eax;
        }

        BinaryPrimitives.WriteUInt32LittleEndian(sum, ecx);
        BinaryPrimitives.WriteUInt32LittleEndian(sum[4..], edx);
    }
}