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
    [Route("api/trailers")]
    public class TrailersController :Controller
    {
        private readonly DataContext _dataContext;

        public TrailersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpPost]
        public IActionResult CreateTrailer(TrailerCreateDto trailerCreateDto)
        {
            var response = new Response();
            if (trailerCreateDto == null)
            {
                response.AddError("", "Critical error, please contact admin");
                return BadRequest(response);
            }

            //Company errors
                if (string.IsNullOrEmpty(trailerCreateDto.Company))
                {
                    response.AddError("Company", "Company should  not be empty");
                }
                if (trailerCreateDto.Company != null && trailerCreateDto.Company.Length > 30)
                {
                    response.AddError("Company", "Company must be less than 30 characters");
                }

            //TrailerNumber errors
                if (trailerCreateDto.TrailerNumber == 0)
                {
                    response.AddError("TrailerNumber", "TrailerNumber should  not be empty");
                }
                if (trailerCreateDto.TrailerNumber < 100000)
                {
                    response.AddError("TrailerNumber", "TrailerNumber must be 6 characters");
                }
                var anyTrailersExistWithTrailerNumber = _dataContext.Trailers.Any(x => x.TrailerNumber == trailerCreateDto.TrailerNumber);
                if (anyTrailersExistWithTrailerNumber)
                {
                    response.AddError("TrailerNumber", "another trailer already has this trailer number");
                }
            //TruckId errors
                if (trailerCreateDto.TruckId == 0)
                {
                    trailerCreateDto.TruckId = null;                   
                }
                    else
                    {
                        //checking if the TruckId actaully exists in Truck table
                        var anyTrailersExistWithTruckTable = _dataContext.Trucks.Any(x => x.Id == trailerCreateDto.TruckId);
                        if (!anyTrailersExistWithTruckTable)
                        {
                            response.AddError("TruckId", "This TruckId doesn't exist");
                        }

                        //checking if TruckId exists 2 or less times in Trailer table
                        var TruckIdExistsMoreThanTwice = _dataContext.Trailers.Count(x => x.TruckId == trailerCreateDto.TruckId);
                        if (TruckIdExistsMoreThanTwice>= 2)
                        {
                            response.AddError("TruckId", "This TruckId is already being used twice");
                        }
                    }
               





            if (response.HasErrors)
            {
                return BadRequest(response);
            }



            var trailerToCreate = new Trailer
            {
                Company = trailerCreateDto.Company,
                TrailerNumber = trailerCreateDto.TrailerNumber,
                TruckId = trailerCreateDto.TruckId
            };

            _dataContext.Trailers.Add(trailerToCreate);
            _dataContext.SaveChanges();

            var trailerToReturn = new TrailerGetDto
            {
                Company = trailerCreateDto.Company,
                TrailerNumber = trailerCreateDto.TrailerNumber,
                TruckId = trailerCreateDto.TruckId
            };

            response.Data = trailerToReturn;

            return Ok(response);







        }








    }
}
