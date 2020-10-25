using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.DataAccessLayer.Models.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.DataAccessLayer.Models
{
    public class Group : ValidatableModel, IDbModel
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public virtual List<User> Users { get; set; } = new List<User>();
        
        [JsonIgnore]
        public virtual IEnumerable<Bill> Bills { get; set; } = new List<Bill>();
    }
}
