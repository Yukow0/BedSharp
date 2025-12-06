using System.Buffers.Binary;

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
    
}