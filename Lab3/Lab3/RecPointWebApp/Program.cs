using RecPointWebApp.Services;
using RecyclingPointLib.Data;
using Microsoft.EntityFrameworkCore;
using RecyclingPointLib.Models;
using RecPointWebApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
string connString = builder.Configuration.GetConnectionString("SQLConnection");
builder.Services.AddDbContext<RecPointContext>(options => options.UseSqlServer(connString));

builder.Services.AddMemoryCache();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<ICachedEmployees, CachedEmployees>();
builder.Services.AddScoped<ICachedPositions, CachedPositions>();
builder.Services.AddScoped<ICachedStorages, CachedStorages>();

var app = builder.Build();
var env = builder.Environment;

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();

app.Map("/info", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Информация:</H1>";
        strResponse += "<BR> Сервер: " + context.Request.Host;
        strResponse += "<BR> Путь: " + context.Request.PathBase;
        strResponse += "<BR> Протокол: " + context.Request.Protocol;
        strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
        await context.Response.WriteAsync(strResponse);
    });
});

// Вывод кэшированной информации из таблицы базы данных
app.Map("/positions", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        ICachedPositions cachedPositions = context.RequestServices.GetService<ICachedPositions>();
        IEnumerable<Position> positions = cachedPositions.GetPositions(20);
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
        HtmlString += "</BODY></HTML>";

        await context.Response.WriteAsync(HtmlString);
    });
});

app.Map("/employees", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        ICachedPositions cachedPositions = context.RequestServices.GetService<ICachedPositions>();
        cachedPositions.AddPositions("Positions20", 1000);
        ICachedEmployees cachedEmployees = context.RequestServices.GetService<ICachedEmployees>();
        IEnumerable<Employee> employees = cachedEmployees.GetEmployees(20);
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
        HtmlString += "<TH>Стаж</TH>";
        HtmlString += "</TR>";
        foreach (var employee in employees)
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + employee.Id + "</TD>";
            HtmlString += "<TD>" + employee.Surname + "</TD>";
            HtmlString += "<TD>" + employee.Name + "</TD>";
            HtmlString += "<TD>" + employee.Patronymic + "</TD>";
            HtmlString += "<TD>" + employee.Position?.Name + "</TD>";
            HtmlString += "<TD>" + employee.Experience + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";

        await context.Response.WriteAsync(HtmlString);
    });
});

app.Map("/searchEmployees", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        string employeeName;
        context.Request.Cookies.TryGetValue("employeeName", out employeeName);
        ICachedPositions cachedPositions = context.RequestServices.GetService<ICachedPositions>();
        cachedPositions.AddPositions("Positions20", 1000);
        ICachedEmployees cachedEmployees = context.RequestServices.GetService<ICachedEmployees>();
        IEnumerable<Employee> employees = cachedEmployees.GetEmployees(30);
        string HtmlString = "<HTML><HEAD><TITLE>Сотрудники</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Список сотрудников по имени</H1>" +
        "<BODY><FORM action ='/searchEmployees' / >" +
        "Имя:<BR><INPUT type = 'text' name = 'employeeName' value = " + employeeName + ">" +
        "<BR><BR><INPUT type ='submit' value='Сохранить в cookies и вывести сотрудников с заданным именем'></FORM>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Код</TH>";
        HtmlString += "<TH>Фамилия</TH>";
        HtmlString += "<TH>Имя</TH>";
        HtmlString += "<TH>Отчество</TH>";
        HtmlString += "<TH>Должность</TH>";
        HtmlString += "<TH>Стаж</TH>";
        HtmlString += "</TR>";
        employeeName = context.Request.Query["employeeName"];
        if (employeeName != null)
        {
            context.Response.Cookies.Append("employeeName", employeeName);
        }
        foreach (var employee in employees.Where(e => e.Name.Trim() == employeeName))
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + employee.Id + "</TD>";
            HtmlString += "<TD>" + employee.Surname + "</TD>";
            HtmlString += "<TD>" + employee.Name + "</TD>";
            HtmlString += "<TD>" + employee.Patronymic + "</TD>";
            HtmlString += "<TD>" + employee.Position?.Name + "</TD>";
            HtmlString += "<TD>" + employee.Experience + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";

        await context.Response.WriteAsync(HtmlString);
    });
});

