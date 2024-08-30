using AutoMapper;
using ProductAPI.Data;
using ProductProject.Service.ProductService.Dto;

namespace ProductProject.Service.ProductService;

public class ProductService : IProductService
{
    private readonly ProductContext _dbContext;

    public ProductService(ProductContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
  
    }

    public Task<IList<GetProductDto>> GetProducts(string? owner)
    {
        throw new NotImplementedException();
    }

    public Task<GetProductDto> GetProduct(int id)
    {
        throw new NotImplementedException();
    }

    public Task EditProduct(int id, EditProductDto dto)
    {
        throw new NotImplementedException();
    }

    public Task AddProduct(AddProductDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }
}