using System;

namespace ConsoleApp1
{
    using Cassander;
    using Cassandra;

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new Builder()
                .AddContactPoints("localhost");

            var cluster = builder.Build();

            var session = cluster.Connect("localhost_interstore");

            var statement = new SimpleStatement("SELECT * FROM Assistants_by_id");
            var assistants = session.Query<Assistant>(statement);

            foreach (var assistant in assistants)
            {
                Console.WriteLine($"{assistant.id} {assistant.merchant_location_id} {assistant.creation_date}");
            }
        }
    }
}
