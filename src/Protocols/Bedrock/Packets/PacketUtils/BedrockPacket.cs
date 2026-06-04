using BedSharp.Utils.PacketsHandler;

namespace BedSharp.Protocols.Bedrock.Packets.PacketUtils;

public abstract class BedrockPacket : IBedrockPacket
{
    protected PacketHandler _state;
    protected ReadOnlyMemory<byte> _packetData;

    public BedrockPacket(ReadOnlyMemory<byte> packetData, PacketHandler state)
    {
        _packetData = packetData;
        _state = state;
    }

    public ReadOnlyMemory<byte> GetPacketBytes()
    {
        return _packetData;
    }
    
    public PacketHandler GetState()
    {
        return _state;
    }
    
    
    public abstract IBedrockPacket Clone();
    
    public abstract void Process();
    
    public abstract void Decode();
    
    public abstract void Encode();
}