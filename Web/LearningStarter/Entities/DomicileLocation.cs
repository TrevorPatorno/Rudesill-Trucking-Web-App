using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningStarter.Entities
{
    public class DomicileLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public string StreetAddress1 { get; set; }
        public string? StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public ICollection<Route> Route { get; set; } = new List<Route>();
    }
    public class DomicileLocationCreateDto
    {
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public string StreetAddress1 { get; set; }
        public string? StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
    }
    public class DomicileLocationGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public string StreetAddress1 { get; set; }
        public string? StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
    }
    public class DomicileLocationEditDto
    {
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public string StreetAddress1 { get; set; }
        public string? StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public ICollection <Route> Route { get; set; } = new List<Route>();
    }
    }

