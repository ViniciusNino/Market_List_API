using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MarketList_Business;
using MarketList_Repository;
using MarketList_Data;
using MedPlannerCore.Data.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace MarketList_API
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MarketList_API", Version = "v1" });
            });

            services.AddDbContext<MarketListContext>(options => options.UseNpgsql(Common.GetSettings("DefaultConnectionPGSQL")).UseLazyLoadingProxies());

            services.AddTransient<IUsuarioBusiness, UsuarioBusiness>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IItemBusiness, ItemBusiness>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IListaBusiness, ListaBusiness>();
            services.AddTransient<IListaRepository, ListaRepository>();
            services.AddTransient<IItemListaBusiness, ItemListaBusiness>();
            services.AddTransient<IItemListaRepository, ItemListaRepository>();
            services.AddTransient<IUnidadeBusiness, UnidadeBusiness>();
            services.AddTransient<IUnidadeRepository, UnidadeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors(options =>
                options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MarketList_API v1"));

            app.Run(c => c.Response.WriteAsync("Market List"));
        }
    }
}
