using CCIS.Models;

namespace CCIS.Services;

public class MockCcisDataService
{
    private readonly IReadOnlyList<CcisUser> _users;
    private readonly IReadOnlyList<CcisAccessLog> _accessLogs;
    private readonly IReadOnlyList<CcisAlert> _alerts;

    public MockCcisDataService()
    {
        _users = CreateUsers();
        _accessLogs = CreateAccessLogs();
        _alerts = CreateAlerts();
    }

    public DashboardViewModel GetDashboardViewModel()
    {
        var kpis = new List<DashboardKpiViewModel>
        {
            new("Entries Today", "127", "+12%", "metric-success", "trend-up"),
            new("Exits Today", "98", "+8%", "metric-primary", "trend-up"),
            new("Current Occupancy", "29", "Active", "metric-warning", "trend-neutral"),
            new("Unauthorized Attempts", "5", "+2", "metric-danger", "trend-down"),
        };

        var recentEvents = _accessLogs
            .OrderByDescending(log => log.Timestamp)
            .Take(5)
            .Select(log => new RecentEventViewModel(
                log.UserName,
                log.EmployeeId ?? "Unknown",
                log.Location,
                log.Status,
                log.Direction,
                log.Timestamp,
                CcisFormatting.TimeAgo(log.Timestamp)))
            .ToList();

        var systemStatus = new List<SystemStatusViewModel>
        {
            new("Cameras", "8/10", "online", "metric-success"),
            new("Database", "12ms", "online", "metric-primary"),
            new("Recognition Service", "97.8%", "online", "metric-warning"),
        };

        var presenceSummary = new List<PresenceItemViewModel>
        {
            new("Security Guards", "4 on duty"),
            new("Engineers", "12 on site"),
            new("IT Staff", "8 on site"),
            new("Visitors", "3 in lobby"),
        };

        return new DashboardViewModel(kpis, recentEvents, systemStatus, presenceSummary, DateTime.Now.ToString("MMM d, HH:mm"));
    }

    public FaceAccessViewModel GetFaceAccessViewModel()
    {
        var quickActions = new List<FaceQuickActionViewModel>
        {
            new("Check In", "success"),
            new("Check Out", "warning"),
            new("Register Face", "primary"),
            new("Manual Override", "danger"),
        };

        var recentEvents = _accessLogs
            .OrderByDescending(log => log.Timestamp)
            .Take(5)
            .Select(log => new RecentEventViewModel(
                log.UserName,
                log.EmployeeId ?? "Unknown",
                log.Location,
                log.Status,
                log.Direction,
                log.Timestamp,
                CcisFormatting.TimeAgo(log.Timestamp)))
            .ToList();

        var recognition = new RecognitionResultViewModel(
            "John Smith",
            "EMP001",
            "granted",
            98,
            CcisFormatting.Initials("John Smith"),
            DateTime.Now);

        return new FaceAccessViewModel(false, recognition, recentEvents, quickActions);
    }

