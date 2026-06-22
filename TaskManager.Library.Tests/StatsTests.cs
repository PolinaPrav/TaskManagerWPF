using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Library;
using TaskManager.Library.Services;

namespace TaskManager.Library.Tests
{
    [TestClass]
    public class StatsTests
    {
        private Stats _stats;
        private List<Task> _tasks;

        [TestInitialize]
        public void Setup()
        {
            _stats = new Stats();
            _tasks = new List<Task>
            {
                new Task("Task1", "Desc1", Task.Priority1.Средний, DateTime.Now.AddDays(-1), Task.Status1.Завершена),
                new Task("Task2", "Desc2", Task.Priority1.Высокий, DateTime.Now.AddDays(-2), Task.Status1.ВПроцессе),
                new Task("Task3", "Desc3", Task.Priority1.Низкий, DateTime.Now.AddDays(1), Task.Status1.Новая),
                new Task("Task4", "Desc4", Task.Priority1.Высокий, DateTime.Now.AddDays(-3), Task.Status1.Новая)
            };
        }

        [TestMethod]
        public void GetStatusCounts_ShouldReturnCorrectCounts()
        {
            var result = _stats.GetStatusCounts(_tasks);

            Assert.AreEqual(2, result[Task.Status1.Новая]);
            Assert.AreEqual(1, result[Task.Status1.ВПроцессе]);
            Assert.AreEqual(1, result[Task.Status1.Завершена]);
        }

        [TestMethod]
        public void GetOverdueCount_ShouldReturnCorrectCount()
        {
            var result = _stats.GetOverdueCount(_tasks);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetTotalCount_ShouldReturnCorrectCount()
        {
            var result = _stats.GetTotalCount(_tasks);

            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void GetAllStats_ShouldReturnAllStatistics()
        {
            var result = _stats.GetAllStats(_tasks);

            Assert.IsTrue(result.ContainsKey("Всего задач"));
            Assert.IsTrue(result.ContainsKey("Просрочено"));
            Assert.IsTrue(result.ContainsKey(Task.Status1.Новая.ToString()));
            Assert.IsTrue(result.ContainsKey(Task.Status1.ВПроцессе.ToString()));
            Assert.IsTrue(result.ContainsKey(Task.Status1.Завершена.ToString()));

            Assert.AreEqual(4, result["Всего задач"]);
            Assert.AreEqual(2, result["Просрочено"]);
            Assert.AreEqual(2, result[Task.Status1.Новая.ToString()]);
            Assert.AreEqual(1, result[Task.Status1.ВПроцессе.ToString()]);
            Assert.AreEqual(1, result[Task.Status1.Завершена.ToString()]);
        }

        [TestMethod]
        public void IsOverdue_OverdueTask_ShouldReturnTrue()
        {
            var task = new Task();
            task.DueDate = DateTime.Now.AddDays(-1);
            task.Status = Task.Status1.Новая;

            var result = _stats.IsOverdue(task);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOverdue_NotOverdueTask_ShouldReturnFalse()
        {
            var task = new Task();
            task.DueDate = DateTime.Now.AddDays(5);
            task.Status = Task.Status1.Новая;

            var result = _stats.IsOverdue(task);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsOverdue_CompletedTask_ShouldReturnFalse()
        {
            var task = new Task();
            task.DueDate = DateTime.Now.AddDays(-5);
            task.Status = Task.Status1.Завершена;

            var result = _stats.IsOverdue(task);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsOverdue_NullTask_ShouldReturnFalse()
        {
            var result = _stats.IsOverdue(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetStatusCounts_EmptyList_ShouldReturnAllZero()
        {
            var result = _stats.GetStatusCounts(new List<Task>());

            Assert.AreEqual(0, result[Task.Status1.Новая]);
            Assert.AreEqual(0, result[Task.Status1.ВПроцессе]);
            Assert.AreEqual(0, result[Task.Status1.Завершена]);
        }
    }
}