namespace Arrowgene.DJMaxOnline.Server.Handler;

public class ConnectReqHandler : IPacketHandler
{
    public void Handle(Client client, Packet packet)
    {
        //OnConnectAck
      // e.Socket.Send(Convert.FromHexString(
      //     "0900cc05004d019adfe5671157b2fee80706001000000025002c00c00935380b3eb4a2b0110000cccccccccccccccc"));
      // break;
      // 
      // IBuffer sOnConAckBuf = new StreamBuffer(onConAckBuf);
      // sOnConAckBuf.Position = 7;
      // uint a = sOnConAckBuf.ReadUInt32();
      // uint b = sOnConAckBuf.ReadUInt32();
      // a = ~a;
      // b = ~b;
      // sOnConAckBuf.Position = 7;
      // sOnConAckBuf.WriteUInt32(a);
      // sOnConAckBuf.WriteUInt32(b);
      // //09 00 CC 05 00 4D 01
      // //9A DF E5 67 -> NOT -> 98 1A 20 65
      // //11 57 B2 FE -> NOT -> 01 4D A8 EE
      // //E8 07 06 00 10 00 00 00 25 00 2C 00 C0 09 35 38 0B 3E B4 A2 B0 11 00 00
      // //CC CC CC CC CC CC CC CC 00 00 00 00 00 00 00
      // byte[] r1 = sOnConAckBuf.GetBytes(7, 32);
      // IBuffer bR1 = new StreamBuffer(r1);
      // int s1 = sub_4AF070(r1, 32); // 0x807
      // byte ba2 = (byte)s1; // 0x07
      // //65 20 1A 98 -- seed mt
      // //EE A8 4D 01 -- seed mt
      // //E8 07 06 00 10 00 00 00 25 00 2C 00 C0 09 35 38 0B 3E B4 A2 B0 11 00 00
      // //07

      // uint crc32 = Crc32.GetHash(r1);

      // ushort s2 = sOnConAckBuf.GetUInt16(5); // 0x4D 0x01
      // uint u1 = bR1.GetUInt32(28); // 0xB0 0x11 0x00 0x00

      // sub_49F563(bSnd, u1);
      // sub_49F563(bRcv, u1);
      // 
      // MersenneTwister mt = new MersenneTwister(r1);

      // encrypt(r1, u1);
        
        
    }

    public PacketId Id => PacketId.ConnectReq;
}