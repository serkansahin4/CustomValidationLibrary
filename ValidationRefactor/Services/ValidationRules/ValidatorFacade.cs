﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ValidationRules
{
    public class ValidatorFacade
    {
        public ProductValidator ProductValidator { get; private set; }

        public ValidatorFacade(ProductValidator productValidator)
        {
            ProductValidator = productValidator;
        }
    }
}
