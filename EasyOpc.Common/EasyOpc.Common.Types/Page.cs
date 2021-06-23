using System.Collections.Generic;

namespace EasyOpc.Common.Types
{
    public class Page<T> where T : IIdentifiable
    {
        public int PageNumber { get; set; }

        public int CountInPage { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
