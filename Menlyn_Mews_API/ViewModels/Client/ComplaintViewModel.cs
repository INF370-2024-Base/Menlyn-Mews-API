namespace Menlyn_Mews_API.ViewModels.Client
{
    public class ComplaintViewModel
    {
        public string Complaint_Description { get; set; }
        public DateTime Complaint_Date { get; set; }
        public string Complaint_Status { get; set; }
        public int? EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int ComplaintTypeId { get; set; }
    }
}
