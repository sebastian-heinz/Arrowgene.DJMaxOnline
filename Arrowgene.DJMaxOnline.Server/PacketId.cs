namespace Arrowgene.DJMaxOnline.Server;

public enum PacketId : ushort
{
    // OnInitialize = 0x00, - Not a Packet
    // OnRegister = 0x00, - Not a Packet
    // Resume = 0x00, - Not a Packet
    // OnConnect = 0x00, - Not a Packet

    InitReq = 0x00,
    Suspend = 0x00,
    OnDestroy = 0x00,
    OnDisconnect = 0x00,
    ConnectReq = 0x0A,
    OnConnectAck = 0x09,
    AuthenticateInReq = 0x00,  // possibly left over from korean and jp (used activex)
    AuthenticateInSndAccReq = 0x11,
    AuthenticateInSndeKeyAck = 0x00, 
    OnAuthenticateInAck = 0x10,
    OnAuthenticateInSndeKeyReq = 0x12,
    OnAuthenticateInSndrPwdReq = 0x14,
    AuthenticateInSndrPwdAck = 0x00,
    OnUbsAccountAuthenResAck = 0x12D, // case 301
    OnUbsAwardInfoInf = 0x12E,
    OnUbsAwardAuthenAck = 0x130,
    KeepAuthenticateInReq = 0x00,
    OnKeepAuthenticateInAck = 0x16,
    LogInReq = 0x00,
    OnUserInfoInf = 0x43,
    OnInventoryInfoInf = 0x44,
    OnMessengerInfoInf = 0x45,
    OnLogInAck = 0x1A,
    LogOutReq = 0x00,
    OnLogOutAck = 0x18,
    ClearChannelInfo = 0x00,
    OnChannelInfoInf = 0x0B,
    OnPeerCountInf = 0x0C,
    OnDisconnectPeerInf = 0x08,
    AliveAck = 0x00,
    OnAliveReq = 0x07,
    OnPingTestInf = 0x03,
    PingTestInf = 0x00,
    UserInfoReq = 0x00,
    OnUserInfoAck = 0x1F,
    OnUserInfoResNotFound = 0x1E,
    OnBigNewsInf = 0x97,
    OnChatInf = 0x39, // game sends 0x38.
    OnWChatInf = 0x37, //maybe same pattern?(0x36)
    OnCourseListInf = 0x82,
    OnCourseRankAck = 0x84,
    OnChangeCourseAck = 0x86,
    OnContinueCourseAck = 0x88,
    OnPostCourseItemReq = 0x89,
    OnAwardItemInf = 0x8C,
    OnCipherCommandInf = 0x10E, // my guess is this is some sort of secret gm command, the guy who wrote the netcode and crypto was rehpic, (cipher backwards!)
    OnEnvironmentInf = 0xFC,
    OnSystemInfoAck = 0xFE,
    ClearWaiterInfo = 0x00,
    UpdateWaiterInfo = 0x00,
    EraseWaiterInfo = 0x00,
    OnWaiterInfoUpdateInf = 0x3C,
    OnWaiterInfoEraseInf = 0x3D,
    ClearRoomInfo = 0x00,
    UpdateRoomInfo = 0x00,
    EraseRoomInfo = 0x00,
    OnRoomInfoUpdateInf = 0x3A,
    OnRoomInfoEraseInf = 0x3B,
    UserIdInfoReq = 0x00,
    OnUserIdInfoAck = 0x22,
    OnUserIdInfoInf = 0x20,
    ClearUserIdInfo = 0x00,
    OnJoinerListStart = 0x40,
    OnJoinerListEnt = 0x41,
    OnJoinerListEnd = 0x42,
    CreateRoomReq = 0x00,
    OnCreateRoomAck = 0x4D,
    JoinRoomReq = 0x00,
    OnJoinRoomAck = 0x47,
    OnPostJoinRoomInf = 0x48,
    InviteRejectReq = 0x00,
    OnInviteRejectAck = 0xA7,
    OnRoomDescInf = 0x50,
    OnQuickInviteAck = 0xA1,
    OnInviteReq = 0xA2,
    QuickInviteReq = 0x00,
    RoomChangeInfoReq = 0x00,
    OnRoomChangeInfoAck = 0x9D,
    ClearJoinerInfo = 0x00,
    UpdateJoinerInfo = 0x00,
    EraseJoinerInfo = 0x00,
    TeamControlReq = 0x00,
    OnTeamControlInf = 0x59,
    OnGameTypeInf = 0x5B,
    ReadyReq = 0x00,
    OnReadyInf = 0x5E,
    StartReq = 0x00,
    OnStartInf = 0x60,
    OnJoinEventInf = 0x61,
    OnEventInfoInf = 0x63,
    PlayStartReq = 0x00,
    OnPlayStartInf = 0x65,
    PlaySkipReq = 0x00,
    OnPlaySkipInf = 0x68,
    PlayOverReq = 0x00,
    OnPlayOverInf = 0x6B,
    PlayStateInf = 0x00,
    OnPlayStateInf = 0x6D,
    OnCheckDataReq = 0x71,
    OnLoadCompleteInf = 0x7C,
    StageResultInf = 0x00,
    OnStageResultExInf = 0x70,
    LeaveRoomReq = 0x00,
    OnLeaveRoomAck = 0x74,
    ChangeDiscReq = 0x00,
    OnChangeDiscInf = 0x77,
    OnAwardInfoInf = 0x78,
    OnGameInfoInf = 0x7A,
    UpdateUserAccountNickReq = 0x00,
    OnUpdateUserAccountNickAck = 0x31,
    UpdateUserProfileReq = 0x00,
    OnUpdateUserProfileAck = 0x33,
    OnUpdateUserIconInf = 0x24,
    OnUpdateUserPropertyInf = 0x25,
    OnUpdateUserPropertyRecordInf = 0x28,
    OnUpdateUserPropertyMiscInf = 0x29,
    OnUpdateUserPropertyLevelInf = 0x26,
    OnUpdateUserPropertyMoneyInf = 0x27,
    OnUpdateUserInventoryDefaultItemInf = 0x2A,
    OnUpdateUserInventoryEventItemInf = 0x2B,
    OnUpdateUserInventoryShopItemInf = 0x2C,
    OnUpdateUserInventoryMountItemInf = 0x2D,
    OnUpdateUserInventoryPresentItemInf = 0x2E,
    GetItemReq = 0x00,
    OnCrItemInf = 0xB3,
    OnGetItemFail = 0xB5,
    OnGetItemAck = 0xB6,
    ItemLevelUpReq = 0x00,
    OnItemLevelUpFail = 0xB8,
    OnItemLevelUpAck = 0xB9,
    UseItemReq = 0x00,
    OnUseItemFail = 0xBB,
    OnUseItemAck = 0xBC,
    OnGoodLuckInf = 0xBD,
    OnGoodLuckListInf = 0xBE,
    OnMissionStandItemInf = 0xC0,
    UseEffectorInf = 0x00,
    UseEffectorSetInf = 0x00,
    OnUseEffectorInf = 0xC4,
    OnUseEffectorSetInf = 0xC6,
    UseMountItemInf = 0x00,
    OnUseMountItemInf = 0xC9,
    OnStartParameterInf = 0xCA,
    OnBillingAuthInf = 0xD2,
    OnGameStartInf = 0xD3,
    OnUserAlertInf = 0xD4,
    Report = 0x00,
    MountItemReq = 0x00,
    OnMountItemAck = 0xD8,
    GetPresentItemReq = 0x00,
    OnGetPresentItemAck = 0xDA,
    DeleteItemReq = 0x00,
    OnDeleteItemAck = 0xDC,
    OnExpiredMountItemInf = 0xE4,
    OnExpiredShopItemInf = 0xE5,
    OnAlertCreditInf = 0xE6,
    OnMsgNotifyInf = 0xF1,
    MsgRegisterUserReq = 0x00,
    OnMsgRegisterUserAck = 0xF3,
    OnMsgRegUserInf = 0xF4,
    OnMsgBlkUserInf = 0xF5,
    OnMsgGroupInf = 0xF6,
    PurchaseItemReq = 0x00,
    OnPurchaseItemAck = 0xDE,
    ResaleItemReq = 0x00,
    OnResaleItemAck = 0xE0,
    OnUpdateUserAccountClassInf = 0x2F, // TODO verify
    ClearCourseList = 0x17, // TODO for testing
    VerifyCodeInf = 0x16, // TODO for testing
}
