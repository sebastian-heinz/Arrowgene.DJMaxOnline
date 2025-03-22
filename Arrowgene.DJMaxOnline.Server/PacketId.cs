﻿namespace Arrowgene.DJMaxOnline.Server;

public enum PacketId : ushort
{
    //OnInitialize = 0x00,
    InitReq = 0x00,
    //OnRegister = 0x00,
    Suspend = 0x00,
    //Resume = 0x00,
    OnDestroy = 0x00,
    //OnConnect = 0x00,
    OnDisconnect = 0x00,
    ConnectReq = 0x0A,
    OnConnectAck = 0x09,
    AuthenticateInReq = 0x00,
    AuthenticateInSndAccReq = 0x11,
    AuthenticateInSndeKeyAck = 0x00,
    OnAuthenticateInAck = 0x10,
    OnAuthenticateInSndeKeyReq = 0x00,
    OnAuthenticateInSndrPwdReq = 0x00,
    AuthenticateInSndrPwdAck = 0x00,
    OnUbsAccountAuthenResAck = 0x00,
    OnUbsAwardInfoInf = 0x00,
    OnUbsAwardAuthenAck = 0x00,
    KeepAuthenticateInReq = 0x00,
    OnKeepAuthenticateInAck = 0x00,
    LogInReq = 0x00,
    OnUserInfoInf = 0x00,
    OnInventoryInfoInf = 0x00,
    OnMessengerInfoInf = 0x00,
    OnLogInAck = 0x00,
    LogOutReq = 0x00,
    OnLogOutAck = 0x00,
    ClearChannelInfo = 0x00,
    OnChannelInfoInf = 0x00,
    OnPeerCountInf = 0x00,
    OnDisconnectPeerInf = 0x00,
    AliveAck = 0x00,
    OnAliveReq = 0x00,
    OnPingTestInf = 0x03,
    PingTestInf = 0x00,
    UserInfoReq = 0x00,
    OnUserInfoAck = 0x00,
    OnUserInfoResNotFound = 0x00,
    OnBigNewsInf = 0x00,
    OnChatInf = 0x00,
    OnWChatInf = 0x00,
    OnCourseListInf = 0x00,
    OnCourseRankAck = 0x00,
    OnChangeCourseAck = 0x00,
    OnContinueCourseAck = 0x00,
    OnPostCourseItemReq = 0x00,
    OnAwardItemInf = 0x00,
    OnCipherCommandInf = 0x00,
    OnEnvironmentInf = 0x00,
    OnSystemInfoAck = 0x00,
    ClearWaiterInfo = 0x00,
    UpdateWaiterInfo = 0x00,
    EraseWaiterInfo = 0x00,
    OnWaiterInfoUpdateInf = 0x00,
    OnWaiterInfoEraseInf = 0x00,
    ClearRoomInfo = 0x00,
    UpdateRoomInfo = 0x00,
    EraseRoomInfo = 0x00,
    OnRoomInfoUpdateInf = 0x00,
    OnRoomInfoEraseInf = 0x00,
    UserIdInfoReq = 0x00,
    OnUserIdInfoAck = 0x00,
    OnUserIdInfoInf = 0x00,
    ClearUserIdInfo = 0x00,
    OnJoinerListStart = 0x00,
    OnJoinerListEnt = 0x00,
    OnJoinerListEnd = 0x00,
    CreateRoomReq = 0x00,
    OnCreateRoomAck = 0x00,
    JoinRoomReq = 0x00,
    OnJoinRoomAck = 0x00,
    OnPostJoinRoomInf = 0x00,
    InviteRejectReq = 0x00,
    OnInviteRejectAck = 0x00,
    OnRoomDescInf = 0x00,
    OnQuickInviteAck = 0x00,
    OnInviteReq = 0x00,
    QuickInviteReq = 0x00,
    RoomChangeInfoReq = 0x00,
    OnRoomChangeInfoAck = 0x00,
    ClearJoinerInfo = 0x00,
    UpdateJoinerInfo = 0x00,
    EraseJoinerInfo = 0x00,
    TeamControlReq = 0x00,
    OnTeamControlInf = 0x00,
    OnGameTypeInf = 0x00,
    ReadyReq = 0x00,
    OnReadyInf = 0x00,
    StartReq = 0x00,
    OnStartInf = 0x00,
    OnJoinEventInf = 0x00,
    OnEventInfoInf = 0x00,
    PlayStartReq = 0x00,
    OnPlayStartInf = 0x00,
    PlaySkipReq = 0x00,
    OnPlaySkipInf = 0x00,
    PlayOverReq = 0x00,
    OnPlayOverInf = 0x00,
    PlayStateInf = 0x00,
    OnPlayStateInf = 0x00,
    OnCheckDataReq = 0x00,
    OnLoadCompleteInf = 0x00,
    StageResultInf = 0x00,
    OnStageResultExInf = 0x00,
    LeaveRoomReq = 0x00,
    OnLeaveRoomAck = 0x00,
    ChangeDiscReq = 0x00,
    OnChangeDiscInf = 0x00,
    OnAwardInfoInf = 0x00,
    OnGameInfoInf = 0x00,
    UpdateUserAccountNickReq = 0x00,
    OnUpdateUserAccountNickAck = 0x00,
    UpdateUserProfileReq = 0x00,
    OnUpdateUserProfileAck = 0x00,
    OnUpdateUserIconInf = 0x00,
    OnUpdateUserPropertyInf = 0x00,
    OnUpdateUserPropertyRecordInf = 0x00,
    OnUpdateUserPropertyMiscInf = 0x00,
    OnUpdateUserPropertyLevelInf = 0x00,
    OnUpdateUserPropertyMoneyInf = 0x00,
    OnUpdateUserInventoryDefaultItemInf = 0x00,
    OnUpdateUserInventoryEventItemInf = 0x00,
    OnUpdateUserInventoryShopItemInf = 0x00,
    OnUpdateUserInventoryMountItemInf = 0x00,
    OnUpdateUserInventoryPresentItemInf = 0x00,
    GetItemReq = 0x00,
    OnCrItemInf = 0x00,
    OnGetItemFail = 0x00,
    OnGetItemAck = 0x00,
    ItemLevelUpReq = 0x00,
    OnItemLevelUpFail = 0x00,
    OnItemLevelUpAck = 0x00,
    UseItemReq = 0x00,
    OnUseItemFail = 0x00,
    OnUseItemAck = 0x00,
    OnGoodLuckInf = 0x00,
    OnGoodLuckListInf = 0x00,
    OnMissionStandItemInf = 0x00,
    UseEffectorInf = 0x00,
    UseEffectorSetInf = 0x00,
    OnUseEffectorInf = 0x00,
    OnUseEffectorSetInf = 0x00,
    UseMountItemInf = 0x00,
    OnUseMountItemInf = 0x00,
    OnStartParameterInf = 0x00,
    OnBillingAuthInf = 0x00,
    OnGameStartInf = 0x00,
    OnUserAlertInf = 0x00,
    Report = 0x00,
    MountItemReq = 0x00,
    OnMountItemAck = 0x00,
    GetPresentItemReq = 0x00,
    OnGetPresentItemAck = 0x00,
    DeleteItemReq = 0x00,
    OnDeleteItemAck = 0x00,
    OnExpiredMountItemInf = 0x00,
    OnExpiredShopItemInf = 0x00,
    OnAlertCreditInf = 0x00,
    OnMsgNotifyInf = 0x00,
    MsgRegisterUserReq = 0x00,
    OnMsgRegisterUserAck = 0x00,
    OnMsgRegUserInf = 0x00,
    OnMsgBlkUserInf = 0x00,
    OnMsgGroupInf = 0x00,
    PurchaseItemReq = 0x00,
    OnPurchaseItemAck = 0x00,
    ResaleItemReq = 0x00,
    OnResaleItemAck = 0x00,
    OnUpdateUserAccountClassInf = 0x00,
    ClearCourseList = 0x00,
    VerifyCodeInf = 0x00,
}