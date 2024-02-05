using Entities.Concrete;
using Services.Concrete;
using Services.ValidationRules;

Product product = new Product
{
    Id = 1,
    Name = "Aerkan"
};
ValidatorFacade unitOfValidatorWork = new ValidatorFacade(new ProductValidator());
ProductManager productService=new ProductManager(unitOfValidatorWork);
productService.AddProduct(product);

Console.ReadKey();