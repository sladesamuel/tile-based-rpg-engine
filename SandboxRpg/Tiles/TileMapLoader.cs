using System;
using System.IO;
using Newtonsoft.Json;

namespace SandboxRpg.Tiles
{
    public static class TileMapLoader
    {
        public static TileMap LoadFrom(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Tile map path must be given", nameof(path));

            string content = ReadContent(path);
            return JsonConvert.DeserializeObject<TileMap>(content, new JsonSerializerSettings
            {

            });
        }

        private static string ReadContent(string path)
        {
            using (var file = File.OpenRead(path))
            using (var reader = new StreamReader(file))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
