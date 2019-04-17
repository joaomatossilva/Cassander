using System;

namespace BenchmarkMappers
{
    public class Book
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public int Stock { get; set; }
        public string Name { get; set; }
    }
}
