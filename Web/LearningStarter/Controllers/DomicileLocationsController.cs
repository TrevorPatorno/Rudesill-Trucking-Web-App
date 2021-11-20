using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LearningStarterServer.Data;
using LearningStarter.Common;
using LearningStarter.Entities;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("/api/domicilelocations")]
    public class DomicileLocationsController : Controller
    {
        private readonly DataContext _dataContext;
        public DomicileLocationsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("create")]
        public IActionResult CreateDomicileLocation(DomicileLocationCreateDto domicilelocationCreateDto)
        {
            var response = new Response();

            if (domicilelocationCreateDto == null)
            {
                response.AddError("", "CRITICAL ERROR: please contact admin.");
                return BadRequest(response);
            }

            // Name Errors

            if (string.IsNullOrEmpty(domicilelocationCreateDto.Name))
            {
                response.AddError("Name", "Name should  not be empty.");
            }

            if (domicilelocationCreateDto.Name != null && domicilelocationCreateDto.Name.Length > 8)
            {
                response.AddError("Name", "Name is too long. Must be less than or equal to 8 characters.");
            }

            if (domicilelocationCreateDto.Name != null && domicilelocationCreateDto.Name.Length < 3)
            {
                response.AddError("Name", "Name is too short. Must be greater than or equal to 3 characters.");
            }

            var anyDomicileLocationsExistWithName = _dataContext.DomicileLocations.Any(x => x.Name == domicilelocationCreateDto.Name);

            if (anyDomicileLocationsExistWithName)
            {
                response.AddError("Name", "This Domicile Location already exists.");
            }

            // PhoneNum Errors

            if (string.IsNullOrEmpty(domicilelocationCreateDto.PhoneNum))
            {
                response.AddError("PhoneNum", "Phone Number should not be empty");
            }

            if (domicilelocationCreateDto.PhoneNum != null && domicilelocationCreateDto.PhoneNum.Length != 10)
            {
                response.AddError("PhoneNum", "Phone Number must be 10 characters");
            }

            foreach (var character in domicilelocationCreateDto.PhoneNum)
            {
                var characterAsString = character.ToString();

                var isANumber = int.TryParse(characterAsString, out var _);

                if (!isANumber)
                {
                    response.AddError("PhoneNum", "Phone Number can only be numbers.");
                    break;
                }
            }

            var anyDomicileLocationsExistWithPhoneNum = _dataContext.DomicileLocations.Any(x => x.Name == domicilelocationCreateDto.PhoneNum);

            if (anyDomicileLocationsExistWithPhoneNum)
            {
                response.AddError("PhoneNum", "This Phone Number is already assigned to another Domicile Location.");
            }

            // SteetAddress1 Errors

            if (string.IsNullOrEmpty(domicilelocationCreateDto.StreetAddress1))
            {
                response.AddError("StreetAddress1", "Street Address 1 should not be empty");
            }

            if (domicilelocationCreateDto.StreetAddress1 != null && domicilelocationCreateDto.StreetAddress1.Length > 50)
            {
                response.AddError("StreetAddress1", "Street Address 1 is too long. Must be less than or equal to 50 characters.");
            }

            var anyDomicileLocationsExistWithStreetAddress1 = _dataContext.DomicileLocations.Any(x => x.StreetAddress1 == domicilelocationCreateDto.StreetAddress1);

            if (anyDomicileLocationsExistWithStreetAddress1)
            {
                response.AddError("StreetAddres1", "This Street Address is already assigned to another Domicile Location.");
            }

            // StreetAddress2 Errors

            if (domicilelocationCreateDto.StreetAddress2 != null && domicilelocationCreateDto.StreetAddress2.Length > 50)
            {
                response.AddError("StreetAddress2", "Street Address 2 is too long. Must be less than or equal to 50 characters.");
            }

            var anyDomicileLocationsExistWithStreetAddress2 = _dataContext.DomicileLocations.Any(x => x.StreetAddress2 == domicilelocationCreateDto.StreetAddress2);

            if (anyDomicileLocationsExistWithStreetAddress2)
            {
                response.AddError("StreetAddres2", "This Street Address is already assigned to another Domicile Location.");
            }

            // City Errors

            if (string.IsNullOrEmpty(domicilelocationCreateDto.City))
            {
                response.AddError("City", "City should not be empty");
            }

            foreach (var character in domicilelocationCreateDto.City)
            {
                var characterAsString = character.ToString();

                var isANumber = int.TryParse(characterAsString, out var _);

                if (isANumber)
                {
                    response.AddError("City", "City can only be letters.");
                    break;
                }

                if (domicilelocationCreateDto.City != null && domicilelocationCreateDto.City.Length > 20)
                {
                    response.AddError("City", "City is too long. Must be less than or equal to 20 characters.");
                }
            }
            // State Errors

            if (string.IsNullOrEmpty(domicilelocationCreateDto.State))
            {
                response.AddError("State", "State should not be empty");
            }

            foreach (var character in domicilelocationCreateDto.State)
            {
                var characterAsString = character.ToString();

                var isANumber = int.TryParse(characterAsString, out var _);

                if (isANumber)
                {
                    response.AddError("State", "State can only be letters.");
                    break;
                }

                if (domicilelocationCreateDto.State != null && domicilelocationCreateDto.State.Length > 13)
                {
                    response.AddError("State", "State is too long. Must be less than or equal to 13 characters.");
                }
            }

            // ZipCode Errors

            if (domicilelocationCreateDto.ZipCode == 0)
            {
                response.AddError("ZipCode", "Zip Code should  not be empty");
            }
            if (domicilelocationCreateDto.ZipCode < 10000)
            {
                response.AddError("ZipCode", "Zip code must be 5 characters");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var domicilelocationToCreate = new DomicileLocation
            {
                Name = domicilelocationCreateDto.Name,
                PhoneNum = domicilelocationCreateDto.PhoneNum,
                StreetAddress1 = domicilelocationCreateDto.StreetAddress1,
                StreetAddress2 = domicilelocationCreateDto.StreetAddress2,
                City = domicilelocationCreateDto.City,
                State = domicilelocationCreateDto.State,
                ZipCode = domicilelocationCreateDto.ZipCode,

            };

            _dataContext.DomicileLocations.Add(domicilelocationToCreate);
            _dataContext.SaveChanges();
            var domicilelocationToReturn = new DomicileLocationGetDto
            {
                Id = domicilelocationToCreate.Id,
                Name = domicilelocationToCreate.Name,
                PhoneNum = domicilelocationToCreate.PhoneNum,
                StreetAddress1 = domicilelocationToCreate.StreetAddress1,
                StreetAddress2 = domicilelocationToCreate.StreetAddress2,
                City = domicilelocationToCreate.City,
                State = domicilelocationToCreate.State,
                ZipCode = domicilelocationToCreate.ZipCode,
            };

            response.Data = domicilelocationToReturn;

            return Ok(response);
        }

        // Get Domicile Location by Id

        [HttpGet("{domicilelocationId:int}")]
        public IActionResult GetDomicileLocationById([FromRoute] int domicilelocationId)
        {
            var response = new Response();

            var domicilelocationToSearchFor = _dataContext.DomicileLocations.FirstOrDefault(x => x.Id == domicilelocationId);

            if (domicilelocationToSearchFor == null)
            {
                response.AddError("Id", $"For the domicile location {domicilelocationId}, no domicile location was found");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }



            var domicilelocationToReturn = new DomicileLocationGetDto
            {
                Id = domicilelocationToSearchFor.Id,
                Name = domicilelocationToSearchFor.Name,
                PhoneNum = domicilelocationToSearchFor.PhoneNum,
                StreetAddress1 = domicilelocationToSearchFor.StreetAddress1,
                StreetAddress2 = domicilelocationToSearchFor.StreetAddress2,
                City = domicilelocationToSearchFor.City,
                State = domicilelocationToSearchFor.State,
                ZipCode = domicilelocationToSearchFor.ZipCode,
            };




            response.Data = domicilelocationToReturn;
            return Ok(response);

        }

        // Get All Domicile Locations

        [HttpGet("get")]
        public IActionResult GetAllDomicileLocations()
        {
            var response = new Response();

            var allDomicileLocationsToReturn = _dataContext.DomicileLocations.Select(x => new DomicileLocationGetDto
            {
                Id = x.Id,
                Name = x.Name,
                PhoneNum = x.PhoneNum,
                StreetAddress1 = x.StreetAddress1,
                StreetAddress2 = x.StreetAddress2,
                City = x.City,
                State = x.State,
                ZipCode = x.ZipCode,

            }).ToList();



            response.Data = allDomicileLocationsToReturn;
            return Ok(response);

        }

        // Edit Domicile Location

        [HttpPut("{domicilelocationId:int}")]
        public IActionResult EditDomicileLocation([FromRoute] int domicilelocationId, [FromBody] DomicileLocationEditDto domicilelocationEditDto)
        {
            var response = new Response();

            var DomicileLocationToEdit = _dataContext.DomicileLocations.FirstOrDefault(x => x.Id == domicilelocationId);

            if (DomicileLocationToEdit == null)
            {
                response.AddError("Id", $"For the domicile location {domicilelocationId}, no domicile location was found");
            }

            // Name Errors

            if (string.IsNullOrEmpty(domicilelocationEditDto.Name))
            {
                response.AddError("Name", "Name should  not be empty.");
            }

            if (domicilelocationEditDto.Name != null && domicilelocationEditDto.Name.Length > 8)
            {
                response.AddError("Name", "Name is too long. Must be less than or equal to 8 characters.");
            }

            if (domicilelocationEditDto.Name != null && domicilelocationEditDto.Name.Length < 3)
            {
                response.AddError("Name", "Name is too short. Must be greater than or equal to 3 characters.");
            }

            var anyDomicileLocationsExistWithName = _dataContext.DomicileLocations.Any(x => x.Name == domicilelocationEditDto.Name);

            if (anyDomicileLocationsExistWithName)
            {
                response.AddError("Name", "This Domicile Location already exists.");
            }

            // PhoneNum Errors

            if (string.IsNullOrEmpty(domicilelocationEditDto.PhoneNum))
            {
                response.AddError("PhoneNum", "Phone Number should not be empty");
            }

            if (domicilelocationEditDto.PhoneNum != null && domicilelocationEditDto.PhoneNum.Length != 10)
            {
                response.AddError("PhoneNum", "Phone Number must be 10 characters");
            }

            foreach (var character in domicilelocationEditDto.PhoneNum)
            {
                var characterAsString = character.ToString();

                var isANumber = int.TryParse(characterAsString, out var _);

                if (!isANumber)
                {
                    response.AddError("PhoneNum", "Phone Number can only be numbers.");
                    break;
                }
            }

            var anyDomicileLocationsExistWithPhoneNum = _dataContext.DomicileLocations.Any(x => x.Name == domicilelocationEditDto.PhoneNum);

            if (anyDomicileLocationsExistWithPhoneNum)
            {
                response.AddError("PhoneNum", "This Phone Number is already assigned to another Domicile Location.");
            }

            // SteetAddress1 Errors

            if (string.IsNullOrEmpty(domicilelocationEditDto.StreetAddress1))
            {
                response.AddError("StreetAddress1", "Street Address should not be empty");
            }

            if (domicilelocationEditDto.StreetAddress1 != null && domicilelocationEditDto.StreetAddress1.Length > 50)
            {
                response.AddError("StreetAddress1", "Street Address is too long. Must be less than or equal to 50 characters.");
            }

            var anyDomicileLocationsExistWithStreetAddress1 = _dataContext.DomicileLocations.Any(x => x.StreetAddress1 == domicilelocationEditDto.StreetAddress1);

            if (anyDomicileLocationsExistWithStreetAddress1)
            {
                response.AddError("StreetAddres1", "This Street Address is already assigned to another Domicile Location.");
            }

            // StreetAddress2 Errors

            if (domicilelocationEditDto.StreetAddress2 != null && domicilelocationEditDto.StreetAddress2.Length > 50)
            {
                response.AddError("StreetAddress2", "Street Address is too long. Must be less than or equal to 50 characters.");
            }

            var anyDomicileLocationsExistWithStreetAddress2 = _dataContext.DomicileLocations.Any(x => x.StreetAddress2 == domicilelocationEditDto.StreetAddress2);

            if (anyDomicileLocationsExistWithStreetAddress2)
            {
                response.AddError("StreetAddres2", "This Street Address is already assigned to another Domicile Location.");
            }

            // City Errors

            if (string.IsNullOrEmpty(domicilelocationEditDto.City))
            {
                response.AddError("City", "City should not be empty");
            }

            foreach (var character in domicilelocationEditDto.City)
            {
                var characterAsString = character.ToString();

                var isANumber = int.TryParse(characterAsString, out var _);

                if (isANumber)
                {
                    response.AddError("City", "City can only be letters.");
                    break;
                }

                if (domicilelocationEditDto.City != null && domicilelocationEditDto.City.Length > 20)
                {
                    response.AddError("City", "City is too long. Must be less than or equal to 20 characters.");
                }
            }
            // State Errors

            if (string.IsNullOrEmpty(domicilelocationEditDto.State))
            {
                response.AddError("State", "State should not be empty");
            }

            foreach (var character in domicilelocationEditDto.State)
            {
                var characterAsString = character.ToString();

                var isANumber = int.TryParse(characterAsString, out var _);

                if (isANumber)
                {
                    response.AddError("State", "State can only be letters.");
                    break;
                }

                if (domicilelocationEditDto.State != null && domicilelocationEditDto.State.Length > 13)
                {
                    response.AddError("State", "State is too long. Must be less than or equal to 13 characters.");
                }
            }

            // ZipCode Errors

            if (domicilelocationEditDto.ZipCode == 0)
            {
                response.AddError("ZipCode", "Zip Code should  not be empty");
            }
            if (domicilelocationEditDto.ZipCode < 10000)
            {
                response.AddError("ZipCode", "Zip code must be 5 characters");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            DomicileLocationToEdit.Name = domicilelocationEditDto.Name;
            DomicileLocationToEdit.PhoneNum = domicilelocationEditDto.PhoneNum;
            DomicileLocationToEdit.StreetAddress1 = domicilelocationEditDto.StreetAddress1;
            DomicileLocationToEdit.StreetAddress2 = domicilelocationEditDto.StreetAddress2;
            DomicileLocationToEdit.City = domicilelocationEditDto.City;
            DomicileLocationToEdit.State = domicilelocationEditDto.State;
            DomicileLocationToEdit.ZipCode = domicilelocationEditDto.ZipCode;

            _dataContext.SaveChanges();

            var domicilelocationToReturn = new DomicileLocationGetDto
            {
                Id = DomicileLocationToEdit.Id,
                Name = DomicileLocationToEdit.Name,
                PhoneNum = DomicileLocationToEdit.PhoneNum,
                StreetAddress1 = DomicileLocationToEdit.StreetAddress1,
                StreetAddress2 = DomicileLocationToEdit.StreetAddress2,
                City = DomicileLocationToEdit.City,
                State = DomicileLocationToEdit.State,
                ZipCode = DomicileLocationToEdit.ZipCode,
            };

            response.Data = domicilelocationToReturn;
            return Ok(response);
        }

        // Delete Domicile Location

        [HttpDelete("{domicilelocationId:int}")]
        public IActionResult DeleteDomicileLocation([FromRoute] int domicilelocationId)
        {
            var response = new Response();

            var domicilelocationToDelete = _dataContext.DomicileLocations.FirstOrDefault(x => x.Id == domicilelocationId);

            if (domicilelocationToDelete == null)
            {
                response.AddError("domicilelocationId", $"For the Domicile Location Id of {domicilelocationId}, no domicile location was found in the database.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }


            _dataContext.Remove(domicilelocationToDelete);
            _dataContext.SaveChanges();

            return Ok(response);

        }
    }
}
