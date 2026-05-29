using BedSharp.Utils;

namespace BedSharp.Protocols.RakNet.Packets;

public class ConnectedPong
{
    public static byte[] SendConnectedPong(UInt64 clientTimestamp)
    {
        using (MemoryStream msWrite = new MemoryStream())
        {
            msWrite.WriteByte((byte)MessageIdentifiers.IdConnectedPong);
            
            
            byte[] clientBytes = BitConverter.GetBytes(clientTimestamp);
            msWrite.Write(clientBytes, 0, clientBytes.Length);
            
            byte[] serverBytes = BitConverter.GetBytes(Environment.TickCount64);
            msWrite.Write(serverBytes, 0, serverBytes.Length);
            
            return msWrite.ToArray();
        }
    }
}