namespace ProductProject.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddService();
            builder.Services.AddControllers();
            builder.Services.AddAutoMapper();
            builder.Services.AddHttpContextAccessor(); 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwagger();
            builder.Services.AddAuthorization();
            builder.Services.AddDbContext(builder.Configuration);
            builder.Services.AddIdentity(builder.Configuration);
            builder.Services.AddAuthentication(builder.Configuration);
            
            var app = builder.Build();

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
