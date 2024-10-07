using Arrowgene.Buffers;

namespace Arrowgene.DJMaxOnline;

public class DjMaxCrypto
{
    public static void Main(string[] args)
    {
        DjMaxCrypto c = new DjMaxCrypto();
        c.Decrypt();
    }

    private uint _idx2;
    private uint _idx1;
    private uint _val;
    private IBuffer _t;

    public DjMaxCrypto()
    {
        this._t = new StreamBuffer();
        this._idx1 = 0;
        this._idx2 = 0;
        this._val = 0;
    }

    public void Decrypt()
    {
        // client side
        
        // pre connect
        byte[] seed = new byte[0xFA * 4];
        IBuffer bSeed1 = new StreamBuffer(seed);
        IBuffer bSeed2= new StreamBuffer(seed);
        sub_49F554(bSeed1);
        sub_49F554(bSeed2);
        
        // on connect ack
        byte[] onConAckBuf = new byte[]
        {
            0x09, 0x00, 0xCC, 0x05, 0x00, 0x4D, 0x01,
            0x9A, 0xDF, 0xE5, 0x67,
            0x11, 0x57, 0xB2, 0xFE,
            0xE8, 0x07, 0x06, 0x00, 0x10, 0x00, 0x00, 0x00, 0x25, 0x00, 0x2C, 0x00, 0xC0, 0x09, 0x35, 0x38, 0x0B, 0x3E,
            0xB4, 0xA2, 0xB0, 0x11, 0x00, 0x00,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };
        IBuffer sOnConAckBuf = new StreamBuffer(onConAckBuf);
        sOnConAckBuf.Position = 7;
        uint a = sOnConAckBuf.ReadUInt32();
        uint b = sOnConAckBuf.ReadUInt32();
        a = ~a;
        b = ~b;
        sOnConAckBuf.Position = 7;
        sOnConAckBuf.WriteUInt32(a);
        sOnConAckBuf.WriteUInt32(b);
        //09 00 CC 05 00 4D 01
        //9A DF E5 67 -> NOT -> 98 1A 20 65
        //11 57 B2 FE -> NOT -> 01 4D A8 EE
        //E8 07 06 00 10 00 00 00 25 00 2C 00 C0 09 35 38 0B 3E B4 A2 B0 11 00 00
        //CC CC CC CC CC CC CC CC 00 00 00 00 00 00 00
        byte[] r1 = sOnConAckBuf.GetBytes(7, 32);
        IBuffer bR1 = new StreamBuffer(r1);
        int s1 = sub_4AF070(r1, 32); // 0x807
        byte b1 = (byte)s1; // 0x07
        //65 20 1A 98
        //EE A8 4D 01
        //E8 07 06 00 10 00 00 00 25 00 2C 00 C0 09 35 38 0B 3E B4 A2 B0 11 00 00
        //07

        ushort s2 = sOnConAckBuf.GetUInt16(5); // 0x4D 0x01
        uint u1 = bR1.GetUInt32(28); // 0xB0 0x11 0x00 0x00


        sub_49F563(bSeed1, u1);
        Console.WriteLine(Util.HexDump(bSeed1.GetAllBytes()));
    }
    
    private void sub_49F563(IBuffer b, uint u1)
    {
        b.Position = 0;
        b.WriteUInt32(u1);
        sub_49F50E(b);
    }

    private int sub_4AF070(byte[] buf, int len)
    {
        int result = 0;
        for (int i = 0; i < len; i++)
        {
            result += buf[i];
        }

        return result;
    }

    private void sub_49F554(IBuffer b)
    {
        b.Position = 0;
        b.WriteUInt32(0);
        sub_49F50E(b);
    }
    
    private void sub_49F50E(IBuffer b)
    {
        IBuffer b1 = new StreamBuffer(new byte[0xFA * 4]);
        b.Position = 4;
        b.WriteUInt32(0);
        b.WriteUInt32(0x67);
        b.Position = 0;
        for (int i = 0xFA; i > 0; i--)
        {
            int r = sub_49F4D9(b, 0);
            b1.Position = (i * 4) - 4;
            b1.WriteInt32(r);
        }

        uint v4 = 0xFFFFFFFF;
        uint v5 = 0x80000000;


        int pos = 0x18;
        while (v5 != 0)
        {
            b.Position = pos;
            int result = b.ReadInt32();
            result = result & (int)v4;
            result = result | (int)v5;
            b.Position = pos;
            b.WriteInt32(result);
            v5 >>= 1;
            v4 >>= 1;
            pos += 0x1C;
        }
    }

    private int sub_49F4D9(IBuffer b, int pos)
    {
        b.Position = pos;
        int val = b.ReadInt32();
        for (int i = 0; i < 32 / 4; i++)
        {
            int u1 = val >> 2;
            int u2 = u1 ^ val;
            int u3 = u2 >> 3;

            int x1 = val;
            int x2 = x1 >> 1;

            int s3 = u3 ^ x2;
            int s4 = s3 ^ val;
            int s5 = s4 >> 1;
            int s6 = s5 ^ val;
            int s7 = s6 >> 1;
            int s8 = s7 >> 1;
            int s9 = s8 ^ x2;
            int s10 = s9 ^ val;

            int w = val << 0x1F;
            int v = s10 & 0x1;

            int y = w | x2;

            int z = y ^ v;

            val = z;
        }

        b.Position = pos;
        b.WriteInt32(val);
        return val;
    }

    private void sub_49F884(byte[] data)
    {
        IBuffer d = new StreamBuffer(data);
        d.SetPositionStart();
        for (int i = 0; i < data.Length; i++)
        {
            byte k = 0;
            byte b = d.ReadByte();
            byte r = (byte)(b ^ k);
        }
    }

    public uint sub_49F4A2()
    {
        uint v1 = _t.GetUInt32((int)_idx1);
        uint v1x = _t.GetUInt32((int)_idx2);
        uint x = v1 ^ v1x;
        _t.Position = (int)_idx1;
        _t.WriteUInt32(x);
        uint result = _t.GetUInt32((int)_idx1);
        _idx1++;
        if (_idx1 == 250)
        {
            _idx1 = 0;
        }

        _idx2++;
        if (_idx2 == 250)
        {
            _idx2 = 0;
        }

        return result;
    }
}