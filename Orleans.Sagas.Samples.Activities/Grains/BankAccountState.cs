using System;
using System.Collections.Generic;

namespace Orleans.Sagas.Samples.Activities.Grains
{
    public class BankAccountState
    {
        public int Balance { get; set; }
        public Dictionary<Guid, int> Transactions { get; set; }

        public BankAccountState()
        {
            Transactions = new Dictionary<Guid, int>();
        }
    }
}