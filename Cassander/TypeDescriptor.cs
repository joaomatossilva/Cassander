namespace Cassander
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class TypeDescriptor<T>
    {
        public FieldTypeDescriptor<T>[] Fields { get; set; }
        public Func<T> InstanceCreatorHandler { get; set; }

        public static TypeDescriptor<T> CreateDescriptor()
        {
            var type = typeof(T);

            var members = type.GetProperties();

            var descriptor = new TypeDescriptor<T>();
            descriptor.InstanceCreatorHandler = DynamicMethodCompiler.CreateInstantiateObjectHandler<T>();
            descriptor.Fields = members.Select(x => new FieldTypeDescriptor<T>()
            {
                PropertyInfo = x,
                SetHandler = DynamicMethodCompiler.CreateSetHandler<T>(x)
            }).ToArray();

            return descriptor;
        }
    }

    public class FieldTypeDescriptor<T>
    {
        public PropertyInfo PropertyInfo { get; set; }
        public Action<T, object> SetHandler { get; set; }
    }
}
