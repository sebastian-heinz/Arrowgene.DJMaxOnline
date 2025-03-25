using System.Buffers.Binary;
using Arrowgene.Buffers;

namespace Arrowgene.DJMaxOnline.Server;

public class DjMaxCrypto
{
    private class DjMaxCryptoState
    {
        private readonly MersenneTwister _mt;
        private readonly byte[] _rngBuffer;
        private readonly byte[] _sumBuffer;
        private readonly byte[] _clearBuffer;

        public int Idx { get; set; }

        public DjMaxCryptoState(byte[] mtSeed, uint sumSeed)
        {
            Idx = 0;
            _sumBuffer = new byte[8];
            _rngBuffer = new byte[8];
            _clearBuffer = new byte[8];
            _mt = new MersenneTwister(mtSeed);
            NextRngBuffer();
            BinaryPrimitives.WriteUInt32LittleEndian(_sumBuffer, sumSeed);
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

        public ReadOnlySpan<byte> GetRngBuffer()
        {
            return new ReadOnlySpan<byte>(_rngBuffer);
        }

        public Span<byte> GetClearBuffer()
        {
            return new Span<byte>(_clearBuffer);
        }
    }

    private DjMaxCryptoState _enc;
    private DjMaxCryptoState _dec;
    private readonly uint _sumSeed;
    private readonly byte[] _mtSeed;

    public DjMaxCrypto(byte[] mtSeed, uint sumSeed)
    {
        _sumSeed = sumSeed;
        _mtSeed = mtSeed;
        _enc = new DjMaxCryptoState(_mtSeed, _sumSeed);
        _dec = new DjMaxCryptoState(_mtSeed, _sumSeed);
    }

    public void Reset()
    {
        _enc = new DjMaxCryptoState(_mtSeed, _sumSeed);
        _dec = new DjMaxCryptoState(_mtSeed, _sumSeed);
    }

    public void Decrypt(ref Span<byte> data)
    {
        ReadOnlySpan<byte> rng = _dec.GetRngBuffer();
        Span<byte> sum = _dec.GetSumBuffer();
        Span<byte> clear = _dec.GetClearBuffer();

        for (int i = 0; i < data.Length; i++)
        {
            if (_dec.Idx > 7)
            {
                if (i < _dec.Idx)
                {
                    Console.WriteLine("un");
                    Span<byte> s = data.Slice(0, i);
                    s.CopyTo(clear.Slice(8 - i, i));
                }
                else
                {
                    data.Slice(i - 8, 8).CopyTo(clear);
                }

                Console.WriteLine("Clear:" + BitConverter.ToString(clear.ToArray()).Replace("-", " "));

                Update(ref sum, ref clear);
                _dec.Idx = 0;
                rng = _dec.NextRngBuffer();
            }

            byte key = (byte)(sum[_dec.Idx] ^ rng[_dec.Idx]);
            data[i] = (byte)(data[i] ^ key);
            _dec.Idx++;
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
                //    Update(ref sum, clearBlock2, clearBlock1);
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

    private void Update(ref Span<byte> sum, ref Span<byte> clear)
    {
        uint edi = 0;
        uint eax = 0;
        uint esi = 0;
        uint edx = BinaryPrimitives.ReadUInt32LittleEndian(sum[4..]);
        uint ecx = BinaryPrimitives.ReadUInt32LittleEndian(sum);

        uint clearBlock2 = BinaryPrimitives.ReadUInt32LittleEndian(clear[4..]);
        uint clearBlock1 = BinaryPrimitives.ReadUInt32LittleEndian(clear);

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

    public static DjMaxCrypto FromOnConnectAckPacket(Packet packet)
    {
        IBuffer buf = packet.GetBuffer();
        uint a = buf.ReadUInt32();
        uint b = buf.ReadUInt32();
        byte[] c = buf.ReadBytes(32 - 4 - 4);
        a = ~a;
        b = ~b;
        IBuffer outBuf = new StreamBuffer();
        outBuf.WriteUInt32(a);
        outBuf.WriteUInt32(b);
        outBuf.WriteBytes(c);
        byte[] mtSeed = outBuf.GetAllBytes();
        uint seed = outBuf.GetUInt32(28);
        return new DjMaxCrypto(mtSeed, seed);
    }

    public Packet ToOnConnectAckPacket()
    {
        IBuffer buf = new StreamBuffer(_mtSeed);
        uint a = buf.ReadUInt32();
        uint b = buf.ReadUInt32();
        byte[] c = buf.ReadBytes(32 - 4 - 4);
        a = ~a;
        b = ~b;
        buf.SetPositionStart();
        buf.WriteBytes(new byte[] { 0xCC, 0x05, 0x00, 0x4d, 0x01 });
        buf.WriteUInt32(a);
        buf.WriteUInt32(b);
        buf.WriteBytes(c);
        Packet packet = new Packet(PacketMeta.OnConnectAck, buf.GetAllBytes());
        return packet;
    }
}