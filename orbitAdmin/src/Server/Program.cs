﻿using SchoolV01.Application.Extensions;
using SchoolV01.Server.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolV01.Infrastructure.Extensions;
using WkHtmlToPdfDotNet;
using Microsoft.AspNetCore.Builder;
using Hangfire;
using SchoolV01.Server.Managers.Preferences;
using Microsoft.Extensions.Configuration;
using WkHtmlToPdfDotNet.Contracts;
using Microsoft.AspNetCore.Mvc;
using SchoolV01.Server.Middlewares;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using SchoolV01.Server.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var _configuration = builder.Configuration;
// This method gets called by the runtime. Use this method to add services to the container.
// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

builder.Host.UseSerilog();
builder.Services.AddCors();
builder.Services.AddSignalR();
builder.Services.AddHealthChecks();
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});
builder.Services.AddCurrentUserService();


builder.Services.AddSerialization();
builder.Services.AddDatabase(_configuration);
builder.Services.AddServerStorage(); //TODO - should implement ServerStorageProvider to work correctly!
builder.Services.AddScoped<ServerPreferenceManager>();

builder.Services.AddServerLocalization();
builder.Services.AddIdentity();
builder.Services.AddJwtAuthentication(builder.Services.GetApplicationSettings(_configuration));
builder.Services.AddApplicationLayer();
builder.Services.AddApplicationServices();
builder.Services.AddRepositories();
builder.Services.AddExtendedAttributesUnitOfWork();
builder.Services.AddSharedInfrastructure(_configuration);
builder.Services.RegisterSwagger();
builder.Services.AddInfrastructureMappings();
builder.Services.AddHangfire(x => x.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(_configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();
builder.Services.AddControllers();
builder.Services.AddValidators();
builder.Services.AddExtendedAttributesValidators();
builder.Services.AddExtendedAttributesHandlers();
builder.Services.AddRazorPages();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});
builder.Services.AddLazyCache();



var app = builder.Build();

app.InitializeDatabase();

// Configure the HTTP request pipeline.
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseExceptionHandling(app.Environment);
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
    RequestPath = new PathString("/Files")
});
app.UseRequestLocalizationByCulture();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    DashboardTitle = "BlazorHero Jobs",
    Authorization = [new HangfireAuthorizationFilter()]
});
app.UseHangfireServer();
app.UseEndpoints();
app.ConfigureSwagger();

app.Run();


