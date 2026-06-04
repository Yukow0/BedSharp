using BedSharp.Utils.PacketsHandler;

namespace BedSharp.Protocols.Bedrock.Packets.PacketUtils;

public interface IBedrockPacket
{
    PacketHandler GetState();
    
    ReadOnlyMemory<byte> GetPacketBytes();

    IBedrockPacket Clone();
}