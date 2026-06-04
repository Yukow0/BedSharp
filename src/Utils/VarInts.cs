using System.Numerics;

namespace BedSharp.Utils;

public class VarInts
{
    private static readonly long Long7F = 0x7FL;
    private static readonly long Long80 = 0x80L;

    public static int WriteInt(Span<byte> buffer, int value)
    {
        return Encode(buffer, ((value << 1) ^ (value >> 31)) & 0xFFFFFFFFL);
    }

    public static (int value, int nextOffset) ReadInt(Span<byte> buffer, int offset = 0)
    {
        (long value, int offset) values = Decode(buffer, 32, offset);
        int n = (int)values.value;
        return ((n >>> 1) ^ -(n & 1), values.offset);
    }

    public static int WriteUnsignedInt(Span<byte> buffer, int value)
    {
        return Encode(buffer, value & 0xFFFFFFFFL);
    }
    
    public static (int value, int nextOffset) ReadUnsignedInt(Span<byte> buffer, int offset = 0)
    {
        (long value, int offset) values = Decode(buffer, 32, offset);
        return ((int)values.value, values.offset);
    }
    
    public static int WriteLong(Span<byte> buffer, long value)
    {
        return Encode(buffer, (value << 1) ^ (value >> 63));
    }
    
    public static (long value, int nextOffset) ReadLong(Span<byte> buffer, int offset = 0)
    {
        (long value, int offset) values = Decode(buffer, 64, offset);
        long n = values.value;
        return ((n >>> 1) ^ -(n & 1), values.offset);
    }
    
    public static int WriteUnsignedLong(Span<byte> buffer, long value)
    {
        return Encode(buffer, value);
    }
    
    public static (long value, int nextOffset) ReadUnsignedLong(Span<byte> buffer, int offset = 0)
    {
        (long value, int offset) values = Decode(buffer, 64, offset);
        return (values.value, values.offset);
    }

    private static int Encode(Span<byte> buffer, long value)
    {
        if ((value & ~0x7FL) == 0)
        {
            buffer[0] = (byte)value;
            return 1;
        }
        else if ((value & ~0x3FFFL) == 0)
        {
            int number = (int)((value & 0x7FL | 0x80L) << 8 | (value >>> 7));
            buffer[0] = (byte)(number >> 8);
            buffer[1] = (byte)(number);
            return 2;
        }
        else
        {
            return EncodeFull(buffer, value);
        }
    }

