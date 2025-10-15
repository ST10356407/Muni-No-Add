Overview

This application serves as a digital platform for municipal services, allowing residents to:

Report Issues: Submit and track municipal issues with image attachments.

View Local Events & Announcements: Browse upcoming community events and announcements.

Check Service Request Status: Monitor the status of submitted service requests.

Technology Stack

Framework: ASP.NET Core 8.0 MVC

Database: SQL Server with Entity Framework Core

Frontend: HTML5, CSS3, JavaScript, Bootstrap 5

Icons: Font Awesome 6.0

Styling: Custom glassmorphism design with a blue color theme

Prerequisites

Before running the application, ensure you have the following installed:

.NET 8.0 SDK or later

SQL Server (LocalDB, Express, or Full version)

Visual Studio 2022 or Visual Studio Code

Git (for version control)

Getting Started
1. Clone the Repository
git clone <repository-url>
cd Municicpality

2. Configure the Database

Update the connection string in appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MunicipalityDB;TrustServerCertificate=True;Trusted_Connection=True;"
  }
}


For LocalDB (recommended for development):

"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MunicipalityDB;Trusted_Connection=true;MultipleActiveResultSets=true"

3. Restore Dependencies
dotnet restore

4. Build the Application
dotnet build

5. Run the Application
dotnet run

Features
1. Issue Reporting System

Report municipal issues with location, category, and description.

Upload related photos or documents.

Custom linked list structure used for in-memory issue management.

Dynamic success messages based on issue type.

2. Local Events & Announcements

Browse sample events and community announcements.

Advanced filtering by category, date range, and keyword.

Smart recommendations based on user search patterns.

Display of trending categories.

3. Service Request Status

“Coming Soon” page with placeholder design for future implementation.

Designed for future expansion to include service tracking.

Data Structures Implementation

The application demonstrates practical use of advanced data structures:

Issue Reports

Linked List: Custom IssueReportLinkedList for storing reports in memory.

Entity Framework Core: SQL Server database integration for persistence.

Events Management

Stack: Tracks recently viewed events.

Queue: Handles event processing in order.

Priority Queue: Manages high-priority events.

Hash Table: Fast event lookup by ID.

Dictionary: Organizes events by category.

Sorted Dictionary: Sorts events by date.

HashSet: Maintains a unique list of categories.

Design Features
User Interface

Modern glassmorphism-inspired layout with translucent panels.

Consistent blue color palette for a professional look.

Responsive design for desktop and mobile devices.

Smooth CSS transitions and animations.

User Experience

Simple and intuitive navigation.

Advanced event filtering and search options.

Clear visual feedback for actions and errors.

Accessibility features including proper contrast and keyboard navigation.

API Endpoints
HomeController

GET / – Landing page

GET /Home/Create – Issue report form

POST /Home/Create – Submit issue

GET /Home/Status – Service request status

EventsController

GET /Events – View all events and announcements

POST /Events/Search – Search or filter events

GET /Events/Details/{id} – Event details view

GET /Events/ByCategory/{category} – Filter events by category

IssueReportController

POST /IssueReport/Create – Submit a new issue report
