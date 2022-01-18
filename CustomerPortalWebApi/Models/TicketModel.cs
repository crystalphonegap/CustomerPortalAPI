using System;

namespace CustomerPortalWebApi.Models
{
    public class TicketModel
    {
        public long Idbint { get; set; }
        public long IDbint { get; set; }
        public string RefNovtxt { get; set; }
        public DateTime RefDatedate { get; set; }
        public string CustomerCodevtxt { get; set; }
        public string CustomerNamevtxt { get; set; }

        public int? Departmentidint { get; set; }

        public string DepartmentNamevtxt { get; set; }
        public string AssignTo { get; set; }
        public string Typevtxt { get; set; }

        public string Subjectvtxt { get; set; }

        public string Descriptionvtxt { get; set; }

        public string Statusvtxt { get; set; }
        public string Actionvtxt { get; set; }

        public string AttachmentFile { get; set; }

        public string AttachmentFileNamevtxt { get; set; }

        public string AttachmentFilePathvtxt { get; set; }

        public string CreatedByvtxt { get; set; }
        public DateTime Createddatetimedatetime { get; set; }
        public DateTime TicketClosedDatedate { get; set; }
        public int Ageing { get; set; }

        public int Edit { get; set; }

        public long HIDbint { get; set; }

        public string UserCodevtxt { get; set; }
        public string UserNametxt { get; set; }
        public string UserTypevtxt { get; set; }
        public string Remarksvtxt { get; set; }
        public string Priorityvtxt { get; set; }

        public string CustomerEmail { get; set; }

        public string EmployeeEmail { get; set; }

        public string DetailCreatedBy { get; set; }

        public string DetailDate { get; set; }

        public string NormalComplaintCount { get; set; }
        public string LowComplaintCount { get; set; }
        public string HighComplaintCount { get; set; }
        public string LowFeedBackCount { get; set; }
        public string NormalFeedBackCount { get; set; }
        public string HighFeedBackCount { get; set; }
        public string Total { get; set; }
        public string FirstDays { get; set; }
        public string SecondDays { get; set; }
        public string ThirdDays { get; set; }
        public string FourthDays { get; set; }
        public string FithDays { get; set; }
        public string AssignTovtxt { get; set; }
    }
}