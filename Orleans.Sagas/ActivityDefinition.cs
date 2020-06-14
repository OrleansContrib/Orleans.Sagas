using System;

namespace Orleans.Sagas
{
    public class ActivityDefinition<TConfig> : ActivityDefinition
    {
        public ActivityDefinition() : base()
        {
        }

        public ActivityDefinition(Type type, TConfig config) : base(type)
        {
            Config = config;
        }

        public TConfig Config { get; }
    }

    public class ActivityDefinition
    {
        public Type Type;

        public ActivityDefinition()
        {
        }

        public ActivityDefinition(Type type)
        {
            Type = type;
        }
    }
}
