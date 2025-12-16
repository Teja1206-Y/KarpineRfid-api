// Models/SessionTag.cs
using System;

namespace KarpineRfid.App.Models
{
    public class SessionTag
    {
        public string Id { get; set; } = string.Empty;
        public int Rssi { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public int ReadCount { get; set; }
        public string? Note { get; set; }
    }
}

