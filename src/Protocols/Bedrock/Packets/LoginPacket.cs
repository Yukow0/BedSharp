using System.Buffers.Binary;
using BedSharp.Protocols.Bedrock.Packets.PacketUtils;
using BedSharp.Utils.PacketsHandler;

namespace BedSharp.Protocols.Bedrock.Packets;

public class LoginPacket : BedrockPacket
{

    private int _protocolVersion;
    
    private AuthPayload _authPayload;

    private string clientJwt;
    public LoginPacket(ReadOnlyMemory<byte> packetData, PacketHandler state) : base(packetData, state)
    {
        
    }

    public PacketHandler handle(BedrockPacketHandler handler)
    {
        return handler.handle(this);
    }

    public override IBedrockPacket Clone()
    {
        return new LoginPacket(_packetData, _state);
    }

    public override void Decode()
    {
        int protocole = BinaryPrimitives.ReadInt32BigEndian(_packetData.Span);
        _packetData = _packetData.Slice(4);
        
    }

    public override void Process()
    {
        throw new NotImplementedException();
    }

    public override void Encode()
    {
        throw new NotImplementedException();
    }
}