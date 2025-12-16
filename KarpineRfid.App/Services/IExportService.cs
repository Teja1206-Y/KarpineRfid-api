// Services/IExportService.cs
using System.Threading.Tasks;
using KarpineRfid.App.Models;

namespace KarpineRfid.App.Services
{
    public interface IExportService
    {
        /// <summary>
        /// Export a session into requested formats and fields. Returns absolute file paths saved.
        /// formats: e.g. "CSV", "JSON"
        /// fields: array of field names to include (sessionId, title, createdAt, notes, tags)
        /// </summary>
        Task<string[]> ExportAsync(Session session, string[] formats, string[] fields);

        // legacy/utility helpers (optional - implementations may forward to ExportAsync)
        Task<string> ExportSessionToCsvAsync(Session session, bool includeNote = false);
        Task<string> ExportSessionToJsonAsync(Session session);
        Task<string> WriteTextToFileAsync(string filename, string content);
    }
}

