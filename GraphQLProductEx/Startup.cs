using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQLProductEx.Data;
using GraphQLProductEx.Query;
using GraphQLProductEx.Schema;
using GraphQLProductEx.Settings;
using GraphQLProductEx.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GraphQLProductEx
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddTransient<ProductType>();
            services.AddTransient<ProductInputType>();
            services.AddTransient<ProductQuery>();
            services.AddTransient<ProductSchema>();
            services.AddTransient<ProductQuery>();
            services.AddTransient<IProductRepository,ProductRepository>();
            
            var connectionStrings = Configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();
            services.AddSingleton(connectionStrings);

            services.AddGraphQL(b => b
                .AddSystemTextJson()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                .AddSchema<ProductSchema>()
                .AddGraphTypes(typeof(Startup).Assembly)
            );
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "GraphQLProductEx", Version = "v1"});
            });
            
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphQLProductEx v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseGraphQL<ISchema>();
            app.UseGraphQLAltair();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}