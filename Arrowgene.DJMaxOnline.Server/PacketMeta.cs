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

    public static readonly PacketMeta KeepAuthenticateInReq = new(
        PacketId.KeepAuthenticateInReq,
        0xF,
        PacketSource.Client
    );

    public static readonly PacketMeta OnKeepAuthenticateInAck = new(
        PacketId.OnKeepAuthenticateInAck,
        0xF,
        PacketSource.Server
    );

    public static readonly PacketMeta OnAuthenticateInAck = new(
        PacketId.OnAuthenticateInAck,
        0x5C,
        PacketSource.Server
    );

    public static readonly PacketMeta VerifyCodeInf = new(
        PacketId.VerifyCodeInf,
        0x23,
        PacketSource.Client
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

    public static readonly PacketMeta StartReq = new(
        PacketId.StartReq,
        0xD,
        PacketSource.Client
    );

    public static readonly PacketMeta PlayStartReq = new(
        PacketId.PlayStartReq,
        0xB,
        PacketSource.Client
    );

    public static readonly PacketMeta PlaySkipReq = new(
        PacketId.PlaySkipReq,
        0xB,
        PacketSource.Client
    );

    public static readonly PacketMeta PlayOverReq = new(
        PacketId.PlayOverReq,
        0xB,
        PacketSource.Client
    );

    public static readonly PacketMeta PlayStateInf = new(
        PacketId.PlayStateInf,
        0x1E,
        PacketSource.Client
    );

    public static readonly PacketMeta StageResultInf = new(
        PacketId.StageResultInf,
        0x3B,
        PacketSource.Client
    );

    public static readonly PacketMeta LeaveRoomReq = new(
        PacketId.LeaveRoomReq,
        0xB,
        PacketSource.Client
    );

    public static readonly PacketMeta ChangeDiscReq = new(
        PacketId.ChangeDiscReq,
        0x11,
        PacketSource.Client
    );

    public static readonly PacketMeta UpdateUserAccountNickReq = new(
        PacketId.UpdateUserAccountNickReq,
        0x1C,
        PacketSource.Client
    );

    public static readonly PacketMeta UpdateUserProfileReq = new(
        PacketId.UpdateUserProfileReq,
        0x12,
        PacketSource.Client
    );

    public static readonly PacketMeta GetItemReq = new(
        PacketId.GetItemReq,
        0xB,
        PacketSource.Client
    );

    public static readonly PacketMeta ItemLevelUpReq = new(
        PacketId.ItemLevelUpReq,
        0xB,
        PacketSource.Client
    );

    public static readonly PacketMeta UseItemReq = new(
        PacketId.UseItemReq,
        0xC,
        PacketSource.Client
    );

    public static readonly PacketMeta UseEffectorInf = new(
        PacketId.UseEffectorInf,
        35,//can go larger.
        PacketSource.Client
    );

    public static readonly PacketMeta MountItemReq = new(
        PacketId.MountItemReq,
        0x43,
        PacketSource.Client
    );

    public static readonly PacketMeta GetPresentItemReq = new(
        PacketId.GetPresentItemReq,
        0xF,
        PacketSource.Client
    );

    public static readonly PacketMeta DeleteItemReq = new(
        PacketId.DeleteItemReq,
        0x13,
        PacketSource.Client
    ); 

    public static readonly PacketMeta MsgRegisterUserReq = new(
        PacketId.MsgRegisterUserReq,
        0x24,
        PacketSource.Client
    );

    public static readonly PacketMeta PurchaseItemReq = new(
        PacketId.PurchaseItemReq,
        0x23,
        PacketSource.Client
    );

    public static readonly PacketMeta ResaleItemReq = new(
        PacketId.ResaleItemReq,
        0x13,
        PacketSource.Client
    );

    public static readonly PacketMeta OnUserIdInfoInf = new(
        PacketId.OnUserIdInfoInf,
        0x8D,
        PacketSource.Server
    );

    public static readonly PacketMeta OnEnvironmentInf = new(
        PacketId.OnEnvironmentInf,
        0x13F,
        PacketSource.Server
    );

    public static readonly PacketMeta OnUserInfoInf = new(
        PacketId.OnUserInfoInf,
        0x8A,
        PacketSource.Server
    );

    public static readonly PacketMeta OnInventoryInfoInf = new(
        PacketId.OnInventoryInfoInf,
        0x2EF,
        PacketSource.Server
    );

    public static readonly PacketMeta OnMessengerInfoInf = new(
        PacketId.OnMessengerInfoInf,
        0x3B9,
        PacketSource.Server
    );

    public static readonly PacketMeta OnLogInAck = new(
        PacketId.OnLogInAck,
        47,
        PacketSource.Server
    );

    public static readonly PacketMeta OnChatInf = new(
        PacketId.OnChatInf,
        0x35,
        PacketSource.Server
    );

    public static readonly PacketMeta OnWaiterInfoUpdateInf = new(
        PacketId.OnWaiterInfoUpdateInf,
        0x4C,
        PacketSource.Server
    );

    public static readonly PacketMeta OnRoomInfoUpdateInf = new(
        PacketId.OnRoomInfoUpdateInf,
        0x33,
        PacketSource.Server
    );

    public static readonly PacketMeta OnInviteRejectAck = new(
        PacketId.OnInviteRejectAck,
        0xC,
        PacketSource.Server
    );

    public static readonly PacketMeta PingTestInf = new(
        PacketId.PingTestInf,
        0x3,
        PacketSource.Client
    );

    public static readonly PacketMeta CreateRoomReq = new(
        PacketId.CreateRoomReq,
        0x3B,
        PacketSource.Client
    );

    public static readonly PacketMeta OnRoomDescInf = new(
        PacketId.OnRoomDescInf,
        0x30,
        PacketSource.Server
    );

    public static readonly PacketMeta OnCreateRoomAck = new(
        PacketId.OnCreateRoomAck,
        0x34,
        PacketSource.Server
    );

    public static readonly PacketMeta OnUpdateJoinerInfoInf  = new(
        PacketId.OnUpdateJoinerInfoInf ,
        135,
        PacketSource.Server
    );

    public static readonly PacketMeta OnPostJoinRoomInf = new(
        PacketId.OnPostJoinRoomInf,
        0xB,
        PacketSource.Server
    );


    public static readonly PacketMeta OnGameInfoInf = new(
        PacketId.OnGameInfoInf,
        9841,
        PacketSource.Server
    );

    public static readonly PacketMeta OnJoinEventInf = new(
        PacketId.OnJoinEventInf,
        13,
        PacketSource.Server
    );

    public static readonly PacketMeta OnUseEffectorInf = new(
        PacketId.OnUseEffectorInf,
        36,
        PacketSource.Server
    );

    public static readonly PacketMeta OnUseMountItemInf = new(
        PacketId.OnUseMountItemInf,
        70,
        PacketSource.Server
    );

    public static readonly PacketMeta OnStartParameterInf = new(
        PacketId.OnStartParameterInf,
        20,
        PacketSource.Server
    );

    public static readonly PacketMeta OnStartInf = new(
        PacketId.OnStartInf,
        14,
        PacketSource.Server
    );

    public static readonly PacketMeta OnPlayStartInf = new(
        PacketId.OnPlayStartInf,
        11,
        PacketSource.Server
    );

    public static readonly PacketMeta OnCheckDataReq = new(
        PacketId.OnCheckDataReq,
        15,
        PacketSource.Server
    );

    public static readonly PacketMeta OnUpdateUserInventoryDefaultItemInf = new(
        PacketId.OnUpdateUserInventoryDefaultItemInf,
        199,
        PacketSource.Server
    );

    public static readonly PacketMeta OnUpdateUserPropertyInf = new(
        PacketId.OnUpdateUserPropertyInf,
        73,
        PacketSource.Server
    );

    public static readonly PacketMeta OnStageResultExInf = new(
        PacketId.OnStageResultExInf,
        51,
        PacketSource.Server
    );

    public static readonly PacketMeta OnPlayOverInf = new(
        PacketId.OnPlayOverInf,
        12,
        PacketSource.Server
    );

    public static readonly PacketMeta OnReadyInf = new(
        PacketId.OnReadyInf,
        18,
        PacketSource.Server
    );

    public static readonly PacketMeta OnLeaveRoomAck = new(
        PacketId.OnLeaveRoomAck,
        12,
        PacketSource.Server
    );

    public static readonly PacketMeta OnRoomInfoEraseInf = new(
        PacketId.OnRoomInfoEraseInf,
        13,
        PacketSource.Server
    );

    public static readonly PacketMeta OnLogOutAck = new(
        PacketId.OnLogOutAck,
        13,
        PacketSource.Server
    );
    public static readonly PacketMeta AuthenticateInSndeKeyAck = new(
        PacketId.AuthenticateInSndeKeyAck,
        20,
        PacketSource.Client
    );
 
    public static readonly PacketMeta OnResaleItemAck = new(
        PacketId.OnResaleItemAck,
        249,
        PacketSource.Server
    );
    public static readonly PacketMeta OnPurchaseItemAck = new(
        PacketId.OnPurchaseItemAck,
        253,
        PacketSource.Server
    );
     public static readonly PacketMeta OnMsgGroupInf = new(
        PacketId.OnMsgGroupInf,
        233,
        PacketSource.Server
    ); 
    public static readonly PacketMeta OnMsgBlkUserInf = new(
        PacketId.OnMsgBlkUserInf,
        243,
        PacketSource.Server
    ); 
    public static readonly PacketMeta OnMsgRegUserInf = new(
        PacketId.OnMsgRegUserInf,
        483,
        PacketSource.Server
    );
    public static readonly PacketMeta OnExpiredShopItemInf = new(
        PacketId.OnExpiredShopItemInf,
        640,
        PacketSource.Server
    );
    public static readonly PacketMeta OnExpiredMountItemInf = new(
        PacketId.OnExpiredMountItemInf,
        144,
        PacketSource.Server
    );
    public static readonly PacketMeta OnDeleteItemAck = new(
        PacketId.OnDeleteItemAck,
        245,
        PacketSource.Server
    );
    public static readonly PacketMeta OnGetPresentItemAck = new(
        PacketId.OnGetPresentItemAck,
        365,
        PacketSource.Server
    );
    public static readonly PacketMeta OnMountItemAck = new(
        PacketId.OnMountItemAck,
        64,
        PacketSource.Server
    );

    public static readonly PacketMeta UseMountItemInf = new(
        PacketId.UseMountItemInf,
        64,
        PacketSource.Server
    );
    public static readonly PacketMeta UseEffectorSetInf = new(
        PacketId.UseEffectorSetInf,
        19,
        PacketSource.Server
    );
    public static readonly PacketMeta OnUpdateUserInventoryPresentItemInf = new(
        PacketId.OnUpdateUserInventoryPresentItemInf,
        127,
        PacketSource.Server
    );
    public static readonly PacketMeta OnUpdateUserInventoryMountItemInf = new(
        PacketId.OnUpdateUserInventoryMountItemInf,
        47,
        PacketSource.Server
    );
    public static readonly PacketMeta OnUpdateUserInventoryShopItemInf = new(
        PacketId.OnUpdateUserInventoryShopItemInf,
        247,
        PacketSource.Server
    );
    public static readonly PacketMeta OnUpdateUserInventoryEventItemInf = new(
        PacketId.OnUpdateUserInventoryEventItemInf,
        135,
        PacketSource.Server
    );
    public static readonly PacketMeta  OnRoomChangeInfoAck = new(
        PacketId.OnRoomChangeInfoAck,
        51,
        PacketSource.Server
    );
      public static readonly PacketMeta  QuickInviteReq = new(
        PacketId.QuickInviteReq,
        0xB,
        PacketSource.Client
    );
    
    public static readonly PacketMeta  RoomChangeInfoReq = new(
        PacketId.RoomChangeInfoReq,
        0x32,
        PacketSource.Client
    );
    public static readonly PacketMeta  OnJoinRoomAck = new(
        PacketId.OnJoinRoomAck,
        15,
        PacketSource.Server
    );    
    public static readonly PacketMeta  JoinRoomReq = new(
        PacketId.JoinRoomReq,
        0x19,
        PacketSource.Client
    ); 

    public static readonly PacketMeta  OnUserIdInfoAck = new(
        PacketId.OnUserIdInfoAck,
        141,
        PacketSource.Server
    );    
    public static readonly PacketMeta  OnAliveReq = new(
        PacketId.OnAliveReq,
        3,
        PacketSource.Server
    );      
    public static readonly PacketMeta  AliveAck = new(
        PacketId.AliveAck,
        0x3,
        PacketSource.Client
    );
    public static readonly PacketMeta  OnPeerCountInf = new(
        PacketId.OnPeerCountInf,
        17,
        PacketSource.Server
    );     
        public static readonly PacketMeta  OnUbsAwardInfoInf = new(
        PacketId.OnUbsAwardInfoInf,
        176,
        PacketSource.Server
    );     
    
        public static readonly PacketMeta OnWaiterInfoEraseInf = new(
        PacketId.OnWaiterInfoEraseInf,
        17,
        PacketSource.Server
    );
    public static readonly PacketMeta UserIdInfoReq = new(
        PacketId.UserIdInfoReq,
        15,
        PacketSource.Client
    );
    public static readonly PacketMeta  OnChangeDiscInf = new(
        PacketId. OnChangeDiscInf,
        17,
        PacketSource.Server
    );
       public static readonly PacketMeta sub_434390 = new(
        PacketId.sub_434390,
        0xD,
        PacketSource.Client
    );

    public static readonly PacketMeta sub_434450 = new(
        PacketId.sub_434450,
        0xD,
        PacketSource.Client
    );

    public static readonly PacketMeta sub_434510 = new(
        PacketId.sub_434510,
        0xD,
        PacketSource.Client
    );

    public static readonly PacketMeta sub_434620 = new(
        PacketId.sub_434620,
        0xB,
        PacketSource.Client
    );

    public static readonly PacketMeta sub_434B40 = new(
        PacketId.sub_434B40,
        0xD,
        PacketSource.Client
    );

    public static readonly PacketMeta sub_435F20 = new(
        PacketId.sub_435F20,
        0xC,
        PacketSource.Client
    );

    public static readonly PacketMeta sub_436120 = new(
        PacketId.sub_436120,
        0xB,
        PacketSource.Client
    );

    public static readonly PacketMeta sub_4362D0 = new(
        PacketId.sub_4362D0,
        0xC,
        PacketSource.Client
    );

    public static readonly PacketMeta sub_437370 = new(
        PacketId.sub_437370,
        0x103,
        PacketSource.Client
    );

    public static readonly PacketMeta sub_437900 = new(
        PacketId.sub_437900,
        0xF,
        PacketSource.Client
    );

    public static readonly PacketMeta ProbeObfuscated = new(
        PacketId.ProbeObfuscated,
        0x90,
        PacketSource.Client
    );

    public static readonly PacketMeta sub_432210 = new(
        PacketId.sub_432210,
        0xB,
        PacketSource.Client
    );

    public static readonly PacketMeta sub_4323D0 = new(
        PacketId.sub_4323D0,
        0x26,
        PacketSource.Client
    );

    private static readonly Dictionary<PacketId, PacketMeta> Lookup = new()
    {
        { PacketId.OnPingTestInf, OnPingTestInf },
        { PacketId.ConnectReq, ConnectReq },
        { PacketId.OnConnectAck, OnConnectAck },
        { PacketId.AuthenticateInSndAccReq, AuthenticateInSndAccReq },
        { PacketId.OnAuthenticateInAck, OnAuthenticateInAck },
        { PacketId.OnUpdateUserAccountClassInf, OnUpdateUserAccountClassInf },
        { PacketId.KeepAuthenticateInReq, KeepAuthenticateInReq },
        { PacketId.VerifyCodeInf, VerifyCodeInf },
        { PacketId.OnGameStartInf, OnGameStartInf },
        { PacketId.OnChannelInfoInf, OnChannelInfoInf },
        { PacketId.LogOutReq, LogOutReq },
        { PacketId.UserInfoReq, UserInfoReq },
        { PacketId.InviteRejectReq, InviteRejectReq },
        { PacketId.OnInviteReq, OnInviteReq },
        { PacketId.TeamControlReq, TeamControlReq },
        { PacketId.ReadyReq, ReadyReq },
        { PacketId.StartReq, StartReq },
        { PacketId.PlayStartReq, PlayStartReq },
        { PacketId.PlaySkipReq, PlaySkipReq },
        { PacketId.PlayOverReq, PlayOverReq },
        { PacketId.PlayStateInf, PlayStateInf },
        { PacketId.StageResultInf, StageResultInf },
        { PacketId.LeaveRoomReq, LeaveRoomReq },
        { PacketId.ChangeDiscReq, ChangeDiscReq },
        { PacketId.UpdateUserAccountNickReq, UpdateUserAccountNickReq },
        { PacketId.UpdateUserProfileReq, UpdateUserProfileReq },
        { PacketId.GetItemReq, GetItemReq },
        { PacketId.ItemLevelUpReq, ItemLevelUpReq },
        { PacketId.UseItemReq, UseItemReq },
        { PacketId.UseEffectorInf, UseEffectorInf },
        { PacketId.MountItemReq, MountItemReq },
        { PacketId.GetPresentItemReq, GetPresentItemReq },
        { PacketId.DeleteItemReq, DeleteItemReq },
        { PacketId.MsgRegisterUserReq, MsgRegisterUserReq },
        { PacketId.PurchaseItemReq, PurchaseItemReq },
        { PacketId.ResaleItemReq, ResaleItemReq },
        { PacketId.OnKeepAuthenticateInAck, OnKeepAuthenticateInAck },
        { PacketId.LogInReq, LogInReq },
        { PacketId.OnUserIdInfoInf, OnUserIdInfoInf },
        { PacketId.OnEnvironmentInf, OnEnvironmentInf },
        { PacketId.OnUserInfoInf, OnUserInfoInf },
        { PacketId.OnInventoryInfoInf, OnInventoryInfoInf },
        { PacketId.OnMessengerInfoInf, OnMessengerInfoInf },
        { PacketId.OnLogInAck, OnLogInAck },
        { PacketId.OnChatInf, OnChatInf },
        { PacketId.OnWaiterInfoUpdateInf, OnWaiterInfoUpdateInf },
        { PacketId.OnRoomInfoUpdateInf, OnRoomInfoUpdateInf },
        { PacketId.OnInviteRejectAck, OnInviteRejectAck },
        { PacketId.PingTestInf, PingTestInf },
        { PacketId.CreateRoomReq, CreateRoomReq },
        { PacketId.OnRoomDescInf, OnRoomDescInf },
        { PacketId.OnCreateRoomAck, OnCreateRoomAck },
        { PacketId.OnUpdateJoinerInfoInf , OnUpdateJoinerInfoInf  },
        { PacketId.OnPostJoinRoomInf, OnPostJoinRoomInf },
        { PacketId.OnGameInfoInf, OnGameInfoInf },
        { PacketId.OnJoinEventInf, OnJoinEventInf },
        { PacketId.OnUseEffectorInf, OnUseEffectorInf },
        { PacketId.OnUseMountItemInf, OnUseMountItemInf },
        { PacketId.OnStartParameterInf, OnStartParameterInf },
        { PacketId.OnStartInf, OnStartInf },
        { PacketId.OnPlayStartInf, OnPlayStartInf },
        { PacketId.OnCheckDataReq, OnCheckDataReq },
        { PacketId.OnUpdateUserInventoryDefaultItemInf, OnUpdateUserInventoryDefaultItemInf },
        { PacketId.OnUpdateUserPropertyInf, OnUpdateUserPropertyInf },
        { PacketId.OnStageResultExInf, OnStageResultExInf },
        { PacketId.OnPlayOverInf, OnPlayOverInf },
        { PacketId.OnLeaveRoomAck, OnLeaveRoomAck },
        { PacketId.OnRoomInfoEraseInf, OnRoomInfoEraseInf },
        { PacketId.OnReadyInf, OnReadyInf },
        { PacketId.OnLogOutAck, OnLogOutAck },
        { PacketId.AuthenticateInSndeKeyAck, AuthenticateInSndeKeyAck },
        { PacketId.OnResaleItemAck, OnResaleItemAck },
        { PacketId.OnPurchaseItemAck, OnPurchaseItemAck },
        { PacketId.OnMsgGroupInf, OnMsgGroupInf },
        { PacketId.OnMsgBlkUserInf, OnMsgBlkUserInf },
        { PacketId.OnMsgRegUserInf, OnMsgRegUserInf },
        { PacketId.OnExpiredShopItemInf, OnExpiredShopItemInf },
        { PacketId.OnExpiredMountItemInf, OnExpiredMountItemInf },
        { PacketId.OnDeleteItemAck, OnDeleteItemAck },
        { PacketId.OnGetPresentItemAck, OnGetPresentItemAck },
        { PacketId.OnMountItemAck, OnMountItemAck },
        { PacketId.UseMountItemInf, UseMountItemInf },
        { PacketId.UseEffectorSetInf, UseEffectorSetInf },
        { PacketId.OnUpdateUserInventoryPresentItemInf, OnUpdateUserInventoryPresentItemInf },
        { PacketId.OnUpdateUserInventoryMountItemInf, OnUpdateUserInventoryMountItemInf },
        { PacketId.OnUpdateUserInventoryShopItemInf, OnUpdateUserInventoryShopItemInf },
        { PacketId.OnUpdateUserInventoryEventItemInf, OnUpdateUserInventoryEventItemInf },
        { PacketId.OnRoomChangeInfoAck, OnRoomChangeInfoAck },
        { PacketId.QuickInviteReq, QuickInviteReq },
        { PacketId.RoomChangeInfoReq, RoomChangeInfoReq },
        { PacketId.OnJoinRoomAck, OnJoinRoomAck },
        { PacketId.JoinRoomReq, JoinRoomReq },
        { PacketId.OnUserIdInfoAck, OnUserIdInfoAck },
        { PacketId.OnAliveReq, OnAliveReq },
        { PacketId.AliveAck, AliveAck },
        { PacketId.OnPeerCountInf, OnPeerCountInf },
        { PacketId.OnUbsAwardInfoInf, OnUbsAwardInfoInf },
        { PacketId.UserIdInfoReq, UserIdInfoReq },
        { PacketId.OnWaiterInfoEraseInf,OnWaiterInfoEraseInf},
        { PacketId.OnChangeDiscInf, OnChangeDiscInf},
        { PacketId.ProbeObfuscated, ProbeObfuscated},
        { PacketId.sub_434390, sub_434390 },
        { PacketId.sub_434450, sub_434450 },
        { PacketId.sub_434510, sub_434510 },
        { PacketId.sub_434620, sub_434620 },
        { PacketId.sub_434B40, sub_434B40 },
        { PacketId.sub_435F20, sub_435F20 },
        { PacketId.sub_436120, sub_436120 },
        { PacketId.sub_4362D0, sub_4362D0 },
        { PacketId.sub_437370, sub_437370 },
        { PacketId.sub_437900, sub_437900 },
        { PacketId.sub_432210, sub_432210 },
        { PacketId.sub_4323D0, sub_4323D0 }

    };
}
