using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallTracerLibrary.DataProviders;

using CallTracerLibrary.Middlewares;
using CallTracerLibrary.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleWebAPI.DataProviders;

//using Steeltoe.CloudFoundry.Connector.MySql;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;

namespace SampleWebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }  //PCF WHAT IS THIS ??

        public Startup(IConfiguration configuration) //PCF
        {
            Configuration = configuration;

        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // Add MySqlConnection configured from Configuration
            services.AddDbContext<TraceMetadataContext>(options => options.UseMySql(Configuration)); //PCF

            services.AddRouting();


            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
            });
                
           
            services.AddSingleton<IProductsProvider, ProductsProviderMongo>();

          // services.AddSingleton<IRepository<TraceMetadata,int>, MySQLRepository>();    //InMemory,Mongo,MySQL --> Availible Repositoreis 
              services.AddSingleton<IRepository<TraceMetadata, int>, MongoRepository>();
            // services.AddSingleton<IRepository<TraceMetadata, int>, InMemoryRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Random", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowAll");
            app.UseStaticFiles();
            app.UseSwagger().UseSwaggerUI( c=>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Random");
            });
            //  app.TraceEndPoint();
            app.UseMiddleware<TracingMiddleware>();
           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.Run(async (context) =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            }

            else if(env.IsProduction())  //Not in Devlopment mode --> Extra Addtion
            {
                app.UseExceptionHandler(appBuilder=>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected Fault happended ");

                    });

                });
                
            }
            else
            {
                //
            }
            
            app.UseMvc();
           

        }
    }
}
