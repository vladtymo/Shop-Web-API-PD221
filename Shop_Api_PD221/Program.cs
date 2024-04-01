using Core;
using Core.Interfaces;
using Hangfire;
using Infrastructure;
using WebApi;
using WebApi.Helpers;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

string connStr = builder.Configuration.GetConnectionString("LocalDb")!;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.DisableAutoDataAnnotationValidation();

builder.Services.AddDbContext(connStr);
builder.Services.AddIdentity();
builder.Services.AddRepositories();
builder.Services.AddJWT(builder.Configuration);

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

// hangfire
builder.Services.AddHangfire(connStr);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.SeedRoles().Wait();
    scope.ServiceProvider.SeedAdmin().Wait();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:4200", "http://localhost:55756")
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/dash");
JobConfigurator.AddJobs();

app.MapControllers();

app.Run();
