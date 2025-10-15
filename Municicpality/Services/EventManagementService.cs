using Municicpality.Models;
using System.Collections.Generic;
using System.Linq;

namespace Municicpality.Services
{
    // Service to manage local events and announcements using advanced data structures
    public class EventManagementService
    {
        // Stack for managing recently added events (LIFO)
        private readonly Stack<LocalEvent> _recentEvents = new Stack<LocalEvent>();
        
        // Queue for managing event processing order (FIFO)
        private readonly Queue<LocalEvent> _eventProcessingQueue = new Queue<LocalEvent>();
        
        // Priority queue for managing events by priority
        private readonly SortedDictionary<int, Queue<LocalEvent>> _priorityEvents = new SortedDictionary<int, Queue<LocalEvent>>();
        
        // Hash table for fast event lookup by ID
        private readonly Dictionary<int, LocalEvent> _eventLookup = new Dictionary<int, LocalEvent>();
        
        // Sorted dictionary for events organized by date
        private readonly SortedDictionary<DateTime, List<LocalEvent>> _eventsByDate = new SortedDictionary<DateTime, List<LocalEvent>>();
        
        // Set for unique categories
        private readonly HashSet<string> _uniqueCategories = new HashSet<string>();
        
        // Dictionary for category-based event organization
        private readonly Dictionary<string, List<LocalEvent>> _eventsByCategory = new Dictionary<string, List<LocalEvent>>();
        
        // List to store search history for recommendations
        private readonly List<EventSearchHistory> _searchHistory = new List<EventSearchHistory>();
        
        private int _nextId = 1;

        // Constructor to initialize with sample data
        public EventManagementService()
        {
            InitializeSampleData();
        }

