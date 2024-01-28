using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFLTestTask.Dal.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PositionId { get; set; }
        public decimal Salary { get; set; }
        public int? LeaderId { get; set; }

        public User User { get; set; }
        public Position Position { get; set; }
    }
}

