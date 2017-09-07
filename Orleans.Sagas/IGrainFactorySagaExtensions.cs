namespace Orleans
{
    public static class IGrainFactorySagaExtensions
    {
        /// <summary>
        /// Provides an ISagaBuilder which can be used to prepare and execute a saga.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public static ISagaBuilder CreateSaga(this IGrainFactory that)
        {
            return new SagaBuilder(that);
        }
    }
}