        // Initialize the service with sample events and announcements
        private void InitializeSampleData()
        {
            // Add sample events
            AddEvent("Community Clean-up Day", 
                "Join us for a community-wide clean-up initiative. We'll provide gloves, bags, and refreshments. Help keep our neighborhood beautiful!", 
                "Community", 
                DateTime.Now.AddDays(7), 
                "Central Park", 
                "Municipality Environmental Department", 
                "environment@municipality.gov.za", 
                false, 2);

            AddEvent("Road Maintenance Notice", 
                "Scheduled road maintenance on Main Street will begin next week. Expect temporary traffic delays and detours.", 
                "Infrastructure", 
                DateTime.Now.AddDays(3), 
                "Main Street", 
                "Municipality Public Works", 
                "publicworks@municipality.gov.za", 
                true, 1);

            AddEvent("Youth Sports Tournament", 
                "Annual youth soccer tournament for ages 12-18. Registration required. Prizes for winners!", 
                "Sports", 
                DateTime.Now.AddDays(14), 
                "Municipal Sports Complex", 
                "Youth Development Committee", 
                "youth@municipality.gov.za", 
                false, 3);

            AddEvent("Health and Wellness Fair", 
                "Free health screenings, nutrition advice, and fitness demonstrations. All ages welcome!", 
                "Health", 
                DateTime.Now.AddDays(21), 
                "Community Center", 
                "Health Department", 
                "health@municipality.gov.za", 
                false, 3);

            AddEvent("Water Conservation Workshop", 
                "Learn about water-saving techniques and sustainable practices for your home and garden.", 
                "Environment", 
                DateTime.Now.AddDays(10), 
                "Library Meeting Room", 
                "Environmental Education Team", 
                "environment@municipality.gov.za", 
                false, 4);

            AddEvent("Safety Awareness Campaign", 
                "Important safety tips for residents. Learn about emergency procedures and neighborhood watch programs.", 
                "Safety", 
                DateTime.Now.AddDays(5), 
                "Police Station Community Room", 
                "Community Safety Unit", 
                "safety@municipality.gov.za", 
                true, 2);

            AddEvent("Educational Technology Expo", 
                "Discover the latest in educational technology. Free workshops for teachers and parents.", 
                "Education", 
                DateTime.Now.AddDays(28), 
                "High School Auditorium", 
                "Education Department", 
                "education@municipality.gov.za", 
                false, 4);

            AddEvent("Holiday Market", 
                "Local vendors, crafts, and festive activities. Perfect for holiday shopping and family fun!", 
                "Community", 
                DateTime.Now.AddDays(35), 
                "Town Square", 
                "Events Committee", 
                "events@municipality.gov.za", 
                false, 3);

            // Additional sample events to reach 15 total
            AddEvent("Public Library Book Sale", 
                "Annual book sale with thousands of books at discounted prices. Proceeds support library programs.", 
                "Education", 
                DateTime.Now.AddDays(12), 
                "Central Library", 
                "Library Friends Association", 
                "library@municipality.gov.za", 
                false, 4);

            AddEvent("Traffic Light Maintenance", 
                "Scheduled maintenance on traffic lights at Main Street intersection. Expect brief delays.", 
                "Infrastructure", 
                DateTime.Now.AddDays(2), 
                "Main Street & Oak Avenue", 
                "Traffic Department", 
                "traffic@municipality.gov.za", 
                true, 1);

            AddEvent("Senior Citizens Health Fair", 
                "Free health screenings, flu shots, and wellness information for residents 65 and older.", 
                "Health", 
                DateTime.Now.AddDays(18), 
                "Senior Center", 
                "Health Department", 
                "health@municipality.gov.za", 
                false, 3);

            AddEvent("Community Garden Workshop", 
                "Learn sustainable gardening techniques and join our community garden initiative.", 
                "Environment", 
                DateTime.Now.AddDays(25), 
                "Community Garden", 
                "Environmental Group", 
                "environment@municipality.gov.za", 
                false, 4);

            AddEvent("Neighborhood Watch Meeting", 
                "Monthly meeting to discuss community safety and crime prevention strategies.", 
                "Safety", 
                DateTime.Now.AddDays(8), 
                "Community Center", 
                "Police Department", 
                "safety@municipality.gov.za", 
                false, 2);

            AddEvent("Youth Art Exhibition", 
                "Showcase of artwork created by local students. Awards ceremony and refreshments provided.", 
                "Education", 
                DateTime.Now.AddDays(30), 
                "Art Gallery", 
                "Education Department", 
                "education@municipality.gov.za", 
                false, 4);

            AddEvent("Waste Collection Schedule Change", 
                "Due to holiday, waste collection will be delayed by one day next week. Please adjust accordingly.", 
                "Infrastructure", 
                DateTime.Now.AddDays(1), 
                "All Areas", 
                "Waste Management", 
                "waste@municipality.gov.za", 
                true, 2);
        }

        // Add a new event using multiple data structures
        public LocalEvent AddEvent(string title, string description, string category, DateTime eventDate, 
            string location, string organizer = "", string contactInfo = "", bool isAnnouncement = false, int priority = 3)
        {
            var newEvent = new LocalEvent
            {
                Id = _nextId++,
                Title = title,
                Description = description,
                Category = category,
                EventDate = eventDate,
                Location = location,
                Organizer = organizer,
                ContactInfo = contactInfo,
                IsAnnouncement = isAnnouncement,
                CreatedAt = DateTime.Now,
                Priority = priority
            };

            // Add to stack (most recent first)
            _recentEvents.Push(newEvent);
            
            // Add to processing queue
            _eventProcessingQueue.Enqueue(newEvent);
            
            // Add to priority queue
            if (!_priorityEvents.ContainsKey(priority))
                _priorityEvents[priority] = new Queue<LocalEvent>();
            _priorityEvents[priority].Enqueue(newEvent);
            
            // Add to hash table for fast lookup
            _eventLookup[newEvent.Id] = newEvent;
            
            // Add to sorted dictionary by date
            if (!_eventsByDate.ContainsKey(eventDate.Date))
                _eventsByDate[eventDate.Date] = new List<LocalEvent>();
            _eventsByDate[eventDate.Date].Add(newEvent);
            
            // Add to category set and dictionary
            _uniqueCategories.Add(category);
            if (!_eventsByCategory.ContainsKey(category))
                _eventsByCategory[category] = new List<LocalEvent>();
            _eventsByCategory[category].Add(newEvent);

            return newEvent;
        }

