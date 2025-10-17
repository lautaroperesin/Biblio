using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authentication;
using Service.Interfaces;
using Service.Services;
using Web.Components;
using Web.Handlers;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddScoped<FirebaseAuthService>();
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<ILibroService, LibroService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPrestamoService, PrestamoService>();
builder.Services.AddTransient<AuthenticationHandler>();
builder.Services.AddMemoryCache();
builder.Services.AddSweetAlert2();

// Configura el HttpClient para tu API de backend
builder.Services.AddHttpClient("BackendApi", client =>
{
    client.BaseAddress = new Uri(Service.Properties.Resources.urlApi);
})
.AddHttpMessageHandler<AuthenticationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
