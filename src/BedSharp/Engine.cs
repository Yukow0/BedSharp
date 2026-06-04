using System.Buffers.Binary;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using BedSharp.Protocols.RakNet;
using BedSharp.Protocols.RakNet.Packets;
using BedSharp.Utils;

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
        _listener = new UdpClient(AddressFamily.InterNetworkV6);
        _listener.Client.DualMode = true;
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.IPv6Any, port);
        _listener.Client.Bind(localEndPoint);
        _serverId = ((ulong)_rnd.Next() << 32) | (uint)_rnd.Next();
        _serverData = ServerInfo.GetServerData(_serverId, _motd, _maxPlayers);
        _clientEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
    }

    public void Start()
    {
        try
        {
            while (true)
            {
                byte[] datagram = _listener.Receive(ref _clientEndPoint);
                Console.WriteLine($"Received {datagram.Length} bytes from {_clientEndPoint}");
                Console.WriteLine($"Datagram: {BitConverter.ToString(datagram)}");
                
               
                using (MemoryStream msRead = new MemoryStream(datagram))
                using (BinaryReader br = new BinaryReader(msRead))
                {
                    if (datagram[0] == (byte)MessageIdentifiers.IdUnconnectedPing)
                    {
                        Console.WriteLine("Ping received");
                        br.ReadByte(); 
                        
                        long clientTime = BinaryPrimitives.ReadInt64BigEndian(br.ReadBytes(8));
                        
                        byte[] responsePacket = PingPacket.SendPong(clientTime, _clientEndPoint, _serverId, _serverData);
                        _listener.Send(responsePacket, responsePacket.Length, _clientEndPoint);
                        
                    }
                    else if (datagram[0] == (byte)MessageIdentifiers.IdOpenConnectionRequest1)
                    {
                        Console.WriteLine("OpenConnectionRequest1 received");
                        br.ReadByte();

                        byte[] responsePacket = OpenConnectionReply1.SendOpenConnection(_serverId);
                        _listener.Send(responsePacket, responsePacket.Length, _clientEndPoint);
                        

                    }
                    else if (datagram[0] == (byte)MessageIdentifiers.IdOpenConnectionRequest2)
                    {
                        br.ReadByte();
                        

                        byte[] responsePacket =
                            OpenConnectionReply2.SendOpenConnection(_serverId, _clientEndPoint);
                        _listener.Send(responsePacket, responsePacket.Length, _clientEndPoint);
                    }

                    if (datagram[0] >= 0x80 && datagram[0] <= 0x8F)
                    { 
                        byte frameSet = br.ReadByte();
                        msRead.Seek(3, SeekOrigin.Current);
                        byte reliabilityFlag =  br.ReadByte();
                        int importantInfo = reliabilityFlag >> 5;
                        int totalJump = 0;
                        
                        switch (importantInfo)
                        {
                            case 0:
                                break;
                            case 1:
                                totalJump = 3;
                                break;
                            case 2:
                                totalJump = 3;
                                break;
                            case 3:
                                totalJump = 7;
                                break;
                            case 4:
                                totalJump = 6;
                                break;
                        }

                        if ((reliabilityFlag & 0x10) != 0)
                        {
                            totalJump += 12;
                        }

                        totalJump += 2;

                        msRead.Seek(totalJump, SeekOrigin.Current);

                        
                        byte innerPacketId = br.ReadByte();
                        
                        Console.Write(innerPacketId);
                        if (innerPacketId == (byte)MessageIdentifiers.IdConnectionRequest)
                        {
                            long timestamp = (long)br.ReadUInt64();
                            byte[] responsePacket = PacketEncapsulater.FrameSetPacketGenerate(
                                ConnectionRequestAccepted.SendConnectionRequestAccepted(_clientEndPoint, timestamp), 1,
                                0x40);
                            _listener.Send(responsePacket, responsePacket.Length, _clientEndPoint);
                        }
                        if (innerPacketId == (byte)MessageIdentifiers.IdNewIncomingConnection)
                        {
                            byte serverAddrType = br.ReadByte();
                            
                            msRead.Seek(serverAddrType == 4 ? 6 : 28, SeekOrigin.Current);
                            
                            for (int i = 0; i < 10; i++)
                            {
                                byte clientAddrType = br.ReadByte();
                                msRead.Seek(clientAddrType == 4 ? 6 : 28, SeekOrigin.Current);
                            }

                            
                            ulong clientSendTime = br.ReadUInt64();
                            ulong serverSendTime = br.ReadUInt64();
                            
                        }
                        else if (innerPacketId == (byte)MessageIdentifiers.IdConnectedPing)
                        {
                            UInt64 timestampClient = br.ReadUInt64();

                            byte[] responsePacket =
                                PacketEncapsulater.FrameSetPacketGenerate(
                                    ConnectedPong.SendConnectedPong(timestampClient), 2, 0x40);
                            
                            _listener.Send(responsePacket, responsePacket.Length, _clientEndPoint);
                        }
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