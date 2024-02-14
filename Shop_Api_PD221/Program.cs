using BusinessLogic;
using DataAccess;
using Shop_Api_PD221;

var builder = WebApplication.CreateBuilder(args);

string connStr = builder.Configuration.GetConnectionString("LocalDb")!;

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext(connStr);
builder.Services.AddIdentity();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// auto mapper configuration
builder.Services.AddAutoMapper();
// fluent validators configuration
builder.Services.AddFluentValidator();

// add custom servies
builder.Services.AddCustomServices();
builder.Services.AddCartService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
