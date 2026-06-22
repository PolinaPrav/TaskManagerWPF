using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Library;
using TaskManager.Library.Services;

namespace TaskManager.Library.Tests
{
    [TestClass]
    public class SorterTests
    {
        private Sorter _sorter;
        private List<Task> _tasks;

        [TestInitialize]
        public void Setup()
        {
            _sorter = new Sorter();
            _tasks = new List<Task>
            {
                new Task("Task1", "Desc1", Task.Priority1.Низкий, DateTime.Now.AddDays(3), Task.Status1.Новая),
                new Task("Task2", "Desc2", Task.Priority1.Высокий, DateTime.Now.AddDays(1), Task.Status1.ВПроцессе),
                new Task("Task3", "Desc3", Task.Priority1.Средний, DateTime.Now.AddDays(2), Task.Status1.Завершена)
            };
        }

        [TestMethod]
        public void SortByPriority_ShouldSortDescending()
        {
            var result = _sorter.SortByPriority(_tasks);

            Assert.AreEqual(Task.Priority1.Высокий, result[0].Priority);
            Assert.AreEqual(Task.Priority1.Средний, result[1].Priority);
            Assert.AreEqual(Task.Priority1.Низкий, result[2].Priority);
        }

        [TestMethod]
        public void SortByDueDate_ShouldSortAscending()
        {
            var result = _sorter.SortByDueDate(_tasks);

            Assert.IsTrue(result[0].DueDate <= result[1].DueDate);
            Assert.IsTrue(result[1].DueDate <= result[2].DueDate);
        }

        [TestMethod]
        public void SortByPriorityAndDueDate_ShouldSortByPriorityThenDueDate()
        {
            var tasks = new List<Task>
            {
                new Task("Task1", "Desc1", Task.Priority1.Высокий, DateTime.Now.AddDays(5), Task.Status1.Новая),
                new Task("Task2", "Desc2", Task.Priority1.Высокий, DateTime.Now.AddDays(1), Task.Status1.ВПроцессе),
                new Task("Task3", "Desc3", Task.Priority1.Средний, DateTime.Now.AddDays(2), Task.Status1.Завершена)
            };

            var result = _sorter.SortByPriorityAndDueDate(tasks);

            Assert.AreEqual(Task.Priority1.Высокий, result[0].Priority);
            Assert.AreEqual(Task.Priority1.Высокий, result[1].Priority);
            Assert.IsTrue(result[0].DueDate <= result[1].DueDate);
            Assert.AreEqual(Task.Priority1.Средний, result[2].Priority);
        }

        [TestMethod]
        public void SortByPriority_EmptyList_ShouldReturnEmpty()
        {
            var result = _sorter.SortByPriority(new List<Task>());

            Assert.AreEqual(0, result.Count);
        }
    }
}