Municipal Services Web Application (South Africa)
Overview

This application serves as a digital platform for municipal services, allowing residents to:

Report Issues: Submit and track municipal issues with image attachments.

View Local Events & Announcements: Browse upcoming community events and announcements.

Check Service Request Status: Monitor the progress of submitted service requests.

Technology Stack

Framework: ASP.NET Core 8.0 MVC

Database: SQL Server with Entity Framework Core

Frontend: HTML5, CSS3, JavaScript, Bootstrap 5

Icons: Font Awesome 6.0

Styling: Custom glassmorphism design with a blue color theme

Prerequisites

Before running the application, ensure the following are installed:

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

Submit municipal issues with location, category, and description.

Upload images or supporting documents.

Uses a custom linked list for in-memory issue management.

Displays dynamic success messages based on issue type.

2. Local Events & Announcements

Browse sample community events and announcements.

Filter events by category, date range, or keyword.

Provides smart recommendations based on user searches.

Displays trending categories based on recent interactions.

3. Service Request Status

Includes a “Coming Soon” page with placeholder design.

Built with expansion in mind for future service tracking features.

Data Structures Implementation

The project demonstrates the practical use of advanced data structures.

Issue Reports

Linked List: Custom IssueReportLinkedList for storing reports in memory.

Entity Framework Core: SQL Server integration for persistent storage.

Events Management

Stack: Tracks recently viewed events.

Queue: Handles event processing in order.

Priority Queue: Manages high-priority events.

Hash Table: Provides fast event lookup by ID.

Dictionary: Organizes events by category.

Sorted Dictionary: Sorts events by date.

HashSet: Maintains a unique list of event categories.

Design Features
User Interface

Modern glassmorphism-inspired layout with translucent panels.

Consistent blue color palette for a professional look.

Responsive design optimized for both desktop and mobile.

Smooth CSS transitions and hover animations.

User Experience

Simple and intuitive navigation structure.

Advanced search and filtering options for events.

Clear visual feedback for user actions and validation.

Built with accessibility in mind, including keyboard navigation and contrast compliance.

API Endpoints
HomeController

GET / – Landing page

GET /Home/Create – Issue report form

POST /Home/Create – Submit issue

GET /Home/Status – Service request status

EventsController

GET /Events – View all events and announcements

POST /Events/Search – Search or filter events

GET /Events/Details/{id} – View event details

GET /Events/ByCategory/{category} – Filter events by category

IssueReportController

POST /IssueReport/Create – Submit a new issue report
