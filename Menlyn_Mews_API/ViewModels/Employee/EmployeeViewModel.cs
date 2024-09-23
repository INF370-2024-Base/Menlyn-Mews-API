namespace Menlyn_Mews_API.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public string? Employee_Name { get; set; } 
        public string? Employee_Surname { get; set; } 
        public string? Employee_ID_Number { get; set; } 
        public string? Employee_Email_Address { get; set; } 
        public string? Employee_Contact_Number { get; set; } 
        public string? Employee_Gender { get; set; } 
        public string? Employee_Address { get; set; } 
        public string? Employee_Photo { get; set; } 
        public int EmployeeTypeId { get; set; }
        public int PositionId { get; set; }
        public int RateId { get; set; }
    }

}
