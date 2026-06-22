using System;

namespace TaskManager
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority1 Priority { get; set; }
        public DateTime DueDate { get; set; }
        public Status1 Status { get; set; }
        public bool IsImportant { get; set; }

        public string ImportantMark => IsImportant ? "⭐" : "";

        public Task()
        {
            Id = Guid.NewGuid();
            Status = Status1.Новая;
            Priority = Priority1.Средний;
            DueDate = DateTime.Now.AddDays(7);
        }

        public Task(string name, string description, Priority1 priority,
                      DateTime dueDate, Status1 status, bool isImportant = false)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Priority = priority;
            DueDate = dueDate;
            Status = status;
            IsImportant = isImportant;
        }

        public enum Priority1
        {
            Низкий,
            Средний,
            Высокий
        }

        public enum Status1
        {
            Новая,
            ВПроцессе,
            Завершена
        }

    }
}