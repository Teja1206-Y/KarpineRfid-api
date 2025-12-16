using KarpineRfid.App.Models;
using KarpineRfid.App.Services;
using KarpineRfid.App.ViewModels;
using System.Text;
using System.Text.Json;
namespace KarpineRfid.App.Views;

[QueryProperty(nameof(SessionId), "sessionId")]
public partial class SessionDetailPage : ContentPage
{
    private readonly ISessionStore _store;
    private Session? _session;

    public SessionDetailPage(ISessionStore store)
    {
        InitializeComponent();
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    string sessionId = string.Empty;
    public string SessionId
    {
        get => sessionId;
        set
        {
            sessionId = value ?? string.Empty;
            _ = LoadAsync(sessionId);
        }
    }

    private async Task LoadAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

        try
        {
            _session = await _store.GetSessionAsync(id);
            if (_session is null)
            {
                await DisplayAlert("Not found", "Session not found.", "OK");
                return;
            }

            // Update UI
            LblTitle.Text = _session.Title ?? "(no title)";
            LblCreated.Text = _session.CreatedAt.ToString("dd MMM yyyy HH:mm");
            LblNotes.Text = string.IsNullOrWhiteSpace(_session.Notes) ? "(no notes)" : _session.Notes;
            CvTags.ItemsSource = _session.Tags ?? new System.Collections.Generic.List<SessionTag>();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // Export both CSV + JSON, save files, then share both files
    private async void OnExportTapped(object sender, EventArgs e)
    {
        if (_session is null)
        {
            await DisplayAlert("Export", "Nothing to export.", "OK");
            return;
        }

        try
        {
            // --- Build CSV ---
            var sb = new StringBuilder();
            sb.AppendLine("SessionId,Title,CreatedAt,Notes");
            sb.AppendLine($"\"{_session.SessionId}\",\"{(_session.Title ?? "").Replace("\"", "\"\"")}\",\"{_session.CreatedAt:O}\",\"{(_session.Notes ?? "").Replace("\"", "\"\"")}\"");
            sb.AppendLine();
            sb.AppendLine("TagId,Rssi,FirstSeen,LastSeen,ReadCount,Note");

            foreach (var t in _session.Tags ?? new System.Collections.Generic.List<SessionTag>())
            {
                sb.Append("\"").Append(t.Id).Append("\",");
                sb.Append(t.Rssi).Append(",");
                sb.Append("\"").Append(t.FirstSeen.ToString("O")).Append("\",");
                sb.Append("\"").Append(t.LastSeen.ToString("O")).Append("\",");
                sb.Append(t.ReadCount).Append(",");
                sb.Append("\"").Append((t.Note ?? "").Replace("\"", "\"\"")).AppendLine("\"");
            }

            var csvText = sb.ToString();

            // --- Build JSON ---
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonText = JsonSerializer.Serialize(_session, jsonOptions);

            // --- File paths ---
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var safeTitle = MakeSafeFileName(_session.Title ?? "session");
            var csvFileName = $"session-{safeTitle}-{_session.SessionId}-{timestamp}.csv";
            var jsonFileName = $"session-{safeTitle}-{_session.SessionId}-{timestamp}.json";

            var folder = FileSystem.AppDataDirectory;
            var csvPath = Path.Combine(folder, csvFileName);
            var jsonPath = Path.Combine(folder, jsonFileName);

            // --- Write files ---
            await File.WriteAllTextAsync(csvPath, csvText, Encoding.UTF8);
            await File.WriteAllTextAsync(jsonPath, jsonText, Encoding.UTF8);

            // --- Copy JSON to clipboard as convenience (best-effort) ---
            try
            {
                await Clipboard.SetTextAsync(jsonText);
            }
            catch
            {
                // ignore clipboard failures
            }

            // --- Share both files ---
            var files = new[]
            {
                new ShareFile(csvPath),
                new ShareFile(jsonPath)
            };

            var multiRequest = new ShareMultipleFilesRequest
            {
                Title = $"Export {_session.Title}",
                Files = files.ToList()
            };

            await Share.RequestAsync(multiRequest);

            // --- Inform user where files are saved ---
            await DisplayAlert("Exported", $"CSV and JSON exported and share sheet opened.\n\nSaved:\n{csvPath}\n{jsonPath}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Export failed", ex.Message, "OK");
        }
    }

    // helper: make a safe filename (remove invalid chars, limit length)
    private static string MakeSafeFileName(string input)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var cleaned = new string(input.Where(c => !invalid.Contains(c)).ToArray()).Trim();
        if (cleaned.Length == 0) cleaned = "session";
        if (cleaned.Length > 30) cleaned = cleaned.Substring(0, 30);
        // also replace spaces with dashes
        cleaned = string.Join("-", cleaned.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        return cleaned;
    }
}
    
