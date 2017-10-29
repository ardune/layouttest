namespace LayoutTest.StateManagement
{
    public interface IHeldStateBuilder<TState>
    {
        void InitializeFrom(TState state);
        TState Build();
        TState DefaultState();
    }
}