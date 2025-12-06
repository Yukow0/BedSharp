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

            // Client timestamp (Big Endian)
            bigEndianWriter.WriteBigEndian((ulong)clientTime);

            // Server GUID (Big Endian)
            
            bigEndianWriter.WriteBigEndian(serverId);

            // Magic
            bigEndianWriter.Write(ServerInfo.Magic);

            // String length (Big Endian, 2 bytes = ushort)
            byte[] serverDataBytes = Encoding.UTF8.GetBytes(serverData);
            byte[] lengthBytes = new byte[2];
            bigEndianWriter.WriteBigEndian((ushort)serverDataBytes.Length);
            

            // Server data string
            bigEndianWriter.Write(serverDataBytes);

            // Send the response
            byte[] responsePacket = msWrite.ToArray();

            return responsePacket;
        }
    }
}