    public UserManagementViewModel GetUserManagementViewModel(string? searchQuery)
    {
        var query = searchQuery?.Trim() ?? string.Empty;

        var users = _users
            .Where(user => string.IsNullOrWhiteSpace(query)
                || user.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                || user.EmployeeId.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Select(user => new UserRowViewModel(
                user.Id,
                user.Name,
                user.EmployeeId,
                user.Role,
                user.Department,
                user.Status,
                user.FaceEnrolled,
                user.LastAccess,
                CcisFormatting.Initials(user.Name)))
            .ToList();

        return new UserManagementViewModel(query, users);
    }

    public AccessLogsViewModel GetAccessLogsViewModel(string? searchQuery, string statusFilter, string directionFilter, string dateFilter)
    {
        var query = searchQuery?.Trim() ?? string.Empty;

        var filteredLogs = _accessLogs
            .Where(log => string.IsNullOrWhiteSpace(query)
                || log.UserName.Contains(query, StringComparison.OrdinalIgnoreCase)
                || (log.EmployeeId?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false))
            .Where(log => statusFilter == "all" || log.Status == statusFilter)
            .Where(log => directionFilter == "all" || log.Direction == directionFilter)
            .Where(log => MatchesDateFilter(log.Timestamp, dateFilter))
            .OrderByDescending(log => log.Timestamp)
            .Select(log => new AccessLogRowViewModel(
                log.Id,
                log.UserName,
                log.EmployeeId ?? "—",
                log.Direction,
                log.Status,
                log.Timestamp,
                log.Confidence.ToString(),
                log.Location,
                CcisFormatting.Initials(log.UserName)))
            .ToList();

        var summaries = new List<AccessLogSummaryViewModel>
        {
            new("Total Events", filteredLogs.Count.ToString(), "summary-total"),
            new("Granted Access", filteredLogs.Count(log => log.Status == "granted").ToString(), "summary-success"),
            new("Denied Access", filteredLogs.Count(log => log.Status == "denied").ToString(), "summary-danger"),
            new("Unknown Persons", filteredLogs.Count(log => log.UserName == "Unknown").ToString(), "summary-warning"),
        };

        return new AccessLogsViewModel(query, statusFilter, directionFilter, dateFilter, summaries, filteredLogs);
    }

    public AlertsCenterViewModel GetAlertsCenterViewModel(string filter)
    {
        var filteredAlerts = _alerts
            .Where(alert => filter == "all"
                || (filter == "unacknowledged" && !alert.Acknowledged)
                || (filter == "acknowledged" && alert.Acknowledged)
                || alert.Type == filter)
            .OrderByDescending(alert => alert.Timestamp)
            .Select(alert => new AlertCardViewModel(
                alert.Id,
                alert.Type,
                alert.Title,
                alert.Message,
                alert.Location,
                alert.Timestamp,
                alert.Acknowledged,
                alert.Type switch
                {
                    "critical" => "accent-danger",
                    "warning" => "accent-warning",
                    _ => "accent-primary"
                },
                alert.Type switch
                {
                    "critical" => "Security",
                    "warning" => "Warning",
                    _ => "Normal"
                }))
            .ToList();

        return new AlertsCenterViewModel(
            filter,
            _alerts.Count,
            _alerts.Count(alert => alert.Type == "critical"),
            _alerts.Count(alert => alert.Type == "warning"),
            _alerts.Count(alert => !alert.Acknowledged),
            filteredAlerts);
    }

    private static bool MatchesDateFilter(DateTime timestamp, string dateFilter)
    {
        var now = DateTime.Now;

        return dateFilter switch
        {
            "week" => timestamp >= now.AddDays(-7),
            "month" => timestamp >= now.AddDays(-30),
            "all" => true,
            _ => timestamp.Date == now.Date,
        };
    }

    private static IReadOnlyList<CcisUser> CreateUsers() =>
    [
        new(1, "John Smith", "EMP001", "Security Guard", "Security", "active", true, DateTime.Now.AddHours(-1)),
        new(2, "Sarah Johnson", "EMP002", "Operations Engineer", "Operations", "active", true, DateTime.Now.AddHours(-2)),
        new(3, "Michael Chen", "EMP003", "IT Administrator", "IT", "active", true, DateTime.Now.AddMinutes(-30)),
        new(4, "Emily Davis", "EMP004", "Security Guard", "Security", "active", true, DateTime.Now.AddHours(-1.5)),
        new(5, "Robert Wilson", "EMP005", "Operations Engineer", "Operations", "inactive", true, DateTime.Now.AddDays(-1)),
        new(6, "Lisa Anderson", "EMP006", "IT Administrator", "IT", "active", false, DateTime.Now.AddDays(-2)),
        new(7, "David Martinez", "EMP007", "Security Guard", "Security", "active", true, DateTime.Now.AddMinutes(-15)),
        new(8, "Jennifer Taylor", "EMP008", "Operations Engineer", "Operations", "active", true, DateTime.Now.AddMinutes(-45)),
    ];

    private static IReadOnlyList<CcisAccessLog> CreateAccessLogs() =>
    [
        new(1, 1, "John Smith", "EMP001", "in", "granted", DateTime.Now.AddMinutes(-5), 98, "Main Gate"),
        new(2, 2, "Sarah Johnson", "EMP002", "out", "granted", DateTime.Now.AddMinutes(-15), 95, "Main Gate"),
        new(3, null, "Unknown", null, "in", "denied", DateTime.Now.AddMinutes(-25), 45, "Main Gate"),
        new(4, 3, "Michael Chen", "EMP003", "in", "granted", DateTime.Now.AddMinutes(-35), 99, "Side Entrance"),
        new(5, 4, "Emily Davis", "EMP004", "in", "granted", DateTime.Now.AddMinutes(-45), 92, "Main Gate"),
        new(6, 5, "Robert Wilson", "EMP005", "out", "granted", DateTime.Now.AddMinutes(-55), 88, "Main Gate"),
        new(7, null, "Unknown", null, "in", "denied", DateTime.Now.AddMinutes(-65), 32, "Side Entrance"),
        new(8, 7, "David Martinez", "EMP007", "in", "granted", DateTime.Now.AddMinutes(-75), 96, "Main Gate"),
        new(9, 8, "Jennifer Taylor", "EMP008", "out", "granted", DateTime.Now.AddMinutes(-85), 94, "Main Gate"),
        new(10, 1, "John Smith", "EMP001", "out", "granted", DateTime.Now.AddMinutes(-95), 97, "Main Gate"),
    ];

    private static IReadOnlyList<CcisAlert> CreateAlerts() =>
    [
        new(1, "critical", "Unknown Face Detected", "Unrecognized person attempted access at Main Gate", DateTime.Now.AddMinutes(-5), "Main Gate", false),
        new(2, "warning", "Repeated Failed Attempts", "3 consecutive failed access attempts from unknown person", DateTime.Now.AddMinutes(-25), "Main Gate", false),
        new(3, "critical", "Blacklist Match", "Person on watchlist detected at Side Entrance", DateTime.Now.AddHours(-2), "Side Entrance", true),
        new(4, "warning", "Camera Offline", "Camera at Rear Entrance is not responding", DateTime.Now.AddHours(-4), "Rear Entrance", true),
        new(5, "normal", "System Update Available", "New security patch available for installation", DateTime.Now.AddHours(-8), "System", false),
    ];
}