using System;
using System.Collections.Generic;

namespace Capstone4ToDoListFinal.Models
{
    public partial class ToDoItem
    {
        public ToDoItem()
        {
            InverseIdNetUsersNavigation = new HashSet<ToDoItem>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool Complete { get; set; }
        public int? IdNetUsers { get; set; }

        public virtual ToDoItem IdNetUsersNavigation { get; set; }
        public virtual ICollection<ToDoItem> InverseIdNetUsersNavigation { get; set; }
    }
}
