using System.ComponentModel.DataAnnotations;
using BillChopBE.DataAccessLayer.Models;

namespace BillChopBE.Services.Models
{
    public class UserWithToken: User
    {        
        public UserWithToken(User user, string token)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Password  = user.Password;
            Groups = user.Groups;
            Loans = user.Loans;
            Bills = user.Bills;
            PaymentsMade = user.PaymentsMade;
            PaymentsReceived = user.PaymentsReceived;
            Token = token;
        }

        [Required]
        public string Token { get; set; }
    }
}