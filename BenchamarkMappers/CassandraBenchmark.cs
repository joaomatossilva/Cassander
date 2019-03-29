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

            MappingConfiguration.Global.Define<BooksMappings>();

            Session.Execute("TRUNCATE TABLE books");

            for (int i = 0; i < 1000; i++)
            {
                var book = new Book
                {
                    id = Guid.NewGuid(),
                    creation_date = DateTimeOffset.Now,
                    name = Guid.NewGuid().ToString(),
                    stock = i
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
                id = x.GetValue<Guid>("id"),
                stock = x.GetValue<int>("stock"),
                creation_date = x.GetValue<DateTimeOffset>("creation_date"),
                name = x.GetValue<string>("name")
            }).ToList();
        }
    }
}
