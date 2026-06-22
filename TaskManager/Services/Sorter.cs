using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Library;

namespace TaskManager.Library.Services
{
    public class Sorter
    {
        public List<Task> SortByPriority(List<Task> tasks)
        {
            return tasks.OrderByDescending(t => t.Priority).ToList();
        }

        public List<Task> SortByDueDate(List<Task> tasks)
        {
            return tasks.OrderBy(t => t.DueDate).ToList();
        }

        public List<Task> SortByPriorityAndDueDate(List<Task> tasks)
        {
            return tasks
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToList();
        }
    }
}