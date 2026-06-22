using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Library;

namespace TaskManager.Library.Services
{
    public class Searcher
    {
        public List<Task> Search(List<Task> tasks, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return tasks.ToList();

            query = query.ToLower().Trim();

            return tasks.Where(t =>
                t.Name.ToLower().Contains(query) ||
                t.Description.ToLower().Contains(query)
            ).ToList();
        }

        public List<Task> SearchByExactName(List<Task> tasks, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return tasks.ToList();

            return tasks.Where(t =>
                t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
    }
}