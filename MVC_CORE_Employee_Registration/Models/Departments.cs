using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_CORE_Employee_Registration.Models
{
    public class Departments
    {
        [Key]
        public int ID { get; set; }
        public string Department { get; set; }
    }
}
