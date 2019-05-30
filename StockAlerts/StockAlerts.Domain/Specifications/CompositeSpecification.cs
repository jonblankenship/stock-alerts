using System;
using System.Collections.Generic;

namespace StockAlerts.Domain.Specifications
{
    public abstract class CompositeSpecification<TCandidate> : ISpecification<TCandidate>
    {
        protected readonly List<ISpecification<TCandidate>> _childSpecifications = new List<ISpecification<TCandidate>>();

        public void AddChildSpecification(ISpecification<TCandidate> childSpecification)
        {
            _childSpecifications.Add(childSpecification);
        }

        public abstract bool IsSatisfiedBy(TCandidate candidate);

        public IReadOnlyCollection<ISpecification<TCandidate>> Children => _childSpecifications.AsReadOnly();
    }
}
