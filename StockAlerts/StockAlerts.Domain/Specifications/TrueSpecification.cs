namespace StockAlerts.Domain.Specifications
{
    public class TrueSpecification<TCandidate> : ISpecification<TCandidate>
    {
        public bool IsSatisfiedBy(TCandidate candidate) => true;
    }
}
