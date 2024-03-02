using System.Text.Json;

var AllowSpecificOrigins = "_AllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
    policy  =>
    {
        policy.WithOrigins("http://localhost:3000", "http://testtaskfe:3000")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowSpecificOrigins);


app.MapGet("/projects/all", () =>
{
    try
    {
        var json = File.ReadAllText("./example-jsons/projects-list.json");
        var jsonObject = JsonSerializer.Deserialize<object>(json);

        return jsonObject;
    }
    catch
    {
        return Results.NotFound();
    }
});


app.MapGet("/project/{name}", (string name) => 
{
    try
    {
        var json = File.ReadAllText("./example-jsons/" + name + ".json");
        var jsonObject = JsonSerializer.Deserialize<object>(json);

        return jsonObject;
    }
    catch 
    {
        return Results.NotFound();
    }
});

app.Run();

