using System;
using Orleans.Sagas.Samples.Activities.Interfaces;
using System.Threading.Tasks;
using Orleans.Sagas.Samples.Activities.Exceptions;

namespace Orleans.Sagas.Samples.Activities.Grains
{
    public class BankAccountGrain : Grain<BankAccountState>, IBankAccountGrain
    {
        public Task<int> GetBalance()
        {
            return Task.FromResult(State.Balance);
        }

        public async Task ModifyBalance(Guid transactionId, int amount)
        {
            if (State.Transactions.ContainsKey(transactionId))
            { 
                return;
            }

            var newBalance = State.Balance + amount;

            if (newBalance >= 0 &&
                newBalance <= 100)
            { 
                State.Balance += amount;
                State.Transactions[transactionId] = amount;
                await WriteStateAsync();
                return;
            }

            throw new InvalidBalanceException();
        }

        public async Task RevertBalanceModification(Guid transactionId)
        {
            if (State.Transactions.ContainsKey(transactionId))
            {
                if (State.Transactions[transactionId] == 0)
                {
                    return;
                }

                State.Balance -= State.Transactions[transactionId];
            }

            State.Transactions[transactionId] = 0;

            await WriteStateAsync();
        }
    }
}
