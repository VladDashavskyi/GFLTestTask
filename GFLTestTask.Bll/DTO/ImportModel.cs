using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GFLTestTask.Bll.DTO
{
    public class ImportModel
    {       
        public User User { get; set; }        
    }

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Employee Employee { get; set; }

    }

    public class Employee
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Position Position { get; set; }
        public decimal Salary { get; set; }
        public int? LeaderId { get; set; }
    }

    public class Position
    {
        public int PositionId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
