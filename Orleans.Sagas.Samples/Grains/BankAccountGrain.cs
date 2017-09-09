using System;
using Orleans.Sagas.Samples.Interfaces;
using System.Threading.Tasks;
using Orleans.Sagas.Samples.Exceptions;

namespace Orleans.Sagas.Samples.Grains
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
            if (State.Transactions.ContainsKey(transactionId) &&
                State.Transactions[transactionId] != 0)
            {
                State.Balance -= State.Transactions[transactionId];
                State.Transactions[transactionId] = 0;
            }
            else
            {
                State.Transactions[transactionId] = 0;
            }

            await WriteStateAsync();
        }
    }
}