        // Get all events using hash table lookup
        public List<LocalEvent> GetAllEvents()
        {
            return _eventLookup.Values.OrderBy(e => e.EventDate).ToList();
        }

        // Get events by category using dictionary
        public List<LocalEvent> GetEventsByCategory(string category)
        {
            return _eventsByCategory.ContainsKey(category) ? _eventsByCategory[category] : new List<LocalEvent>();
        }

        // Get events by date using sorted dictionary
        public List<LocalEvent> GetEventsByDate(DateTime date)
        {
            return _eventsByDate.ContainsKey(date.Date) ? _eventsByDate[date.Date] : new List<LocalEvent>();
        }

        // Get recent events using stack
        public List<LocalEvent> GetRecentEvents(int count = 5)
        {
            var recent = new List<LocalEvent>();
            var tempStack = new Stack<LocalEvent>();
            
            // Pop from original stack and add to temp stack
            for (int i = 0; i < count && _recentEvents.Count > 0; i++)
            {
                var eventItem = _recentEvents.Pop();
                recent.Add(eventItem);
                tempStack.Push(eventItem);
            }
            
            // Restore original stack
            while (tempStack.Count > 0)
            {
                _recentEvents.Push(tempStack.Pop());
            }
            
            return recent;
        }

        // Get high priority events using priority queue
        public List<LocalEvent> GetHighPriorityEvents()
        {
            var highPriority = new List<LocalEvent>();
            
            // Get events with priority 1 and 2
            foreach (var priority in _priorityEvents.Keys.Where(p => p <= 2))
            {
                var queue = _priorityEvents[priority];
                var tempQueue = new Queue<LocalEvent>();
                
                while (queue.Count > 0)
                {
                    var eventItem = queue.Dequeue();
                    highPriority.Add(eventItem);
                    tempQueue.Enqueue(eventItem);
                }
                
                // Restore queue
                while (tempQueue.Count > 0)
                {
                    queue.Enqueue(tempQueue.Dequeue());
                }
            }
            
            return highPriority.OrderBy(e => e.Priority).ThenBy(e => e.EventDate).ToList();
        }

        // Get unique categories using set
        public HashSet<string> GetUniqueCategories()
        {
            return new HashSet<string>(_uniqueCategories);
        }

