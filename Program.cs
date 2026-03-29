using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using ModuloCadastro.Service;

var builder = WebApplication.CreateBuilder(args);


// DbContext
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ModuloCadastro.Context.ModuloCadastroContext>(options =>
{
    options.UseMySql(conn,
        ServerVersion.AutoDetect(conn),
        x => x.EnableRetryOnFailure());
});

#region Scoped Service
builder.Services.AddScoped<UsuarioService>();
#endregion


// Add services to the container.
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
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
//app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseRouting();
app.Run();
