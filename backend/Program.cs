using TesApi.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Daftarkan CORS dulu, sebelum Build()
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
          .AllowAnyOrigin()   // boleh dari mana saja (dev only)
          .AllowAnyMethod()   // GET, POST, PUT, DELETE, OPTIONS, dll.
          .AllowAnyHeader();  // Content-Type, Authorization, dll.
    });
});

// 2. Daftarkan service lain
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// 3. Build
var app = builder.Build();

// 4. Pakai CORS sebelum MapControllers
app.UseCors();  // tanpa nama -> pakai DefaultPolicy

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
