using System.Buffers.Binary;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using BedSharp.Protocols.RakNet;
using BedSharp.Protocols.RakNet.Packets;

namespace BedSharp.BedSharp;

public class Engine
{
    private readonly int _port;
    private readonly ulong _serverId;
    private readonly string _serverData;
    private readonly byte[] _magic;
    private readonly UdpClient _listener;
    private readonly Random _rnd = new Random();
    private readonly string _motd;
    private readonly int _maxPlayers;
    private IPEndPoint _clientEndPoint;
    
    
    public Engine(int port, string motd, int maxPlayers)
    {
        _port = port;
        _motd = motd;
        _maxPlayers = maxPlayers;
        _magic = ServerInfo.Magic;
        _listener = new UdpClient(port);
        _serverId = ((ulong)_rnd.Next() << 32) | (uint)_rnd.Next();
        _serverData = ServerInfo.GetServerData(_serverId, _motd, _maxPlayers);
        _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
        
    }

    public void Start()
    {
        try
        {
            while (true)
            {
                byte[] datagram = _listener.Receive(ref _clientEndPoint);
                
                Console.WriteLine($"\n========================================");
                Console.WriteLine(
                    $"Received {datagram.Length} bytes from {_clientEndPoint.Address}:{_clientEndPoint.Port}");
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
                        
                        byte[] responsePacket = PingPacket.SendPong(clientTime, _clientEndPoint, _serverId, _serverData);
                        _listener.Send(responsePacket, responsePacket.Length, _clientEndPoint);
                        Console.WriteLine($"Response packet ({responsePacket.Length} bytes): {BitConverter.ToString(responsePacket)}");
                        Console.WriteLine($"Server data: {_serverData}");
                        Console.WriteLine($"Sent Unconnected Pong to {_clientEndPoint.Address}:{_clientEndPoint.Port}");
                    }
                }
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            _listener.Close();
        }
    }
}