using System.Buffers.Binary;
using BedSharp.BedSharp;
using BedSharp.Protocols.Bedrock.Packets.PacketUtils;
using BedSharp.Utils;
using BedSharp.Utils.PacketsHandler;

namespace BedSharp.Protocols.Bedrock.Packets;

public class NetworkSettings : BedrockPacket
{
    
    private int _protocolVersion;
    public bool IsVersionValid { get; private set; }

    public NetworkSettings(ReadOnlyMemory<byte> packetData, PacketHandler status) : base(packetData, status)
    {
        
    }

    public override IBedrockPacket Clone()
    {
        return new NetworkSettings(_packetData, _state);
    }
    
    public override void Process()
    {
        if (_protocolVersion != ServerInfo.ProtocolVersion)
        {
            Console.WriteLine("Protocol version mismatch");
            IsVersionValid = false;
            return;
        }
        Console.WriteLine("Protocol version match");
        IsVersionValid = true;
    }
    
    public override void Decode()
    {
        _protocolVersion = BinaryPrimitives.ReadInt32BigEndian(_packetData.Span);
        _packetData = _packetData.Slice(4);
        Process();
    }
    
    public override byte[] Encode()
    {
        byte[] data = new byte[12];
    
        
        int offset = VarInts.WriteUnsignedInt(data.AsSpan(0), 143);
        
        BinaryPrimitives.WriteUInt16LittleEndian(data.AsSpan(offset), 100);
        offset += 2;
        
        BinaryPrimitives.WriteUInt16LittleEndian(data.AsSpan(offset), 0);
        offset += 2;
        
        data[offset++] = 1;
        data[offset++] = 0;
        BinaryPrimitives.WriteSingleLittleEndian(data.AsSpan(offset), 0.0f);
    
        return data;
    }
    
    
}