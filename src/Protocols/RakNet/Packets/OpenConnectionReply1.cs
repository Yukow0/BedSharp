using System.Net;
using System.Text;
using BedSharp.BedSharp;
using BedSharp.Utils;

namespace BedSharp.Protocols.RakNet.Packets;

public class OpenConnectionReply1
{
    public static byte[] SendOpenConnection(ulong serverId)
    {
        using (MemoryStream msWrite = new MemoryStream())
        using (BigEndianWriter bigEndianWriter = new BigEndianWriter(msWrite))
        {
            bigEndianWriter.Write((byte)MessageIdentifiers.IdOpenConnectionReply1);

            
            bigEndianWriter.Write(ServerInfo.Magic);
            
            bigEndianWriter.WriteBigEndian(serverId);
            bigEndianWriter.Write(false);
            bigEndianWriter.WriteBigEndian(1500);
            
            byte[] responsePacket = msWrite.ToArray();

            return responsePacket;
        }
    }
}