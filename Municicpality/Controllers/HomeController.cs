using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Municicpality.Models;
using Municicpality.Services;
using Municicpality.Data;
using System.Diagnostics;

namespace Municicpality.Controllers
{
    ////Tutorialsteacher.com. (2019).Controller in ASP.NET MVC. [online] Available at: https://www.tutorialsteacher.com/mvc/mvc-controller.
    // HomeController manages the main portal views and issue reporting
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IssueReportService _reportService;
        private readonly ApplicationDbContext _context;

        // Constructor injection for logger, environment, service, and DB context
        public HomeController(
            ILogger<HomeController> logger,
            IWebHostEnvironment env,
            IssueReportService reportService,
            ApplicationDbContext context)
        {
            _logger = logger; // Logger reference
            _env = env; // Web host environment for uploads
            _reportService = reportService; // In-memory linked list service
            _context = context; // EF DB context
        }

        // GET: Home/Index
        public IActionResult Index()
        {
            // Convert linked list to List for easy display in view
          
            var nodes = new List<IssueReportNode>();
            var current = _reportService.Reports.Head;
            while (current != null)
            {
                nodes.Add(current);
                current = current.Next;
            }

            return View(_reportService.Reports); // Pass linked list to view
        }

        // GET: Home/Create
        public IActionResult Create() => View(); // Simple Create page

        // GET: Home/Announcements - Redirect to Events controller
        public IActionResult Announcements() => RedirectToAction("Index", "Events");

        // GET: Home/Status - Placeholder for Service Request Status
        public IActionResult Status() => View(); // Placeholder page

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form, IFormFile? attachment)
        {
            // Retrieve form data submitted by user
            string location = form["Location"].ToString() ?? string.Empty;
            string category = form["Category"].ToString() ?? string.Empty;
            string description = form["Description"].ToString() ?? string.Empty;
            string attachmentPath = null;

            // Handle file upload if attachment exists
            if (attachment != null && attachment.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var fileName = Path.GetFileName(attachment.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await attachment.CopyToAsync(stream);

                attachmentPath = "/uploads/" + fileName; // Relative path for frontend
            }

            // Add issue to in-memory linked list
            var reportNode = _reportService.AddReport(location, category, description, attachmentPath ?? string.Empty);

            // Persist report to SQL Server
            var dbReport = new IssueReport
            {
                Location = reportNode.Location,
                Category = reportNode.Category,
                Description = reportNode.Description,
                AttachmentPath = reportNode.AttachmentPath,
                SubmittedAt = reportNode.SubmittedAt
            };

            _context.IssueReports.Add(dbReport);
            await _context.SaveChangesAsync(); // Save changes in DB

            TempData["Message"] = "Issue reported successfully!"; // Feedback to user
            return RedirectToAction(nameof(Create));
        }

        // GET: Home/Privacy
        public IActionResult Privacy() => View(); // Static privacy page

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); // Error page
    }
}
