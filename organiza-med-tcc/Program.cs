using System.Reflection;
using OrganizaMed.Aplicacao.Servicos;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Medicos;
using OrganizaMed.Infra.Atividades;
using OrganizaMed.Infra.Compartilhado;
using OrganizaMed.Infra.Medicos;

namespace organiza_med_tcc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<OrganizaMedDbContext>();

            builder.Services.AddScoped<IRepositorioMedicos, RepositorioMedicos>();
            builder.Services.AddScoped<MedicosServico>();

            builder.Services.AddScoped<IRepositorioAtividades, RepositorioAtividades>();
            builder.Services.AddScoped<AtividadesServico>();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
