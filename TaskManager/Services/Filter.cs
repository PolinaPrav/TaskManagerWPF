using System.Collections.Generic;
using System.Linq;
using static TaskManager.Task;

namespace TaskManager.Library.Services
{
    public class Filter
    {
        public List<Task> FilterByStatus(List<Task> tasks, Status1 status)
        {
            return tasks.Where(t => t.Status == status).ToList();
        }

        public List<Task> FilterByStatuses(List<Task> tasks, List<Status1> statuses)
        {
            return tasks.Where(t => statuses.Contains(t.Status)).ToList();
        }

        public List<Task> FilterByImportant(List<Task> tasks)
        {
            return tasks.Where(t => t.IsImportant).ToList();
        }

        public List<Task> FilterByPriority(List<Task> tasks, Priority1 priority)
        {
            return tasks.Where(t => t.Priority == priority).ToList();
        }
    }
}