app.Map("/storages", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        ICachedStorages cachedStorages = context.RequestServices.GetService<ICachedStorages>();
        IEnumerable<Storage> storages = cachedStorages.GetStorages(20);
        string HtmlString = "<HTML><HEAD><TITLE>Складские помещения</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Список складских помещений</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Код</TH>";
        HtmlString += "<TH>Название</TH>";
        HtmlString += "<TH>Площадь</TH>";
        HtmlString += "<TH>Изношенность</TH>";
        HtmlString += "<TH>Заполненность</TH>";
        HtmlString += "<TH>Вместимость</TH>";
        HtmlString += "</TR>";
        foreach (var storage in storages)
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + storage.Id + "</TD>";
            HtmlString += "<TD>" + storage.Name + "</TD>";
            HtmlString += "<TD>" + storage.Square + "</TD>";
            HtmlString += "<TD>" + storage.Depreciation + "</TD>";
            HtmlString += "<TD>" + storage.Occupancy + "</TD>";
            HtmlString += "<TD>" + storage.Capacity + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";

        await context.Response.WriteAsync(HtmlString);
    });
});

app.Map("/searchStorages", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        int square;
        if (context.Session.Keys.Contains("square"))
        {
            square = context.Session.Get<int>("square");
        }       
        square = Convert.ToInt32(context.Request.Query["square"]);
        ICachedStorages cachedStorages = context.RequestServices.GetService<ICachedStorages>();
        IEnumerable<Storage> storages = cachedStorages.GetStorages(20);
        string HtmlString = "<HTML><HEAD><TITLE>Складские помещения</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Список складских помещений по площади</H1>" +
        "<BODY><FORM action = '/searchStorages'/>" +
        "Площадь: <BR><INPUT type = 'text' name = 'square' value = " + square + ">" +
        "<BR><BR><INPUT type = 'submit' value = 'Сохранить в сессию и вывести склады с площадью больше заданной'></FORM>" + 
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Код</TH>";
        HtmlString += "<TH>Название</TH>";
        HtmlString += "<TH>Площадь</TH>";
        HtmlString += "<TH>Изношенность</TH>";
        HtmlString += "<TH>Заполненность</TH>";
        HtmlString += "<TH>Вместимость</TH>";
        HtmlString += "</TR>";
        foreach (var storage in storages.Where(s => s.Square > square))
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + storage.Id + "</TD>";
            HtmlString += "<TD>" + storage.Name + "</TD>";
            HtmlString += "<TD>" + storage.Square + "</TD>";
            HtmlString += "<TD>" + storage.Depreciation + "</TD>";
            HtmlString += "<TD>" + storage.Occupancy + "</TD>";
            HtmlString += "<TD>" + storage.Capacity + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";

        await context.Response.WriteAsync(HtmlString);
    });
});

app.Run((context) =>
{
    ICachedPositions cachedPositions = context.RequestServices.GetService<ICachedPositions>();
    cachedPositions.AddPositions("Positions20", 1000);
    ICachedEmployees cachedEmployees = context.RequestServices.GetService<ICachedEmployees>();
    cachedEmployees.AddEmployees("Employees20", 21000);
    ICachedStorages cachedStorages = context.RequestServices.GetService<ICachedStorages>();
    cachedStorages.AddStorages("Storages20", 21000);
    string HtmlString = "<HTML><HEAD><TITLE>Главная</TITLE></HEAD>" +
    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
    "<BODY><H1>Главная</H1>";
    HtmlString += "<H2>Данные записаны в кэш сервера</H2>";
    HtmlString += "<BR><A href='/'>Главная</A></BR>";
    HtmlString += "<BR><A href='/positions'>Должности</A></BR>";
    HtmlString += "<BR><A href='/employees'>Сотрудники</A></BR>";
    HtmlString += "<BR><A href='/searchEmployees'>Поиск по сотрудникам</A></BR>";
    HtmlString += "<BR><A href='/storages'>Складские помещения</A></BR>";
    HtmlString += "<BR><A href='/searchStorages'>Поиск по складским помещениям</A></BR>";
    HtmlString += "<BR><A href='/info'>Информация о клиенте</A></BR>";
    HtmlString += "</BODY></HTML>";

    return context.Response.WriteAsync(HtmlString);
});

app.Run();
