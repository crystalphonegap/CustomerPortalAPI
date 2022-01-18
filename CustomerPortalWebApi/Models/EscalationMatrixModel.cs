using System;

namespace CustomerPortalWebApi.Models
{
    public class EscalationMatrixModel
    {
        public long IDbint { get; set; }
        public string CategoryIDint { get; set; }
        public string CategoryNamevtxt { get; set; }

        public string AssignTovtxt { get; set; }
        public int EscalationDays1int { get; set; }

        public string Escalation1AssignTovtxt { get; set; }

        public int EscalationDays2int { get; set; }

        public string Escalation2AssignTovtxt { get; set; }

        public int EscalationDays3int { get; set; }

        public string Escalation3AssignTovtxt { get; set; }

        public string CreatedByvtxt { get; set; }
        public DateTime CreatedDatetimedatetime { get; set; }

        public string Modifyvtxt { get; set; }
        public DateTime ModifyDatetimedatetime { get; set; }
    }
}