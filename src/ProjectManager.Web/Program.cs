using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectManager.Core;
using ProjectManager.Infrastructure;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.ListStartupServices;
using ProjectManager.Infrastructure.Data;
using ProjectManager.Web;
using Microsoft.OpenApi.Models;
using AutoMapper;
using ProjectManager.Web.MappingProfiles;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ProjectManager.Web.Authentication;
using Autofac.Core;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.Services;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using Microsoft.AspNetCore.OData;
using ProjectManager.Core.Logging;
using ProjectManager.Web.Validation;
using FluentValidation.AspNetCore;
using System.Reflection;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.DirectApiCalls.Services;
using Majorsoft.Blazor.Components.Common.JsInterop.Scroll;
using ProjectManager.Web.FileServices.Interfaces;
using ProjectManager.Web.FileServices.Services;
using ProjectManager.Infrastructure.Data.Config;

var builder = WebApplication.CreateBuilder(args);

var envConfig = builder.Configuration;
envConfig.AddEnvironmentVariables("ProjectManagerWebApp_");

if (envConfig.GetValue<string>("Database:ConnectionString") == "Server=test-db;Port=5432;Database=projectmanagerDb;User ID=postgres;Password=postgrespw;")
{
  AppDbContextOptions.IsE2ETesting = true;
  AppDbContextOptions.ConnectionString = envConfig.GetValue<string>("Database:ConnectionString");
}

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

var config = new MapperConfiguration(c =>
{
  c.AddProfile<MappingProfile>();
});

builder.Services.AddSingleton<IMapper>(s => config.CreateMapper());
//string connectionString = builder.Configuration.GetConnectionString("SqliteConnection");  //Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext();
builder.Services
  .AddControllers()
  .AddFluentValidation(options =>
  {
    options.RegisterValidatorsFromAssemblyContaining<Program>();
    options.DisableDataAnnotationsValidation = true;
  })
  .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
  .AddOData(x => x.Select().Filter().OrderBy().Expand());

builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddAuthenticationCore();

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddHostedService<BackgroundWorkerService>();

//var serviceProvider = builder.Services.BuildServiceProvider();
//var loggers = serviceProvider.GetService<ILogger<AssignmentController>>();
//builder.Services.AddSingleton(typeof(ILogger), loggers);
//builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
//builder.Services.AddLogging();

builder.Services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
builder.Services.AddScoped<IImageService, ImageService>();

// API SERVICES
builder.Services.AddTransient<IUserCallService, UserCallService>();
builder.Services.AddTransient<IProjectCallService, ProjectCallService>();
builder.Services.AddTransient<IChatMessageCallService, ChatMessageCallService>();
builder.Services.AddTransient<IAssignmentCallService, AssignmentCallService>();
builder.Services.AddTransient<IAssignmentStageCallService, AssignmentStageCallService>();
builder.Services.AddTransient<IAppointmentCallService, AppointmentCallService>();
builder.Services.AddTransient<INotificationCallService, NotificationCallService>();
builder.Services.AddTransient<IPrivateMessageCallService, PrivateMessageCallService>();
builder.Services.AddTransient<IChatChannelCallService, ChatChannelCallService>();
builder.Services.AddTransient<IInvitationLinkCallService, InvitationLinkCallService>();

// CORE SERVICES
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IChatMessageService, ChatMessageService>();
builder.Services.AddTransient<IAssignmentService, AssignmentService>();
builder.Services.AddTransient<IAssignmentStageService, AssignmentStageService>();
builder.Services.AddTransient<IAppointmentService, AppointmentService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IPrivateMessageService, PrivateMessageService>();
builder.Services.AddTransient<IChatChannelService, ChatChannelService>();
builder.Services.AddTransient<IInvitationLinkService, InvitationLinkService>();

// FRONTEND SERVICES
builder.Services.AddTransient<IScrollHandler, ScrollHandler>();

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
  c.EnableAnnotations();
});

// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
  config.Services = new List<ServiceDescriptor>(builder.Services);

  // optional - default path to view services is /listallservices - recommended to choose your own path
  config.Path = "/listservices";
});

builder.Services.AddHttpClient("api", cfg =>
{
  cfg.BaseAddress = new Uri(envConfig.GetValue<string>("Api:ApiBaseUrl"));
});
builder.Services.AddHttpContextAccessor();


builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  //containerBuilder.RegisterModule(new DefaultCoreModule());
  containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));
});

//builder.Logging.AddAzureWebAppDiagnostics(); add this if deploying to Azure

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware();
}
else
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}
app.UseRouting();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseEndpoints(endpoints =>
{
  endpoints.MapDefaultControllerRoute();
  endpoints.MapRazorPages();
  endpoints.MapBlazorHub();
  endpoints.MapFallbackToPage("/_Host");
  endpoints.MapControllers();
});

app.UseODataQueryRequest();
app.UseODataBatching();
app.UseODataRouteDebug();

// Seed Database
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  try
  {
    var context = services.GetRequiredService<AppDbContext>();
    //context.Database.Migrate();
    context.Database.EnsureCreated();
    //SeedData.Initialize(services);
  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
  }
}

app.Run();
