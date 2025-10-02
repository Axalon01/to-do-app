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
			//DueDateInput.SelectedDate = DateTime.Today;
		}

		private void AddTask_Click(object sender, RoutedEventArgs e)
		{
			// Build the full due date/tiem
			DateTime? dueDateTime = null;

			if (DueDateInput.SelectedDate.HasValue)
			{
				int hour = HourInput.SelectedItem is int h ? h : 0;
				int minute = 0;
				if (MinuteInput.SelectedItem is string minStr && int.TryParse(minStr, out var m))
					minute = m;
				string ampm = AmPmInput.SelectedItem as string ?? "AM";

				if (ampm == "PM" && hour < 12)
				{
					hour += 12;
				}
				if (ampm == "AM" && hour == 12)
				{
					hour = 0;
				}

				dueDateTime = new DateTime(
						DueDateInput.SelectedDate.Value.Year,
						DueDateInput.SelectedDate.Value.Month,
						DueDateInput.SelectedDate.Value.Day,
						hour,
						minute,
						0);
			}

			var newTask = new TaskModel
			{
				Title = string.IsNullOrWhiteSpace(TitleInput.Text) ? "New Task" : TitleInput.Text,
				Description = DescriptionInput.Text,
				DueDate = dueDateTime, // This will be null if nothing was selected
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
			// -1 clears the dropdowns so the fields are empty again next time
			HourInput.SelectedIndex = -1;
			MinuteInput.SelectedIndex = -1;
			AmPmInput.SelectedIndex = -1;
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

			if (task.DueDate.HasValue)
			{
				var due = task.DueDate.Value;
				DueDateInput.SelectedDate = due.Date;

				int hour = due.Hour;
				string ampm = "AM";

				if (hour == 0)
				{
					hour = 12;
				}
				else if (hour >= 12)
				{
					ampm = "PM";
					if (hour > 12) hour -= 12;
				}

				HourInput.SelectedItem = hour;
				MinuteInput.SelectedItem = due.Minute.ToString("00"); // Match dropdown values like "05"

				AmPmInput.SelectedItem = ampm;
			}
			else
			{
				DueDateInput.SelectedDate = null;
				HourInput.SelectedIndex = -1;
				MinuteInput.SelectedIndex = -1;
				AmPmInput.SelectedIndex = -1;
			}

			AddButton.Visibility = Visibility.Collapsed;
			SaveButton.Visibility = Visibility.Visible;
		}

		private void SaveEdit_Click(object sender, RoutedEventArgs e)
		{
			if (_taskBeingEdited == null) return; // Nothing to save

			_taskBeingEdited.Title = TitleInput.Text;
			_taskBeingEdited.Description = DescriptionInput.Text;
		
			DateTime? dueDateTime = null;

			if (DueDateInput.SelectedDate.HasValue)
			{
				int hour = HourInput.SelectedItem is int h ? h : 0;
				int minute = 0;
				if (MinuteInput.SelectedItem is string minStr && int.TryParse(minStr, out var m))
					minute = m;
				string ampm = AmPmInput.SelectedItem as string ?? "AM";

				if (ampm == "PM" && hour < 12)
				{
					hour += 12;
				}
				if (ampm == "AM" && hour == 12)
				{
					hour = 0;
				}

				dueDateTime = new DateTime(
					DueDateInput.SelectedDate.Value.Year,
					DueDateInput.SelectedDate.Value.Month,
					DueDateInput.SelectedDate.Value.Day,
					hour,
					minute,
					0);
			}
			
			_taskBeingEdited.DueDate = dueDateTime;

			_manager.UpdateTask(_taskBeingEdited);

			// Refresh the listbox UI
			TaskList.ItemsSource = _manager.LoadTasks();

			//Clear inputs after adding
			TitleInput.Text = "";
			DescriptionInput.Text = "";
			DueDateInput.SelectedDate = null;
			HourInput.SelectedIndex = -1;
			MinuteInput.SelectedIndex = -1;
			AmPmInput.SelectedIndex = -1;

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