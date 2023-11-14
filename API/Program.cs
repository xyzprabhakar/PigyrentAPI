using API;
using API.Classes;
using AutoMapper;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

Constant.ROOT_PATH = builder.Environment.ContentRootPath;



builder.Services.Configure<GRPCServices>(builder.Configuration.GetSection("GRPCServices"));
builder.Services.AddScoped(typeof(InMemoryCache));

builder.Services.AddCors(option=> option.AddPolicy(  name: "allowedPigyRentUI", p => { p.WithOrigins("http://localhost:4200"); }));


// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("allowedPigyRentUI");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
