using System;
using System.ComponentModel.DataAnnotations;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.Validation;

namespace BillChopBE.Services.Models
{
    public class UserWithoutPassword : ValidatableModel
    {
        public UserWithoutPassword() 
        { 
        }

        public UserWithoutPassword(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
        }

        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            var item = obj as UserWithoutPassword;

            if (item == null)
            {
                return false;
            }

            return Id.Equals(item.Id) &&
                Name.Equals(item.Name) &&
                Email.Equals(item.Email);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class UserWithToken: UserWithoutPassword
    {
        public UserWithToken() 
        {
        }

        public UserWithToken(User user, string token): base(user) 
        {
            Token = token;
        }

        [Required]
        public string Token { get; set; } = null!;
    }
}