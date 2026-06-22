using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Library;
using TaskManager.Library.Services;

namespace TaskManager.Library.Tests
{
    [TestClass]
    public class FilterTests
    {
        private Filter _filter;
        private List<Task> _tasks;

        [TestInitialize]
        public void Setup()
        {
            _filter = new Filter();
            _tasks = new List<Task>
            {
                new Task("Task1", "Desc1", Task.Priority1.Средний, DateTime.Now.AddDays(1), Task.Status1.Новая),
                new Task("Task2", "Desc2", Task.Priority1.Высокий, DateTime.Now.AddDays(2), Task.Status1.ВПроцессе, true),
                new Task("Task3", "Desc3", Task.Priority1.Низкий, DateTime.Now.AddDays(3), Task.Status1.Завершена),
                new Task("Task4", "Desc4", Task.Priority1.Высокий, DateTime.Now.AddDays(4), Task.Status1.Новая, true)
            };
        }

        [TestMethod]
        public void FilterByStatus_ShouldReturnCorrectTasks()
        {
            var result = _filter.FilterByStatus(_tasks, Task.Status1.Новая);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(t => t.Status == Task.Status1.Новая));
        }

        [TestMethod]
        public void FilterByStatuses_ShouldReturnCorrectTasks()
        {
            var statuses = new List<Task.Status1> { Task.Status1.Новая, Task.Status1.ВПроцессе };

            var result = _filter.FilterByStatuses(_tasks, statuses);

            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.All(t => statuses.Contains(t.Status)));
        }

        [TestMethod]
        public void FilterByImportant_ShouldReturnImportantTasks()
        {
            var result = _filter.FilterByImportant(_tasks);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(t => t.IsImportant));
        }

        [TestMethod]
        public void FilterByPriority_ShouldReturnCorrectTasks()
        {
            var result = _filter.FilterByPriority(_tasks, Task.Priority1.Высокий);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(t => t.Priority == Task.Priority1.Высокий));
        }

        [TestMethod]
        public void FilterByStatus_EmptyList_ShouldReturnEmpty()
        {
            var result = _filter.FilterByStatus(new List<Task>(), Task.Status1.Новая);

            Assert.AreEqual(0, result.Count);
        }
    }
}