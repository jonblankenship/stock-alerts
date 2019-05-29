using System;

namespace StockAlerts.Domain.Specifications
{
    public abstract class CompositeSpecification<TCandidate> : ISpecification<TCandidate>
    {
        public abstract bool IsSatisfiedBy(TCandidate candidate);
    }
}
