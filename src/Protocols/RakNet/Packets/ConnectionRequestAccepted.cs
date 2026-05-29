using System.Net;
using BedSharp.BedSharp;
using BedSharp.Utils;

namespace BedSharp.Protocols.RakNet.Packets;

public class ConnectionRequestAccepted
{
    public static byte[] SendConnectionRequestAccepted(IPEndPoint client, long clientTimestamp)
    {
        using (MemoryStream msWrite = new MemoryStream())
        using (BigEndianWriter bigEndianWriter = new BigEndianWriter(msWrite))
        {
            IPEndPoint server = new IPEndPoint(new IPAddress(new byte[] { 255, 255, 255, 255 }), 19133);
            bigEndianWriter.Write((byte)MessageIdentifiers.IdConnectionRequestAccepted);

            
            bigEndianWriter.WriteClientAddress(client);

            bigEndianWriter.WriteClientAddress(new IPEndPoint(new IPAddress(new byte[] { 10, 5, 40, 90 }), 19133));
            for (int i = 0; i < 18; i++)
            {
                bigEndianWriter.WriteClientAddress(server);
            }
            bigEndianWriter.WriteBigEndian((ushort)clientTimestamp);
            bigEndianWriter.WriteBigEndian((ushort)Environment.TickCount64);
            
            
            byte[] responsePacket = msWrite.ToArray();

            return responsePacket;
        }
    }
}