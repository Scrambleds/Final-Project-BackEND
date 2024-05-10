using Final_Project_BackEND;
using Final_Project_BackEND.Data;
using Final_Project_BackEND.Repositorys;
using Final_Project_BackEND.Service;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

string timeZoneId = "Asia/Bangkok"; // Adjust with the desired time zone ID
TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
builder.Services.AddSingleton(timeZone);

builder.Services.AddSingleton(provider =>
{
    var defaultTimeZone = provider.GetRequiredService<TimeZoneInfo>();
    return new TimezoneConverter(defaultTimeZone);
});

builder.Services.AddSingleton<TimezoneConverter>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<IImportRepository, ImportRepository>();
builder.Services.AddScoped<IListRepository, ListRepository>();
builder.Services.AddScoped<IListService, ListService>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder => builder.WithOrigins("http://localhost:3000", "https://sci-grading-system.vercel.app","https://app2.sci.src.ku.ac.th", "https://app2.sci.src.ku.ac.th/grading")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Set EPPlus LicenseContext
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors("AllowLocalhost3000");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
