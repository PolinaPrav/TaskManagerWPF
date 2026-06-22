using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TaskManager.Library;
using TaskManager.Library.Services;

namespace TaskManager.WPF.Windows
{
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow(List<Task> tasks)
        {
            InitializeComponent();
            LoadStatistics(tasks);
        }

        private void LoadStatistics(List<Task> tasks)
        {
            var stats = new Stats();
            var statsData = stats.GetAllStats(tasks);

            var overdueCount = tasks.Count(t => t.DueDate < System.DateTime.Now && t.Status != Task.Status1.Завершена);
            statsData["Просрочено"] = overdueCount;

            var statsList = new List<KeyValuePair<string, int>>(statsData);
            StatsListBox.ItemsSource = statsList;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}