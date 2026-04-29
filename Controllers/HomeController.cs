using CCIS.Services;
using Microsoft.AspNetCore.Mvc;

namespace CCIS.Controllers;

public class HomeController : Controller
{
    private readonly MockCcisDataService _dataService;

    public HomeController(MockCcisDataService dataService)
    {
        _dataService = dataService;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(Dashboard));
    }

    public IActionResult Dashboard()
    {
        ApplyPageMetadata("Dashboard", "Real-time access control overview", "dashboard");
        return View(_dataService.GetDashboardViewModel());
    }

    public IActionResult FaceAccess()
    {
        ApplyPageMetadata("Face Access", "Real-time face recognition and access management", "face-access");
        return View(_dataService.GetFaceAccessViewModel());
    }

    public IActionResult Users(string? searchQuery)
    {
        ApplyPageMetadata("User Management", "Manage system users and access permissions", "users");
        return View(_dataService.GetUserManagementViewModel(searchQuery));
    }

    public IActionResult AccessLogs(string? searchQuery, string statusFilter = "all", string directionFilter = "all", string dateFilter = "today")
    {
        ApplyPageMetadata("Access Logs", "View and search access event history", "logs");
        return View(_dataService.GetAccessLogsViewModel(searchQuery, statusFilter, directionFilter, dateFilter));
    }

    public IActionResult AlertsCenter(string filter = "all")
    {
        ApplyPageMetadata("Alerts Center", "Monitor and manage security alerts", "alerts");
        return View(_dataService.GetAlertsCenterViewModel(filter));
    }

    public IActionResult Error()
    {
        ViewData["PageTitle"] = "Application Error";
        ViewData["PageDescription"] = "An unexpected error occurred.";
        ViewData["ActivePage"] = string.Empty;
        return View();
    }

    private void ApplyPageMetadata(string title, string description, string activePage)
    {
        ViewData["PageTitle"] = title;
        ViewData["PageDescription"] = description;
        ViewData["ActivePage"] = activePage;
    }
}