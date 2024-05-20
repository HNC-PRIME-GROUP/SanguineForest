using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.IO;

namespace Sanguine_Forest
{
    /// <summary>
    /// It loads data from json and create instance for curent scene
    /// </summary>
    static class FileLoader
    {

        public static string RootFolder;

        public static void SaveToJson<T>(T obj, string filePath)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static T LoadFromJson<T>(string filePath) 
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

    }
}
