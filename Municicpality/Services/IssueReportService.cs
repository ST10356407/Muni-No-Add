using Municicpality.Models;

namespace Municicpality.Services
{
    // Service to manage in-memory issue reports
    public class IssueReportService
    {
        private readonly IssueReportLinkedList _reports = new IssueReportLinkedList(); // In-memory storage

        // Add a new report to the linked list
        public IssueReportNode AddReport(string location, string category, string description, string attachmentPath)
        {
            return _reports.AddReport(location, category, description, attachmentPath);
        }

        // Expose the linked list so controllers/views can access report
        public IssueReportLinkedList Reports => _reports;

        // Clear all reports from memory
        public void Clear() => _reports.Clear();
    }
}
//Microsoft Learn. (2024). Overview of ASP.NET Core MVC. from Microsoft Learn website: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-9.0Microsoft Learn. (2024). Overview of ASP.NET Core MVC. Retrieved 10 September 2025, from Microsoft Learn website: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-9.0
//Microsoft Q&A. (2025, 24 January). purpose of the services in building asp.net mvc app [Question]. from Microsoft Learn website: https://learn.microsoft.com/en-us/answers/questions/2151062/purpose-of-the-services-in-building-asp-net-mvc-ap