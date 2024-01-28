using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFLTestTask.Bll.DTO
{
    public class TreeDataModelDto
    {
        public int Id { get; set; }

        public int? Parent { get; set; }

        public string Text { get; set; }
    }
}
