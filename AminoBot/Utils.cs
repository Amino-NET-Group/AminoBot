using System.Text.Json;

namespace AminoBot
{
    public class Utils
    {

        public const string APP_ROOT = "app_config";


        public Task CheckFiles()
        {
            if(!Directory.Exists(APP_ROOT)) { CreateFiles(); }
            if(!File.Exists($"{APP_ROOT}/config.json")) { CreateFiles(); }
            if (!File.Exists($"{APP_ROOT}/webDevs.txt")) { CreateFiles(); }
            return Task.CompletedTask;
        }

        private Task CreateFiles()
        {
            Directory.CreateDirectory(APP_ROOT);
            using (StreamWriter sw = File.CreateText($"{APP_ROOT}/config.json"))
            {
                sw.Write(JsonSerializer.Serialize(new Objects.Config(), options: new() { WriteIndented = true }));
            }
            File.CreateText($"{APP_ROOT}/webDevs.txt").Close();
            return Task.CompletedTask;
        }

        public Objects.Config GetConfig() => JsonSerializer.Deserialize<Objects.Config>(File.ReadAllText($"{APP_ROOT}/config.json"))!;
        public List<string> GetWebDeviceIds() => File.ReadAllLines($"{APP_ROOT}/webDevs.txt").ToList();
    }
}
