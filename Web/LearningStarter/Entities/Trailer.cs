using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningStarter.Entities
{
    public class Trailer
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public int TrailerNumber { get; set; }
        public int? TruckId { get; set; }
        public Truck Truck{get; set;}

}

public class TrailerCreateDto
    {
        public string Company { get; set; }
        public int TrailerNumber { get; set; }
        public int? TruckId { get; set; }
    }

    public class TrailerGetDto
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public int TrailerNumber { get; set; }
        public int? TruckId { get; set; }
    }
}
