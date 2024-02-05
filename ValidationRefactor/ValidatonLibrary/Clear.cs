using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ValidatonLibrary
{
    public class ProductService
    {
        private readonly ValidatorFacade _validator;
        public ProductService(ValidatorFacade unitOfValidatorWork)
        {
            _validator = unitOfValidatorWork; 
        }

        public void AddProduct(Product product)
        {
            ValidatorResult validatorResult = _validator.ProductValidator.Validate(product);
            Console.WriteLine(validatorResult.IsValid);
            
            foreach (PropertyResult item in validatorResult.PropertyResults)
            {
                Console.WriteLine(item.GlobalErrorMessage);
                Console.WriteLine(item.PropertyName);
                foreach (string errorMessage in item.Errors)
                {
                    Console.WriteLine(errorMessage);
                }
            }
        }
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ProductValidator : Validator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().StartWith('A').WithRule(x => x.Id < 5, "Oh may got").WithMessage("selamlar");
            RuleFor(x => x.Id).NotBigThenTen().WithRule(NotFak,"Faklamıyor").WithMessage("Oh my got shit!");
        }

        public bool NotFak(Product product)
        {
            if (product.Id>5)
                return true;
            return false;
        }
    }

    public class Validator<TModel>
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

    public abstract class RuleBuilder<T>
    {
        protected List<PredicateMessages<T>> predicates = new List<PredicateMessages<T>>();
        protected string PropertyName { get; set; }
        public string GlobalErrorMessage { get; protected set; }
        public PropertyResult Build(T model)
        {
            PropertyResult propertyResult = new PropertyResult();
            propertyResult.PropertyName = PropertyName;
            propertyResult.IsValid = true;
            propertyResult.GlobalErrorMessage = GlobalErrorMessage;
            foreach (PredicateMessages<T> item in predicates)
            {
                bool isValid = item.Predicate.Invoke(model);
                if (!isValid)
                {
                    propertyResult.IsValid = false;
                    propertyResult.Errors.Add(item.Message);
                }
            }

            return propertyResult;
        }
        public virtual void WithMessage(string message)
        {
            GlobalErrorMessage = message;
        }
    }
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

    public class ValidatorResult
    {
        public bool IsValid { get; set; } = true;
        public List<PropertyResult> PropertyResults { get; set; } = new List<PropertyResult>();
    }
    public class PropertyResult
    {
        public string PropertyName { get; set; }
        public bool IsValid { get; set; } = true;
        public string GlobalErrorMessage { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class PredicateMessages<T>
    {
        public string Message { get; set; }
        public Predicate<T> Predicate { get; set; }
    }
}
