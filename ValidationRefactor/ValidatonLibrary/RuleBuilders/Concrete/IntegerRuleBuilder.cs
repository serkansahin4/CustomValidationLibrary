using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ValidatonLibrary.Builders.Abstract;
using ValidatonLibrary.Modals;

namespace ValidatonLibrary.Builders.Concrete
{
    public class IntegerRuleBuilder<T> : RuleBuilder<T>
        where T : class, new()
    {
        protected Expression<Func<T, int>> _expression;
        public IntegerRuleBuilder(Expression<Func<T, int>> expression)
        {
            _expression = expression;
            MemberExpression memberExpression = _expression.Body as MemberExpression;
            PropertyName = memberExpression.Member.Name;
        }
        public IntegerRuleBuilder<T> NotBigThenTen()
        {
            predicates.Add(new PredicateMessages<T>
            {
                Message = "10 dan büyük.",
                Predicate = x => (int)typeof(T).GetProperty(PropertyName).GetValue(x) < 10
            });
            return this;
        }
        public IntegerRuleBuilder<T> WithRule(Predicate<T> predicate, string message)
        {
            predicates.Add(new PredicateMessages<T>
            {
                Message = message,
                Predicate = predicate
            });
            return this;
        }
    }
}
