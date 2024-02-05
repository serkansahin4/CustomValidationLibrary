using ValidatonLibrary;

Product product = new Product
{
    Id = 1,
    Name = "Aerkan"
};
ValidatorFacade unitOfValidatorWork = new ValidatorFacade(new ProductValidator());
ProductService productService=new ProductService(unitOfValidatorWork);
productService.AddProduct(product);

Console.ReadKey();