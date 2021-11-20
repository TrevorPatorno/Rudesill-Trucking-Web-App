using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningStarter.Entities
{
    public class Route
    {
        public int Id { get; set; }
        public string TimeOfDay { get; set; }
        public bool EnRoute { get; set; }
        public int DomicileLocationId { get; set; }
        public DomicileLocation DomicileLocation { get; set; }
        public ICollection<Truck> Truck { get; set; } = new List<Truck>();


    }

    public class RouteCreateDto
    {
        public string TimeOfDay { get; set; }
        public bool EnRoute { get; set; }
        public int DomicileLocationId { get; set; }
    }

    public class RouteGetDto
    {
        public int Id { get; set; }
        public string TimeOfDay { get; set; }
        public bool EnRoute { get; set; }
        public int DomicileLocationId { get; set; }
    }
}
