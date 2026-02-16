using System;

namespace lab24.Strategies
{
    public class CubeOperationStrategy : INumericOperationStrategy
    {
        public string OperationName => "Cube";

        public double Execute(double value)
        {
            return Math.Pow(value, 3);
        }
    }
}
