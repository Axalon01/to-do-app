using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		private TaskManager _manager;
		private TaskModel? _taskBeingEdited;
		public MainWindow()
        {
            InitializeComponent();
			_manager = new TaskManager("tasks.json");

			//Load tasks into the list when the app starts
			TaskList.ItemsSource = _manager.LoadTasks();
        }

		private void ShowAddPanel_Click(object sender, RoutedEventArgs e)
		{
			InputPanel.Visibility = Visibility.Visible;
			TitleInput.Focus();
			DueDateInput.SelectedDate = DateTime.Today;
		}

		private void AddTask_Click(object sender, RoutedEventArgs e)
		{
			var newTask = new TaskModel
			{
				Title = string.IsNullOrWhiteSpace(TitleInput.Text) ? "New Task" : TitleInput.Text,
				Description = DescriptionInput.Text,
				DueDate = DueDateInput.SelectedDate ?? DateTime.MinValue, // Fallback if no date is picked
				IsComplete = false,
			};

			// Save it via TaskManager
			_manager.AddTask(newTask);

			// Refresh the listbox UI
			TaskList.ItemsSource = _manager.LoadTasks();

			//Clear inputs after adding
			TitleInput.Text = "";
			DescriptionInput.Text = "";
			DueDateInput.SelectedDate = null;
		}

		private void CancelAdd_Click(object sender, RoutedEventArgs e)
		{
			//Hide and clear inputs
			InputPanel.Visibility = Visibility.Collapsed;
			TitleInput.Text = "";
			DescriptionInput.Text = "";
			DueDateInput.SelectedDate = null;
		}

		private void EditTask_Click(object sender, RoutedEventArgs e)
		{
			var task = TaskList.SelectedItem as TaskModel;
			if (task == null) return; // Safety check

			_taskBeingEdited = task;

			// Show the panel so the user can edit it
			InputPanel.Visibility= Visibility.Visible;

			TitleInput.Text = task.Title;
			DescriptionInput.Text = task.Description;
			DueDateInput.SelectedDate = task.DueDate;

			AddButton.Visibility = Visibility.Collapsed;
			SaveButton.Visibility = Visibility.Visible;
		}

		private void SaveEdit_Click(object sender, RoutedEventArgs e)
		{
			if (_taskBeingEdited == null) return; // Nothing to save

			_taskBeingEdited.Title = TitleInput.Text;
			_taskBeingEdited.Description = DescriptionInput.Text;
			_taskBeingEdited.DueDate = DueDateInput.SelectedDate;

			_manager.UpdateTask(_taskBeingEdited);

			// Refresh the listbox UI
			TaskList.ItemsSource = _manager.LoadTasks();

			//Clear inputs after adding
			TitleInput.Text = "";
			DescriptionInput.Text = "";
			DueDateInput.SelectedDate = null;

			//Clear _taskBeingEdited
			_taskBeingEdited = null;
		}

		private void DeleteTask_Click(object sender, RoutedEventArgs e)
		{
			var task = TaskList.SelectedItem as TaskModel;
			if (task == null) return; // Safety check

			var result = MessageBox.Show("Are you sure you want to delete this task?",
				"Confirm Delete",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);

			if (result == MessageBoxResult.Yes)
			{
				_manager.DeleteTask(task.Id);

				TaskList.ItemsSource = _manager.LoadTasks();
			}
		}
	}
}