namespace CCIS.Models;

public record DashboardKpiViewModel(string Title, string Value, string Change, string ColorClass, string TrendClass);

public record SystemStatusViewModel(string Label, string Value, string State, string AccentClass);

public record RecentEventViewModel(string UserName, string EmployeeId, string Location, string Status, string Direction, DateTime Timestamp, string TimeAgo);

public record PresenceItemViewModel(string Label, string Value);

public record DashboardViewModel(
    IReadOnlyList<DashboardKpiViewModel> Kpis,
    IReadOnlyList<RecentEventViewModel> RecentEvents,
    IReadOnlyList<SystemStatusViewModel> SystemStatus,
    IReadOnlyList<PresenceItemViewModel> PresenceSummary,
    string LastUpdated);

public record RecognitionResultViewModel(string Name, string EmployeeId, string Status, int Confidence, string Initials, DateTime CapturedAt);

public record FaceQuickActionViewModel(string Label, string Variant);

public record FaceAccessViewModel(
    bool CameraActive,
    RecognitionResultViewModel? RecognitionResult,
    IReadOnlyList<RecentEventViewModel> RecentEvents,
    IReadOnlyList<FaceQuickActionViewModel> QuickActions);

public record UserRowViewModel(
    int Id,
    string Name,
    string EmployeeId,
    string Role,
    string Department,
    string Status,
    bool FaceEnrolled,
    DateTime? LastAccess,
    string Initials);

public record UserManagementViewModel(string SearchQuery, IReadOnlyList<UserRowViewModel> Users);

public record AccessLogRowViewModel(
    int Id,
    string UserName,
    string EmployeeId,
    string Direction,
    string Status,
    DateTime Timestamp,
    string Confidence,
    string Location,
    string Initials);

public record AccessLogSummaryViewModel(string Label, string Value, string ValueClass);

public record AccessLogsViewModel(
    string SearchQuery,
    string StatusFilter,
    string DirectionFilter,
    string DateFilter,
    IReadOnlyList<AccessLogSummaryViewModel> Summaries,
    IReadOnlyList<AccessLogRowViewModel> Logs);

public record AlertCardViewModel(
    int Id,
    string Type,
    string Title,
    string Message,
    string Location,
    DateTime Timestamp,
    bool Acknowledged,
    string AccentClass,
    string IconLabel);

public record AlertsCenterViewModel(
    string Filter,
    int TotalCount,
    int CriticalCount,
    int WarningCount,
    int UnacknowledgedCount,
    IReadOnlyList<AlertCardViewModel> Alerts);

public record CcisUser(
    int Id,
    string Name,
    string EmployeeId,
    string Role,
    string Department,
    string Status,
    bool FaceEnrolled,
    DateTime? LastAccess);

public record CcisAccessLog(
    int Id,
    int? UserId,
    string UserName,
    string? EmployeeId,
    string Direction,
    string Status,
    DateTime Timestamp,
    int Confidence,
    string Location);

public record CcisAlert(
    int Id,
    string Type,
    string Title,
    string Message,
    DateTime Timestamp,
    string Location,
    bool Acknowledged);

public record CcisKpis(int EntriesToday, int ExitsToday, int CurrentOccupancy, int UnauthorizedAttempts);

public record CcisSystemStatus(int CamerasOnline, int CamerasTotal, bool DatabaseConnected, int DatabaseLatency, bool RecognitionRunning, decimal RecognitionAccuracy, DateTime LastBackup);

public record CcisCurrentUser(string Name, string Role);

public static class CcisFormatting
{
    public static string TimeAgo(DateTime dateTime)
    {
        var seconds = (DateTime.Now - dateTime).TotalSeconds;

        if (seconds < 60)
        {
            return $"{Math.Floor(seconds)}s ago";
        }

        if (seconds < 3600)
        {
            return $"{Math.Floor(seconds / 60)}m ago";
        }

        if (seconds < 86400)
        {
            return $"{Math.Floor(seconds / 3600)}h ago";
        }

        return dateTime.ToString("MMM d");
    }

    public static string Initials(string name)
    {
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return string.Concat(parts.Select(part => part[0])).ToUpperInvariant();
    }
}