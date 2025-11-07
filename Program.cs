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
        const short MTU_SIZE = 1492;
        
        Console.WriteLine($"Server ID: {SERVER_ID}");
        string serverData = $"MCPE;Serveur C# de Rieno;859;1.21.120;0;20;{SERVER_ID};BedSharp;Survival;1;19132;19133;";
        
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

                Console.WriteLine($"\n========================================");
                Console.WriteLine($"Received {datagram.Length} bytes from {clientEndPoint.Address}:{clientEndPoint.Port}");
                Console.WriteLine($"Packet ID: 0x{datagram[0]:X2}");
                Console.WriteLine($"Full packet: {BitConverter.ToString(datagram)}");
                Console.WriteLine($"========================================");

                if (datagram[0] == 0x01 && datagram.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(datagram))
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    using (BinaryReader br = new BinaryReader(ms))
                    {
                        br.ReadByte(); // Skip the packet ID
                        
                        //Read the timestamp
                        long clientTime = BinaryPrimitives.ReadInt64BigEndian(br.ReadBytes(8));
                        
                        //Read and verify the magic
                        byte[] receivedMagic = br.ReadBytes(16);
                        bool magicValid = receivedMagic.SequenceEqual(MAGIC);

                        Console.WriteLine($"Client Time: {clientTime}");
                        //Packet ID
                        bw.Write((byte)0x1c);
                        
                        //TimeStamp of the client
                        byte[] timeBytes = BitConverter.GetBytes(clientTime);

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