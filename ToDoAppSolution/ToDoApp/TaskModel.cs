using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEditing { get; set; }
        public string DueDateDisplay
        {
            get
            {
                if (DueDate == null) return "";

                var dt = DueDate.Value;
                // If time is midnight AND user didn't explicitly set time, just show the date
                if (dt.Hour == 0 && dt.Minute == 0)
                {
                    return dt.ToString("MMM dd, yyyy");
                }
                return dt.ToString("MMM dd, yyyy h:mm tt");
            }

        }
    }
}