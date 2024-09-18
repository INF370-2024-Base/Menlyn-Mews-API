namespace Menlyn_Mews_API.Models.Domain.SignUp
{
    public class RegisterUser
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public string? Client_Name { get; set; } 

        public string? Client_Surname { get; set; } 

        public string? Client_ID_Number { get; set; }

        public string? Client_Email_Address { get; set; } 

        public string? Client_Contact_Number { get; set; }

        public string? Client_Gender { get; set; } 

        public string? Client_Address { get; set; } 

        public string? Title { get; set; } 
    }
}
