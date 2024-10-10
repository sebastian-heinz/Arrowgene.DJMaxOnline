using System.Runtime.Intrinsics.Arm;
using Arrowgene.Buffers;
using Crc32 = ConsoleApp1.Crc32;

namespace Arrowgene.DJMaxOnline;

public class DjMaxCrypto
{
    public static void Main(string[] args)
    {
        DjMaxCrypto c = new DjMaxCrypto();
        c.Decrypt();
    }


    public DjMaxCrypto()
    {
    }

    public void Decrypt()
    {
        // client side

        // pre connect
        byte[] seed = new byte[(0xFA * 4) + 12];
        IBuffer bSnd = new StreamBuffer(seed);
        IBuffer bRcv = new StreamBuffer(seed);
        sub_49F554(bSnd);
        sub_49F554(bRcv);

        // recv on connect ack
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
        byte ba2 = (byte)s1; // 0x07
        //65 20 1A 98
        //EE A8 4D 01
        //E8 07 06 00 10 00 00 00 25 00 2C 00 C0 09 35 38 0B 3E B4 A2 B0 11 00 00
        //07

        uint crc32 = Crc32.GetHash(r1);

        ushort s2 = sOnConAckBuf.GetUInt16(5); // 0x4D 0x01
        uint u1 = bR1.GetUInt32(28); // 0xB0 0x11 0x00 0x00

        sub_49F563(bSnd, u1);
        sub_49F563(bRcv, u1);
        
        // TODO implement this
        sub_49F92C();
        sub_49F92C();
        
        // Missing send encryption
        sub_49F4A2(bSnd);
        // TODO encrypt implementation
        
        // recv XX
        sub_49F4A2(bRcv);
        byte[] onXX = new byte[]
        {
            0x10, 0x00, 0x1C, 0xF9, 0x05, 0x00, 0x00,
            0xE2, 0x9F, 0xF5, 0xEE, 0x30, 0x70, 0x3C, 0x52, 0xC2, 0x99, 0x80, 0xEB, 0x70, 0x68, 0x44, 0xFB,
            0x87, 0x57, 0x75, 0x25, 0xF1, 0x07, 0xE7, 0xC4, 0xE3, 0x07, 0x6C, 0xC1, 0x4B, 0x1B, 0xB8, 0xE4,
            0x84, 0xD9, 0x76, 0x9C, 0xDE, 0x24, 0x9C, 0xBC, 0xC0, 0xEE, 0xD8, 0xF0, 0xDD, 0x4E, 0x13, 0x1B,
            0x7D, 0x34, 0xC1, 0x9E, 0x34, 0x57, 0x84, 0xC3, 0x92, 0xD8, 0xEB, 0x03, 0xAA, 0x05, 0x75, 0x3A,
            0x24, 0x94, 0x54, 0xFB, 0x60, 0xDF, 0x75, 0xCE, 0x95, 0xA3, 0xFA, 0x07, 0xAE, 0x64, 0x27, 0x6F,
            0xAA, 0x69, 0xBF, 0x35, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };
        IBuffer onXXB = new StreamBuffer(onXX);
        sub_49F884(bRcv, onXXB);
        
        Console.WriteLine(Util.HexDump(bSnd.GetAllBytes()));
    }

