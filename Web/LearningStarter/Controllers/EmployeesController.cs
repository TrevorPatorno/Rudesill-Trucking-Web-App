using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LearningStarter.Common;
using LearningStarter.Entities;
using LearningStarterServer.Data;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public EmployeesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public IActionResult CreateEmployee(EmployeeCreateDto employeeCreateDto)
        {
            var response = new Response();

            if (employeeCreateDto == null)
            {
                response.AddError("", "Critical Error. We're not in Kansas anymore. Contact Admin!");
                return BadRequest(response);
            }

            //First Name and Last Name Errors
            if (employeeCreateDto.FirstName == null)
            {
                response.AddError("FirstName", "FirstName should not be null.");
            }
            else
            {
                if (string.IsNullOrEmpty(employeeCreateDto.FirstName))
                {
                    response.AddError("FirstName", "FirstName should not be empty.");
                }

                foreach (var characterFN in employeeCreateDto.FirstName)
                {
                    var characterAsStringFN = characterFN.ToString();
                    var isANumberFN = int.TryParse(characterAsStringFN, out var _);

                    if (isANumberFN)
                    {
                        response.AddError("FirstName", "FirstName can only be letters.");
                        break;
                    }
                }
            }

            if (employeeCreateDto.LastName == null)
            {
                response.AddError("LastName", "LastName should not be null.");
            }
            else
            {
                if (string.IsNullOrEmpty(employeeCreateDto.LastName))
                {
                    response.AddError("LastName", "LastName should not be empty.");
                }

                foreach (var characterLN in employeeCreateDto.LastName)
                {
                    var characterAsStringLN = characterLN.ToString();
                    var isANumberLN = int.TryParse(characterAsStringLN, out var _);

                    if (isANumberLN)
                    {
                        response.AddError("LastName", "LastName can only be letters.");
                        break;
                    }
                }
            }

            if (employeeCreateDto.FirstName != null && employeeCreateDto.FirstName.Length > 30)
            {
                response.AddError("FirstName", "FirstName is too long. Must be less than or equal to 30 characters.");
            }

            if (employeeCreateDto.LastName != null && employeeCreateDto.LastName.Length > 30)
            {
                response.AddError("LastName", "LastName is too long. Must be less than or equal to 30 characters.");
            }

            //DOB and DOE Errors
            var year1900 = new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero);

            if (employeeCreateDto.DateOfBirth == null)
            {
                response.AddError("DateOfBirth", "DateOfBirth cannot be null.");
            }
            else
            {
                if (employeeCreateDto.DateOfBirth <= year1900)
                {
                    response.AddError("DateOfBirth", "Date of Birth must be after the year 1900.");
                }
            }

            if (employeeCreateDto.DateOfEmployment == null)
            {
                response.AddError("DateOfEmployment", "DateOfEmployment cannot be null.");
            }
            else
            {
                if (employeeCreateDto.DateOfEmployment <= year1900)
                {
                    response.AddError("DateOfEmployment", "Date of Employment must be after the year 1990.");
                }
            }

            //LicenseNumber Errors
            if (employeeCreateDto.LicenseNumber == null)
            {
                response.AddError("LicenseNumber", "LicenseNumber cannot be null.");
            }
            else
            {
                if (string.IsNullOrEmpty(employeeCreateDto.LicenseNumber))
                {
                    response.AddError("LicenseNumber", "Error: LicenseNumber is required.");
                }

                var anyEmployeeExistsWithLicenseNumberPost = _dataContext.Employees.Any(x => x.LicenseNumber == employeeCreateDto.LicenseNumber);

                if (anyEmployeeExistsWithLicenseNumberPost)
                {
                    response.AddError("LicenseNumber", "Error: An Employee already exists with this LicenseNumber.");
                }

                int iterationsLN = 0;
                bool isANumberTrue = false;

                foreach (var character in employeeCreateDto.LicenseNumber)
                {
                    var characterAsString = character.ToString();
                    var isANumber = int.TryParse(characterAsString, out var _);

                    if (!isANumber)
                    {
                        response.AddError("LicenseNumber", "LicenseNumber must be only be numbers and has to have 9 of them.");
                        isANumberTrue = false;
                        break;
                    }

                    else if (isANumber)
                    {
                        iterationsLN++;
                        isANumberTrue = true;
                    }

                    if (isANumberTrue == true && iterationsLN == employeeCreateDto.LicenseNumber.Length && iterationsLN != 9)
                    {
                        response.AddError("LicenseNumber", "LicenseNumber must be 9 numbers.");
                    }
                }
            }

            //SocialSecurityNumber Errors
            if (employeeCreateDto.SocialSecurityNumber == null)
            {
                response.AddError("SocialSecurityNumber", "SocialSecurityNumber cannot be null.");
            }
            else
            {
                if (string.IsNullOrEmpty(employeeCreateDto.SocialSecurityNumber))
                {
                    response.AddError("", "Error: Social Security Number is required.");
                    return BadRequest(response);
                }

                var anyEmployeeExistsWithSSN = _dataContext.Employees.Any(x => x.SocialSecurityNumber == employeeCreateDto.SocialSecurityNumber);

                if (anyEmployeeExistsWithSSN)
                {
                    response.AddError("SocialSecurityNumber", "Error: An Employee already exists with this SocialSecurityNumber.");
                }

                int iterationsSSN = 0;

                foreach (var character in employeeCreateDto.SocialSecurityNumber)
                {
                    var characterAsString = character.ToString();
                    var isANumber = int.TryParse(characterAsString, out var _);

                    if (!isANumber)
                    {
                        response.AddError("SocialSecurityNumber", "Social Security Number can only be numbers.");
                        break;
                    }

                    if (isANumber)
                    {
                        iterationsSSN++;
                    }
                }

                if (iterationsSSN != 9)
                {
                    response.AddError("SocialSecurityNumber", "Social Security Number must be 9 numbers.");
                }
            }

            //LicenseExp and MedExp Errors
            if (employeeCreateDto.LicenseExpiration == null)
            {
                response.AddError("LicenseExpiration", "LicenseExpiration cannot be null.");
            }
            else
            {
                if (employeeCreateDto.LicenseExpiration <= year1900)
                {
                    response.AddError("LicenseExpiration", "License Expiration must be after the year 1900.");
                }
            }

            if (employeeCreateDto.MedicalExpiration == null)
            {
                response.AddError("MedicalExpiration", "MedicalExpiration cannot be null.");
            }
            else
            {
                if (employeeCreateDto.MedicalExpiration <= year1900)
                {
                    response.AddError("MedicalExpiration", "Medical Expiration must be after the year 1900.");
                }
            }

            //TruckId errors
            if (employeeCreateDto.TruckId == 0)
            {
                employeeCreateDto.TruckId = null;
            }
            else
            {
                var TruckIdExistsInTruckTable = _dataContext.Trucks.Any(x => x.Id == employeeCreateDto.TruckId);
                if (!TruckIdExistsInTruckTable)
                {
                    response.AddError("TruckId", "This TruckId doesn't exist");
                }

                var TruckIdExistsInEmployeeTable = _dataContext.Employees.Any(x => x.TruckId == employeeCreateDto.TruckId);
                if (TruckIdExistsInEmployeeTable)
                {
                    response.AddError("TruckId", "This TruckId is already assigned to another driver");
                }
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var employeeToCreate = new Employee
            {
                FirstName = employeeCreateDto.FirstName.Trim(),
                LastName = employeeCreateDto.LastName.Trim(),
                SocialSecurityNumber = employeeCreateDto.SocialSecurityNumber,
                DateOfBirth = employeeCreateDto.DateOfBirth.Value,
                DateOfEmployment = employeeCreateDto.DateOfEmployment.Value,
                LicenseNumber = employeeCreateDto.LicenseNumber,
                LicenseExpiration = employeeCreateDto.LicenseExpiration.Value,
                MedicalExpiration = employeeCreateDto.MedicalExpiration.Value,
                TruckId = employeeCreateDto.TruckId,
            };

            _dataContext.Employees.Add(employeeToCreate);
            _dataContext.SaveChanges();

            var employeeToReturn = new EmployeeGetDto
            {
                Id = employeeToCreate.Id,
                FirstName = employeeToCreate.FirstName,
                LastName = employeeToCreate.LastName,
                DateOfBirth = employeeToCreate.DateOfBirth,
                DateOfEmployment = employeeToCreate.DateOfEmployment,
                LicenseNumber = employeeToCreate.LicenseNumber,
                LicenseExpiration = employeeToCreate.LicenseExpiration,
                MedicalExpiration = employeeToCreate.MedicalExpiration,
                TruckId = employeeToCreate.TruckId,
            };

            response.Data = employeeToReturn;

            return Ok(response);
        }

        [HttpGet("{employeeId:int}")]
        public IActionResult GetOne([FromRoute] int employeeId)
        {
            var response = new Response();

            var DatabaseEmployee = _dataContext.Employees.FirstOrDefault(x => x.Id == employeeId);

            if (DatabaseEmployee == null)
            {
                response.AddError("employeeId", $"For the employeeId of {employeeId}, no employee was found in the database.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var employeeToReturn = new EmployeeGetDto
            {
                Id = DatabaseEmployee.Id,
                FirstName = DatabaseEmployee.FirstName,
                LastName = DatabaseEmployee.LastName,
                DateOfBirth = DatabaseEmployee.DateOfBirth,
                DateOfEmployment = DatabaseEmployee.DateOfEmployment,
                LicenseNumber = DatabaseEmployee.LicenseNumber,
                LicenseExpiration = DatabaseEmployee.LicenseExpiration,
                MedicalExpiration = DatabaseEmployee.MedicalExpiration,
                TruckId = DatabaseEmployee.TruckId,
            };

            response.Data = employeeToReturn;

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var response = new Response();

            var allEmployeesToReturn = _dataContext.Employees.Select(x => new EmployeeGetDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                DateOfEmployment = x.DateOfEmployment,
                LicenseNumber = x.LicenseNumber,
                LicenseExpiration = x.LicenseExpiration,
                MedicalExpiration = x.MedicalExpiration,
                TruckId = x.TruckId,
            }).ToList();

            response.Data = allEmployeesToReturn;

            return Ok(response);
        }


        [HttpPut("{employeeId:int}")]
        public IActionResult EditEmployee([FromRoute] int employeeId, [FromBody] EmployeeEditDto employeeEditDto)
        {
            var response = new Response();

            if (employeeEditDto == null)
            {
                response.AddError("Critical Error", "We're not in Kansas anymore! Contact Admin!");
                return BadRequest(response);
            }

            //First Name and Last Name Errors
            if (employeeEditDto.FirstName == null)
            {
                response.AddError("FirstName", "FirstName should not be null.");
            }
            else
            {
                if (string.IsNullOrEmpty(employeeEditDto.FirstName))
                {
                    response.AddError("FirstName", "FirstName should not be empty.");
                }

                foreach (var characterFN in employeeEditDto.FirstName)
                {
                    var characterAsStringFN = characterFN.ToString();

                    var isANumberFN = int.TryParse(characterAsStringFN, out var _);

                    if (isANumberFN)
                    {
                        response.AddError("FirstName", "FirstName can only be letters.");

                        break;
                    }
                }
            }

            if (employeeEditDto.LastName == null)
            {
                response.AddError("LastName", "LastName should not be null.");
            }
            else
            {
                if (string.IsNullOrEmpty(employeeEditDto.LastName))
                {
                    response.AddError("LastName", "LastName should not be empty.");
                }

                foreach (var characterLN in employeeEditDto.LastName)
                {
                    var characterAsStringLN = characterLN.ToString();
                    var isANumberLN = int.TryParse(characterAsStringLN, out var _);

                    if (isANumberLN)
                    {
                        response.AddError("LastName", "LastName can only be letters.");
                        break;
                    }
                }
            }

            if (employeeEditDto.FirstName != null && employeeEditDto.FirstName.Length > 30)
            {
                response.AddError("FirstName", "FirstName is too long. Must be less than or equal to 30 characters.");
            }

            if (employeeEditDto.LastName != null && employeeEditDto.LastName.Length > 30)
            {
                response.AddError("LastName", "LastName is too long. Must be less than or equal to 30 characters.");
            }

            //DOB and DOE Errors
            var year1900 = new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero);
            
            if (employeeEditDto.DateOfBirth == null)
            {
                response.AddError("DateOfBirth", "DateOfBirth cannot be null.");
            }
            else
            {
                if (employeeEditDto.DateOfBirth <= year1900)
                {
                    response.AddError("DateOfBirth", "Date of Birth must be after the year 1900.");
                }
            }

            if (employeeEditDto.DateOfEmployment == null)
            {
                response.AddError("DateOfEmployment", "DateOfEmployment cannot be null.");
            }
            else
            {
                if (employeeEditDto.DateOfEmployment <= year1900)
                {
                    response.AddError("DateOfEmployment", "Date of Employment must be after the year 1990.");
                }
            }

            //LicenseNumber Errors
            if (employeeEditDto.LicenseNumber == null)
            {
                response.AddError("LicenseNumber", "LicenseNumber cannot be null.");
            }
            else
            {
                if (string.IsNullOrEmpty(employeeEditDto.LicenseNumber))
                {
                    response.AddError("LicenseNumber", "Error: LicenseNumber is required.");
                }

                int iterationsLN = 0;
                bool isANumberTrue = false;

                foreach (var character in employeeEditDto.LicenseNumber)
                {
                    var characterAsString = character.ToString();
                    var isANumber = int.TryParse(characterAsString, out var _);

                    if (!isANumber)
                    {
                        response.AddError("LicenseNumber", "LicenseNumber must be only be numbers and has to have 9 of them.");
                        isANumberTrue = false;
                        break;
                    }
                    else if (isANumber)
                    {
                        iterationsLN++;
                        isANumberTrue = true;
                    }

                    if (isANumberTrue == true && iterationsLN == employeeEditDto.LicenseNumber.Length && iterationsLN != 9)
                    {
                        response.AddError("LicenseNumber", "LicenseNumber must be 9 numbers.");
                    }
                }
            }

            //LicenseExp and MedExp Errors
            if (employeeEditDto.LicenseExpiration == null)
            {
                response.AddError("LicenseExpiration", "LicenseExpiration cannot be null.");
            }
            else
            {
                if (employeeEditDto.LicenseExpiration <= year1900)
                {
                    response.AddError("LicenseExpiration", "License Expiration must be after the year 1900.");
                }
            }

            if (employeeEditDto.MedicalExpiration == null)
            {
                response.AddError("MedicalExpiration", "MedicalExpiration cannot be null.");
            }
            else
            {
                if (employeeEditDto.MedicalExpiration <= year1900)
                {
                    response.AddError("MedicalExpiration", "Medical Expiration must be after the year 1900.");
                }
            }

            //TruckId errors
            if (employeeEditDto.TruckId == 0)
            {
                employeeEditDto.TruckId = null;
            }
            else
            {
                var TruckIdExistsInTruckTable = _dataContext.Trucks.Any(x => x.Id == employeeEditDto.TruckId);
                if (!TruckIdExistsInTruckTable)
                {
                    response.AddError("TruckId", "This TruckId doesn't exist");
                }

                var TruckIdExistsInEmployeeTable = _dataContext.Employees.Any(x => x.TruckId == employeeEditDto.TruckId);
                if (TruckIdExistsInEmployeeTable)
                {
                    response.AddError("TruckId", "This TruckId is already assigned to another driver");
                }
            }

            var employeeToEdit = _dataContext.Employees.FirstOrDefault(x => x.Id == employeeId);

            if (employeeToEdit == null)
            {
                response.AddError("employeeId", $"For the employeeId of {employeeId}, no employee was found in the database.");
                return BadRequest(response);
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            employeeToEdit.FirstName = employeeEditDto.FirstName.Trim();
            employeeToEdit.LastName = employeeEditDto.LastName.Trim();
            employeeToEdit.DateOfBirth = employeeEditDto.DateOfBirth.Value;
            employeeToEdit.DateOfEmployment = employeeEditDto.DateOfEmployment.Value;
            employeeToEdit.LicenseNumber = employeeEditDto.LicenseNumber;
            employeeToEdit.LicenseExpiration = employeeEditDto.LicenseExpiration.Value;
            employeeToEdit.MedicalExpiration = employeeEditDto.MedicalExpiration.Value;
            employeeToEdit.TruckId = employeeEditDto.TruckId;

            _dataContext.SaveChanges();

            var employeeToReturn = new EmployeeGetDto
            {
                Id = employeeToEdit.Id,
                FirstName = employeeToEdit.FirstName,
                LastName = employeeToEdit.LastName,
                DateOfBirth = employeeToEdit.DateOfBirth,
                DateOfEmployment = employeeToEdit.DateOfEmployment,
                LicenseNumber = employeeToEdit.LicenseNumber,
                LicenseExpiration = employeeToEdit.LicenseExpiration,
                MedicalExpiration = employeeToEdit.MedicalExpiration,
                TruckId = employeeToEdit.TruckId,
            };

            response.Data = employeeToReturn;

            return Ok(response);
        }

        [HttpDelete("{employeeId:int}")]
        public IActionResult DeleteEmployee([FromRoute] int employeeId)
        {
            var response = new Response();

            var employeeToDelete = _dataContext.Employees.FirstOrDefault(x => x.Id == employeeId);

            if (employeeToDelete == null)
            {
                response.AddError("employeeId", $"For the employeeId of {employeeId}, no employee was found in the database.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            _dataContext.Remove(employeeToDelete);
            _dataContext.SaveChanges();

            return Ok(response);
        }
    }
}
