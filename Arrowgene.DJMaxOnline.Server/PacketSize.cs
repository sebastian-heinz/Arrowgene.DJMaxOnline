namespace Arrowgene.DJMaxOnline.Server;

public static class PacketSize
{
    public const int OnPingTestInf = 0x03;
    public const int ConnectReq = 0x17;
    public const int OnConnectAck = 0x2F;
    public const int AuthenticateInSndAccReq = 0x43;
    public const int OnAuthenticateInAck = 0x5C;
    public const int OnUpdateUserAccountClassInf = 0xEF;
    public const int ClearCourseList = 0xF;
    public const int VerifyCodeInf = 0xF;
    private static readonly Dictionary<PacketId, int> Lookup = new Dictionary<PacketId, int>()
    {
        { PacketId.OnPingTestInf, OnPingTestInf },
        { PacketId.ConnectReq, ConnectReq },
        { PacketId.OnConnectAck, OnConnectAck },
        { PacketId.AuthenticateInSndAccReq, AuthenticateInSndAccReq },
        { PacketId.OnAuthenticateInAck, OnAuthenticateInAck },
        { PacketId.OnUpdateUserAccountClassInf, OnUpdateUserAccountClassInf },
        { PacketId.ClearCourseList, ClearCourseList },
        { PacketId.VerifyCodeInf, VerifyCodeInf },
    };

    public static int Get(PacketId packetId)
    {
        return Lookup[packetId];
    }

    public static bool TryGet(PacketId packetId, out int size)
    {
        return Lookup.TryGetValue(packetId, out size);
    }
}