using System;

namespace Orleans.Sagas.Samples.Activities
{
    [Serializable]
    public class CustomException : Exception
    {
        public int CustomErrorCode { get; set; }

        public CustomException()
        {
        }

        public CustomException(int code, string message) : base(message)
        {
            CustomErrorCode = code;
        }

        protected CustomException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
