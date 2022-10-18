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
            app.Map("/positions", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //обращение к сервису
                    ICachedPositions cachedPositions = context.RequestServices.GetService<ICachedPositions>();
                    IEnumerable<Position> positions = cachedPositions.GetPositions(10);
                    string HtmlString = "<HTML><HEAD><TITLE>Должности</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список должностей</H1>" +
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
                    HtmlString += "<BR><A href='/positions'>Должности</A></BR>";
                    HtmlString += "<BR><A href='/employees'>Сотрудники</A></BR>";
                    HtmlString += "<BR><A href='/storages'>Складские помещения</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // Вывод данных
                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/employees", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //обращение к сервису
                    ICachedEmployees cachedEmployees  = context.RequestServices.GetService<ICachedEmployees>();
                    IEnumerable<Employee> employees = cachedEmployees.GetEmployees(10);
                    string HtmlString = "<HTML><HEAD><TITLE>Сотрудники</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список сотрудников</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Код</TH>";
                    HtmlString += "<TH>Фамилия</TH>";
                    HtmlString += "<TH>Имя</TH>";
                    HtmlString += "<TH>Отчество</TH>";
                    HtmlString += "<TH>Должность</TH>";
                    HtmlString += "</TR>";
                    foreach (var employee in employees)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + employee.Id + "</TD>";
                        HtmlString += "<TD>" + employee.Surname + "</TD>";
                        HtmlString += "<TD>" + employee.Name + "</TD>";
                        HtmlString += "<TD>" + employee.Patronymic + "</TD>";
                        HtmlString += "<TD>" + employee.Position?.Name + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/positions'>Должности</A></BR>";
                    HtmlString += "<BR><A href='/employees'>Сотрудники</A></BR>";
                    HtmlString += "<BR><A href='/storages'>Складские помещения</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // Вывод данных
                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/storages", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //обращение к сервису
                    ICachedStorages cachedStorages = context.RequestServices.GetService<ICachedStorages>();
                    IEnumerable<Storage> storages = cachedStorages.GetStorages(10);
                    string HtmlString = "<HTML><HEAD><TITLE>Складские помещения</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список складских помещений</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Код</TH>";
                    HtmlString += "<TH>Наименование</TH>";
                    HtmlString += "<TH>Номер</TH>";
                    HtmlString += "</TR>";
                    foreach (var storage in storages)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + storage.Id + "</TD>";
                        HtmlString += "<TD>" + storage.Name+ "</TD>";
                        HtmlString += "<TD>" + storage.Number + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
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
                cachedEmployees.AddEmployees("Employees10", 21000);
                cachedStorages.AddStorages("Storages10");
                string HtmlString = "<HTML><HEAD><TITLE>Главная</TITLE></HEAD>" +
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
