using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Models.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillChopBE.Services.Models
{
    public class CreateNewGroup : ValidatableModel
    {
        [Required]
        public string Name { get; set; } = null!;

        public Group ToGroup()
        {
            return new Group() { Name = Name };
        }
    }
}
