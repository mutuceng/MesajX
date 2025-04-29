var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(opt => opt.AddPolicy(name: "CorsPolicy", builder =>
    builder.WithOrigins("http://localhost:5173")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials()));

builder.Services.AddAuthentication().AddJwtBearer("YARPAuthenticationScheme",
    options =>
    {
        options.Authority = builder.Configuration["IdentityServerUrl"];
        options.Audience = "ResourceYARP";
        options.RequireHttpsMetadata = false;
    });



var app = builder.Build();

app.UseCors("CorsPolicy");

app.MapGet("/", () => "Hello World!");
app.MapReverseProxy();

app.Run();
