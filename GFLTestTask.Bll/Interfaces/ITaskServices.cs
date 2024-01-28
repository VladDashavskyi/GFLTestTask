using GFLTestTask.Bll.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFLTestTask.Bll.Interfaces
{
    public interface ITaskServices
    {
        Task<List<TreeDataModelDto>> GetDataFromDB();
        Task<List<TreeDataModelDto>> GetDataFromDBbyId(int id);
        Task ReadInputJsonFile(string content);
        Task ReadInputTxtFile(string[] content);
    }
}
