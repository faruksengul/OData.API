using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OData.API.Model;

namespace OData.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConStr"]);
            });

            services.AddOData();
            services.AddControllers();
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            var builder = new ODataConventionModelBuilder();

            //CategoriesContoller
            //[entity set Name]Controller
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Product>("Products");

            //.../odata/category(1)/totalproductprice
            builder.EntityType<Category>().Action("TotalProductPrice").Returns<int>();
            builder.EntityType<Category>().Collection.Action("TotalProductPrice2").Returns<int>();


            //odata/category/totalproductprice
            builder.EntityType<Category>().Collection.Action("TotalProductPriceWithParametre").Returns<int>().Parameter<int>("categoryId");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {

                endpoints.Select().Expand().OrderBy().MaxTop(null).Count().Filter();
                //www.api.com/odata/products
                endpoints.MapODataRoute("odata", "odata", builder.GetEdmModel());
                endpoints.MapControllers();
            });
        }
    }
}
