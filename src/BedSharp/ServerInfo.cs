using BedSharp.Protocols.Bedrock;

namespace BedSharp.BedSharp;

public class ServerInfo
{
    
    public const int DefaultPort = 19132;

    public static readonly byte[] Magic = new byte[] 
    { 
        0x00, 0xFF, 0xFF, 0x00, 0xFE, 0xFE, 0xFE, 0xFE, 
        0xFD, 0xFD, 0xFD, 0xFD, 0x12, 0x34, 0x56, 0x78 
    };
    
    
    public const int ProtocolVersion = BedrockProtocolInfo.CurrentProtocol;
    public const string MinecraftVersion = BedrockProtocolInfo.MinecraftVersion;
    
    
    public static string GetServerData(ulong serverId, string motd, int maxPlayers = 20)
    {
        return $"MCPE;{motd};{ProtocolVersion};{MinecraftVersion};0;{maxPlayers};{serverId};BedSharp;Survival;1;{DefaultPort};{DefaultPort + 1};";
    }
}