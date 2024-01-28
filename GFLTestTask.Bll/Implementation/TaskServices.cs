using GFLTestTask.Bll.DTO;
using GFLTestTask.Bll.Interfaces;
using GFLTestTask.Dal.Entities;
using GFLTestTask.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GFLTestTask.Bll.Implementation
{
    
    public class TaskServices : ITaskServices
    {
        private readonly IApplicationContext _appContext;
        public TaskServices(IApplicationContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<List<TreeDataModelDto>> GetDataFromDB()
        {
            var userList = await _appContext.Employee.Include(i => i.User).Include(i => i.Position)
                .Select(s => new TreeDataModelDto
                {
                    Id = s.Id,
                    Parent = s.LeaderId == null ? 0 : s.LeaderId,
                    Text = s.User.FirstName + " " + s.User.LastName + " (" + s.Position.DisplayName + ")"
                }) 
                .ToListAsync();

            return userList;
                
        }

        public async Task<List<TreeDataModelDto>> GetDataFromDBbyId(int id)
        {
            var userList = await _appContext.Employee.Include(i => i.User).Include(i => i.Position)
                .Where(w => w.Id == id || w.LeaderId == id)
                .Select(s => new TreeDataModelDto
                {
                    Id = s.Id,
                    Parent = s.LeaderId == null ? 0 : s.LeaderId,
                    Text = s.User.FirstName + " " + s.User.LastName + " (" + s.Position.DisplayName + ")"
                })
                .ToListAsync();

            return userList;

        }

        public async Task ReadInputJsonFile(string content)
        {
            List<ImportModel> userObjects = null;

            userObjects = JsonConvert.DeserializeObject<List<ImportModel>>(content);

            if(userObjects != null && userObjects.Any())
            {
                await UploadDataToDB(userObjects);
            }
        }

        public async Task ReadInputTxtFile(string[] lines)
        {
            var csv = new List<string[]>();
  
            foreach (string line in lines)
                csv.Add(line.Split(':'));

            var res = CsvToJson(lines);
        }

        private IEnumerable<JObject> CsvToJson(string[] csvLines)
        {
            var csvLinesList = csvLines.ToList();

            var header = csvLinesList[0].Split(':');
            for (int i = 1; i < csvLinesList.Count; i++)
            {
                var thisLineSplit = csvLinesList[i].Split(':');
                var pairedWithHeader = header.Zip(thisLineSplit, (h, v) => new KeyValuePair<string, string>(h, v));

                yield return new JObject(pairedWithHeader.Select(j => new JProperty(j.Key, j.Value)));
            }
        }

        private async Task UploadDataToDB(List<ImportModel> userObjects)
        {
            foreach(var userObject in userObjects) 
            {
                //Get Position
                var position = _appContext.Position.FirstOrDefault(w => w.Id == userObject.User.Employee.Position.PositionId);
                if (position == null)
                {
                    throw new ArgumentException("Position Id not found");
                }
                //Insert user
                var userFromDb = _appContext.User
                    .AsNoTracking()
                    .FirstOrDefault(w => w.FirstName == userObject.User.FirstName
                    && w.LastName == userObject.User.LastName
                    && w.MiddleName == userObject.User.MiddleName);
                if (userFromDb == null)
                {
                    var user = _appContext.User.Add(new Dal.Entities.User
                    {

                        FirstName = userObject.User.FirstName,
                        LastName = userObject.User.LastName,
                        MiddleName = userObject.User.MiddleName
                    });
                    await _appContext.SaveChangesAsync();
                    //Insert Employee
                    _appContext.Employee.Add(new Dal.Entities.Employee
                    {
                        Id = user.Entity.Id,
                        StartDate = userObject.User.Employee.StartDate,
                        EndDate = userObject.User.Employee.EndDate,
                        Salary = userObject.User.Employee.Salary,
                        PositionId = position.Id,
                        LeaderId = userObject.User.Employee.LeaderId,
                    });
                    await _appContext.SaveChangesAsync();
                }
                else
                {
                    _appContext.User.Update(new Dal.Entities.User
                    {
                        Id = userFromDb.Id,
                        FirstName = userObject.User.FirstName,
                        LastName = userObject.User.LastName,
                        MiddleName = userObject.User.MiddleName
                    });

                        var employeeFromDb = _appContext.Employee.AsNoTracking().Include(i=>i.User).FirstOrDefault(w => w.Id == userFromDb.Id);
                        if (employeeFromDb != null)
                        {
                           var e = _appContext.Employee.Update(new Dal.Entities.Employee
                            {
                                Id = userFromDb.Id,
                                StartDate = userObject.User.Employee.StartDate,
                                EndDate = userObject.User.Employee.EndDate,
                                Salary = userObject.User.Employee.Salary,
                                PositionId = position.Id,
                                LeaderId = userObject.User.Employee.LeaderId,
                            });
                        }
                        else
                        {
                            _appContext.Employee.Add(new Dal.Entities.Employee
                            {
                                Id = userFromDb.Id,
                                StartDate = userObject.User.Employee.StartDate,
                                EndDate = userObject.User.Employee.EndDate,
                                Salary = userObject.User.Employee.Salary,
                                PositionId = position.Id,
                                LeaderId = userObject.User.Employee.LeaderId,
                            });
                        }
                        await _appContext.SaveChangesAsync();
  
                }

            }
        }
    }
}
