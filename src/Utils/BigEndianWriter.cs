using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace BedSharp.Utils;

public class BigEndianWriter : BinaryWriter
{
    public BigEndianWriter(Stream stream) : base(stream)
    {
        
    }
    
    /// <summary>
    /// Writes a ulong in big endian format to the stream
    /// </summary>
    public void WriteBigEndian(ulong value)
    {
        // Using a Span for better performance
        Span<byte> bytes = stackalloc byte[8];
        BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
        Write(bytes); 
    }
    
    /// <summary>
    /// Write a ushort in big endian format to the stream
    /// </summary>
    public void WriteBigEndian(ushort value)
    {
        Span<byte> bytes = stackalloc byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        Write(bytes);
    }

    public void WriteClientAddress(IPEndPoint clientIp)
    {
        byte[] bytes = null;
        if (clientIp.Address.IsIPv4MappedToIPv6 || clientIp.AddressFamily == AddressFamily.InterNetwork)
        {
            bytes = new byte[7];
            bytes[0] = 4;
            clientIp.Address.MapToIPv4().GetAddressBytes().CopyTo(bytes, 1);
            BinaryPrimitives.WriteUInt16BigEndian(bytes.AsSpan(5), (UInt16)clientIp.Port);
        }
        else if (clientIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
        {
            bytes = new byte[29];
            bytes[0] = 6;
            short afInet6;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                afInet6 = 10; 
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                afInet6 = 23;
            }
            else 
            {
                afInet6 = 30;
            }

            BinaryPrimitives.WriteInt16LittleEndian(bytes.AsSpan(1), afInet6);
            BinaryPrimitives.WriteUInt16BigEndian(bytes.AsSpan(3), (UInt16)clientIp.Port);
            clientIp.Address.GetAddressBytes().CopyTo(bytes, 9);
        }
        Write(bytes);
    }
    
}