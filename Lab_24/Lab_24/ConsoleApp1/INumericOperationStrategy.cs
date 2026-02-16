namespace lab24.Strategies
{
    public interface INumericOperationStrategy
    {
        double Execute(double value);
        string OperationName { get; }
    }
}
