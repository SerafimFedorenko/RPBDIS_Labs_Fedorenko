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
        string strResponse = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>����������:</H1>";
        strResponse += "<BR> ������: " + context.Request.Host;
        strResponse += "<BR> ����: " + context.Request.PathBase;
        strResponse += "<BR> ��������: " + context.Request.Protocol;
        strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";
        await context.Response.WriteAsync(strResponse);
    });
});

// ����� ������������ ���������� �� ������� ���� ������
app.Map("/positions", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        ICachedPositions cachedPositions = context.RequestServices.GetService<ICachedPositions>();
        IEnumerable<Position> positions = cachedPositions.GetPositions(20);
        string HtmlString = "<HTML><HEAD><TITLE>���������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>������ ����������</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>������������</TH>";
        HtmlString += "</TR>";
        foreach (var position in positions)
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + position.Id + "</TD>";
            HtmlString += "<TD>" + position.Name + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
        string HtmlString = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>������ �����������</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>�������</TH>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>��������</TH>";
        HtmlString += "<TH>���������</TH>";
        HtmlString += "<TH>����</TH>";
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
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
        string HtmlString = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>������ ����������� �� �����</H1>" +
        "<BODY><FORM action ='/searchEmployees' / >" +
        "���:<BR><INPUT type = 'text' name = 'employeeName' value = " + employeeName + ">" +
        "<BR><BR><INPUT type ='submit' value='��������� � cookies � ������� ����������� � �������� ������'></FORM>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>�������</TH>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>��������</TH>";
        HtmlString += "<TH>���������</TH>";
        HtmlString += "<TH>����</TH>";
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
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
        string HtmlString = "<HTML><HEAD><TITLE>��������� ���������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>������ ��������� ���������</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>��������</TH>";
        HtmlString += "<TH>�������</TH>";
        HtmlString += "<TH>������������</TH>";
        HtmlString += "<TH>�������������</TH>";
        HtmlString += "<TH>�����������</TH>";
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
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
        string HtmlString = "<HTML><HEAD><TITLE>��������� ���������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>������ ��������� ��������� �� �������</H1>" +
        "<BODY><FORM action = '/searchStorages'/>" +
        "�������: <BR><INPUT type = 'text' name = 'square' value = " + square + ">" +
        "<BR><BR><INPUT type = 'submit' value = '��������� � ������ � ������� ������ � �������� ������ ��������'></FORM>" + 
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>��������</TH>";
        HtmlString += "<TH>�������</TH>";
        HtmlString += "<TH>������������</TH>";
        HtmlString += "<TH>�������������</TH>";
        HtmlString += "<TH>�����������</TH>";
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
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
    string HtmlString = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
    "<BODY><H1>�������</H1>";
    HtmlString += "<H2>������ �������� � ��� �������</H2>";
    HtmlString += "<BR><A href='/'>�������</A></BR>";
    HtmlString += "<BR><A href='/positions'>���������</A></BR>";
    HtmlString += "<BR><A href='/employees'>����������</A></BR>";
    HtmlString += "<BR><A href='/searchEmployees'>����� �� �����������</A></BR>";
    HtmlString += "<BR><A href='/storages'>��������� ���������</A></BR>";
    HtmlString += "<BR><A href='/searchStorages'>����� �� ��������� ����������</A></BR>";
    HtmlString += "<BR><A href='/info'>���������� � �������</A></BR>";
    HtmlString += "</BODY></HTML>";

    return context.Response.WriteAsync(HtmlString);
});

app.Run();
