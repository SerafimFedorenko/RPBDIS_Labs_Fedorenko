using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuelStation.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecPointWebApp.Services;
using RecyclingPointLib.Data;
using RecyclingPointLib.Models;

namespace RecPointWebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("SQLConnection");
            services.AddDbContext<RecPointContext>(options => options.UseSqlServer(connection));

            services.AddMemoryCache();

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddScoped<ICachedEmployees, CachedEmployees>();
            services.AddScoped<ICachedPositions, CachedPositions>();
            services.AddScoped<ICachedStorages, CachedStorages>();

            //Использование MVC - отключено
            //services.AddControllersWithViews();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // добавляем поддержку статических файлов
            app.UseStaticFiles();

            // добавляем поддержку сессий
            app.UseSession();
            //Запоминание в Сookies значений, введенных в форме
            //...

            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Формирование строки для вывода 
                    string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Информация:</H1>";
                    strResponse += "<BR> Сервер: " + context.Request.Host;
                    strResponse += "<BR> Путь: " + context.Request.PathBase;
                    strResponse += "<BR> Протокол: " + context.Request.Protocol;
                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
                    // Вывод данных
                    await context.Response.WriteAsync(strResponse);
                });
            });

            // Вывод кэшированной информации из таблицы базы данных
            app.Map("/tanks", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //обращение к сервису
                    ICachedPositions cachedTanksService = context.RequestServices.GetService<ICachedPositions>();
                    IEnumerable<Position> positions = cachedTanksService.GetPositions("Positions10");
                    string HtmlString = "<HTML><HEAD><TITLE>Емкости</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список емкостей</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Код</TH>";
                    HtmlString += "<TH>Наименование</TH>";
                    HtmlString += "</TR>";
                    foreach (var position in positions)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + position.Id + "</TD>";
                        HtmlString += "<TD>" + position.Name + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/positions'>Должности</A></BR>";
                    HtmlString += "<BR><A href='/employees'>Сотрудники</A></BR>";
                    HtmlString += "<BR><A href='/storages'>Складские помещения</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // Вывод данных
                    await context.Response.WriteAsync(HtmlString);
                });
            });

            // Стартовая страница и кэширование данных таблицы на web-сервере
            app.Run((context) =>
            {
                //обращение к сервису
                ICachedPositions cachedPositions = context.RequestServices.GetService<ICachedPositions>();
                ICachedEmployees cachedEmployees = context.RequestServices.GetService<ICachedEmployees>();
                ICachedStorages cachedStorages = context.RequestServices.GetService<ICachedStorages>();
                cachedPositions.AddPositions("Positions10");
                cachedEmployees.AddEmployees("Employees10");
                cachedStorages.AddStorages("Storages10");
                string HtmlString = "<HTML><HEAD><TITLE>Емкости</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>Главная</H1>";
                HtmlString += "<H2>Данные записаны в кэш сервера</H2>";
                HtmlString += "<BR><A href='/'>Главная</A></BR>";
                HtmlString += "<BR><A href='/positions'>Должности</A></BR>";
                HtmlString += "<BR><A href='/employees'>Сотрудники</A></BR>";
                HtmlString += "<BR><A href='/storages'>Складские помещения</A></BR>";
                HtmlString += "</BODY></HTML>";

                return context.Response.WriteAsync(HtmlString);
            });

            //Использование MVC - отключено
            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

        }
    }
}
