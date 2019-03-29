using System;

namespace BenchmarkMappers
{
    public class Book
    {
        public Guid id { get; set; }
        public DateTimeOffset creation_date { get; set; }
        public int stock { get; set; }
        public string name { get; set; }
    }
}
