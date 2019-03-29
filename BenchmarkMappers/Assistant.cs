using System;
using System.Collections.Generic;
using System.Text;

namespace BenchmarkMappers
{
    public class Assistant
    {
        public Guid id { get; set; }
        public DateTimeOffset creation_date { get; set; }
        public int merchant_location_id { get; set; }
    }
}
