using System.Buffers.Binary;
using System.Net;
using System.Text;
using BedSharp.BedSharp;
using BedSharp.Utils;

namespace BedSharp.Protocols.RakNet.Packets;

public class PingPacket
{
    public static byte[] SendPong(long clientTime, IPEndPoint clientEndPoint, ulong serverId, string serverData)
    {
        using (MemoryStream msWrite = new MemoryStream())
        using (BigEndianWriter bigEndianWriter = new BigEndianWriter(msWrite))
        {
            bigEndianWriter.Write((byte)MessageIdentifiers.IdUnconnectedPong);
            bigEndianWriter.WriteBigEndian((ulong)clientTime);
            bigEndianWriter.WriteBigEndian(serverId);
            bigEndianWriter.Write(ServerInfo.Magic);

            
            byte[] serverDataBytes = Encoding.UTF8.GetBytes(serverData);
            byte[] lengthBytes = new byte[2];
            bigEndianWriter.WriteBigEndian((ushort)serverDataBytes.Length);
            

            
            bigEndianWriter.Write(serverDataBytes);

            
            byte[] responsePacket = msWrite.ToArray();

            return responsePacket;
        }
    }
}