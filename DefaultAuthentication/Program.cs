using DefaultAuthentication;
using DefaultAuthentication.CustomHandler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var key = "keydemoshouldbelongtouseforalgorithm";

//authenticate with JWT
/*
builder.Services.AddSingleton<IJwtAuthenticationManager>(e => new JwtAuthenticationManager(key, e.GetService<IRefreshTokenGenerator>()!));
builder.Services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
builder.Services.AddSingleton<ITokenRefresher>(e => new TokenRefresher(key, e.GetService<IJwtAuthenticationManager>())); 
builder.Services.AddAuthentication(e =>
{
    e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    e.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(e => { 
    e.RequireHttpsMetadata = false;
    e.SaveToken = true;
    e.TokenValidationParameters = new TokenValidationParameters() {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
*/

//authenticate with customer handler -> use Guid as token instead of Jwt
builder.Services.AddSingleton<ICustomAuthenticationManager, CustomAuthenticationManager>();
builder.Services.AddAuthentication("Basic")
    .AddScheme<CustomAuthenticationOptions, CustomAuthenticationHandler>("Basic",null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<AuthHandler>("Test");  //Basic Authentication
app.UseHttpsRedirection();

app.UseAuthentication(); //JWT
app.UseAuthorization();

app.MapControllers();

app.Run();
