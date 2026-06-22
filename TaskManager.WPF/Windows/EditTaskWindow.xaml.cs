using System;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Library;

namespace TaskManager.WPF.Windows
{
    public partial class EditTaskWindow : Window
    {
        private Task _originalTask;
        public Task EditedTask { get; private set; }

        public EditTaskWindow(Task task)
        {
            InitializeComponent();
            _originalTask = task;
            LoadTaskData();
        }

        private void LoadTaskData()
        {
            NameTextBox.Text = _originalTask.Name;
            DescriptionTextBox.Text = _originalTask.Description;
            PriorityComboBox.SelectedIndex = (int)_originalTask.Priority;
            DueDatePicker.SelectedDate = _originalTask.DueDate;
            StatusComboBox.SelectedIndex = (int)_originalTask.Status;
            ImportantCheckBox.IsChecked = _originalTask.IsImportant;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Введите название задачи!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                NameTextBox.Focus();
                return;
            }

            EditedTask = new Task(
                name: NameTextBox.Text.Trim(),
                description: DescriptionTextBox.Text.Trim(),
                priority: (Task.Priority1)Enum.Parse(typeof(Task.Priority1),
                           (PriorityComboBox.SelectedItem as ComboBoxItem).Content.ToString()),
                dueDate: DueDatePicker.SelectedDate ?? DateTime.Now.AddDays(7),
                status: (Task.Status1)Enum.Parse(typeof(Task.Status1),
                        (StatusComboBox.SelectedItem as ComboBoxItem).Content.ToString()),
                isImportant: ImportantCheckBox.IsChecked ?? false
            );

            EditedTask.Id = _originalTask.Id;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}