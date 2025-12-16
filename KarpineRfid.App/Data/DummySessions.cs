using KarpineRfid.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarpineRfid.App.Data
{
    public static class DummySessions
    {
        public static List<Session> All { get; } = new()
        {
            new Session
            {
                SessionId = "1",
                Title = "RFID Inventory Session",
                CreatedAt = DateTime.Now.AddDays(-1),
                Tags = new List<SessionTag>
                {
                    new SessionTag { Id = "EPC-300A", Rssi = -56, FirstSeen = DateTime.Now, LastSeen = DateTime.Now, ReadCount = 5, Note = "Box A3" },
                    new SessionTag { Id = "EPC-301B", Rssi = -62, FirstSeen = DateTime.Now, LastSeen = DateTime.Now, ReadCount = 4, Note = "Pallet 12" }
                },
                Notes = "Warehouse morning sweep — dummy notes."
            },

            new Session
            {
                SessionId = "2",
                Title = "Shipment Verification",
                CreatedAt = DateTime.Now.AddDays(-2),
                Tags = new List<SessionTag>
                {
                    new SessionTag { Id = "EPC-420C", Rssi = -48, FirstSeen = DateTime.Now, LastSeen = DateTime.Now, ReadCount = 8, Note = "Package 44" },
                    new SessionTag { Id = "EPC-421D", Rssi = -50, FirstSeen = DateTime.Now, LastSeen = DateTime.Now, ReadCount = 7, Note = "Bin 7" }
                },
                Notes = "Outgoing shipment verified."
            },

            new Session
            {
                SessionId = "3",
                Title = "RFID Asset Audit",
                CreatedAt = DateTime.Now.AddDays(-3),
                Tags = new List<SessionTag>
                {
                    new SessionTag { Id = "EPC-900X", Rssi = -40, FirstSeen = DateTime.Now, LastSeen = DateTime.Now, ReadCount = 2 },
                    new SessionTag { Id = "EPC-901Y", Rssi = -44, FirstSeen = DateTime.Now, LastSeen = DateTime.Now, ReadCount = 3 }
                },
                Notes = "Office audit (dummy)."
            }
        };
    }
}

