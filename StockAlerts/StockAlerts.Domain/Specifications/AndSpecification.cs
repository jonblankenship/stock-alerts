using System.Collections.Generic;
using System.Linq;

namespace StockAlerts.Domain.Specifications
{
    public class AndSpecification<TCandidate> : CompositeSpecification<TCandidate>
    {
        private readonly List<ISpecification<TCandidate>> _childSpecifications = new List<ISpecification<TCandidate>>();

        public void AddChildSpecification(ISpecification<TCandidate> childSpecification)
        {
            _childSpecifications.Add(childSpecification);
        }

        public override bool IsSatisfiedBy(TCandidate candidate)
        {
            if (!_childSpecifications.Any()) return false;

            foreach (var s in _childSpecifications)
            {
                if (!s.IsSatisfiedBy(candidate)) return false;
            }

            return true;
        }
    }
}
