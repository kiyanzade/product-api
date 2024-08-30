using ProductProject.Service.ProductService.Dto;

namespace ProductProject.Service.ProductService;

public interface IProductService
{
    Task<IList<GetProductDto>> GetProducts(string? owner);
    Task<GetProductDto> GetProduct(int id);
    Task EditProduct(int id, EditProductDto dto);
    Task AddProduct(AddProductDto dto);
    Task DeleteProduct(int id);
}