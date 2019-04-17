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
                .Column(x => x.Id, cm => cm.WithName("id"))
                .Column(x => x.Stock, cm => cm.WithName("stock"))
                .Column(x => x.CreationDate, cm => cm.WithName("creation_date"))
                .Column(x => x.Name, cm => cm.WithName("name"))
                .ExplicitColumns();
        }
    }
}
