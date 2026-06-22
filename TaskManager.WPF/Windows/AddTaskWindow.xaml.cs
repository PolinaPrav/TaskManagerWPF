using System;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Library;

namespace TaskManager.WPF.Windows
{
    public partial class AddTaskWindow : Window
    {
        public Task NewTask { get; private set; }

        public AddTaskWindow()
        {
            InitializeComponent();
            DueDatePicker.SelectedDate = DateTime.Now.AddDays(7);
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

            NewTask = new Task(
                name: NameTextBox.Text.Trim(),
                description: DescriptionTextBox.Text.Trim(),
                priority: (Task.Priority1)Enum.Parse(typeof(Task.Priority1),
                           (PriorityComboBox.SelectedItem as ComboBoxItem).Content.ToString()),
                dueDate: DueDatePicker.SelectedDate ?? DateTime.Now.AddDays(7),
                status: (Task.Status1)Enum.Parse(typeof(Task.Status1),
                        (StatusComboBox.SelectedItem as ComboBoxItem).Content.ToString()),
                isImportant: ImportantCheckBox.IsChecked ?? false
            );

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