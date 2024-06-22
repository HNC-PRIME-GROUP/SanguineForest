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

        public static void SaveToJson<T>(T obj, string relativeFilePath)
        {
            string fullPath = Path.Combine(RootFolder, relativeFilePath);
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(fullPath, json);
        }

        public static T LoadFromJson<T>(string relativeFilePath)
        {
            string fullPath = Path.Combine(RootFolder, relativeFilePath);
            string json = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static void DeleteFile(string relativeFilePath)
        {
            string fullPath = Path.Combine(RootFolder, relativeFilePath);
            File.Delete(fullPath);
        }

    }
}