        // Enhanced search with relevance scoring and fuzzy matching
        public List<LocalEvent> SearchEvents(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAllEvents();

            var term = searchTerm.ToLower().Trim();
            var searchWords = term.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            var scoredResults = new Dictionary<LocalEvent, double>();

            foreach (var eventItem in _eventLookup.Values)
            {
                double score = 0;
                var titleLower = eventItem.Title.ToLower();
                var descriptionLower = eventItem.Description.ToLower();
                var categoryLower = eventItem.Category.ToLower();
                var locationLower = eventItem.Location.ToLower();

                // Exact match bonus
                if (titleLower.Contains(term))
                    score += 100;
                if (descriptionLower.Contains(term))
                    score += 50;
                if (categoryLower.Contains(term))
                    score += 80;
                if (locationLower.Contains(term))
                    score += 60;

                // Word-by-word matching with partial matches
                foreach (var word in searchWords)
                {
                    if (titleLower.Contains(word))
                        score += 20;
                    if (descriptionLower.Contains(word))
                        score += 10;
                    if (categoryLower.Contains(word))
                        score += 15;
                    if (locationLower.Contains(word))
                        score += 12;
                }

                // Priority bonus (higher priority events rank higher)
                score += (6 - eventItem.Priority) * 5;

                // Recency bonus (events happening soon rank higher)
                var daysUntilEvent = (eventItem.EventDate - DateTime.Now).TotalDays;
                if (daysUntilEvent >= 0 && daysUntilEvent <= 7)
                    score += 10;
                else if (daysUntilEvent <= 14)
                    score += 5;
                else if (daysUntilEvent <= 30)
                    score += 2;

                // Announcement bonus
                if (eventItem.IsAnnouncement)
                    score += 8;

                if (score > 0)
                {
                    scoredResults[eventItem] = score;
                }
            }

            return scoredResults
                .OrderByDescending(kvp => kvp.Value)
                .ThenBy(kvp => kvp.Key.EventDate)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        // Enhanced search history tracking with better categorization
        public void AddSearchHistory(string searchTerm, string category, string userSession)
        {
            // Auto-detect category if not provided
            if (string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(searchTerm))
            {
                category = DetectCategoryFromSearchTerm(searchTerm);
            }

            _searchHistory.Add(new EventSearchHistory
            {
                Id = _searchHistory.Count + 1,
                SearchTerm = searchTerm,
                Category = category ?? "General",
                SearchDate = DateTime.Now,
                UserSession = userSession
            });
        }

        // Auto-detect category from search terms
        private string DetectCategoryFromSearchTerm(string searchTerm)
        {
            var term = searchTerm.ToLower();
            
            // Health-related keywords
            if (term.Contains("health") || term.Contains("medical") || term.Contains("doctor") || 
                term.Contains("hospital") || term.Contains("wellness") || term.Contains("fitness"))
                return "Health";

            // Education-related keywords
            if (term.Contains("school") || term.Contains("education") || term.Contains("library") || 
                term.Contains("book") || term.Contains("learning") || term.Contains("student"))
                return "Education";

            // Environment-related keywords
            if (term.Contains("environment") || term.Contains("green") || term.Contains("recycle") || 
                term.Contains("garden") || term.Contains("conservation") || term.Contains("sustainability"))
                return "Environment";

            // Safety-related keywords
            if (term.Contains("safety") || term.Contains("police") || term.Contains("security") || 
                term.Contains("emergency") || term.Contains("crime") || term.Contains("watch"))
                return "Safety";

            // Infrastructure-related keywords
            if (term.Contains("road") || term.Contains("traffic") || term.Contains("construction") || 
                term.Contains("maintenance") || term.Contains("repair") || term.Contains("utility"))
                return "Infrastructure";

            // Sports-related keywords
            if (term.Contains("sport") || term.Contains("game") || term.Contains("tournament") || 
                term.Contains("fitness") || term.Contains("exercise") || term.Contains("athletic"))
                return "Sports";

            // Community-related keywords (default)
            if (term.Contains("community") || term.Contains("event") || term.Contains("meeting") || 
                term.Contains("gathering") || term.Contains("festival") || term.Contains("market"))
                return "Community";

            return "General";
        }

        // Get trending categories based on recent searches
        public List<string> GetTrendingCategories(int count = 5)
        {
            return _searchHistory
                .Where(s => s.SearchDate >= DateTime.Now.AddDays(-7)) // Last 7 days
                .Where(s => !string.IsNullOrEmpty(s.Category))
                .GroupBy(s => s.Category)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(count)
                .ToList();
        }

        // Get user's search preferences
        public Dictionary<string, int> GetUserSearchPreferences(string userSession)
        {
            return _searchHistory
                .Where(s => s.UserSession == userSession)
                .Where(s => !string.IsNullOrEmpty(s.Category))
                .GroupBy(s => s.Category)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // Filter events by date range with accurate date calculations
        public List<LocalEvent> FilterEventsByDate(List<LocalEvent> events, string dateFilter)
        {
            var today = DateTime.Today;
            var now = DateTime.Now;

            return dateFilter.ToLower() switch
            {
                "today" => events.Where(e => e.EventDate.Date == today).ToList(),
                "tomorrow" => events.Where(e => e.EventDate.Date == today.AddDays(1)).ToList(),
                "thisweek" => events.Where(e => e.EventDate >= today && e.EventDate < today.AddDays(7)).ToList(),
                "nextweek" => events.Where(e => e.EventDate >= today.AddDays(7) && e.EventDate < today.AddDays(14)).ToList(),
                "thismonth" => events.Where(e => e.EventDate >= today && e.EventDate < GetFirstDayOfNextMonth(today)).ToList(),
                "nextmonth" => events.Where(e => e.EventDate >= GetFirstDayOfNextMonth(today) && e.EventDate < GetFirstDayOfNextMonth(GetFirstDayOfNextMonth(today))).ToList(),
                _ => events
            };
        }

        // Helper method to get the first day of the next month
        private DateTime GetFirstDayOfNextMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1);
        }

