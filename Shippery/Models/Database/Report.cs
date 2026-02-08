using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shippery.Models.Database
{
    public enum ReportClass
    {
        InnapropiateUsername,
        InnapropiateDescription,
        Spamming,
        Grieffing,
    }
    public class Report
    {
        ReportClass type;
        string reportedUser;
        string description;

        public Report() { }
        public Report(string reportedUser) { ReportedUser = reportedUser; }
        public Report(ReportClass type, string description)
        {
            Type = type;
            Description = description;
        }

        public ReportClass Type { get => type; set => type = value; }
        public string Description { get => description; set => description = value; }
        public string ReportedUser { get => reportedUser; set => reportedUser = value; }
    }
}
