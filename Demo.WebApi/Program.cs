using Tayvey.Tools.Extensions;

var builder = WebApplication.CreateBuilder(args);

#region Services
builder.Services.AddControllers();

builder.Services.AddTvSoap();
builder.Services.AddTvAutoDI();
#endregion

var app = builder.Build();

#region App
app.UseTvSoap();
app.UseTvAutoMw();

app.MapControllers();
app.Run(); 
#endregion