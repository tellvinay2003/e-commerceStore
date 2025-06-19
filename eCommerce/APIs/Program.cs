using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy 
builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder => {
                builder.WithOrigins("http://localhost:4200");
                builder.WithMethods("GET", "POST");
                builder.AllowAnyHeader();
            });
        });


# region Service region

// Add services to the container.

builder.Services.AddControllers();

// add db context with options for SqlLite
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    

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

// Use CORS policy
app.UseCors();
// middleware sequence 3
// ensures policy checks
app.UseAuthorization();

// middleware sequence 4
// handle controller routes
app.MapControllers();

// middleware sequence 5
app.Run();

#endregion Middleware


