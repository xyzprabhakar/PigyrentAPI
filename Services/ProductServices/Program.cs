using AutoMapper;
using Microsoft.AspNetCore.Builder;
using ProductServices;
using ProductServices.DB;
using ProductServicesProt;
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
    //add grpc reflection
    //builder.Services.AddGrpcReflection();
    var app = builder.Build();

    app.MapGrpcService<CategoryService>();
    app.MapGet("/", () => "Product services");
    //IWebHostEnvironment env = app.Environment;
    //if (env.IsDevelopment())
    //{
    //    app.MapGrpcReflectionService();
    //}
    app.Run();

