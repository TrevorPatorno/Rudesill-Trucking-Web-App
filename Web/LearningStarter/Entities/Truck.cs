using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningStarter.Entities
{
    public class Truck
    {
        public int Id { get; set; }
        public string Vin { get; set; }
        public string LicensePlate { get; set; }
        public int TruckNumber { get; set; }
        public DateTimeOffset DateArrivedAtLocation { get; set; } 
        public int? RouteId { get; set; }
        public Route Route { get; set; }

        public ICollection <Trailer> Trailer { get; set; } = new List<Trailer>();
        public ICollection<Employee> Employee { get; set; } = new List<Employee>();

    }

    public class TruckCreateDto
    {
        public string Vin { get; set; }
        public string LicensePlate { get; set; }
        public int TruckNumber { get; set; }
        public DateTimeOffset DateArrivedAtLocation { get; set; } 
        public int? RouteId { get; set; }
    }

    public class TruckGetDto
    {
        public int Id { get; set; }
        public string Vin { get; set; }  // we may have to change this based on user roles
        public string LicensePlate { get; set; } // we may have to change this based on user roles
        public int TruckNumber { get; set; }
        public DateTimeOffset DateArrivedAtLocation { get; set; } 
        public int? RouteId { get; set; }
    }

    public class TruckEditDto
    {
        public string LicensePlate { get; set; }
        public int TruckNumber { get; set; }
        public DateTimeOffset DateArrivedAtLocation { get; set; }
        public int? RouteId { get; set; }
    }

    public class TruckDateEditDto //for future use
    {
       
        public DateTimeOffset DateArrivedAtLocation { get; set; }
       
    }

}
