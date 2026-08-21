namespace BedSharp.Utils;

public class Config
{
    public static async Task CreateFile()
    {
        if (!File.Exists("server.properties"))
        {
            var file = File.Create("server.properties");
            await file.DisposeAsync();
            List<string> lines = new List<string>()
            {
                "server-port=19132",
                "server-ip=",
                "max-players=20"
            };
            foreach (string line in lines)
            {
                await File.AppendAllTextAsync("server.properties", line + "\n");
            }
        }
    }
    
    public static async Task<Dictionary<string, string>> LoadFile()
    {
        await CreateFile();
        Dictionary<string, string> config = new Dictionary<string, string>();
        foreach (string line in File.ReadAllLines("server.properties"))
        {
            string part1 = line.Split('=')[0];
            string part2 = line.Split('=')[1];
            
            config.Add(part1, part2);
        }
        return config;
    }
}