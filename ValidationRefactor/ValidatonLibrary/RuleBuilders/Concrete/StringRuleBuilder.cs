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
    public class StringRuleBuilder<T> : RuleBuilder<T>
        where T : class, new()
    {
        protected Expression<Func<T, string>> _expression;

        public StringRuleBuilder(Expression<Func<T, string>> expression)
        {
            _expression = expression;
            MemberExpression memberExpression = _expression.Body as MemberExpression;
            PropertyName = memberExpression.Member.Name;
        }

        public StringRuleBuilder<T> NotEmpty()
        {
            predicates.Add(new PredicateMessages<T>
            {
                Message = "Boş bro boşşş",
                Predicate = x => !string.IsNullOrEmpty(typeof(T).GetProperty(PropertyName).GetValue(x).ToString())
            });
            return this;
        }
        public StringRuleBuilder<T> StartWith(char letter)
        {
            predicates.Add(new PredicateMessages<T>
            {
                Message = "A ile başlamıyor.",
                Predicate = x => typeof(T).GetProperty(PropertyName).GetValue(x).ToString()[0] == letter
            });
            return this;
        }

        public StringRuleBuilder<T> WithRule(Predicate<T> predicate, string message)
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
