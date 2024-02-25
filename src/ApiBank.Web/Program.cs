using ApiBank.Web.Database;
using ApiBank.Web.Models;
using ApiBank.Web.Repositories;
using ApiBank.Web.Repositories.Interfaces;
using ApiBank.Web.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
//.AddFluentValidation( x => x.RegisterValidatorsFromAssemblyContaining<ContaValidator>());

//builder.Services.AddControllers().AddFluentValidation();
//builder.Services.AddTransient<IValidator<Conta>, ContaValidator>();

//builder.Services.AddScoped<IValidator<Conta>, ContaValidator>();

builder.Services.AddMemoryCache();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.ResolveConflictingActions(apiDescription => apiDescription.First());
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiBank.Web", Version = "v1" });
        c.SwaggerDoc("v2", new OpenApiInfo { Title = "ApiBank.Web", Version = "v2" });
    });

builder.Services.AddDbContext<ApiBankContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));    
    });

// DI
builder.Services.AddScoped<IContaRepository, ContaRepository>();

builder.Services.AddApiVersioning(cfg => {
    cfg.ReportApiVersions = true;

    //Formas de receber numero da versão da api
    cfg.ApiVersionReader = new HeaderApiVersionReader("api-version");
    cfg.AssumeDefaultVersionWhenUnspecified = true;
    cfg.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
   
    app.UseSwaggerUI(c => 
    { 
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiBank.Web v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "ApiBank.Web v2");        
    });
}

app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();