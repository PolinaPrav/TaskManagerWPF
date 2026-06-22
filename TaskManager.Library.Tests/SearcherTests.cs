using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Library;
using TaskManager.Library.Services;

namespace TaskManager.Library.Tests
{
    [TestClass]
    public class SearcherTests
    {
        private Searcher _searcher;
        private List<Task> _tasks;

        [TestInitialize]
        public void Setup()
        {
            _searcher = new Searcher();
            _tasks = new List<Task>
            {
                new Task("Buy milk", "Go to store and buy milk", Task.Priority1.Средний, DateTime.Now.AddDays(1), Task.Status1.Новая),
                new Task("Write code", "Write unit tests for library", Task.Priority1.Высокий, DateTime.Now.AddDays(2), Task.Status1.ВПроцессе),
                new Task("Read book", "Read C# programming book", Task.Priority1.Низкий, DateTime.Now.AddDays(3), Task.Status1.Завершена)
            };
        }

        [TestMethod]
        public void Search_ShouldFindTasksByTitle()
        {
            var result = _searcher.Search(_tasks, "milk");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Buy milk", result[0].Name);
        }

        [TestMethod]
        public void Search_ShouldFindTasksByDescription()
        {
            var result = _searcher.Search(_tasks, "unit tests");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Write code", result[0].Name);
        }

        [TestMethod]
        public void Search_ShouldBeCaseInsensitive()
        {
            var result = _searcher.Search(_tasks, "MILK");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Buy milk", result[0].Name);
        }

        [TestMethod]
        public void Search_EmptyQuery_ShouldReturnAllTasks()
        {
            var result = _searcher.Search(_tasks, "");

            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void Search_WhitespaceQuery_ShouldReturnAllTasks()
        {
            var result = _searcher.Search(_tasks, "   ");

            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void Search_NoMatches_ShouldReturnEmpty()
        {
            var result = _searcher.Search(_tasks, "nonexistent");

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void SearchByExactName_ShouldFindExactMatch()
        {
            var result = _searcher.SearchByExactName(_tasks, "Buy milk");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Buy milk", result[0].Name);
        }

        [TestMethod]
        public void SearchByExactName_ShouldBeCaseInsensitive()
        {
            var result = _searcher.SearchByExactName(_tasks, "BUY MILK");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Buy milk", result[0].Name);
        }

        [TestMethod]
        public void Search_EmptyTaskList_ShouldReturnEmpty()
        {
            var result = _searcher.Search(new List<Task>(), "test");

            Assert.AreEqual(0, result.Count);
        }
    }
}