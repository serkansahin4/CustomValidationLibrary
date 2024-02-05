using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidatonLibrary.Modals;
using Services.ValidationRules;

namespace Services.Concrete
{
    public class ProductManager:IProductService
    {
        private readonly ValidatorFacade _validator;
        public ProductManager(ValidatorFacade unitOfValidatorWork)
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
}
