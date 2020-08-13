using System;

namespace Orleans.Sagas
{
    public class ActivityDefinition
    {
        public Type Type { get; }
        public ISagaPropertyBag Properties { get; }

        public ActivityDefinition(Type type, ISagaPropertyBag properties)
        {
            Type = type;
            Properties = properties;
        }
    }
}
