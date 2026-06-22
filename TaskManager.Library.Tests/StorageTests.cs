using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Library;
using TaskManager.Library.Services;

namespace TaskManager.Library.Tests
{
    [TestClass]
    public class StorageTests
    {
        private Storage _storage;
        private string _testJsonPath;
        private string _testXmlPath;
        private List<Task> _testTasks;

        [TestInitialize]
        public void Setup()
        {
            _storage = new Storage();

            var guid = Guid.NewGuid().ToString().Substring(0, 8);
            _testJsonPath = Path.Combine(Path.GetTempPath(), $"test_tasks_{guid}.json");
            _testXmlPath = Path.Combine(Path.GetTempPath(), $"test_tasks_{guid}.xml");

            _testTasks = new List<Task>
            {
                new Task("Task1", "Description1", Task.Priority1.Средний,
                        DateTime.Now.AddDays(1), Task.Status1.Новая, true),
                new Task("Task2", "Description2", Task.Priority1.Высокий,
                        DateTime.Now.AddDays(2), Task.Status1.ВПроцессе, false)
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                if (File.Exists(_testJsonPath))
                    File.Delete(_testJsonPath);
                if (File.Exists(_testXmlPath))
                    File.Delete(_testXmlPath);
            }
            catch 
            { 
            }
        }

        [TestMethod]
        public void SaveToJson_ShouldCreateFile()
        {
            _storage.SaveToJson(_testTasks, _testJsonPath);

            Assert.IsTrue(File.Exists(_testJsonPath), $"Файл {_testJsonPath} не создан");
            Assert.IsTrue(new FileInfo(_testJsonPath).Length > 0, "Файл пустой");
        }

        [TestMethod]
        public void LoadFromJson_ShouldLoadCorrectTasks()
        {
            _storage.SaveToJson(_testTasks, _testJsonPath);
            var result = _storage.LoadFromJson(_testJsonPath);

            Assert.AreEqual(2, result.Count, "Загружено неверное количество задач");
            Assert.AreEqual(_testTasks[0].Name, result[0].Name);
            Assert.AreEqual(_testTasks[0].Description, result[0].Description);
            Assert.AreEqual(_testTasks[0].Priority, result[0].Priority);
            Assert.AreEqual(_testTasks[0].Status, result[0].Status);
            Assert.AreEqual(_testTasks[0].IsImportant, result[0].IsImportant);
            Assert.AreEqual(_testTasks[1].Name, result[1].Name);
        }

        [TestMethod]
        public void LoadFromJson_NonExistentFile_ShouldThrowFileNotFoundException()
        {
            var nonExistentPath = Path.Combine(Path.GetTempPath(), $"nonexistent_{Guid.NewGuid()}.json");

            try
            {
                _storage.LoadFromJson(nonExistentPath);
                Assert.Fail("Ожидалось FileNotFoundException");
            }
            catch (FileNotFoundException)
            {
            }
        }

        [TestMethod]
        public void SaveToXml_ShouldCreateFile()
        {
            _storage.SaveToXml(_testTasks, _testXmlPath);

            Assert.IsTrue(File.Exists(_testXmlPath), $"Файл {_testXmlPath} не создан");
            Assert.IsTrue(new FileInfo(_testXmlPath).Length > 0, "Файл пустой");
        }

        [TestMethod]
        public void LoadFromXml_ShouldLoadCorrectTasks()
        {
            _storage.SaveToXml(_testTasks, _testXmlPath);
            var result = _storage.LoadFromXml(_testXmlPath);

            Assert.AreEqual(2, result.Count, "Загружено неверное количество задач");
            Assert.AreEqual(_testTasks[0].Name, result[0].Name);
            Assert.AreEqual(_testTasks[0].Description, result[0].Description);
            Assert.AreEqual(_testTasks[0].Priority, result[0].Priority);
            Assert.AreEqual(_testTasks[0].Status, result[0].Status);
            Assert.AreEqual(_testTasks[0].IsImportant, result[0].IsImportant);
            Assert.AreEqual(_testTasks[1].Name, result[1].Name);
        }

        [TestMethod]
        public void LoadFromXml_NonExistentFile_ShouldThrowFileNotFoundException()
        {
            var nonExistentPath = Path.Combine(Path.GetTempPath(), $"nonexistent_{Guid.NewGuid()}.xml");

            try
            {
                _storage.LoadFromXml(nonExistentPath);
                Assert.Fail("Ожидалось FileNotFoundException");
            }
            catch (FileNotFoundException)
            {
            }
        }

        [TestMethod]
        public void LoadFromFile_JsonFile_ShouldLoadCorrectly()
        {
            _storage.SaveToJson(_testTasks, _testJsonPath);
            var result = _storage.LoadFromFile(_testJsonPath);

            Assert.AreEqual(2, result.Count, "Загружено неверное количество задач");
            Assert.AreEqual(_testTasks[0].Name, result[0].Name);
        }

        [TestMethod]
        public void LoadFromFile_XmlFile_ShouldLoadCorrectly()
        {
            _storage.SaveToXml(_testTasks, _testXmlPath);
            var result = _storage.LoadFromFile(_testXmlPath);

            Assert.AreEqual(2, result.Count, "Загружено неверное количество задач");
            Assert.AreEqual(_testTasks[0].Name, result[0].Name);
        }

        [TestMethod]
        public void LoadFromFile_UnknownExtension_ShouldThrowNotSupportedException()
        {
            var invalidPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.txt");

            try
            {
                _storage.LoadFromFile(invalidPath);
                Assert.Fail("Ожидалось NotSupportedException");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void SaveToFile_Json_ShouldCreateJsonFile()
        {
            _storage.SaveToFile(_testTasks, _testJsonPath);

            Assert.IsTrue(File.Exists(_testJsonPath), $"Файл {_testJsonPath} не создан");
            Assert.IsTrue(_testJsonPath.EndsWith(".json"), "Файл должен иметь расширение .json");
        }

        [TestMethod]
        public void SaveToFile_Xml_ShouldCreateXmlFile()
        {
            _storage.SaveToFile(_testTasks, _testXmlPath);

            Assert.IsTrue(File.Exists(_testXmlPath), $"Файл {_testXmlPath} не создан");
            Assert.IsTrue(_testXmlPath.EndsWith(".xml"), "Файл должен иметь расширение .xml");
        }

        [TestMethod]
        public void SaveToFile_UnknownExtension_ShouldThrowNotSupportedException()
        {
            var invalidPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.txt");

            try
            {
                _storage.SaveToFile(_testTasks, invalidPath);
                Assert.Fail("Ожидалось NotSupportedException");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void SaveAndLoad_ShouldPreserveAllProperties()
        {
            var now = DateTime.Now;
            var task = new Task("Test", "Description", Task.Priority1.Высокий,
                               now.AddDays(5), Task.Status1.ВПроцессе, true);
            var tasks = new List<Task> { task };

            _storage.SaveToJson(tasks, _testJsonPath);
            var result = _storage.LoadFromJson(_testJsonPath);

            var loaded = result[0];
            Assert.AreEqual(task.Name, loaded.Name);
            Assert.AreEqual(task.Description, loaded.Description);
            Assert.AreEqual(task.Priority, loaded.Priority);
            Assert.AreEqual(task.Status, loaded.Status);
            Assert.AreEqual(task.IsImportant, loaded.IsImportant);
            Assert.IsTrue(Math.Abs((task.DueDate - loaded.DueDate).TotalSeconds) < 1,
                $"Даты не совпадают: ожидалось {task.DueDate}, получено {loaded.DueDate}");
        }
    }
}