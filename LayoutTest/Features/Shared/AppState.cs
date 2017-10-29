using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LayoutTest.Features.Shared
{
    public struct AppState
    {
        public Page[] Pages { get; set; }
        public int? PrimaryPageSelectionIndex { get; set; }
    }

    public struct AppStateBuilder : IHeldStateBuilder<AppState>
    {
        public Page[] Pages { get; set; }
        public int? PrimaryPageSelectionIndex { get; set; }

        public void InitializeFrom(AppState state)
        {
            Pages = state.Pages;
            PrimaryPageSelectionIndex = state.PrimaryPageSelectionIndex;
        }

        public AppState Build()
        {
            return new AppState
            {
                Pages = Pages,
                PrimaryPageSelectionIndex = PrimaryPageSelectionIndex
            };
        }

        public AppState DefaultState()
        {
            return new AppState
            {
                Pages = new Page[0],
                PrimaryPageSelectionIndex = PrimaryPageSelectionIndex
            };
        }
    }

    public struct Page
    {
        public int PageNumber { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class AppStateHolder : StateHolder<AppState, AppStateBuilder>
    {
        
    }
    
    //SEE: https://spin.atomicobject.com/2017/07/31/c-sharp-state-holder/
    public abstract class StateHolder<THeldState, THeldStateBuilder> 
        where THeldState : struct 
        where THeldStateBuilder : struct, IHeldStateBuilder<THeldState>
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
                nextSubject.OnNext(newState);
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

    public interface IHeldStateBuilder<TState>
    {
        void InitializeFrom(TState state);
        TState Build();
        TState DefaultState();
    }
}
