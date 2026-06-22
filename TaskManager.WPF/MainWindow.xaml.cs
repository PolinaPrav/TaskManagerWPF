using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using TaskManager.Library;
using TaskManager.Library.Services;
using TaskManager.WPF.Windows;

namespace TaskManager.WPF
{
    public partial class MainWindow : Window
    {
        private Manager _manager = new Manager();
        private Storage _storage = new Storage();
        private Sorter _sorter = new Sorter();
        private Filter _filter = new Filter();
        private Searcher _searcher = new Searcher();

        private string _projectFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "tasks.json"
        );

        public MainWindow()
        {
            InitializeComponent();
            LoadTasksFromProject();
            UpdateTasksList();
        }

        private void LoadTasksFromProject()
        {
            try
            {
                if (File.Exists(_projectFilePath))
                {
                    var tasks = _storage.LoadFromJson(_projectFilePath);
                    foreach (var task in tasks)
                        _manager.Add(task);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTasksList()
        {
            if (FilterComboBox == null || SortComboBox == null || TasksListBox == null)
                return;

            _currentTasks = _manager.GetAll();

            if (FilterComboBox.SelectedItem is ComboBoxItem selectedFilter &&
                selectedFilter.Content.ToString() != "Все")
            {
                var statusText = selectedFilter.Content.ToString();
                var status = ParseStatus(statusText);
                _currentTasks = _filter.FilterByStatus(_currentTasks, status);
            }

            if (SortComboBox.SelectedItem is ComboBoxItem selectedSort)
            {
                switch (selectedSort.Content.ToString())
                {
                    case "По приоритету":
                        _currentTasks = _sorter.SortByPriority(_currentTasks);
                        break;
                    case "По сроку":
                        _currentTasks = _sorter.SortByDueDate(_currentTasks);
                        break;
                }
            }

            TasksListBox.ItemsSource = _currentTasks;
        }

        private Task.Status1 ParseStatus(string statusText)
        {
            return statusText switch
            {
                "Новая" => Task.Status1.Новая,
                "В процессе" => Task.Status1.ВПроцессе,
                "ВПроцессе" => Task.Status1.ВПроцессе,
                "Завершена" => Task.Status1.Завершена,
                _ => Task.Status1.Новая
            };
        }

        private List<Task> _currentTasks = new List<Task>();

        private void SearchButton_Click(object sender, RoutedEventArgs e) => ApplySearch();
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => ApplySearch();

        private void ApplySearch()
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                UpdateTasksList();
                return;
            }

            var allTasks = _manager.GetAll();
            var searchResult = _searcher.Search(allTasks, SearchTextBox.Text);
            TasksListBox.ItemsSource = searchResult;
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded || SortComboBox == null)
                return;

            UpdateTasksList();
        }
        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded || FilterComboBox == null)
                return;

            UpdateTasksList();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddTaskWindow();
            if (addWindow.ShowDialog() == true && addWindow.NewTask != null)
            {
                _manager.Add(addWindow.NewTask);
                UpdateTasksList();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e) => EditSelectedTask();
        private void TasksListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) => EditSelectedTask();

        private void EditSelectedTask()
        {
            if (TasksListBox.SelectedItem is Task selectedTask)
            {
                var editWindow = new EditTaskWindow(selectedTask);
                if (editWindow.ShowDialog() == true && editWindow.EditedTask != null)
                {
                    _manager.Edit(editWindow.EditedTask);
                    UpdateTasksList();
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksListBox.SelectedItem is Task selectedTask)
            {
                var result = MessageBox.Show($"Удалить задачу \"{selectedTask.Name}\"?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _manager.Delete(selectedTask.Id);
                    UpdateTasksList();
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            var statsWindow = new StatisticsWindow(_manager.GetAll());
            statsWindow.ShowDialog();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";
            dialog.FileName = "tasks";
            dialog.DefaultExt = ".json";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _storage.SaveToFile(_manager.GetAll(), dialog.FileName);
                    MessageBox.Show("Файл успешно сохранён!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var tasks = _storage.LoadFromFile(dialog.FileName);
                    _manager.AddRange(tasks);
                    UpdateTasksList();
                    MessageBox.Show($"Загружено {tasks.Count} задач!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _storage.SaveToJson(_manager.GetAll(), _projectFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка автосохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}