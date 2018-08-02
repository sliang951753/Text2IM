using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MessageDeliver.Domain
{
    public class Specification<T>
    {
        private readonly Expression<Func<T, bool>> expression;
        private Func<T, bool> evaluateExpression;

        public Specification(Expression<Func<T, bool>> predicate)
        {
            this.expression = predicate;
        }

        protected Specification()
        {
        }

        public bool IsSatisfiedBy(T candidate)
        {
            if (evaluateExpression == null)
            {
                evaluateExpression = expression.Compile();
            }

            return evaluateExpression(candidate);
        }
    }
}
