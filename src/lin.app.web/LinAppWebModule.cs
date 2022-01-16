using System;
using lin.app.Application;
using lin.app.Application.Contracts;
using lin.app.Application.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Identity.Web;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpSwashbuckleModule),

    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(LinAppEntityFrameworkModule),
    typeof(LinAppApplicationContractsModule),
    typeof(LinAppApplicationModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpAccountWebModule)
    

)]
public class LinAppWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;

        //... other configarations.

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options
                .ConventionalControllers
                .Create(typeof(LinAppApplicationModule).Assembly);
        });
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {

        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseSwagger();
        app.UseAbpSwaggerUI();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseConfiguredEndpoints(
            option =>
            {
                option.Map("/", () => $"lin app web,now:{DateTime.Now}");
                
            }
        );
    }
}