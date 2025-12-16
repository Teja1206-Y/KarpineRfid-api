// Services/ExportService.cs
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KarpineRfid.App.Models;
using Microsoft.Maui.Storage;

namespace KarpineRfid.App.Services
{
    public class ExportService : IExportService
    {
        // --- existing helpers (kept) ---
        public Task<string> ExportSessionToCsvAsync(Session session, bool includeNote = false)
        {
            var sb = new StringBuilder();
            // header
            sb.Append("Id,RSSI,FirstSeen,LastSeen,Reads");
            if (includeNote) sb.Append(",Note");
            sb.AppendLine();

            foreach (var t in session.Tags.OrderByDescending(x => x.ReadCount))
            {
                var line = string.Join(",",
                    CsvEscape(t.Id),
                    t.Rssi.ToString(),
                    t.FirstSeen.ToString("o"),
                    t.LastSeen.ToString("o"),
                    t.ReadCount.ToString());
                if (includeNote) line += "," + CsvEscape(t.Note ?? string.Empty);
                sb.AppendLine(line);
            }

            return Task.FromResult(sb.ToString());
        }

        public Task<string> ExportSessionToJsonAsync(Session session)
        {
            var json = JsonSerializer.Serialize(session, new JsonSerializerOptions { WriteIndented = true });
            return Task.FromResult(json);
        }

        public async Task<string> WriteTextToFileAsync(string filename, string content)
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, filename);
            await File.WriteAllTextAsync(path, content);
            return path;
        }

        private static string CsvEscape(string? s)
        {
            if (s == null) return "";
            if (s.Contains(',') || s.Contains('"') || s.Contains('\n'))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }

        // --- NEW: implement ExportAsync required by IExportService ---
        public async Task<string[]> ExportAsync(Session session, string[] formats, string[] fields)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (formats == null || formats.Length == 0) formats = new[] { "CSV" };
            if (fields == null) fields = new string[] { "sessionId", "title", "createdAt", "notes", "tags" };

            var saved = new System.Collections.Generic.List<string>();
            var safeTitle = MakeSafeFileName(session.Title ?? "session");
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            // Helper to test inclusion
            bool Include(string name) => fields.Contains(name, StringComparer.OrdinalIgnoreCase);

            // CSV: we will create a CSV based on selected fields (not only tags list)
            if (formats.Any(f => f.Equals("CSV", StringComparison.OrdinalIgnoreCase)))
            {
                // Build CSV header and row according to chosen fields
                var sb = new StringBuilder();
                var headers = new System.Collections.Generic.List<string>();
                if (Include("sessionId")) headers.Add("SessionId");
                if (Include("title")) headers.Add("Title");
                if (Include("createdAt")) headers.Add("CreatedAt");
                if (Include("notes")) headers.Add("Notes");
                if (Include("tags")) headers.Add("Tags");
                sb.AppendLine(string.Join(",", headers));

                var row = new System.Collections.Generic.List<string>();
                if (Include("sessionId")) row.Add(EscapeCsv(session.SessionId));
                if (Include("title")) row.Add(EscapeCsv(session.Title));
                if (Include("createdAt")) row.Add(EscapeCsv(session.CreatedAt.ToString("O")));
                if (Include("notes")) row.Add(EscapeCsv(session.Notes ?? ""));
                if (Include("tags"))
                {
                    var tagStrings = session.Tags?.Select(t => t.Id) ?? Enumerable.Empty<string>();
                    row.Add(EscapeCsv(string.Join(";", tagStrings)));
                }

                sb.AppendLine(string.Join(",", row));

                var csvName = $"session-{safeTitle}-{session.SessionId}-{timestamp}.csv";
                var csvPath = await WriteTextToFileAsync(csvName, sb.ToString());
                saved.Add(csvPath);
            }

            // JSON: create trimmed object containing only requested fields
            if (formats.Any(f => f.Equals("JSON", StringComparison.OrdinalIgnoreCase)))
            {
                var dto = new System.Dynamic.ExpandoObject() as System.Collections.Generic.IDictionary<string, object?>;

                if (Include("sessionId")) dto["sessionId"] = session.SessionId;
                if (Include("title")) dto["title"] = session.Title;
                if (Include("createdAt")) dto["createdAt"] = session.CreatedAt;
                if (Include("notes")) dto["notes"] = session.Notes;
                if (Include("tags"))
                {
                    dto["tags"] = session.Tags?.Select(t => new
                    {
                        id = t.Id,
                        rssi = t.Rssi,
                        firstSeen = t.FirstSeen,
                        lastSeen = t.LastSeen,
                        readCount = t.ReadCount,
                        note = t.Note
                    }).ToArray();
                }

                var jsonOptions = new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var payload = JsonSerializer.Serialize(dto, jsonOptions);

                var jsonName = $"session-{safeTitle}-{session.SessionId}-{timestamp}.json";
                var jsonPath = await WriteTextToFileAsync(jsonName, payload);
                saved.Add(jsonPath);
            }

            return saved.ToArray();
        }

        private static string EscapeCsv(string? v)
        {
            if (v == null) return "\"\"";
            var s = v.Replace("\"", "\"\"");
            return $"\"{s}\"";
        }

        private static string MakeSafeFileName(string input)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var cleaned = new string(input.Where(c => !invalid.Contains(c)).ToArray()).Trim();
            if (cleaned.Length == 0) cleaned = "session";
            if (cleaned.Length > 30) cleaned = cleaned.Substring(0, 30);
            cleaned = string.Join("-", cleaned.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            return cleaned;
        }
    }
}

