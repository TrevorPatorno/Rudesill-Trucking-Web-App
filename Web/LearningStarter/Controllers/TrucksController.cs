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
    [Route("/api/trucks")]
    public class TrucksController : Controller
    {
        private readonly DataContext _dataContext;

        public TrucksController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }



        [HttpPost("create")] //Create truck
        public IActionResult CreateTruck([FromBody]TruckCreateDto truckCreateDto)
        {
            var response = new Response();

            if (truckCreateDto == null)
            {
                response.AddError("", "Critical error, please contact admin");
                return BadRequest(response);
            }

            //Vin errors  
                if (string.IsNullOrEmpty(truckCreateDto.Vin))
                {
                    response.AddError("Vin", "Vin should  not be empty");
                }
                if (truckCreateDto.Vin != null && truckCreateDto.Vin.Length != 17)
                {
                    response.AddError("Vin", "Vin must be 17 characters");
                }


                var anyTrucksExistWithVin = _dataContext.Trucks.Any(x => x.Vin == truckCreateDto.Vin);
                if (anyTrucksExistWithVin)
                {
                    response.AddError("Vin", "another truck already has this Vin");
                }
            //licenseplate errors
                if (string.IsNullOrEmpty(truckCreateDto.LicensePlate))
                {
                    response.AddError("LicensePlate", "license plate should  not be empty");
                }
                if (truckCreateDto.LicensePlate != null && (truckCreateDto.LicensePlate.Length < 5 || truckCreateDto.LicensePlate.Length >8))
                {
                    response.AddError("LicensePlate", "license plate must be between 5 and 8 characters");
                }


                var anyTrucksExistWithLicensePlate = _dataContext.Trucks.Any(x => x.LicensePlate == truckCreateDto.LicensePlate);
                if (anyTrucksExistWithLicensePlate)
                {
                    response.AddError("LicensePlate", "another truck already has this license plate");
                }


            //TruckNumber errors
                if (truckCreateDto.TruckNumber == 0) 
                {
                    response.AddError("TruckNumber", "truck Number should  not be empty");
                }
                if (truckCreateDto.TruckNumber<100000)
                {
                    response.AddError("TruckNumber", "truck Number must be 6 characters");
                }
                

                var anyTrucksExistWithTruckNumber = _dataContext.Trucks.Any(x => x.TruckNumber == truckCreateDto.TruckNumber);
                if (anyTrucksExistWithTruckNumber)
                {
                    response.AddError("TruckNumber", "another truck already has this truck number");
                }

            //DateArrivedAtLocation errors
                var year1900 = new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.Zero);
                if (truckCreateDto.DateArrivedAtLocation <= year1900) 
                 {
                     response.AddError("DateArrivedAtLocation", "DateArrivedAtLocation must be after 1900");
                 }
            //RouteId errors
                if (truckCreateDto.RouteId == 0)
                {
                    truckCreateDto.RouteId = null;
                }
                else
                {
                    var RouteIdExistsInRouteTable = _dataContext.Routes.Any(x => x.Id == truckCreateDto.RouteId);
                    if (!RouteIdExistsInRouteTable)
                    {
                        response.AddError("RouteId", "This RouteId doesn't exist");
                    }
                }



            if (response.HasErrors)
            {
                return BadRequest(response);
            }




            var truckToCreate = new Truck
            {
                Vin = truckCreateDto.Vin,
                LicensePlate = truckCreateDto.LicensePlate,
                TruckNumber = truckCreateDto.TruckNumber,
                DateArrivedAtLocation = truckCreateDto.DateArrivedAtLocation,
                RouteId = truckCreateDto.RouteId  

            };

            _dataContext.Trucks.Add(truckToCreate);
            _dataContext.SaveChanges();
            var truckToReturn = new TruckGetDto
            {
                Id = truckToCreate.Id,
                Vin = truckToCreate.Vin,
                LicensePlate = truckToCreate.LicensePlate,
                TruckNumber = truckToCreate.TruckNumber,
                DateArrivedAtLocation = truckToCreate.DateArrivedAtLocation,
                RouteId = truckToCreate.RouteId 
            };

            response.Data = truckToReturn;

            return Ok(response);
        }



        [HttpGet("{truckId:int}")] //get by Id
        public IActionResult GetTruckById([FromRoute]int truckId)
        {
            var response = new Response();
            
            var truckToSearchFor = _dataContext.Trucks.FirstOrDefault(x => x.Id == truckId);

            if (truckToSearchFor == null)
            {
                response.AddError("Id", $"For the truck {truckId}, no truck was found");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }



            var truckToReturn = new TruckGetDto
            {
                Id = truckToSearchFor.Id,
                Vin = truckToSearchFor.Vin,
                LicensePlate = truckToSearchFor.LicensePlate,
                TruckNumber = truckToSearchFor.TruckNumber,
                DateArrivedAtLocation = truckToSearchFor.DateArrivedAtLocation,
                RouteId = truckToSearchFor.RouteId
            };




            response.Data = truckToReturn;
            return Ok(response);

        }


        [HttpGet] //Get All
        public IActionResult GetAllTrucks()
        {
            var response = new Response();

            var allTrucksToReturn = _dataContext.Trucks.Select(x => new TruckGetDto
            {
                Id = x.Id,
                Vin = x.Vin,
                LicensePlate = x.LicensePlate,
                TruckNumber = x.TruckNumber,
                DateArrivedAtLocation = x.DateArrivedAtLocation,
                RouteId = x.RouteId,
        
            }).ToList();

                      

            response.Data = allTrucksToReturn;
            return Ok(response);

        }



        [HttpPut("{truckId:int}")] //Update Truck
        public IActionResult EditTruck ([FromRoute]int truckId,[FromBody]TruckEditDto truckEditDto)
        {
            var response = new Response();

            var TruckToEdit = _dataContext.Trucks.FirstOrDefault(x => x.Id == truckId);

            if (TruckToEdit == null)
            {
                response.AddError("Id", $"For the truck {truckId}, no truck was found");
            }

            //LicensePlate errors
                if (string.IsNullOrEmpty(truckEditDto.LicensePlate))
                {
                    response.AddError("LicensePlate", "license plate should  not be empty");
                }
                if (truckEditDto.LicensePlate != null && (truckEditDto.LicensePlate.Length < 5 || truckEditDto.LicensePlate.Length > 8))
                {
                    response.AddError("LicensePlate", "license plate must be between 5 and 8 characters");
                }


                var anyTrucksExistWithLicensePlate = _dataContext.Trucks.Any(x => x.LicensePlate == truckEditDto.LicensePlate);
                if (anyTrucksExistWithLicensePlate)
                {
                    response.AddError("LicensePlate", "another truck already has this license plate");
                }
            //TruckNumber errors
                if (truckEditDto.TruckNumber == 0)
                {
                    response.AddError("TruckNumber", "truck Number should  not be empty");
                }
                if (truckEditDto.TruckNumber < 100000)
                {
                    response.AddError("TruckNumber", "truck Number must be 6 characters");
                }


                var anyTrucksExistWithTruckNumber = _dataContext.Trucks.Any(x => x.TruckNumber == truckEditDto.TruckNumber);
                if (anyTrucksExistWithTruckNumber)
                {
                    response.AddError("TruckNumber", "another truck already has this truck number");
                }
            //DateArrivedAtLocation errors
                var year1900 = new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.Zero);
                if (truckEditDto.DateArrivedAtLocation <= year1900)
                {
                    response.AddError("DateArrivedAtLocation", "DateArrivedAtLocation must be after 1900");
                }
            //RouteId errors
                if (truckEditDto.RouteId == 0)
                {
                    truckEditDto.RouteId = null;
                }
                else
                {
                    var RouteIdExistsInRouteTable = _dataContext.Routes.Any(x => x.Id == truckEditDto.RouteId);
                    if (!RouteIdExistsInRouteTable)
                    {
                        response.AddError("RouteId", "This RouteId doesn't exist");
                    }
                }


            if (response.HasErrors)
            {
                return BadRequest(response);
            }


            TruckToEdit.LicensePlate = truckEditDto.LicensePlate;
            TruckToEdit.TruckNumber = truckEditDto.TruckNumber;
            TruckToEdit.DateArrivedAtLocation = truckEditDto.DateArrivedAtLocation;
            TruckToEdit.RouteId = truckEditDto.RouteId;

            _dataContext.SaveChanges();

            var truckToReturn = new TruckGetDto
            {
                Id = TruckToEdit.Id,
                Vin = TruckToEdit.Vin,
                LicensePlate = TruckToEdit.LicensePlate,
                TruckNumber = TruckToEdit.TruckNumber,
                DateArrivedAtLocation = TruckToEdit.DateArrivedAtLocation,
                RouteId = TruckToEdit.RouteId
            };

            response.Data = truckToReturn;
            return Ok(response);


        }



        /* [HttpPut("{truckNumber:int}")] //updates date arrived using the truck number
         public IActionResult EditTruckDate([FromRoute] int truckNumber, [FromBody]TruckDateEditDto truckDateEditDto)
         {
             var response = new Response();

             var TruckToEdit = _dataContext.Trucks.FirstOrDefault(x => x.TruckNumber == truckNumber);

             if (TruckToEdit == null)
             {
                 response.AddError("TruckNumber", $"For the truck {truckNumber}, no truck was found");
             }




             //DateArrivedAtLocation errors
                 var year1900 = new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.Zero);
                 if (truckDateEditDto.DateArrivedAtLocation <= year1900)
                 {
                     response.AddError("DateArrivedAtLocation", "DateArrivedAtLocation must be after 1900");
                 }



             if (response.HasErrors)
             {
                 return BadRequest(response);
             }


             TruckToEdit.DateArrivedAtLocation = truckDateEditDto.DateArrivedAtLocation;

             _dataContext.SaveChanges();

             var truckToReturn = new TruckGetDto
             {
                 Id = TruckToEdit.Id,
                 Vin = TruckToEdit.Vin,
                 LicensePlate = TruckToEdit.LicensePlate,
                 TruckNumber = TruckToEdit.TruckNumber,
                 DateArrivedAtLocation = TruckToEdit.DateArrivedAtLocation,
                 RouteId = TruckToEdit.RouteId
             };

             response.Data = truckToReturn;
             return Ok(response);


         }*/

        [HttpDelete("{truckIdId:int}")]
        public IActionResult DeleteTruck([FromRoute] int truckId)
        {
            var response = new Response();

            var truckToDelete = _dataContext.Employees.FirstOrDefault(x => x.Id == truckId);

            if (truckToDelete == null)
            {
                response.AddError("truckId", $"For the truckId of {truckId}, no truck was found in the database.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }


            _dataContext.Remove(truckToDelete);
            _dataContext.SaveChanges();

            return Ok(response);

        }

       }
}