    /// <summary>
    /// Decryption
    /// </summary>
    private void sub_49F884(IBuffer key, IBuffer data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            byte k = 0;
            byte b = d.ReadByte();
            byte r = (byte)(b ^ k);
        }
    }

    private uint sub_49F92C()
    {
        sub_49E2A4();
        v6 = sub_49F629();
        return 0;
    }
    private uint sub_49F629()
    {
        char *v1; // esi
        int v2; // edi

        v1 = this + 32;
        v2 = sub_49E34B(this + 32);
        sub_49E34B(v1);
        return v2;
    }
    
    private uint sub_49E34B()
    {
        int v1; // eax
        int i; // esi
        unsigned int *v3; // edx
        unsigned int *v4; // edx
        unsigned int v5; // edx
        int v6; // eax
        unsigned int v7; // edx
        unsigned int v8; // edx

        v1 = this[624];
        if ( v1 >= 624 )
        {
            if ( v1 == 625 )
                sub_49E25E(5489);
            for ( i = 0; i < 227; ++i )
            {
                v3 = &this[i];
                *v3 = v3[397] ^ dword_55AAA0[v3[1] & 1] ^ ((*v3 ^ (*v3 ^ v3[1]) & 0x7FFFFFFF) >> 1);
            }
            while ( i < 623 )
            {
                v4 = &this[i++];
                *v4 = *(v4 - 227) ^ dword_55AAA0[v4[1] & 1] ^ ((*v4 ^ (*v4 ^ v4[1]) & 0x7FFFFFFF) >> 1);
            }
            v5 = ((this[623] ^ (*this ^ this[623]) & 0x7FFFFFFFu) >> 1) ^ this[396] ^ dword_55AAA0[*(_BYTE *)this & 1];
            this[624] = 0;
            this[623] = v5;
        }
        v6 = this[624];
        v7 = this[v6];
        this[624] = v6 + 1;
        v8 = (((((((v7 >> 11) ^ v7) & 0xFF3A58AD) << 7) ^ (v7 >> 11) ^ v7) & 0xFFFFDF8C) << 15) ^ ((((v7 >> 11) ^ v7) & 0xFF3A58AD) << 7) ^ (v7 >> 11) ^ v7;
        return v8 ^ (v8 >> 18);
    }
    private uint sub_49E2A4(int a1, int a2)
    {
        _DWORD *v2; // ecx
        int v3; // esi
        int result; // eax
        int v5; // edi
        int v6; // edi
        int v7; // [esp+Ch] [ebp-4h]

        sub_49E25E(19650218);
        v3 = a2;
        result = 1;
        v5 = 0;
        if ( a2 < 624 )
            v3 = 624;
        v7 = v3;
        do
        {
            v2[result] = v5 + *(_DWORD *)(a1 + 4 * v5) + (v2[result] ^ (1664525 * (v2[result - 1] ^ (v2[result - 1] >> 30))));
            ++result;
            ++v5;
            if ( result >= 624 )
            {
                *v2 = v2[623];
                result = 1;
            }
            if ( v5 >= a2 )
                v5 = 0;
            --v7;
        }
        while ( v7 );
        v6 = 623;
        do
        {
            v2[result] = (v2[result] ^ (1566083941 * (v2[result - 1] ^ (v2[result - 1] >> 30)))) - result;
            if ( ++result >= 624 )
            {
                *v2 = v2[623];
                result = 1;
            }
            --v6;
        }
        while ( v6 );
        *v2 = 0x80000000;
        return result;
    }
    private uint sub_49E2A4()
    {
        _DWORD *v2; // ecx
        int v3; // esi
        int result; // eax
        int v5; // edi
        int v6; // edi
        int v7; // [esp+Ch] [ebp-4h]

        sub_49E25E(19650218);
        v3 = a2;
        result = 1;
        v5 = 0;
        if ( a2 < 624 )
            v3 = 624;
        v7 = v3;
        do
        {
            v2[result] = v5 + *(_DWORD *)(a1 + 4 * v5) + (v2[result] ^ (1664525 * (v2[result - 1] ^ (v2[result - 1] >> 30))));
            ++result;
            ++v5;
            if ( result >= 624 )
            {
                *v2 = v2[623];
                result = 1;
            }
            if ( v5 >= a2 )
                v5 = 0;
            --v7;
        }
        while ( v7 );
        v6 = 623;
        do
        {
            v2[result] = (v2[result] ^ (1566083941 * (v2[result - 1] ^ (v2[result - 1] >> 30)))) - result;
            if ( ++result >= 624 )
            {
                *v2 = v2[623];
                result = 1;
            }
            --v6;
        }
        while ( v6 );
        *v2 = 0x80000000;
        return result;
    }
    private uint sub_49E25E()
    {
        int v2; // edx
        _DWORD *result; // eax

        *this = a2;
        this[624] = 1;
        do
        {
            v2 = this[624];
            result = &this[v2];
            *result = v2 + 1812433253 * (*(result - 1) ^ (*(result - 1) >> 30));
            ++this[624];
        }
        while ( (int)this[624] < 624 );
        return result;
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
        b.Position = 4;
        b.WriteUInt32(0);
        b.WriteUInt32(0x67);
        b.Position = 0;
        for (int i = 0xFA; i > 0; i--)
        {
            uint r = sub_49F4D9(b, 0);
            b.Position = ((i * 4) - 4) + 12;
            b.WriteUInt32(r);
        }

        uint v4 = 0xFFFFFFFF;
        uint v5 = 0x80000000;
        int pos = 0x18;
        while (v5 != 0)
        {
            b.Position = pos;
            uint result = b.ReadUInt32();
            result = result & v4;
            result = result | v5;
            b.Position = pos;
            b.WriteUInt32(result);
            v5 >>= 1;
            v4 >>= 1;
            pos += 0x1C;
        }
    }

    private uint sub_4C2E70(IBuffer b, int len)
    {
        uint eax = 0xFFFFFFFF;
        uint ecx = 0;
        for (int i = 0; i < len; i++)
        {
            uint edi = b.ReadByte();
            uint ebx = eax;
            ebx = ebx & 0xFF;
            edi = edi ^ ebx;
        }

        return 0;
    }

    private uint sub_49F4D9(IBuffer b, int pos)
    {
        b.Position = pos;
        uint val = b.ReadUInt32();
        for (int i = 0; i < 32; i++)
        {
            uint u1 = val >> 2;
            uint u2 = u1 ^ val;
            uint u3 = u2 >> 3;
            uint x1 = val;
            uint x2 = x1 >> 1;
            uint s3 = u3 ^ x2;
            uint s4 = s3 ^ val;
            uint s5 = s4 >> 1;
            uint s6 = s5 ^ val;
            uint s7 = s6 >> 1;
            uint s9 = s7 ^ x2;
            uint s10 = s9 ^ val;
            uint w = val << 0x1F;
            uint v = s10 & 0x1;
            uint y = w | x2;
            uint z = y ^ v;
            val = z;
        }

        b.Position = pos;
        b.WriteUInt32(val);
        return val;
    }
    
    public uint sub_49F4A2(IBuffer b)
    {
        uint idx1 = b.GetUInt32(4);
        uint idx2 = b.GetUInt32(8);
        uint idx1a = (idx1 + 3) * 4;
        uint idx2a = (idx2 + 3) * 4;

        uint v1 = b.GetUInt32((int)idx1a);
        uint v2 = b.GetUInt32((int)idx2a);
        uint x = v1 ^ v2;
        b.Position = (int)idx1a;
        b.WriteUInt32(x);


        uint result = b.GetUInt32((int)idx1a);
        idx1++;
        if (idx1 == 0xFA)
        {
            idx1 = 0;
        }

        b.Position = 4;
        b.WriteUInt32(idx1);

        idx2++;
        if (idx2 == 0xFA)
        {
            idx2 = 0;
        }

        b.Position = 8;
        b.WriteUInt32(idx2);
        return result;
    }
}