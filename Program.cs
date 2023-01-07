using Microsoft.AspNetCore.Mvc;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<HouseDbContext>(options => options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddScoped<IHouseRepository, HouseRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>();

var app = builder.Build();


app.Use(async (context, next) =>
{
    var methodvalue = context.Request.Method;
    if (!string.IsNullOrEmpty(methodvalue))
    {
 
        if (methodvalue == HttpMethods.Options || methodvalue == HttpMethods.Head)
        {
            await context.Response.WriteAsync("Option Request");
        }
        else
        {
            await next();
        }
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();    
    app.UseCors(p => p.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
}


app.UseHttpsRedirection();

app.MapHouseEndpoints();
app.MapBidEndpoints();

/*
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("Ok");
            }
    
    await next.Invoke();
});
*/


app.Run();
