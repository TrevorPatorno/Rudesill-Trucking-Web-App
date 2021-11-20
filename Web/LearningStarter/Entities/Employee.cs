using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningStarter.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SocialSecurityNumber { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset DateOfEmployment { get; set; }
        public string LicenseNumber { get; set; }
        public DateTimeOffset LicenseExpiration { get; set; }
        public DateTimeOffset MedicalExpiration { get; set; }
        public int? TruckId { get; set; }
        public Truck Truck { get; set; }
    }

    public class EmployeeCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SocialSecurityNumber { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public DateTimeOffset? DateOfEmployment { get; set; }
        public string LicenseNumber { get; set; }
        public DateTimeOffset? LicenseExpiration { get; set; }
        public DateTimeOffset? MedicalExpiration { get; set; }
        public int? TruckId { get; set; }

    }

    public class EmployeeGetDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset DateOfEmployment { get; set; }
        public string LicenseNumber { get; set; }
        public DateTimeOffset LicenseExpiration { get; set; }
        public DateTimeOffset MedicalExpiration { get; set; }
        public int? TruckId { get; set; }


    }

    public class EmployeeEditDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public DateTimeOffset? DateOfEmployment { get; set; }
        public string LicenseNumber { get; set; }
        public DateTimeOffset? LicenseExpiration { get; set; }
        public DateTimeOffset? MedicalExpiration { get; set; }
        public int? TruckId { get; set; }


    }
}
