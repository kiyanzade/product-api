using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductProject.Database.Contexts;
using ProductProject.Database.Entities;
using ProductProject.Service.ProductService;
using ProductProject.Service.ProductService.Dto;
using ProductProject.Service.UserService;

namespace ProductProject.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(config =>
            {
                config.CreateMap<ProductModel, GetProductDto>();
                config.CreateMap<ProductModel, AddProductDto>();
                config.CreateMap<ProductModel, EditProductDto>();

                config.CreateMap<AddProductDto, ProductModel>();
                
            });
            builder.Services.AddHttpContextAccessor(); 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {

                options.EnableAnnotations();


                // Authorize in swagger with Token
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token in the following format: Bearer {token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
            });
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<ProductContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // for comfort during registration
                options.Password.RequireDigit = false; 
                options.Password.RequiredLength = 6; 
                options.Password.RequireNonAlphanumeric = false; 
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false; 
                options.Password.RequiredUniqueChars = 1; 
            })
                .AddEntityFrameworkStores<ProductContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).
             AddJwtBearer(options =>
{            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerAndUi();
            }

        
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.InitializeDatabase();
            app.MapControllers();

            app.Run();
        }
    }
}
