namespace __Project__.Scripts.FSM
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}