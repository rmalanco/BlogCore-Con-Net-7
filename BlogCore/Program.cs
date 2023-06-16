using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlogCore.Data;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models;
using BlogCore.AccesoDatos.Data.Inicializador;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("BlogCore.AccesoDatos")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();

builder.Services.AddControllersWithViews();

// agregamos el contenedor de trabajo
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();
//Siembra de datos - Paso 1
builder.Services.AddScoped<IInicializadorDB, InicializadorDB>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//Método que ejecuta la siembra de datos
SiembraDeDatos();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Cliente}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// Siembra de datos - Paso 3 - Para que se ejecute la siembra de datos
//Funcionalidad método SiembraDeDatos();
void SiembraDeDatos()
{
    using var scope = app.Services.CreateScope();
    var inicializadorDb = scope.ServiceProvider.GetRequiredService<IInicializadorDB>();
    inicializadorDb.Inicializar();
}