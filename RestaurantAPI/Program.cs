using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.ValidationRules;
using System.Reflection;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<APIContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();  
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
WebApplication app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    APIContext? db = scope.ServiceProvider.GetRequiredService<APIContext>();
    db.Database.Migrate();         
    db.SeedIcons();              
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();