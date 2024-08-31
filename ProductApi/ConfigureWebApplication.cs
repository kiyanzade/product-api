using Microsoft.EntityFrameworkCore;
using ProductProject.Database.Contexts;

namespace ProductProject.Api
{
    public static class ConfigureWebApplication
    {

        public static void UseSwaggerAndUi(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        public static void InitializeDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ProductContext>();
            db.Database.Migrate();
        }
    }

}
