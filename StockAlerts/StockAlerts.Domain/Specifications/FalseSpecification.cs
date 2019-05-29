namespace StockAlerts.Domain.Specifications
{
    public class FalseSpecification<TCandidate> : ISpecification<TCandidate>
    {
        public bool IsSatisfiedBy(TCandidate candidate) => false;
    }
}
