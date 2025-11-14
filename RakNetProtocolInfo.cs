namespace BedSharp;

public enum MessageIdentifiers : byte
{
    IdConnectedPing = 0x00,
    IdUnconnectedPing = 0x01,
    IdUnconnectedPingOpenConnections = 0x02,
    IdConnectedPong = 0x03,
    IdDetectLostConnections = 0x04,
    IdOpenConnectionRequest1 = 0x05,
    IdOpenConnectionReply1 = 0x06,
    IdOpenConnectionRequest2 = 0x07,
    IdOpenConnectionReply2 = 0x08,
    IdConnectionRequest = 0x09,
    IdRemoteSystemRequiresPublicKey = 0x0A,
    IdOurSystemRequiresSecurity = 0x0B,
    IdPublicKeyMismatch = 0x0C,
    IdOutOfBandInternal = 0x0D,
    IdSndReceiptAcked = 0x0E,
    IdSndReceiptLoss = 0x0F,

    IdConnectionRequestAccepted = 0x10,
    IdConnectionAttemptFailed = 0x11,
    IdAlreadyConnected = 0x12,
    IdNewIncomingConnection = 0x13,
    IdNoFreeIncomingConnections = 0x14,
    IdDisconnectionNotification = 0x15,
    IdConnectionLost = 0x16,
    IdConnectionBanned = 0x17,
    IdInvalidPassword = 0x18,
    IdIncompatibleProtocolVersion = 0x19,
    IdIpRecentlyConnected = 0x1A,
    IdTimestamp = 0x1B,
    IdUnconnectedPong = 0x1C,
    IdAdvertiseSystem = 0x1D,
    IdDownloadProgress = 0x1E,

    IdRemoteDisconnectionNotification = 0x1F,
    IdRemoteConnectionLost = 0x20,
    IdRemoteNewIncomingConnection = 0x21,

    IdFileListTransferHeader = 0x22,
    IdFileListTransferFile = 0x23,
    IdFileListReferencePushAck = 0x24,

    IdDdtDownloadRequest = 0x25,

    IdTransportString = 0x26,

    IdReplicaManagerConstruction = 0x27,
    IdReplicaManagerScopeChange = 0x28,
    IdReplicaManagerSerialize = 0x29,
    IdReplicaManagerDownloadStarted = 0x2A,
    IdReplicaManagerDownloadComplete = 0x2B,

    IdRakvoiceOpenChannelRequest = 0x2C,
    IdRakvoiceOpenChannelReply = 0x2D,
    IdRakvoiceCloseChannel = 0x2E,
    IdRakvoiceData = 0x2F,

    IdAutopatcherGetChangelistSinceDate = 0x30,
    IdAutopatcherCreationList = 0x31,
    IdAutopatcherDeletionList = 0x32,
    IdAutopatcherGetPatch = 0x33,
    IdAutopatcherPatchList = 0x34,
    IdAutopatcherRepositoryFatalError = 0x35,
    IdAutopatcherCannotDownloadOriginalUnmodifiedFiles = 0x36,
    IdAutopatcherFinishedInternal = 0x37,
    IdAutopatcherFinished = 0x38,
    IdAutopatcherRestartApplication = 0x39,

    IdNatPunchthroughRequest = 0x3A,
    IdNatConnectAtTime = 0x3B,
    IdNatGetMostRecentPort = 0x3C,
    IdNatClientReady = 0x3D,

    IdNatTargetNotConnected = 0x3E,
    IdNatTargetUnresponsive = 0x3F,
    IdNatConnectionToTargetLost = 0x40,
    IdNatAlreadyInProgress = 0x41,
    IdNatPunchthroughFailed = 0x42,
    IdNatPunchthroughSucceeded = 0x43,

    IdReadyEventSet = 0x44,
    IdReadyEventUnset = 0x45,
    IdReadyEventAllSet = 0x46,
    IdReadyEventQuery = 0x47,

    IdLobbyGeneral = 0x48,

    IdRpcRemoteError = 0x49,
    IdRpcPlugin = 0x4A,

    IdFileListReferencePush = 0x4B,
    IdReadyEventForceAllSet = 0x4C,

    IdRoomsExecuteFunc = 0x4D,
    IdRoomsLogonStatus = 0x4E,
    IdRoomsHandleChange = 0x4F,

    IdLobby2SendMessage = 0x50,
    IdLobby2ServerError = 0x51,

    IdFcm2NewHost = 0x52,
    IdFcm2RequestFcmguid = 0x53,
    IdFcm2RespondConnectionCount = 0x54,
    IdFcm2InformFcmguid = 0x55,
    IdFcm2UpdateMinTotalConnectionCount = 0x56,
    IdFcm2VerifiedJoinStart = 0x57,
    IdFcm2VerifiedJoinCapable = 0x58,
    IdFcm2VerifiedJoinFailed = 0x59,
    IdFcm2VerifiedJoinAccepted = 0x5A,
    IdFcm2VerifiedJoinRejected = 0x5B,

    IdUdpProxyGeneral = 0x5C,

    IdSqLite3Exec = 0x5D,
    IdSqLite3UnknownDb = 0x5E,
    IdSqlLiteLogger = 0x5F,

    IdNatTypeDetectionRequest = 0x60,
    IdNatTypeDetectionResult = 0x61,

    IdRouter2Internal = 0x62,
    IdRouter2ForwardingNoPath = 0x63,
    IdRouter2ForwardingEstablished = 0x64,
    IdRouter2Rerouted = 0x65,

    IdTeamBalancerInternal = 0x66,
    IdTeamBalancerRequestedTeamFull = 0x67,
    IdTeamBalancerRequestedTeamLocked = 0x68,
    IdTeamBalancerTeamRequestedCancelled = 0x69,
    IdTeamBalancerTeamAssigned = 0x6A,

    IdLightspeedIntegration = 0x6B,
    IdXboxLobby = 0x6C,

    IdTwoWayAuthenticationIncomingChallengeSuccess = 0x6D,
    IdTwoWayAuthenticationOutgoingChallengeSuccess = 0x6E,
    IdTwoWayAuthenticationIncomingChallengeFailure = 0x6F,
    IdTwoWayAuthenticationOutgoingChallengeFailure = 0x70,
    IdTwoWayAuthenticationOutgoingChallengeTimeout = 0x71,
    IdTwoWayAuthenticationNegotiation = 0x72,

    IdCloudPostRequest = 0x73,
    IdCloudReleaseRequest = 0x74,
    IdCloudGetRequest = 0x75,
    IdCloudGetResponse = 0x76,
    IdCloudUnsubscribeRequest = 0x77,
    IdCloudServerToServerCommand = 0x78,
    IdCloudSubscriptionNotification = 0x79,

    IdLibVoice = 0x7A,

    IdRelayPlugin = 0x7B,
    IdNatRequestBoundAddresses = 0x7C,
    IdNatRespondBoundAddresses = 0x7D,
    IdFcm2UpdateUserContext = 0x7E,
    IdReserved3 = 0x7F,
    IdReserved4 = 0x80,
    IdReserved5 = 0x81,
    IdReserved6 = 0x82,
    IdReserved7 = 0x83,
    IdReserved8 = 0x84,
    IdReserved9 = 0x85,

    IdUserPacketEnum = 0x86
}
