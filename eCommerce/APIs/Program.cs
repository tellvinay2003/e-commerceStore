using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

# region Service region

// Add services to the container.

builder.Services.AddControllers();

// add db context with options for SqlLite
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion Service region


var app = builder.Build();

#region Middleware Region

// Middllewarw region Start 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// middleware sequence 1
app.UseHttpsRedirection();

// middleware sequence 2
// sets HttpContext.User 
app.UseAuthentication();

// middleware sequence 3
// ensures policy checks
app.UseAuthorization();

// middleware sequence 4
// handle controller routes
app.MapControllers();

// middleware sequence 5
app.Run();

#endregion Middleware


