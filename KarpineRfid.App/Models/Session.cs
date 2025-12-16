using System;
using System.Collections.Generic;

namespace KarpineRfid.App.Models
{
    public class Session
    {
        public string SessionId { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<SessionTag> Tags { get; set; } = new();

        public string Notes { get; set; } = string.Empty;

        public string TagsDisplay =>
            Tags == null || Tags.Count == 0
            ? "Tags: 0"
            : $"Tags: {Tags.Count} ({string.Join(", ", Tags.ConvertAll(t => t.Id))})";
    }

    // Simple SessionTag type — your code attempted to add SessionTag instances

}
