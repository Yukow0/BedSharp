using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BedSharp;

class Program
{
    public static void Main(string[] args)
    {
        int port = 19132;
        Random rnd = new Random();
        ulong SERVER_ID = ((ulong)rnd.Next() << 32) | (uint)rnd.Next();
        

        Console.WriteLine($"Server ID: {SERVER_ID}");
        string serverData = $"MCPE;Serveur C# de Rieno;859;1.21.122;0;20;{SERVER_ID};BedSharp;Survival;1;19132;19133;";

        byte[] MAGIC = new byte[] 
        { 
        0x00, 0xFF, 0xFF, 0x00, 0xFE, 0xFE, 0xFE, 0xFE, 
        0xFD, 0xFD, 0xFD, 0xFD, 0x12, 0x34, 0x56, 0x78 
        };

        int magic;

        

        UdpClient listener = new UdpClient(port);

        Console.WriteLine($"Listening on port {port}..");

        IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            while (true)
            {
                byte[] datagram = listener.Receive(ref clientEndPoint);

                Console.WriteLine($"\n========================================");
                Console.WriteLine(
                    $"Received {datagram.Length} bytes from {clientEndPoint.Address}:{clientEndPoint.Port}");
                Console.WriteLine($"Packet ID: 0x{datagram[0]:X2}");
                Console.WriteLine($"Full packet: {BitConverter.ToString(datagram)}");
                Console.WriteLine($"========================================");

                using (MemoryStream msRead = new MemoryStream(datagram))
                using (BinaryReader br = new BinaryReader(msRead))
                {
                    if (datagram[0] == (byte)MessageIdentifiers.IdUnconnectedPing)
                    {
                        br.ReadByte(); // Skip the packet ID (0x01)

                        // Read the timestamp (Big Endian)
                        long clientTime = BinaryPrimitives.ReadInt64BigEndian(br.ReadBytes(8));

                        

                        // Construct response packet (0x1c - Unconnected Pong)
                        using (MemoryStream msWrite = new MemoryStream())
                        using (BinaryWriter bw = new BinaryWriter(msWrite))
                        {
                            // Packet ID
                            bw.Write((byte)MessageIdentifiers.IdUnconnectedPong);

                            // Client timestamp (Big Endian)
                            byte[] timeBytes = new byte[8];
                            BinaryPrimitives.WriteInt64BigEndian(timeBytes, clientTime);
                            bw.Write(timeBytes);

                            // Server GUID (Big Endian)
                            byte[] guidBytes = new byte[8];
                            BinaryPrimitives.WriteInt64BigEndian(guidBytes, (long)SERVER_ID);
                            bw.Write(guidBytes);

                            // Magic
                            bw.Write(MAGIC);

                            // String length (Big Endian, 2 bytes = ushort)
                            byte[] serverDataBytes = Encoding.UTF8.GetBytes(serverData);
                            byte[] lengthBytes = new byte[2];
                            BinaryPrimitives.WriteUInt16BigEndian(lengthBytes, (ushort)serverDataBytes.Length);
                            bw.Write(lengthBytes);

                            // Server data string
                            bw.Write(serverDataBytes);

                            // Send the response
                            byte[] responsePacket = msWrite.ToArray();
                            
                            listener.Send(responsePacket, responsePacket.Length, clientEndPoint);
                            Console.WriteLine($"Response packet ({responsePacket.Length} bytes): {BitConverter.ToString(responsePacket)}");
                            Console.WriteLine($"Server data: {serverData}");
                            Console.WriteLine($"Server data length: {serverDataBytes.Length}");
                            Console.WriteLine(
                                $"Sent Unconnected Pong to {clientEndPoint.Address}:{clientEndPoint.Port}");
                        }
                    }
                    else if (datagram[0] == (byte)MessageIdentifiers.IdOpenConnectionRequest1 && datagram.Length > 0)
                    {
                        Console.WriteLine(">>> Received Open Connection Request 1!");
    
                        br.ReadByte(); // Skip packet ID (0x05)
                        byte[] receivedMagic = br.ReadBytes(16);
                        byte protocolVersion = br.ReadByte();
    
                        Console.WriteLine($"Protocol: {protocolVersion}, Magic Valid: {receivedMagic.SequenceEqual(MAGIC)}");
                        Console.WriteLine($"Client MTU: {datagram.Length}");
    
                        // Construct the answer (0x06 - Open Connection Reply 1)
                        using (MemoryStream msWrite = new MemoryStream())
                        using (BinaryWriter bw = new BinaryWriter(msWrite))
                        {
                            // Packet ID (0x06)
                            bw.Write((byte)MessageIdentifiers.IdOpenConnectionReply1);
        
                            // Magic (16 bytes)
                            bw.Write(MAGIC);
        
                            // Server GUID (Big Endian, 8 bytes)
                            byte[] guidBytes = new byte[8];
                            BinaryPrimitives.WriteInt64BigEndian(guidBytes, (long)SERVER_ID);
                            bw.Write(guidBytes);
        
                            // Use Security (1 byte) - 0x00 = false
                            bw.Write((byte)0x00);
        
                            // MTU Size (Big Endian, 2 bytes)
                            ushort mtuSize = (ushort)(datagram.Length); // Use the MTU of the client
                            byte[] mtuBytes = new byte[2];
                            BinaryPrimitives.WriteUInt16BigEndian(mtuBytes, mtuSize);
                            bw.Write(mtuBytes);
        
                            // Send the packet
                            byte[] responsePacket = msWrite.ToArray();
                            listener.Send(responsePacket, responsePacket.Length, clientEndPoint);
        
                            Console.WriteLine($"Sent Open Connection Reply 1 ({responsePacket.Length} bytes)");
                            Console.WriteLine($"Response: {BitConverter.ToString(responsePacket)}");
                        }
                    }
                    else if (datagram[0] == (byte)MessageIdentifiers.IdOpenConnectionRequest2 && datagram.Length > 0)
                    {
                        Console.WriteLine(">>> Received Open Connection Request 2!");

                        br.ReadByte(); // Skip packet ID (0x07)

                        // Read MTU
                        ushort mtuSize = BinaryPrimitives.ReadUInt16BigEndian(br.ReadBytes(2));

                        // Read client GUID
                        long clientGuid = BinaryPrimitives.ReadInt64BigEndian(br.ReadBytes(8));

                        Console.WriteLine($"MTU: {mtuSize}, Client GUID: {clientGuid}");

                        // Build response (0x08 - Open Connection Reply 2)
                        using (MemoryStream msWrite = new MemoryStream())
                        using (BinaryWriter bw = new BinaryWriter(msWrite))
                        {
                            // Packet ID (0x08)
                            bw.Write((byte)MessageIdentifiers.IdOpenConnectionReply2);

                            // Magic (16 bytes)
                            bw.Write(MAGIC);

                            // Server GUID (Big Endian, 8 bytes)
                            byte[] guidBytes = new byte[8];
                            BinaryPrimitives.WriteInt64BigEndian(guidBytes, (long)SERVER_ID);
                            bw.Write(guidBytes);

                            // Client Address (Version + IP + Port)
                            bw.Write((byte)4); // IPv4

                            // IP Address (4 bytes, inverted with ~)
                            byte[] ipBytes = clientEndPoint.Address.GetAddressBytes();
                            foreach (byte b in ipBytes)
                            {
                                bw.Write((byte)~b);
                            }

                            // Port (Big Endian, 2 bytes)
                            byte[] portBytes = new byte[2];
                            BinaryPrimitives.WriteUInt16BigEndian(portBytes, (ushort)clientEndPoint.Port);
                            bw.Write(portBytes);

                            // MTU Size (Big Endian, 2 bytes)
                            byte[] mtuBytes = new byte[2];
                            BinaryPrimitives.WriteUInt16BigEndian(mtuBytes, mtuSize);
                            bw.Write(mtuBytes);

                            // Security (1 byte) - 0x00 = false
                            bw.Write((byte)0x00);

                            // Send packet
                            byte[] responsePacket = msWrite.ToArray();
                            listener.Send(responsePacket, responsePacket.Length, clientEndPoint);

                            Console.WriteLine($"Sent Open Connection Reply 2 ({responsePacket.Length} bytes)");
                            Console.WriteLine($"Response: {BitConverter.ToString(responsePacket)}");
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            Console.WriteLine(e.StackTrace);
        }
        finally
        {
            listener.Close();
        }
    }
}