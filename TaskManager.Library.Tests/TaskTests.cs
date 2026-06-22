using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Library;

namespace TaskManager.Library.Tests
{
    [TestClass]
    public class TaskTests
    {
        [TestMethod]
        public void Constructor_Default_ShouldSetDefaultValues()
        {
            var task = new Task();

            Assert.AreNotEqual(Guid.Empty, task.Id);
            Assert.AreEqual(Task.Status1.Новая, task.Status);
            Assert.AreEqual(Task.Priority1.Средний, task.Priority);
            Assert.IsTrue(task.DueDate > DateTime.Now);
            Assert.IsFalse(task.IsImportant);
            Assert.AreEqual("", task.ImportantMark);
        }

        [TestMethod]
        public void Constructor_WithParameters_ShouldSetValues()
        {
            var name = "Test Task";
            var description = "Test Description";
            var priority = Task.Priority1.Высокий;
            var dueDate = DateTime.Now.AddDays(5);
            var status = Task.Status1.ВПроцессе;
            var isImportant = true;

            var task = new Task(name, description, priority, dueDate, status, isImportant);

            Assert.AreNotEqual(Guid.Empty, task.Id);
            Assert.AreEqual(name, task.Name);
            Assert.AreEqual(description, task.Description);
            Assert.AreEqual(priority, task.Priority);
            Assert.AreEqual(dueDate, task.DueDate);
            Assert.AreEqual(status, task.Status);
            Assert.IsTrue(task.IsImportant);
            Assert.AreEqual("⭐", task.ImportantMark);
        }

        [TestMethod]
        public void ImportantMark_WhenNotImportant_ShouldReturnEmpty()
        {
            var task = new Task();
            task.IsImportant = false;

            Assert.AreEqual("", task.ImportantMark);
        }

        [TestMethod]
        public void ImportantMark_WhenImportant_ShouldReturnStar()
        {
            var task = new Task();
            task.IsImportant = true;

            Assert.AreEqual("⭐", task.ImportantMark);
        }
    }
}