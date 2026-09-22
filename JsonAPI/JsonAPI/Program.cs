using Application.Handlers;
using Application.Interface;
using Application.Services;
using JsonAPI.Authorization;
using JsonAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



// Add Authentication Configurations
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidIssuer = "JsonApi",

          ValidateAudience = true,
          ValidAudience = "JsonApiUsers",

          ValidateLifetime = true,

          ValidateIssuerSigningKey = true,

          IssuerSigningKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(
                  "THIS_IS_A_VERY_SECRET_KEY_123456"))
      };

      options.Events = new JwtBearerEvents
      {
          OnAuthenticationFailed = context =>
          {
              Console.WriteLine(
                  $"JWT Authentication Failed: {context.Exception.Message}");

              return Task.CompletedTask;
          },

          OnTokenValidated = context =>
          {
              Console.WriteLine(
                  "JWT Authentication Successfully Validated");

              return Task.CompletedTask;
          }
      };
  });
// ===============================
// Authorization Configuration
// ===============================
// ===============================
// 3) Swagger / OpenAPI
// ===============================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecuritySchemeReference("Bearer",document), new List<string>()
        },
    });
});

// Register authorization services.
// This enables attributes like [Authorize] and role-based authorization.

// =============== Authorization Policies ====================
// Register  Handler for UserOwnerOrAdminRequirementass
builder.Services.AddSingleton<IAuthorizationHandler, UserOwnerOrAdminHandler>();

// Register Policy for UserOwnerOrAdminRequirementass

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOwnerOrAdmin", policy =>
    policy.Requirements.Add(new UserOwnerOrAdminRequirementas())
    );
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// add Configuration Service
builder.Configuration.GetSection("ExternalService");

// Add Swagger/OpenAPI support
//builder.Services.AddSwaggerGen();
// Add HttpClient service

// Added Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuth, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PostService>();

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddTransient<AuthHandler>();
builder.Services.AddHttpClient<HttpClientService>()
                .AddHttpMessageHandler<AuthHandler>();



// ===============Configure CORS ====================

builder.Services.AddCors(options =>
{
    options.AddPolicy("UserApiCorsPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:7061",
                "http://localhost:5166"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add the custom middleware to the pipeline

app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("UserApiCorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
