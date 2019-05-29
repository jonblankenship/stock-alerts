namespace StockAlerts.Domain.Specifications
{
    public interface ISpecification<TCandidate>
    {
        bool IsSatisfiedBy(TCandidate candidate);
    }
}
