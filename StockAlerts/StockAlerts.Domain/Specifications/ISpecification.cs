namespace StockAlerts.Domain.Specifications
{
    public interface ISpecification<in TCandidate>
    {
        bool IsSatisfiedBy(TCandidate candidate);
    }
}
