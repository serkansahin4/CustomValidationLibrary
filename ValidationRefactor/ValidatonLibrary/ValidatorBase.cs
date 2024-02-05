using System.Linq.Expressions;
using ValidatonLibrary.Builders.Abstract;
using ValidatonLibrary.Builders.Concrete;
using ValidatonLibrary.Modals;

namespace ValidatonLibrary
{
    public class ValidatorBase<TModel>
        where TModel : class, new()
    {
        Dictionary<string, RuleBuilder<TModel>> rules = new Dictionary<string, RuleBuilder<TModel>>();
        protected StringRuleBuilder<TModel> RuleFor(Expression<Func<TModel, string>> expression)
        {
            RuleBuilder<TModel> stringRuleBuilder = new StringRuleBuilder<TModel>(expression);
            rules.Add((expression.Body as MemberExpression).Member.Name, stringRuleBuilder);
            return (StringRuleBuilder<TModel>)stringRuleBuilder;
        }
        protected IntegerRuleBuilder<TModel> RuleFor(Expression<Func<TModel, int>> expression)
        {
            RuleBuilder<TModel> integerRuleBuilder = new IntegerRuleBuilder<TModel>(expression);

            rules.Add((expression.Body as MemberExpression).Member.Name, integerRuleBuilder);
            return (IntegerRuleBuilder<TModel>)integerRuleBuilder;
        }
        public ValidatorResult Validate(TModel model) //Builderların build metodunu çalıştıacak
        {
            ValidatorResult validatorResult = new ValidatorResult();

            foreach (var item in rules)
            {
                RuleBuilder<TModel> ruleBuilder;
                rules.TryGetValue(item.Key, out ruleBuilder);
                PropertyResult propertyResult = ruleBuilder.Build(model);

                if (!propertyResult.IsValid)
                {
                    validatorResult.PropertyResults.Add(propertyResult);
                    validatorResult.IsValid = false;
                }

            }
            return validatorResult;
        }
    }
}