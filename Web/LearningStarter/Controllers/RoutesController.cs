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
    [Route("api/routes")]
    public class RoutesController :Controller
    {
        private readonly DataContext _dataContext;

        public RoutesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public IActionResult CreateRoute(RouteCreateDto routeCreateDto)
        {
            var response = new Response();

            if (routeCreateDto == null)
            {
                response.AddError("", "Critical error, please contact admin");
                return BadRequest(response);
            }



            //TimeOfDay errors
                if (string.IsNullOrEmpty(routeCreateDto.TimeOfDay))
                {
                    response.AddError("TimeOfDay", "TimeOfDay should  not be empty");
                }
                //must be morning, afternoon, or night 

            //EnRoute errors
                if (routeCreateDto.EnRoute == null)
                {
                    response.AddError("EnRoute", "EnRoute must be true for false");
                }

            //DomicilceLocationId errors
                var DomicileLocationExistsInItsTable = _dataContext.DomicileLocations.Any(x => x.Id == routeCreateDto.DomicileLocationId);
                if (!DomicileLocationExistsInItsTable)
                {
                    response.AddError("DomicileLocation", "This DomicileLocation does not exists in database");
                }





            if (response.HasErrors)
            {
                return BadRequest(response);
            }


            var routeToCreate = new Route
            {
                TimeOfDay = routeCreateDto.TimeOfDay,
                EnRoute = routeCreateDto.EnRoute,
                DomicileLocationId = routeCreateDto.DomicileLocationId 
            };


            _dataContext.Routes.Add(routeToCreate);
            _dataContext.SaveChanges();
            var routeToReturn = new Route
            {
                TimeOfDay = routeCreateDto.TimeOfDay,
                EnRoute = routeCreateDto.EnRoute,
                DomicileLocationId = routeCreateDto.DomicileLocationId

            };

            response.Data = routeToReturn;

            return Ok(response);



        }









    }
}
