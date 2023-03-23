using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BigioHrServices.Db;
using BigioHrServices.Services;
using BigioHrServices.Utilities;
using BigioHrServices.Db.Entities;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AddAuthorizationHeaderOperationFilter>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
});

//JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            // Call this to skip the default logic and avoid using the default response
            context.HandleResponse();

            // Write to the response in any way you wish
            var exceptionResult = JsonConvert.SerializeObject(new { message = "You are not authorized!" });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 401;

            await context.Response.WriteAsync(exceptionResult);
        }
    };
});
builder.Services.AddAuthorization(opts =>
{
    opts.DefaultPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});

builder.Services.AddScoped<IAuthUserService, AuthUserService>();
builder.Services.AddScoped(typeof(IEmployeeService), typeof(EmployeeServices));
builder.Services.AddScoped(typeof(IAuthenticationService), typeof(AuthenticationServices));
builder.Services.AddScoped(typeof(Hasher));
builder.Services.AddScoped(typeof(IDelegationService), typeof(DelegationServices));
builder.Services.AddScoped(typeof(ILeaveService), typeof(LeaveService));
builder.Services.AddScoped(typeof(IPositionService), typeof(PositionServices));
builder.Services.AddScoped(typeof(INotificationService), typeof(NotificationServices));
builder.Services.AddScoped(typeof(IAuditModuleServices), typeof(AuditModuleServices));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
