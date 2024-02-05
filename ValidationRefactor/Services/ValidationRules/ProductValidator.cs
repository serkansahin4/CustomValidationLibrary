using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidatonLibrary;

namespace Services.ValidationRules
{
    public class ProductValidator:ValidatorBase<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().StartWith('A').WithRule(x => x.Id < 5, "Oh may got").WithMessage("selamlar");
            RuleFor(x => x.Id).NotBigThenTen().WithRule(NotFak, "Faklamıyor").WithMessage("Oh my got shit!");
        }

        public bool NotFak(Product product)
        {
            if (product.Id > 5)
                return true;
            return false;
        }
    }
}
