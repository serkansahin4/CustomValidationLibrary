using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidatonLibrary.Modals;

namespace ValidatonLibrary.Builders.Abstract
{
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
}
