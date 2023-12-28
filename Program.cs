using Microsoft.EntityFrameworkCore;
using Ôn_tập;
using Ôn_tập.Data;
using Ôn_tập.Interfaces;
using Ôn_tập.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPokemonRepository , PokemonRepository>();
builder.Services.AddScoped<IReviewRepository , ReviewRepository>();
builder.Services.AddScoped<ICountryRepository , CountryRepository>();
builder.Services.AddScoped<ICategoryRepository , CategoryRepository>();
builder.Services.AddScoped<IOwnerRepository , OwnerRepository>();
builder.Services.AddScoped<IReviewerRepository , ReviewerRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

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
