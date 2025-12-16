using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarpineRfid.App.Models
{
    public class SessionItem
    {
        public string Id { get; set; }            // used for navigation param
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TagsCount { get; set; }

        // convenience for binding to "Tags.Count" in your old XAML
        // if your real model has Tags collection, adapt accordingly
        public int Tags => TagsCount;
    }
}
