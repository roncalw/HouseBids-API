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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();    
    app.UseCors(p => p.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
}

//This had to be moved from running it immediately after the app declaration to below the UseCors method that is above, because it would cause a CORS error otherwise when running in dev. 
//The CORS configuration has to run before working with HTTP verbs such as this method does to avoid the browser error.
//It did not matter when running in production because the CORS configuration in web.config runs before this method. 
app.Use(async (context, next) =>
{
    var methodvalue = context.Request.Method;
    if (!string.IsNullOrEmpty(methodvalue))
    {
 
        if (methodvalue == HttpMethods.Options || methodvalue == HttpMethods.Head)
        {
            await context.Response.WriteAsync("Ok");
            context.Response.StatusCode = 200;
        }
        else
        {
            await next();
        }
    }
});


app.UseHttpsRedirection();

app.MapHouseEndpoints();
app.MapBidEndpoints();

app.Run();
