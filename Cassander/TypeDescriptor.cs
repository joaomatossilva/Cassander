namespace Cassander
{
    using Cassandra;
    using System;
    using System.Linq;
    using System.Reflection;

    public class TypeDescriptor<T>
    {
        public FieldTypeDescriptor<T>[] Fields { get; set; }
        public Func<T> InstanceCreatorHandler { get; set; }

        public static TypeDescriptor<T> CreateDescriptor(CqlColumn[] cqlColumns)
        {
            var type = typeof(T);
            var members = type.GetProperties().Select(x => new {
                NormalizedName = x.Name.NormalizeNames(),
                PropertyInfo = x
            });

            var columns = cqlColumns.Select(x => new
            {
                NormalizedName = x.Name.NormalizeNames(),
                Index = x.Index,
                Type = x.Type
            });

            var descriptor = new TypeDescriptor<T>();
            descriptor.InstanceCreatorHandler = DynamicMethodCompiler.CreateInstantiateObjectHandler<T>();
            descriptor.Fields = members.Join(columns, x => x.NormalizedName, y => y.NormalizedName, (m, c) =>
            {
                return new FieldTypeDescriptor<T>()
                {
                    PropertyInfo = m.PropertyInfo,
                    SetHandler = DynamicMethodCompiler.CreateSetHandler<T>(m.PropertyInfo),
                    SourceIndex = c.Index,
                    SourceType = c.Type
                };
            }).ToArray();

            return descriptor;
        }
    }

    public class FieldTypeDescriptor<T>
    {
        public int SourceIndex { get; set; }
        public Type SourceType { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public Action<T, object> SetHandler { get; set; }
    }
}
