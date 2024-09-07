using Humanizer; 

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.Use(async (context, next) =>
{
    context.Response.ContentType = "text/html; charset=utf-8"; 
    await next();
});


app.Use(async (context, next) =>
{
    if (context.Request.Query.ContainsKey("number"))
    {
        if (int.TryParse(context.Request.Query["number"], out int number))
        {
            if (number < 1 || number > 100000)
            {
                await context.Response.WriteAsync("<span style='font-size: 24px;'>Число вне диапазона (1-100000)</span>");
                return;
            }
        }
        else
        {
            await context.Response.WriteAsync("<span style='font-size: 24px;'>Неверный формат числа</span>");
            return;
        }
    }
    await next();
});


app.Use(async (context, next) =>
{
    if (context.Request.Query.ContainsKey("number"))
    {
        int number = int.Parse(context.Request.Query["number"]);
        string interpretation = InterpretNumber(number);
        await context.Response.WriteAsync($"<span style='font-size: 24px;'>Число {number} ===> {interpretation}</span>");
    }
    else
    {
        await next();
    }
});

app.Run();

string InterpretNumber(int number)
{
    return number.ToWords(); 
}
