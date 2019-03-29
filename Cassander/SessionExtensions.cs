namespace Cassander
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Cassandra;

    public static class SessionExtensions
    {
        private static object DescriptorCache { get; set; }

        public static IEnumerable<T> Query<T>(this ISession session, PreparedStatement preparedStatement, object[] parameters = null)
        {
            var statement = preparedStatement.Bind(parameters);

            return session.Query<T>(statement);
        }

        public static IEnumerable<T> Query<T>(this ISession session, IStatement statement)
        {
            var results = session.Execute(statement);

            var type = (TypeDescriptor<T>)DescriptorCache ?? (TypeDescriptor<T>)(DescriptorCache = TypeDescriptor<T>.CreateDescriptor());

            foreach (var result in results)
            {
                var entity = type.InstanceCreatorHandler();
                foreach (var field in type.Fields)
                {
                    var column = result.GetColumn(field.PropertyInfo.Name);
                    if (column == null)
                    {
                        continue;
                    }

                    var value = result.GetValue(column.Type, column.Index);
                    field.SetHandler(entity, value);
                }

                yield return entity;
            }
        }
    }
}
