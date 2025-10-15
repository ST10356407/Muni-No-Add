# Municipal Services Application for South Africa

A comprehensive web application built with ASP.NET Core MVC that allows citizens to report issues, view local events and announcements, and track service requests with their municipality.

## Features

### Part 1 - Issue Reporting System
- **Report Issues**: Citizens can report various types of issues including sanitation, roads, utilities, and other concerns
- **File Attachments**: Support for uploading images and documents related to reported issues
- **User Engagement**: Dynamic engagement messages to encourage active participation
- **Data Storage**: Uses both in-memory linked list data structure and SQL Server database for persistence

### Part 2 - Local Events and Announcements
- **Event Management**: View upcoming local events and announcements
- **Advanced Search**: Search functionality with category and date filtering
- **Recommendation System**: Personalized recommendations based on user search patterns
- **Advanced Data Structures**: Implementation of stacks, queues, priority queues, hash tables, dictionaries, sorted dictionaries, and sets

### Future Features (Placeholder)
- **Service Request Status**: Track the status of reported issues (to be implemented)

## Technical Requirements

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 or Visual Studio Code
- SQL Server (LocalDB, Express, or Full version)
- Git (for version control)

### Data Structures Implemented

#### Part 1
- **Linked List**: Custom implementation for in-memory issue report storage
- **Entity Framework Core**: For database persistence

#### Part 2
- **Stack**: For managing recently added events (LIFO)
- **Queue**: For managing event processing order (FIFO)
- **Priority Queue**: For managing events by priority using SortedDictionary
- **Hash Table**: Dictionary for fast event lookup by ID
- **Sorted Dictionary**: For events organized by date
- **Set**: HashSet for unique categories
- **Dictionary**: For category-based event organization

## Installation and Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd Municicpality
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Database Setup
The application uses Entity Framework Core with SQL Server. Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MunicipalityDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 4. Run Database Migrations
```bash
dotnet ef database update
```

### 5. Build and Run
```bash
dotnet build
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## Usage Instructions

### Reporting Issues
1. Navigate to the home page
2. Click "Report an Issue"
3. Fill in the required information:
   - Location of the issue
   - Category (Sanitation, Roads, Utilities, Other)
   - Detailed description
   - Optional file attachment
4. Click "Submit Report"
5. Receive confirmation message

### Viewing Events and Announcements
1. From the home page, click "Local Events & Announcements"
2. Browse all events or use the search functionality:
   - Search by title, description, or location
   - Filter by category
   - View personalized recommendations
3. Click on any event for detailed information

### Navigation
- Use the navigation bar to move between sections
- "Back to Home" buttons are available on all pages
- The main menu on the home page provides quick access to all features

## Project Structure

```
Municicpality/
├── Controllers/
│   ├── HomeController.cs          # Main portal controller
│   ├── IssueReportController.cs   # Issue reporting controller
│   └── EventsController.cs        # Events and announcements controller
├── Models/
│   ├── IssueReport.cs             # Database model for issue reports
│   ├── IssueReportLinkedList.cs   # Linked list implementation
│   ├── LocalEvent.cs              # Event/announcement model
│   └── EventSearchHistory.cs      # Search history tracking
├── Services/
│   ├── IssueReportService.cs      # Issue report management service
│   └── EventManagementService.cs  # Event management with advanced data structures
├── Views/
│   ├── Home/                      # Home page views
│   ├── Events/                    # Events and announcements views
│   └── Shared/                    # Shared layout and components
├── Data/
│   └── ApplicationDbContext.cs    # Entity Framework context
└── wwwroot/                       # Static files (CSS, JS, images)
```

## Key Features Explained

### Issue Reporting System
- **Linked List Implementation**: Custom linked list for in-memory storage of issue reports
- **File Upload**: Support for image and document attachments
- **User Engagement**: Dynamic messages to encourage participation
- **Dual Storage**: Both in-memory and database persistence

### Event Management System
- **Advanced Data Structures**: Comprehensive use of various data structures for optimal performance
- **Search Functionality**: Efficient search with multiple criteria
- **Recommendation Engine**: Personalized suggestions based on user behavior
- **Priority Management**: Events organized by priority levels

### User Interface
- **Responsive Design**: Works on desktop and mobile devices
- **Modern Styling**: Glassmorphism design with smooth animations
- **Consistent Theme**: Unified color scheme and typography
- **Accessibility**: Clear labels and intuitive navigation

## Development Notes

### Code Quality
- Comprehensive commenting throughout the codebase
- Consistent naming conventions
- Separation of concerns with proper MVC architecture
- Service layer for business logic

### Performance Considerations
- Efficient data structure usage for different operations
- In-memory caching for frequently accessed data
- Optimized database queries
- Responsive UI with minimal loading times

### Security Features
- Anti-forgery tokens on forms
- Input validation and sanitization
- Secure file upload handling
- SQL injection prevention through Entity Framework

## Troubleshooting

### Common Issues

1. **Database Connection Error**
   - Ensure SQL Server is running
   - Check connection string in appsettings.json
   - Run `dotnet ef database update`

2. **File Upload Issues**
   - Check wwwroot/uploads folder permissions
   - Ensure sufficient disk space
   - Verify file size limits

3. **Build Errors**
   - Run `dotnet clean` then `dotnet build`
   - Ensure all NuGet packages are restored
   - Check .NET version compatibility

### Performance Optimization
- Use the in-memory services for development
- Switch to database-only for production
- Monitor memory usage with large datasets
- Consider implementing pagination for large result sets

## Future Enhancements

### Planned Features
- Service Request Status tracking
- Email notifications
- Mobile app integration
- Advanced reporting and analytics
- Multi-language support
- Geographic mapping integration

### Technical Improvements
- Implement caching strategies
- Add unit and integration tests
- Implement logging and monitoring
- Add API endpoints for mobile access
- Implement real-time updates

## Contributing

This is a student project for educational purposes. The codebase demonstrates:
- Advanced data structure implementation
- MVC architecture patterns
- User interface design principles
- Database integration
- File handling and security

## License

This project is created for educational purposes as part of a university assignment.

## Contact

For questions or issues related to this project, please refer to the academic institution's support channels.

---

**Note**: This application is designed for educational purposes and demonstrates the implementation of various data structures and algorithms in a real-world web application context.


