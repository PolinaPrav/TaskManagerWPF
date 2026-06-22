using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using TaskManager.Library;

namespace TaskManager.Library.Services
{
    public class Storage
    {
        public void SaveToJson(List<Task> tasks, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(filePath, json);
        }

        public List<Task> LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл {filePath} не найден");

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>();
        }

        public void SaveToXml(List<Task> tasks, string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Task>));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, tasks);
        }

        public List<Task> LoadFromXml(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл {filePath} не найден");

            var serializer = new XmlSerializer(typeof(List<Task>));
            using var reader = new StreamReader(filePath);
            return (List<Task>)serializer.Deserialize(reader) ?? new List<Task>();
        }

        public List<Task> LoadFromFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".json" => LoadFromJson(filePath),
                ".xml" => LoadFromXml(filePath),
                _ => throw new NotSupportedException($"Формат {extension} не поддерживается")
            };
        }

        public void SaveToFile(List<Task> tasks, string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".json":
                    SaveToJson(tasks, filePath);
                    break;
                case ".xml":
                    SaveToXml(tasks, filePath);
                    break;
                default:
                    throw new NotSupportedException($"Формат {extension} не поддерживается");
            }
        }
    }
}