using Microsoft.AspNetCore.Mvc;
using Municicpality.Models;
using Municicpality.Services;
using System.Diagnostics;

namespace Municicpality.Controllers
{
    // Controller to manage local events and announcements functionality
    public class EventsController : Controller
    {
        private readonly EventManagementService _eventService;
        private readonly ILogger<EventsController> _logger;

        // Constructor injection for event service and logger
        public EventsController(EventManagementService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService; // Event management service with advanced data structures
            _logger = logger; // Logger for debugging and monitoring
        }

        // GET: Events/Index - Main events and announcements page
        public IActionResult Index()
        {
            // Get all events and announcements for display
            var allEvents = _eventService.GetAllEvents();
            var categories = _eventService.GetUniqueCategories();
            var recentEvents = _eventService.GetRecentEvents(5);
            var highPriorityEvents = _eventService.GetHighPriorityEvents();
            var trendingCategories = _eventService.GetTrendingCategories(5);

            // Create view model with all necessary data
            var viewModel = new EventsIndexViewModel
            {
                AllEvents = allEvents,
                Categories = categories,
                RecentEvents = recentEvents,
                HighPriorityEvents = highPriorityEvents,
                TrendingCategories = trendingCategories,
                SearchTerm = "",
                SelectedCategory = "",
                SelectedDateFilter = ""
            };

            return View(viewModel);
        }

        // POST: Events/Search - Handle search functionality
        [HttpPost]
        public IActionResult Search(string? searchTerm, string? category, string? dateFilter, string? userSession)
        {
            // Add search to history for recommendations
            if (!string.IsNullOrEmpty(searchTerm) || !string.IsNullOrEmpty(category))
            {
                _eventService.AddSearchHistory(searchTerm ?? string.Empty, category ?? string.Empty, userSession ?? "anonymous");
            }

            // Perform search based on criteria
            List<LocalEvent> searchResults;
            
            // Start with all events if no category is selected, or filter by category first
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                searchResults = _eventService.GetEventsByCategory(category);
            }
            else
            {
                searchResults = _eventService.GetAllEvents();
            }
            
            // Apply text search filter if search term is provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchResults = searchResults.Where(e => 
                    e.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    e.Description.ToLower().Contains(searchTerm.ToLower()) ||
                    e.Location.ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            // Apply date filter if specified
            if (!string.IsNullOrEmpty(dateFilter))
            {
                searchResults = _eventService.FilterEventsByDate(searchResults, dateFilter);
            }

            // Get recommendations based on search
            var recommendations = _eventService.GetRecommendations(userSession ?? "anonymous", 3);

            // Get trending categories for the view
            var trendingCategories = _eventService.GetTrendingCategories(5);

            var viewModel = new EventsIndexViewModel
            {
                AllEvents = searchResults,
                Categories = _eventService.GetUniqueCategories(),
                RecentEvents = _eventService.GetRecentEvents(5),
                HighPriorityEvents = _eventService.GetHighPriorityEvents(),
                TrendingCategories = trendingCategories,
                SearchTerm = searchTerm ?? "",
                SelectedCategory = category ?? "",
                SelectedDateFilter = dateFilter ?? "",
                Recommendations = recommendations
            };

            return View("Index", viewModel);
        }

        // GET: Events/Details - View specific event details
        public IActionResult Details(int id)
        {
            var eventItem = _eventService.GetEventById(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // GET: Events/Create - Form to create new event/announcement
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create - Handle new event/announcement creation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LocalEvent eventModel)
        {
            if (ModelState.IsValid)
            {
                // Add the new event using the service
                var newEvent = _eventService.AddEvent(
                    eventModel.Title,
                    eventModel.Description,
                    eventModel.Category,
                    eventModel.EventDate,
                    eventModel.Location,
                    eventModel.Organizer,
                    eventModel.ContactInfo,
                    eventModel.IsAnnouncement,
                    eventModel.Priority
                );

                TempData["Message"] = eventModel.IsAnnouncement ? 
                    "Announcement created successfully!" : 
                    "Event created successfully!";
                
                return RedirectToAction(nameof(Index));
            }

            return View(eventModel);
        }

        // GET: Events/ByCategory - Get events by specific category
        public IActionResult ByCategory(string? category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToAction(nameof(Index));
            }

            var events = _eventService.GetEventsByCategory(category);
            var trendingCategories = _eventService.GetTrendingCategories(5);
            var recommendations = _eventService.GetRecommendations("anonymous", 3);

            var viewModel = new EventsIndexViewModel
            {
                AllEvents = events,
                Categories = _eventService.GetUniqueCategories(),
                RecentEvents = _eventService.GetRecentEvents(5),
                HighPriorityEvents = _eventService.GetHighPriorityEvents(),
                TrendingCategories = trendingCategories,
                Recommendations = recommendations,
                SearchTerm = "",
                SelectedCategory = category,
                SelectedDateFilter = ""
            };

            return View("Index", viewModel);
        }

        // GET: Events/ByDate - Get events by specific date
        public IActionResult ByDate(DateTime date)
        {
            var events = _eventService.GetEventsByDate(date);
            var viewModel = new EventsIndexViewModel
            {
                AllEvents = events,
                Categories = _eventService.GetUniqueCategories(),
                RecentEvents = _eventService.GetRecentEvents(5),
                HighPriorityEvents = _eventService.GetHighPriorityEvents(),
                SearchTerm = "",
                SelectedCategory = ""
            };

            return View("Index", viewModel);
        }

        // GET: Events/Recommendations - Get personalized recommendations
        public IActionResult Recommendations(string? userSession)
        {
            var recommendations = _eventService.GetRecommendations(userSession ?? "anonymous", 10);
            var viewModel = new EventsIndexViewModel
            {
                AllEvents = recommendations,
                Categories = _eventService.GetUniqueCategories(),
                RecentEvents = _eventService.GetRecentEvents(5),
                HighPriorityEvents = _eventService.GetHighPriorityEvents(),
                SearchTerm = "",
                SelectedCategory = "",
                Recommendations = recommendations
            };

            return View("Index", viewModel);
        }
    }

    // View model for the events index page
    public class EventsIndexViewModel
    {
        public List<LocalEvent> AllEvents { get; set; } = new List<LocalEvent>();
        public HashSet<string> Categories { get; set; } = new HashSet<string>();
        public List<LocalEvent> RecentEvents { get; set; } = new List<LocalEvent>();
        public List<LocalEvent> HighPriorityEvents { get; set; } = new List<LocalEvent>();
        public List<LocalEvent> Recommendations { get; set; } = new List<LocalEvent>();
        public List<string> TrendingCategories { get; set; } = new List<string>();
        public string SearchTerm { get; set; } = "";
        public string SelectedCategory { get; set; } = "";
        public string SelectedDateFilter { get; set; } = "";
    }
}
