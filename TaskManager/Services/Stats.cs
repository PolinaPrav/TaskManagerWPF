using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Library;
using static TaskManager.Task;

namespace TaskManager.Library.Services
{
    public class Stats
    {
        public Dictionary<Status1, int> GetStatusCounts(List<Task> tasks)
        {
            var result = new Dictionary<Status1, int>();

            foreach (Status1 status in Enum.GetValues(typeof(Status1)))
            {
                result[status] = tasks.Count(t => t.Status == status);
            }

            return result;
        }

        public int GetOverdueCount(List<Task> tasks)
        {
            return tasks.Count(t => t.DueDate < DateTime.Now && t.Status != Status1.Завершена);
        }

        public int GetTotalCount(List<Task> tasks)
        {
            return tasks.Count;
        }

        public Dictionary<string, int> GetAllStats(List<Task> tasks)
        {
            var stats = new Dictionary<string, int>
            {
                ["Всего задач"] = GetTotalCount(tasks),
                ["Просрочено"] = GetOverdueCount(tasks)
            };

            var statusCounts = GetStatusCounts(tasks);
            foreach (var kvp in statusCounts)
            {
                stats[kvp.Key.ToString()] = kvp.Value;
            }

            return stats;
        }

        public bool IsOverdue(Task task)
        {
            if (task == null) return false;
            return task.DueDate < DateTime.Now && task.Status != Task.Status1.Завершена;
        }
    }
}