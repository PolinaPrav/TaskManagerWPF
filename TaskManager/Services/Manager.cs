using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Library;

namespace TaskManager.Library.Services
{
    public class Manager
    {
        private List<Task> _tasks = new List<Task>();

        public List<Task> GetAll()
        {
            return _tasks.ToList();
        }

        public Task GetById(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void Add(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            _tasks.Add(task);
        }

        public void AddRange(List<Task> tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            _tasks.AddRange(tasks);
        }

        public void Edit(Task updatedTask)
        {
            if (updatedTask == null)
                throw new ArgumentNullException(nameof(updatedTask));

            var existingTask = GetById(updatedTask.Id);
            if (existingTask == null)
                throw new KeyNotFoundException($"Задача с ID {updatedTask.Id} не найдена");

            existingTask.Name = updatedTask.Name;
            existingTask.Description = updatedTask.Description;
            existingTask.Priority = updatedTask.Priority;
            existingTask.DueDate = updatedTask.DueDate;
            existingTask.Status = updatedTask.Status;
            existingTask.IsImportant = updatedTask.IsImportant;
        }

        public void Delete(Guid id)
        {
            var task = GetById(id);
            if (task == null)
                throw new KeyNotFoundException($"Задача с ID {id} не найдена");

            _tasks.Remove(task);
        }

        public void Clear()
        {
            _tasks.Clear();
        }
    }
}