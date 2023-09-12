using AutoMapper;
using Serilog;
using srvStaticWeb.DB;
using srvStaticWeb.Services;
using Microsoft.EntityFrameworkCore;
using srvStaticWeb;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<WebContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<StaticWebService>();
app.MapGet("/", () => "Started StaticWebService");

app.Run();
