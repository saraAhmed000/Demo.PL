﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? Age { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public int Salary { get; set; }
        public bool IsActive { get; set; }
       
        public string EmailAddress { get; set; }
    
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string ImageName  { get; set; }

        public Department Department { get; set; }  // navigation property of [1]
       
        public int? DepartmentId { get; set; } //ForgeinKey 

    }
}