        // Enhanced recommendation system with category relationships and search analysis
        public List<LocalEvent> GetRecommendations(string userSession, int count = 5)
        {
            // Get user's recent searches
            var userSearches = _searchHistory
                .Where(s => s.UserSession == userSession)
                .OrderByDescending(s => s.SearchDate)
                .Take(15)
                .ToList();

            // Get global search patterns for popular categories
            var globalSearches = _searchHistory
                .Where(s => s.SearchDate >= DateTime.Now.AddDays(-30)) // Last 30 days
                .ToList();

            var recommendations = new List<LocalEvent>();

            if (userSearches.Any())
            {
                // Analyze user's search patterns
                var userCategoryFrequency = userSearches
                    .Where(s => !string.IsNullOrEmpty(s.Category))
                    .GroupBy(s => s.Category)
                    .OrderByDescending(g => g.Count())
                    .ThenByDescending(g => g.Max(s => s.SearchDate))
                    .Select(g => new { Category = g.Key, Count = g.Count(), LastSearch = g.Max(s => s.SearchDate) })
                    .ToList();

                // Get related categories based on user's search history
                var relatedCategories = GetRelatedCategories(userCategoryFrequency.Select(c => c.Category).ToList());
                
                // Get global popular categories
                var popularCategories = globalSearches
                    .Where(s => !string.IsNullOrEmpty(s.Category))
                    .GroupBy(s => s.Category)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .Take(5)
                    .ToList();

                // Build recommendations with weighted scoring
                var scoredEvents = new Dictionary<LocalEvent, double>();

                // Score events based on user's preferred categories (highest weight)
                foreach (var userCategory in userCategoryFrequency)
                {
                    var categoryEvents = GetEventsByCategory(userCategory.Category)
                        .Where(e => e.EventDate >= DateTime.Now)
                        .ToList();

                    foreach (var eventItem in categoryEvents)
                    {
                        var score = CalculateEventScore(eventItem, userCategory.Count, userCategory.LastSearch, true);
                        if (!scoredEvents.ContainsKey(eventItem) || scoredEvents[eventItem] < score)
                        {
                            scoredEvents[eventItem] = score;
                        }
                    }
                }

                // Score events from related categories (medium weight)
                foreach (var relatedCategory in relatedCategories)
                {
                    var categoryEvents = GetEventsByCategory(relatedCategory)
                        .Where(e => e.EventDate >= DateTime.Now)
                        .ToList();

                    foreach (var eventItem in categoryEvents)
                    {
                        var score = CalculateEventScore(eventItem, 1, DateTime.Now, false);
                        if (!scoredEvents.ContainsKey(eventItem) || scoredEvents[eventItem] < score)
                        {
                            scoredEvents[eventItem] = score;
                        }
                    }
                }

                // Score popular global events (lower weight)
                foreach (var popularCategory in popularCategories)
                {
                    var categoryEvents = GetEventsByCategory(popularCategory)
                        .Where(e => e.EventDate >= DateTime.Now)
                        .ToList();

                    foreach (var eventItem in categoryEvents)
                    {
                        var score = CalculateEventScore(eventItem, 0.5, DateTime.Now, false);
                        if (!scoredEvents.ContainsKey(eventItem) || scoredEvents[eventItem] < score)
                        {
                            scoredEvents[eventItem] = score;
                        }
                    }
                }

                // Sort by score and take top recommendations
                recommendations = scoredEvents
                    .OrderByDescending(kvp => kvp.Value)
                    .Select(kvp => kvp.Key)
                    .Take(count)
                    .ToList();
            }

            // If no user searches or not enough recommendations, add popular events
            if (recommendations.Count < count)
            {
                var popularEvents = GetPopularEvents(count - recommendations.Count);
                recommendations.AddRange(popularEvents.Where(e => !recommendations.Any(r => r.Id == e.Id)));
            }

            // Final fallback to recent events
            if (recommendations.Count < count)
            {
                var recentEvents = GetRecentEvents(count - recommendations.Count)
                    .Where(e => !recommendations.Any(r => r.Id == e.Id))
                    .Where(e => e.EventDate >= DateTime.Now);
                recommendations.AddRange(recentEvents);
            }

            return recommendations.Take(count).ToList();
        }

