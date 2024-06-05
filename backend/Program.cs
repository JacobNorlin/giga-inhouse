var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<DbService, DbService>();
builder.Services.AddSingleton<CS2Service, CS2Service>();
builder.Services.AddSingleton<UserService, UserService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(config =>
    {
        config.AllowAnyOrigin();
        config.AllowAnyHeader();
        config.AllowAnyMethod();
    });
}

// Register middleware for session token validation
app.UseWhen(context =>
{
    var path = context.Request.Path.Value;
    return !path!.StartsWith("/User");
}, appBuilder =>
{
    appBuilder.UseMiddleware<AuthenticationMiddleware>();
});



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
