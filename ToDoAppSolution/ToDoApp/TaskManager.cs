using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace ToDoApp
{
    public class TaskManager
    {
        private string _filePath;

        public TaskManager(string filePath)
        {
            _filePath = filePath;
        }

        public bool SaveTasks(List<TaskModel> tasks)
        {
            try
            {
                string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                return true; // Success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks: {ex.Message}");
                return false;
            }
        }

        public List<TaskModel> LoadTasks()
        {
            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<TaskModel>>(json) ?? new List<TaskModel>();
            }
            catch (Exception ex)
            {
                // Log error, return empty list so app can keep running
                Console.WriteLine($"Error loading tasks: {ex.Message}");
                return new List<TaskModel>();
            }
        }

        public bool UpdateTask(TaskModel updatedTask)
        {
            var tasks = LoadTasks();
            var taskToUpdate = tasks.FirstOrDefault(t => t.Id == updatedTask.Id);

            if (taskToUpdate != null)
            {
                taskToUpdate.Title = updatedTask.Title;
                taskToUpdate.Description = updatedTask.Description;
                taskToUpdate.IsComplete = updatedTask.IsComplete;
                taskToUpdate.DueDate = updatedTask.DueDate;

                return SaveTasks(tasks);
            }
            return false; // Not found
            //Complete this in the UI layer
        }

        public bool DeleteTask(Guid id)
        {
            var tasks = LoadTasks();
            var taskToDelete = tasks.FirstOrDefault(t =>t.Id == id);

            if (taskToDelete != null)
            {
                tasks.Remove(taskToDelete);
                return SaveTasks(tasks);
            }

            return false; // Nothing found to delete
        }

        public TaskModel AddTask(TaskModel newTask)
        {
            var tasks = LoadTasks();

            // Auto-generates fields if they're missing
            if (newTask.Id == Guid.Empty)
                newTask.Id = Guid.NewGuid();

            if (newTask.CreatedAt == default)
                newTask.CreatedAt = DateTime.Now;

            tasks.Add(newTask);
            SaveTasks(tasks);
            return newTask; // Useful if the caller wants wants the generated Id
        }
    }
}