    private static int EncodeFull(Span<byte> buffer, long value)
    {
        if ((value & ~0x1FFFFFL) == 0)
        {
            int w = (int)((value & 0x7FL | 0x80L) << 16 |
                         ((value >>> 7) & 0x7FL | 0x80L) << 8 |
                         (value >>> 14));
            buffer[0] = (byte)(w >> 16);
            buffer[1] = (byte)(w >> 8);
            buffer[2] = (byte)(w);
            return 3;
        }
        else if ((value & ~0xFFFFFFFL) == 0)
        {
            int w = (int)((value & 0x7F | 0x80) << 24 |
                         ((value >>> 7) & 0x7F | 0x80) << 16 |
                         ((value >>> 14) & 0x7F | 0x80) << 8 |
                         (value >>> 21));
            buffer[0] = (byte)(w >> 24);
            buffer[1] = (byte)(w >> 16);
            buffer[2] = (byte)(w >> 8);
            buffer[3] = (byte)(w);
            return 4;
        }
        else if ((value & ~0x7FFFFFFFFL) == 0)
        {
            int w = (int)((value & 0x7F | 0x80) << 24 |
                         ((value >>> 7) & 0x7F | 0x80) << 16 |
                         ((value >>> 14) & 0x7F | 0x80) << 8 |
                         ((value >>> 21) & 0x7F | 0x80));
            buffer[0] = (byte)(w >> 24);
            buffer[1] = (byte)(w >> 16);
            buffer[2] = (byte)(w >> 8);
            buffer[3] = (byte)(w);
            buffer[4] = (byte)(value >>> 28);
            return 5;
        }
        else if ((value & ~0x3FFFFFFFFFFL) == 0)
        {
            int w = (int)((value & 0x7F | 0x80) << 24 |
                         ((value >>> 7) & 0x7F | 0x80) << 16 |
                         ((value >>> 14) & 0x7F | 0x80) << 8 |
                         ((value >>> 21) & 0x7F | 0x80));
            int w2 = (int)(((value >>> 28) & 0x7FL | 0x80L) << 8 |
                          (value >>> 35));
            buffer[0] = (byte)(w >> 24);
            buffer[1] = (byte)(w >> 16);
            buffer[2] = (byte)(w >> 8);
            buffer[3] = (byte)(w);
            buffer[4] = (byte)(w2 >> 8);
            buffer[5] = (byte)(w2);
            return 6;
        }
        else if ((value & ~0x1FFFFFFFFFFFFL) == 0)
        {
            int w = (int)((value & 0x7F | 0x80) << 24 |
                         ((value >>> 7) & 0x7F | 0x80) << 16 |
                         ((value >>> 14) & 0x7F | 0x80) << 8 |
                         ((value >>> 21) & 0x7F | 0x80));
            int w2 = (int)((((value >>> 28) & 0x7FL | 0x80L) << 16 |
                           ((value >>> 35) & 0x7FL | 0x80L) << 8) |
                           (value >>> 42));
            buffer[0] = (byte)(w >> 24);
            buffer[1] = (byte)(w >> 16);
            buffer[2] = (byte)(w >> 8);
            buffer[3] = (byte)(w);
            buffer[4] = (byte)(w2 >> 16);
            buffer[5] = (byte)(w2 >> 8);
            buffer[6] = (byte)(w2);
            return 7;
        }
        else if ((value & ~0xFFFFFFFFFFFFFFL) == 0)
        {
            long w = (value & 0x7F | 0x80) << 56 |
                    ((value >>> 7) & 0x7F | 0x80) << 48 |
                    ((value >>> 14) & 0x7F | 0x80) << 40 |
                    ((value >>> 21) & 0x7F | 0x80) << 32 |
                    ((value >>> 28) & 0x7FL | 0x80L) << 24 |
                    ((value >>> 35) & 0x7FL | 0x80L) << 16 |
                    ((value >>> 42) & 0x7FL | 0x80L) << 8 |
                    (value >>> 49);
            buffer[0] = (byte)(w >> 56);
            buffer[1] = (byte)(w >> 48);
            buffer[2] = (byte)(w >> 40);
            buffer[3] = (byte)(w >> 32);
            buffer[4] = (byte)(w >> 24);
            buffer[5] = (byte)(w >> 16);
            buffer[6] = (byte)(w >> 8);
            buffer[7] = (byte)(w);
            return 8;
        }
        else if ((value & ~0x7FFFFFFFFFFFFFFFL) == 0)
        {
            long w = (value & 0x7F | 0x80) << 56 |
                    ((value >>> 7) & 0x7F | 0x80) << 48 |
                    ((value >>> 14) & 0x7F | 0x80) << 40 |
                    ((value >>> 21) & 0x7F | 0x80) << 32 |
                    ((value >>> 28) & 0x7FL | 0x80L) << 24 |
                    ((value >>> 35) & 0x7FL | 0x80L) << 16 |
                    ((value >>> 42) & 0x7FL | 0x80L) << 8 |
                    ((value >>> 49) & 0x7FL | 0x80L);
            buffer[0] = (byte)(w >> 56);
            buffer[1] = (byte)(w >> 48);
            buffer[2] = (byte)(w >> 40);
            buffer[3] = (byte)(w >> 32);
            buffer[4] = (byte)(w >> 24);
            buffer[5] = (byte)(w >> 16);
            buffer[6] = (byte)(w >> 8);
            buffer[7] = (byte)(w);
            buffer[8] = (byte)(value >>> 56);
            return 9;
        }
        else
        {
            long w = (value & 0x7F | 0x80) << 56 |
                    ((value >>> 7) & 0x7F | 0x80) << 48 |
                    ((value >>> 14) & 0x7F | 0x80) << 40 |
                    ((value >>> 21) & 0x7F | 0x80) << 32 |
                    ((value >>> 28) & 0x7FL | 0x80L) << 24 |
                    ((value >>> 35) & 0x7FL | 0x80L) << 16 |
                    ((value >>> 42) & 0x7FL | 0x80L) << 8 |
                    ((value >>> 49) & 0x7FL | 0x80L);
            long w2 = ((value >>> 56) & 0x7FL | 0x80L) << 8 |
                      (value >>> 63);
            buffer[0] = (byte)(w >> 56);
            buffer[1] = (byte)(w >> 48);
            buffer[2] = (byte)(w >> 40);
            buffer[3] = (byte)(w >> 32);
            buffer[4] = (byte)(w >> 24);
            buffer[5] = (byte)(w >> 16);
            buffer[6] = (byte)(w >> 8);
            buffer[7] = (byte)(w);
            buffer[8] = (byte)(w2 >> 8);
            buffer[9] = (byte)(w2);
            return 10;
        }
    }

    private static (long value, int offset) Decode(Span<byte> buffer, int maxBits, int offset)
    {
        long result = 0;
        for (int shift = 0; shift < maxBits; shift += 7)
        {
            byte b = buffer[offset];
            offset++;
            result |= (b & 0x7FL) << shift;
            if ((b & 0x80) == 0)
            {
                return (result, offset);
            }
        }
        throw new InvalidOperationException("VarInt too big");
    }

    public static int WriteUnsignedBigVarInt(Span<byte> buffer, long value)
    {
        int offset = 0;
        while (true)
        {
            long bits = value & Long7F;
            value >>= 7;
            if (value == 0)
            {
                buffer[offset] = (byte)bits;
                return offset + 1;
            }
            buffer[offset] = (byte)(bits | Long80);
            offset++;
        }
    }

    public static (long value, int nextOffset) ReadUnsignedBigVarInt(Span<byte> buffer, int maxBits, int offset = 0)
    {
        long value = 0;
        int shift = 0;
        while (true)
        {
            if (shift >= maxBits)
            {
                throw new InvalidOperationException("VarInt too big");
            }
            byte b = buffer[offset];
            offset++;
            value |= (b & 0x7FL) << shift;
            if ((b & 0x80) == 0)
            {
                return (value, offset);
            }
            shift += 7;
        }
    }
}
