using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebAPI
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
            services.AddSingleton<IBookService, BookManager>();
            services.AddSingleton<IBookDal, EfBookDal>();
            //Ba��ml�l�klar� projemizin anlamas� a��s�nda soyut bir referans tipinde
            //nesne �a�r�ld���nda ger�ek s�n�f neye kar��l�k geliyor bunu ��z�ml�yoruz

            //Tabi bu ba��ml�l�klar� ��zmek Autofac ya da Ninject tarz� k�t�phanelerle
            //ayr� birimde ba��ml�l�klar� ��z�mlemek daha profesyonel olacakt�r.�zellikle
            //Autofac'in Interceptor yap�s� olmas� itibariyle AOP denilen nesne y�nelimli kodlama
            //tekni�i uygulamam�za yard�mc� oluyor.O y�zden Autofac kullan�labilir

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader());
            });

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader());
            //UseCors middleware ile api'yi kullanacak olan bir origine izin sa�lan�yor burada 4200 portlu olan angular uygulamas�na
            //api'nin kendisini kullanmas�na izin veriliyor.
            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
