using Microsoft.AspNetCore.Mvc;
using Municicpality.Data;
using Municicpality.Models;

namespace Municicpality.Controllers

////Tutorialsteacher.com. (2019).Controller in ASP.NET MVC. [online] Available at: https://www.tutorialsteacher.com/mvc/mvc-controller.
{
    // Controller to manage issue report creation and persistence
    public class IssueReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        // Constructor injection for DB context and hosting environment
        public IssueReportController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context; // Database context for EF Core
            _env = env; // Hosting environment for file uploads
        }

        // GET: IssueReport/Create
        public IActionResult Create() => View(); // Simple Create form view

        // POST: IssueReport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form, IFormFile? attachment)
        {
            // Extract form data
            string location = form["Location"].ToString() ?? string.Empty;
            string category = form["Category"].ToString() ?? string.Empty;
            string description = form["Description"].ToString() ?? string.Empty;
            string attachmentPath = null;

            // Handle file upload if present
            if (attachment != null && attachment.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads); // Ensure folder exists

                var fileName = Path.GetFileName(attachment.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await attachment.CopyToAsync(stream); // Save file to server
                }

                attachmentPath = "/uploads/" + fileName; // Relative path for frontend
            }

            // Create new IssueReport object
            var dbReport = new IssueReport
            {
                Location = location,
                Category = category,
                Description = description,
                AttachmentPath = attachmentPath ?? string.Empty,
                SubmittedAt = DateTime.Now // Timestamp submission
            };

            // Persist to SQL Server
            _context.IssueReports.Add(dbReport); 
            await _context.SaveChangesAsync(); // Commit changes

            TempData["Message"] = "Issue reported successfully!"; // User feedback
            return RedirectToAction(nameof(Create)); // Reload create form
        }
    }
}
