namespace Arrowgene.DJMaxOnline.Server;

public class PacketMeta
{
    public string Name { get; }
    public PacketId Id { get; }
    public int Size { get; }
    public PacketSource Source { get; }

    public PacketMeta(PacketId id, int size, PacketSource source)
    {
        Name = id.ToString();
        Id = id;
        Size = size;
        Source = source;
    }

    public string ToLog()
    {
        return $"{Name}: [Id:{Id}(0x{(uint)Id:X})] [Size:{Size}] [Source:{Source}]";
    }

    public static PacketMeta Get(PacketId packetId)
    {
        return Lookup[packetId];
    }

    public static bool TryGet(PacketId packetId, out PacketMeta packetMeta)
    {
        return Lookup.TryGetValue(packetId, out packetMeta);
    }

    public static readonly PacketMeta OnPingTestInf = new(
        PacketId.OnPingTestInf,
        0x03,
        PacketSource.Server
    );

    public static readonly PacketMeta ConnectReq = new(
        PacketId.ConnectReq,
        0x17,
        PacketSource.Client
    );

    public static readonly PacketMeta OnConnectAck = new(
        PacketId.OnConnectAck,
        0x2F,
        PacketSource.Server
    );

    public static readonly PacketMeta AuthenticateInSndAccReq = new(
        PacketId.AuthenticateInSndAccReq,
        0x43,
        PacketSource.Client
    );

    public static readonly PacketMeta ClearCourseList = new(
        PacketId.ClearCourseList,
        0xF,
        PacketSource.Client
    );

    public static readonly PacketMeta OnAuthenticateInAck = new(
        PacketId.OnAuthenticateInAck,
        0x5C,
        PacketSource.Server
    );

    public static readonly PacketMeta VerifyCodeInf = new(
        PacketId.VerifyCodeInf,
        0xF,
        PacketSource.Server
    );

    public static readonly PacketMeta OnGameStartInf = new(
        PacketId.OnGameStartInf,
        33,
        PacketSource.Server
    );

    public static readonly PacketMeta OnUpdateUserAccountClassInf = new(
        PacketId.OnUpdateUserAccountClassInf,
        19,
        PacketSource.Server
    );

    public static readonly PacketMeta OnChannelInfoInf = new(
        PacketId.OnChannelInfoInf,
        187,
        PacketSource.Server
    );
     public static readonly PacketMeta LogInReq = new(
        PacketId.LogInReq,
        0x35,
        PacketSource.Client
    );
      public static readonly PacketMeta LogOutReq = new(
        PacketId.LogOutReq,
        0xD,
        PacketSource.Client
    );
      public static readonly PacketMeta UserInfoReq = new(
        PacketId.UserInfoReq,
        0xF,
        PacketSource.Client
    );
    
      public static readonly PacketMeta InviteRejectReq = new(
        PacketId.InviteRejectReq,
        0xC,
        PacketSource.Client
    );
    
      public static readonly PacketMeta OnInviteReq = new(
        PacketId.OnInviteReq,
        0xB,
        PacketSource.Client
    );
        public static readonly PacketMeta TeamControlReq = new(
        PacketId.TeamControlReq,
        0xC,
        PacketSource.Client
    );
        public static readonly PacketMeta ReadyReq = new(
        PacketId.ReadyReq,
        0xB,
        PacketSource.Client
    );
    
    private static readonly Dictionary<PacketId, PacketMeta> Lookup = new Dictionary<PacketId, PacketMeta>()
   {
    { PacketId.OnPingTestInf, OnPingTestInf },
    { PacketId.ConnectReq, ConnectReq },
    { PacketId.OnConnectAck, OnConnectAck },
    { PacketId.AuthenticateInSndAccReq, AuthenticateInSndAccReq },
    { PacketId.OnAuthenticateInAck, OnAuthenticateInAck },
    { PacketId.OnUpdateUserAccountClassInf, OnUpdateUserAccountClassInf },
    { PacketId.ClearCourseList, ClearCourseList },
    { PacketId.VerifyCodeInf, VerifyCodeInf },
    { PacketId.OnGameStartInf, OnGameStartInf },
    { PacketId.OnChannelInfoInf, OnChannelInfoInf },
    { PacketId.LogInReq, LogInReq },
    { PacketId.LogOutReq, LogOutReq },
    { PacketId.UserInfoReq, UserInfoReq },
    { PacketId.InviteRejectReq, InviteRejectReq },
    { PacketId.OnInviteReq, OnInviteReq },
    { PacketId.TeamControlReq, TeamControlReq },
    { PacketId.ReadyReq, ReadyReq }
  };
}