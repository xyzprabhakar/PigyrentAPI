using AutoMapper;
using MasterServices;
using MasterServices.DB;
using ProtoBuf.Grpc.Server;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DbSetting>(
    builder.Configuration.GetSection("DbSetting"));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddCodeFirstGrpc();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
