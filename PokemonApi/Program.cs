using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonApi;
using PokemonApi.Data;
using PokemonApi.Interface;
using PokemonApi.Repository;
using PokemonApi.Shared.Filters;
using PokemonApi.Shared.Validation;
using PokemonApi.Shared.Validation.Category;
using PokemonApi.Shared.Validation.Country;
using PokemonApi.Shared.Validation.Owner;
using PokemonApi.Shared.Validation.Pokemon;
using PokemonApi.Shared.Validation.Review;
using PokemonApi.Shared.Validation.Reviewer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<Seed>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Repositories
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();
//Validation
builder.Services.AddScoped<ModelValidationAttributeFilter>();
builder.Services.AddScoped<CategoryExistsAttribute>();
builder.Services.AddScoped<CountryExistsValidationAttribute>();
builder.Services.AddScoped<OwnerExistValidationAttribute>();
builder.Services.AddScoped<PokemonExistsValidationAttribute>();
builder.Services.AddScoped<ReviewExistsValidationAttribute>();
builder.Services.AddScoped<ReviewerExistsValidationFilter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
