using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<DbService, DbService>();
builder.Services.AddSingleton<CS2Service, CS2Service>();
builder.Services.AddSingleton<UserService, UserService>();
builder.Services.AddSingleton<LobbyService, LobbyService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(config =>
    {
        config.LoginPath = "/User/login";

        config.Cookie = new CookieBuilder
        {
            Name = "session-token",
            SecurePolicy = CookieSecurePolicy.Always,
            SameSite = SameSiteMode.None
        };


    });

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
        config.WithOrigins(["http://localhost:5173"]);
        config.AllowAnyHeader();
        config.AllowAnyMethod();
        config.AllowCredentials();
    });
}



// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Register middleware for session token validation
app.UseWhen(context =>
{
    var path = context.Request.Path.Value;
    return !path!.StartsWith("/User");
}, appBuilder =>
{
    appBuilder.UseMiddleware<AuthenticationMiddleware>();
});



app.MapControllers();

app.Run();
