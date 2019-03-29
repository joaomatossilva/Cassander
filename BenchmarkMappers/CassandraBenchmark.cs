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

            Session = cluster.Connect("localhost_interstore");
            Mapper = new Mapper(Session);

            MappingConfiguration.Global.Define<AssistantMappings>();
        }

        [Benchmark]
        public List<Assistant> UsingCassandraMapper()
        {
            return Mapper.Fetch<Assistant>("SELECT * FROM Assistants_by_id").ToList();
        }

        [Benchmark]
        public List<Assistant> UsingCassander()
        {
            var simpleStatement = new SimpleStatement("SELECT * FROM Assistants_by_id");
            return Session.Query<Assistant>(simpleStatement).ToList();
        }

        [Benchmark]
        public List<Assistant> UsingSession()
        {
            var simpleStatement = new SimpleStatement("SELECT * FROM Assistants_by_id");
            var rowset = Session.Execute(simpleStatement);
            return rowset.Select(x => new Assistant()
            {
                id = x.GetValue<Guid>("id"),
                merchant_location_id = x.GetValue<int>("merchant_location_id"),
                creation_date = x.GetValue<DateTimeOffset>("creation_date")
            }).ToList();
        }
    }
}
