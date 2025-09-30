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
				Title = TitleInput.Text,
				Description = DescriptionInput.Text,
				DueDate = DueDateInput.SelectedDate ?? DateTime.MinValue, // Fallback if no date is picked
				IsComplete = false,
			};

			// Save it via TaskManager
			var manager = new TaskManager("tasks.json");
			manager.AddTask(newTask);

			// Refresh the listbox UI
			TaskList.ItemsSource = manager.LoadTasks();

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
			MessageBox.Show("Edit Task button clicked!");
		}

		private void DeleteTask_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Delete Task button clicked!");
		}

		private void Mark_Complete_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Mark Completed button clicked!");
		}
	}
}