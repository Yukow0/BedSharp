using System.Net;
using BedSharp.BedSharp;
using BedSharp.Utils;

namespace BedSharp.Protocols.RakNet.Packets;

public class OpenConnectionReply2
{
    public static byte[] SendOpenConnection(ulong serverId, IPEndPoint clientIp)
    {
        using (MemoryStream msWrite = new MemoryStream())
        using (BigEndianWriter bigEndianWriter = new BigEndianWriter(msWrite))
        {
            bigEndianWriter.Write((byte)MessageIdentifiers.IdOpenConnectionReply2);

            
            bigEndianWriter.Write(ServerInfo.Magic);
            
            bigEndianWriter.WriteBigEndian(serverId);
            bigEndianWriter.WriteClientAddress(clientIp);
            bigEndianWriter.WriteBigEndian(1500);
            bigEndianWriter.Write(false);
            
            byte[] responsePacket = msWrite.ToArray();

            return responsePacket;
        }
    }
}