namespace BenchmarkMappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BenchmarkDotNet.Attributes;
    using Cassander;
    using Cassandra;
    using Cassandra.Mapping;

    [CoreJob]
    [MemoryDiagnoser]
    public class CassandraBenchmark
    {
        private ISession Session { get; set; }
        private IMapper Mapper { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var builder = new Builder()
                .AddContactPoints("localhost");

            var cluster = builder.Build();

            Session = cluster.Connect("cassander");
            Mapper = new Mapper(Session);

            Session.Execute("CREATE TABLE IF NOT EXISTS books (id uuid, stock int, creation_date Timestamp, name text, PRIMARY KEY (id))");

            MappingConfiguration.Global.Define<BooksMappings>();

            Session.Execute("TRUNCATE TABLE books");

            for (int i = 0; i < 1000; i++)
            {
                var book = new Book
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTimeOffset.Now,
                    Name = Guid.NewGuid().ToString(),
                    Stock = i
                };
                Mapper.Insert(book);
            }

        }

        [Benchmark]
        public List<Book> UsingCassandraMapper()
        {
            return Mapper.Fetch<Book>("SELECT * FROM books").ToList();
        }

        [Benchmark]
        public List<Book> UsingCassander()
        {
            var simpleStatement = new SimpleStatement("SELECT * FROM books");
            return Session.Query<Book>(simpleStatement).ToList();
        }

        [Benchmark(Baseline = true)]
        public List<Book> UsingSession()
        {
            var simpleStatement = new SimpleStatement("SELECT * FROM books");
            var rowset = Session.Execute(simpleStatement);
            return rowset.Select(x => new Book()
            {
                Id = x.GetValue<Guid>("id"),
                Stock = x.GetValue<int>("stock"),
                CreationDate = x.GetValue<DateTimeOffset>("creation_date"),
                Name = x.GetValue<string>("name")
            }).ToList();
        }
    }
}
