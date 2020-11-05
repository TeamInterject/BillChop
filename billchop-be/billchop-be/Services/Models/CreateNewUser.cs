using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BillChopBE.Services.Models
{
    public class CreateNewUser : ValidatableModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        public User ToUser()
        {
            return new User() { Name = Name, Email = Email };
        }
    }
}
