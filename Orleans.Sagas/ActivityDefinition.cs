using System;

namespace Orleans.Sagas
{
    public class ActivityDefinition<TConfig> : ActivityDefinition
    {
        public TConfig Config { get; }

        public ActivityDefinition() : base()
        {
        }

        public ActivityDefinition(Type type, TConfig config) : base(type)
        {
            Config = config;
        }
    }

    public class ActivityDefinition
    {
        public Type Type { get; }

        public ActivityDefinition()
        {
        }

        public ActivityDefinition(Type type)
        {
            Type = type;
        }
    }
}
