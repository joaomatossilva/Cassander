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

            var session = cluster.Connect("cassander");

            session.UserDefinedTypes.Define(
                new[]
                {
                    UdtMap.For<AuthorTitle>()
                        .Map(x => x.Author, "author")
                        .Map(x => x.Title, "title")
                }
            );

            var statement = new SimpleStatement("SELECT * FROM books");
            var books = session.Query<Books>(statement);

            foreach (var book in books)
            {
                Console.WriteLine($"{book.id} {book.stock} {book.creation_date} {book.name} {book.Authortitle?.Title}");
            }
        }
    }
}
