namespace BenchmarkMappers
{
    using Cassandra.Mapping;

    public class BooksMappings : Mappings
    {
        public BooksMappings()
        {
            this.For<Book>()
                .TableName("books")
                .PartitionKey("id")
                .Column(x => x.id, cm => cm.WithName("id"))
                .Column(x => x.stock, cm => cm.WithName("stock"))
                .Column(x => x.creation_date, cm => cm.WithName("creation_date"))
                .Column(x => x.name, cm => cm.WithName("name"))
                .ExplicitColumns();
        }
    }
}
