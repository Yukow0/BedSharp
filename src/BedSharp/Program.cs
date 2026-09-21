using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Text;
using BedSharp.Protocols.RakNet;

namespace BedSharp.BedSharp;

class Program
{
    public static void Main(string[] args)
    {
        Engine engine = new Engine();
        engine.Start();
    }
}