        // Calculate event score based on various factors
        private double CalculateEventScore(LocalEvent eventItem, double categoryWeight, DateTime lastSearch, bool isUserCategory)
        {
            double score = 0;

            // Base score from category frequency
            score += categoryWeight * 10;

            // Bonus for user's own categories
            if (isUserCategory)
                score += 5;

            // Priority bonus (higher priority events get higher scores)
            score += (6 - eventItem.Priority) * 2;

            // Recency bonus (events happening soon get higher scores)
            var daysUntilEvent = (eventItem.EventDate - DateTime.Now).TotalDays;
            if (daysUntilEvent <= 7)
                score += 3;
            else if (daysUntilEvent <= 14)
                score += 2;
            else if (daysUntilEvent <= 30)
                score += 1;

            // Announcement bonus (announcements are often more important)
            if (eventItem.IsAnnouncement)
                score += 2;

            // Random factor to add variety (0.8 to 1.2)
            var random = new Random(eventItem.Id);
            score *= (0.8 + random.NextDouble() * 0.4);

            return score;
        }

        // Get related categories based on category relationships
        private List<string> GetRelatedCategories(List<string> userCategories)
        {
            var relatedCategories = new List<string>();
            var categoryRelationships = GetCategoryRelationships();

            foreach (var userCategory in userCategories)
            {
                if (categoryRelationships.ContainsKey(userCategory))
                {
                    relatedCategories.AddRange(categoryRelationships[userCategory]);
                }
            }

            return relatedCategories.Distinct().Where(c => !userCategories.Contains(c)).ToList();
        }

        // Define category relationships for better recommendations
        private Dictionary<string, List<string>> GetCategoryRelationships()
        {
            return new Dictionary<string, List<string>>
            {
                ["Community"] = new List<string> { "Health", "Education", "Safety" },
                ["Health"] = new List<string> { "Community", "Education", "Environment" },
                ["Education"] = new List<string> { "Community", "Health", "Environment" },
                ["Environment"] = new List<string> { "Health", "Education", "Community" },
                ["Safety"] = new List<string> { "Community", "Infrastructure" },
                ["Infrastructure"] = new List<string> { "Safety", "Environment" },
                ["Sports"] = new List<string> { "Community", "Health" }
            };
        }

        // Get popular events based on global search patterns
        private List<LocalEvent> GetPopularEvents(int count)
        {
            var popularCategories = _searchHistory
                .Where(s => s.SearchDate >= DateTime.Now.AddDays(-30))
                .Where(s => !string.IsNullOrEmpty(s.Category))
                .GroupBy(s => s.Category)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(3)
                .ToList();

            var popularEvents = new List<LocalEvent>();
            foreach (var category in popularCategories)
            {
                var events = GetEventsByCategory(category)
                    .Where(e => e.EventDate >= DateTime.Now)
                    .OrderBy(e => e.Priority)
                    .ThenBy(e => e.EventDate)
                    .Take(count - popularEvents.Count);
                
                popularEvents.AddRange(events);
                if (popularEvents.Count >= count) break;
            }

            return popularEvents.Take(count).ToList();
        }

        // Get event by ID using hash table
        public LocalEvent? GetEventById(int id)
        {
            return _eventLookup.ContainsKey(id) ? _eventLookup[id] : null;
        }

        // Clear all data
        public void ClearAll()
        {
            _recentEvents.Clear();
            _eventProcessingQueue.Clear();
            _priorityEvents.Clear();
            _eventLookup.Clear();
            _eventsByDate.Clear();
            _uniqueCategories.Clear();
            _eventsByCategory.Clear();
            _searchHistory.Clear();
            _nextId = 1;
        }
    }
}
