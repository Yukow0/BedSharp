
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BedSharp;

class Program
{
    public static void Main(string[] args)
    {
        int port = 19132;
        
        const long SERVER_ID = 123456;
        const short MTU_SIZE = 1492;
        
        string serverData = "MCPE;Serveur C# de Rieno;859;1.21.120;0;20;123456;BedSharp;Survival;";
        
        byte[] MAGIC = 
        {
            0x00, 0xFF, 0xFF, 0x00, 0xFE, 0xFE, 0xFE, 0xFE,
            0xFD, 0xFD, 0xFD, 0xFD, 0x12, 0x34, 0x56, 0x78
        };
        
        UdpClient listener = new UdpClient(port);

        Console.WriteLine($"Listening on port {port}..");

        IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            while (true)
            {
                byte[] datagram = listener.Receive(ref clientEndPoint);

                Console.WriteLine($"Received the packet from {clientEndPoint.Address} !");

                if (datagram.Length > 0 && datagram[0] == 0x01)
                {
                    Console.WriteLine($"Ping received: {datagram.Length} bytes from {clientEndPoint.Address}");
                    
                    long clientTimestamp = BitConverter.ToInt64(datagram, 1);
                    
                    using (MemoryStream stream = new MemoryStream())
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        writer.Write((byte)0x1c);
                        writer.Write(clientTimestamp);
                        writer.Write(SERVER_ID);
                        writer.Write(MAGIC);
                        
                        byte[] serverDataBytes = Encoding.UTF8.GetBytes(serverData);
                        writer.Write((short)serverDataBytes.Length);
                        writer.Write(serverDataBytes);
                        
                        byte[] response = stream.ToArray();
                        listener.Send(response, response.Length, clientEndPoint);
                        
                        Console.WriteLine($"Sent the packet to {clientEndPoint.Address}");
                    }
                }
                else if (datagram.Length > 0 && datagram[0] == 0x05)
                {
                    
                    
                    using (MemoryStream stream = new MemoryStream())
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        writer.Write((byte)0x06);
                        writer.Write(MAGIC);
                        writer.Write(IPAddress.HostToNetworkOrder(SERVER_ID));
                        writer.Write((byte)0x00);
                        writer.Write(IPAddress.HostToNetworkOrder(MTU_SIZE));
                        
                        byte[] response = stream.ToArray();
                        listener.Send(response, response.Length, clientEndPoint);

                        Console.WriteLine($"Connection step one reply completed, sent to {clientEndPoint.Address}");
                        
                    }
                }
                else if (datagram.Length > 0 && datagram[0] == 0x07)
                {
                    Console.WriteLine($"Connection step two received from {clientEndPoint.Address}");
                    
                    using (MemoryStream stream = new MemoryStream())
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        writer.Write((byte)0x08);
                        writer.Write(MAGIC);
                        writer.Write(IPAddress.HostToNetworkOrder(SERVER_ID));
                        stream.Write(clientEndPoint.Address.GetAddressBytes());
                        writer.Write(IPAddress.HostToNetworkOrder((short)clientEndPoint.Port));
                        
                        writer.Write(IPAddress.HostToNetworkOrder(MTU_SIZE));
                        
                        writer.Write((byte)0x00);
                        
                        byte[] response = stream.ToArray();
                        listener.Send(response, response.Length, clientEndPoint);

                        Console.WriteLine($"Connection step two reply completed, sent to {clientEndPoint.Address}");
                    }
                }
                else
                {
                    Console.WriteLine($"[INFO] Paquet inconnu reçu (ID: 0x{datagram[0]:X2})");
                }

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            listener.Close();
        }


    }
}