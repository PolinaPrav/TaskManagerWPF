using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Library;
using TaskManager.Library.Services;

namespace TaskManager.Library.Tests
{
    [TestClass]
    public class ManagerTests
    {
        private Manager _manager;
        private Task _testTask;

        [TestInitialize]
        public void Setup()
        {
            _manager = new Manager();
            _testTask = new Task("Test", "Description", Task.Priority1.Средний,
                                DateTime.Now.AddDays(7), Task.Status1.Новая);
        }

        [TestMethod]
        public void Add_ShouldAddTask()
        {
            _manager.Add(_testTask);
            var tasks = _manager.GetAll();
            Assert.AreEqual(1, tasks.Count);
            Assert.AreEqual(_testTask.Id, tasks[0].Id);
        }

        [TestMethod]
        public void Add_NullTask_ShouldThrowArgumentNullException()
        {
            try
            {
                _manager.Add(null);
                Assert.Fail("Ожидалось ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void AddRange_ShouldAddMultipleTasks()
        {
            var tasks = new List<Task> { new Task(), new Task() };
            _manager.AddRange(tasks);
            Assert.AreEqual(2, _manager.GetAll().Count);
        }

        [TestMethod]
        public void AddRange_NullTasks_ShouldThrowArgumentNullException()
        {
            try
            {
                _manager.AddRange(null);
                Assert.Fail("Ожидалось ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllTasks()
        {
            _manager.Add(_testTask);
            _manager.Add(new Task());
            var tasks = _manager.GetAll();
            Assert.AreEqual(2, tasks.Count);
        }

        [TestMethod]
        public void GetById_ExistingTask_ShouldReturnTask()
        {
            _manager.Add(_testTask);
            var result = _manager.GetById(_testTask.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(_testTask.Id, result.Id);
        }

        [TestMethod]
        public void GetById_NonExistingTask_ShouldReturnNull()
        {
            var result = _manager.GetById(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Edit_ShouldUpdateTask()
        {
            _manager.Add(_testTask);
            var updatedTask = new Task("Updated", "New Description", Task.Priority1.Высокий,
                                      DateTime.Now.AddDays(3), Task.Status1.ВПроцессе, true);
            updatedTask.Id = _testTask.Id;

            _manager.Edit(updatedTask);
            var result = _manager.GetById(_testTask.Id);

            Assert.AreEqual("Updated", result.Name);
            Assert.AreEqual("New Description", result.Description);
            Assert.AreEqual(Task.Priority1.Высокий, result.Priority);
            Assert.AreEqual(Task.Status1.ВПроцессе, result.Status);
            Assert.IsTrue(result.IsImportant);
        }

        [TestMethod]
        public void Edit_NullTask_ShouldThrowArgumentNullException()
        {
            try
            {
                _manager.Edit(null);
                Assert.Fail("Ожидалось ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void Edit_NonExistingTask_ShouldThrowKeyNotFoundException()
        {
            var task = new Task();
            try
            {
                _manager.Edit(task);
                Assert.Fail("Ожидалось KeyNotFoundException");
            }
            catch (KeyNotFoundException)
            {
            }
        }

        [TestMethod]
        public void Delete_ShouldRemoveTask()
        {
            _manager.Add(_testTask);
            _manager.Delete(_testTask.Id);
            Assert.AreEqual(0, _manager.GetAll().Count);
        }

        [TestMethod]
        public void Delete_NonExistingTask_ShouldThrowKeyNotFoundException()
        {
            try
            {
                _manager.Delete(Guid.NewGuid());
                Assert.Fail("Ожидалось KeyNotFoundException");
            }
            catch (KeyNotFoundException)
            {
            }
        }

        [TestMethod]
        public void Clear_ShouldRemoveAllTasks()
        {
            _manager.Add(_testTask);
            _manager.Add(new Task());
            _manager.Clear();
            Assert.AreEqual(0, _manager.GetAll().Count);
        }
    }
}