﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductProject.Service.ProductService.Dto;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ProductProject.Database.Contexts;
using ProductProject.Database.Entities;

namespace ProductProject.Service.ProductService;

public class ProductService : IProductService
{
    private readonly ProductContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public ProductService(ProductContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IList<GetProductDto>> GetProducts(string? ownerId)
    {
        IList<GetProductDto> productsDto = new List<GetProductDto>();
        if (string.IsNullOrEmpty(ownerId))
        {

           var productList = await _dbContext.Products.ToListAsync();

           foreach (var product in productList)
           {
               var dto = _mapper.Map<GetProductDto>(product);
               productsDto.Add(dto);
           }

        }
        else
        {
           var productList = await _dbContext.Products
                .Where(p => p.OwnerId == ownerId)
                .ToListAsync();
           foreach (var product in productList)
           {
               var dto = _mapper.Map<GetProductDto>(product);
               productsDto.Add(dto);
           }
        }

        return productsDto;
    }

    public async Task<GetProductDto> GetProduct(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
        {
            throw new FileNotFoundException("There is no product with this Id.");
        }
        var dto = _mapper.Map<GetProductDto>(product);

        return dto;
    }

    public async Task EditProduct(int id, EditProductDto dto)
    {
        var existingProduct = await _dbContext.Products.FindAsync(id);
        if (existingProduct == null)
        {
            throw new FileNotFoundException("There is no product with this Id.");
        }
        if (existingProduct.OwnerId != _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            throw new BadHttpRequestException("You are only able to change your product.");
        }
        var conflictCheck = _dbContext.Products
            .Any(product => product.ProduceDate == dto.ProduceDate && product.ManufactureEmail == dto.ManufactureEmail);

        if (conflictCheck)
        {
            throw new BadHttpRequestException("A product with the same ManufactureEmail and ProduceDate already exists.");
        }

        UpdateProductFields(existingProduct,dto);

        await _dbContext.SaveChangesAsync();

    }

    public async Task AddProduct(AddProductDto dto)
    {
        var conflictCheck = _dbContext.Products
            .Any(product => product.ProduceDate == dto.ProduceDate && product.ManufactureEmail == dto.ManufactureEmail);

        if (conflictCheck)
        {
            throw new BadHttpRequestException("A product with the same ManufactureEmail and ProduceDate already exists.");
        }
          
        var product = _mapper.Map<ProductModel>(dto);
        product.OwnerId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)?? throw new UnauthorizedAccessException();
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProduct(int id)
    {
        var existingProduct = await _dbContext.Products.FindAsync(id);
        if (existingProduct == null)
        {
            throw new FileNotFoundException("There is no product with this Id.");
        }
        if (existingProduct.OwnerId != _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            throw new BadHttpRequestException("You are only able to change your product.");
        }

        _dbContext.Products.Remove(existingProduct);
        await _dbContext.SaveChangesAsync();
    }


    private void UpdateProductFields(ProductModel existingProduct, EditProductDto dto)
    {
        if (dto.IsAvailable != null)
        {
            existingProduct.IsAvailable = dto.IsAvailable.Value;
        }
        if (!string.IsNullOrEmpty(dto.Name))
        {
            existingProduct.Name = dto.Name;
        }
        if (!string.IsNullOrEmpty(dto.ManufactureEmail))
        {
            existingProduct.ManufactureEmail = dto.ManufactureEmail;
        }
        if (!string.IsNullOrEmpty(dto.ManufacturePhone))
        {
            existingProduct.ManufacturePhone = dto.ManufacturePhone;
        }
        if (dto.ProduceDate != null)
        {
            existingProduct.ProduceDate = dto.ProduceDate.Value;
        }
    }
}