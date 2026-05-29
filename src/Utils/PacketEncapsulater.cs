using System.Buffers.Binary;

namespace BedSharp.Utils;

public class PacketEncapsulater
{
    public static byte[] FrameSetPacketGenerate(byte[] payload, int sequenceNumber, int reliableNumber)
    {
        using (MemoryStream msWrite = new MemoryStream())
        using (BigEndianWriter bigEndianWriter = new BigEndianWriter(msWrite))
        {
            msWrite.WriteByte(0x84);
            byte sequenceNumberinByte = (byte)(sequenceNumber & 0xFF);
            byte sequenceNumberMidByte = (byte)((sequenceNumber >> 8) & 0xFF);
            byte sequenceNumberEndByte = (byte)((sequenceNumber >> 16) & 0xFF);
            msWrite.WriteByte(sequenceNumberinByte);
            msWrite.WriteByte(sequenceNumberMidByte);
            msWrite.WriteByte(sequenceNumberEndByte);
            
            msWrite.WriteByte(0x40);
            int lengthinBits = payload.Length * 8;
            ushort finalSize = (ushort)lengthinBits;
            bigEndianWriter.WriteBigEndian(finalSize);
            
            byte reliableNumberBeginByte = (byte)(reliableNumber & 0xFF);
            byte reliableNumberMidByte = (byte)((reliableNumber >> 8) & 0xFF);
            byte reliableNumberEndByte = (byte)((reliableNumber >> 16) & 0xFF);
            msWrite.WriteByte(reliableNumberBeginByte);
            msWrite.WriteByte(reliableNumberMidByte);
            msWrite.WriteByte(reliableNumberEndByte);
            msWrite.Write(payload);
            
            return msWrite.ToArray();
        }
    }
}