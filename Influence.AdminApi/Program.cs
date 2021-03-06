using System.Security.Claims;
using System.Text.Json.Serialization;
using Cosmetic.Model.Enums.Policy;
using FluentValidation.AspNetCore;
using Influence.Model;
using Influence.Service;
using Influence.Service.Configuration;
using Influence.Service.Dependencies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.UseConfigurator(builder.Configuration); 
builder.Services.AddMvc(options => { options.Filters.Add<AdminAuthFilter>(); }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })

    .AddFluentValidation(opt => { opt.RegisterValidatorsFromAssemblyContaining<AssemblyIdentifier>(); });

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy(PolicyEnum.AdminPolicy.ToString(),
        policy => policy.RequireClaim(ClaimTypes.UserData).RequireClaim("RefreshToken")
            .RequireClaim("MemberId").RequireClaim("RoleId"));
});
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