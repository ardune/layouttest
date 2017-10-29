using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Caliburn.Micro;

namespace LayoutTest.StateManagement
{
    public abstract class StateHolder<THeldState, THeldStateBuilder>
        where THeldStateBuilder : IHeldStateBuilder<THeldState>, new()
    {
        private delegate void TransactionFunc(BehaviorSubject<THeldState> heldStateSubject);
        private readonly BehaviorSubject<THeldState> currentStateSubject = new BehaviorSubject<THeldState>(new THeldStateBuilder().DefaultState());
        private readonly ActionBlock<TransactionFunc> transactionBuffer;

        public IObservable<THeldState> CurrentState { get; }
        public THeldState LatestState => currentStateSubject.Value;

        protected StateHolder()
        {
            transactionBuffer = new ActionBlock<TransactionFunc>(transaction => {
                // This block will run serially because MaxDegreeOfParallelism is 1
                // That means we can read from and modify the current state (held in the subject) atomically
                transaction(currentStateSubject);
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });

            CurrentState = currentStateSubject.DistinctUntilChanged();
        }

        public async Task UpdateState(Func<THeldStateBuilder, THeldStateBuilder> updateBlock)
        {
            var taskCompletionSource = new TaskCompletionSource<Unit>();

            void UpdateTransaction(BehaviorSubject<THeldState> nextSubject)
            {
                var builder = new THeldStateBuilder();
                builder.InitializeFrom(nextSubject.Value);
                //TODO: error handling
                var newState = updateBlock(builder).Build();

                PlatformProvider.Current.OnUIThreadAsync(() =>
                {
                    nextSubject.OnNext(newState);
                });
                taskCompletionSource.SetResult(Unit.Default);
            }

            var didSend = await transactionBuffer.SendAsync(UpdateTransaction);

            if (!didSend)
            {
                throw new ApplicationException("UpdateState failed to process transaction. This probably means the BufferBlock is not initialized properly");
            }
            await taskCompletionSource.Task;
        }
        
        public IObservable<T> Project<T>(Func<THeldState, T> block)
        {
            return CurrentState.Select(block).DistinctUntilChanged();
        }
    }
}