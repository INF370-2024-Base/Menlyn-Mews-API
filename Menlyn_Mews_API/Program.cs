using Menlyn_Mews_API.Data;
using Microsoft.EntityFrameworkCore; // added 13/04/2024


// using Menlyn_Mews.Data; //
// still need to add data from DB Context-----------------//
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


///----------------------Copied and pasted so that migrations could work. It does not come precoded in---------------////

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Menlyn_Mews"));
});
////------------------------------------------------------------------------------------------------------------------////////
///


///----------------------Copied and pasted so that migrations could work. It does not come precoded in---------------////

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",  //Policies define sets of origins, HTTP methods, headers, etc., that are allowed to access resources. In this case, we're configuring a policy that allows requests from the Angular application running on http://localhost:4200.
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") //Here, we're allowing requests from the Angular application running on
                   .AllowAnyMethod()
                   .AllowAnyHeader();

        });
});
////------------------------------------------------------------------------------------------------------------------////////


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularDev"); // to use cors // added 13/04/2024

app.UseAuthorization();

app.MapControllers();

app.Run();
