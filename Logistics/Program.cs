using Logistics.Data;
using Logistics.Services;
using Logistics.Services.Utils.TokenGenerator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = InvalidModelStateResponse.MakeValidationResponse;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<AppDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DataBase")));

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TokenGenerator>();
builder.Services.AddSingleton<EmailService>();

builder.Services.AddHostedService<RequestArchiverService>();
builder.Services.AddScoped<RequestService>();

builder.Services.AddScoped<DocumentService>();

builder.Services.AddScoped<TransportationService>();


TokenGeneratorConfiguration tokenGeneratorConfiguration = new TokenGeneratorConfiguration();
builder.Configuration.Bind("Authentication", tokenGeneratorConfiguration);
builder.Services.AddSingleton(tokenGeneratorConfiguration);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenGeneratorConfiguration.AccessTokenSecret)),
            ValidIssuer = tokenGeneratorConfiguration.Issuer,
            ValidAudience = tokenGeneratorConfiguration.Audience,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ResourceAccess", policy => policy.RequireClaim("TokenType", "Access"));
    options.AddPolicy("RefreshTokenAccess", policy => policy.RequireClaim("TokenType", Token.Refresh.ToString()));
    options.AddPolicy("ResetPassword", policy => policy.RequireClaim("TokenType", Token.ResetPassword.ToString()));
    options.AddPolicy("ApproveEmail", policy => policy.RequireClaim("TokenType", Token.ApproveEmail.ToString()));

    options.DefaultPolicy = options.GetPolicy("ResourceAccess");
});




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
