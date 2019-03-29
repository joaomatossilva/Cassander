namespace BenchmarkMappers
{
    using Cassandra.Mapping;

    public class AssistantMappings : Mappings
    {
        public AssistantMappings()
        {
            this.For<Assistant>()
                .TableName("assistants_by_id")
                .PartitionKey("id")
                .Column(x => x.id, cm => cm.WithName("id"))
                .Column(x => x.merchant_location_id, cm => cm.WithName("merchant_location_id"))
                .Column(x => x.creation_date, cm => cm.WithName("creation_date"))
                .ExplicitColumns();
        }
    }
}
