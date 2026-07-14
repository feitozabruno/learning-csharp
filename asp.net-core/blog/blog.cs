using Blog.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddErrorHandling();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentityConfiguration();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseExceptionHandler();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();