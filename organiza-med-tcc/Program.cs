using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using OrganizaMed.Aplicacao.Servicos;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Autenticacao;
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

            builder.Services.AddScoped<AutenticacaoServico>();
            builder.Services.AddIdentity<Usuario, Perfil>()
                .AddEntityFrameworkStores<OrganizaMedDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                    {
                        options.Cookie.Name = "AspNetCore.Cookies";
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                        options.SlidingExpiration = true;
                    }
                );

            builder.Services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Autenticacao/Login";
                    options.AccessDeniedPath = "/Autenticacao/AcessoNegado";
                }
            );

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

//            using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var medicoServico = services.GetRequiredService<MedicosServico>();
//    var atividadeServico = services.GetRequiredService<AtividadesServico>();

//    // Adicionar médicos pré-determinados
//    var medicos = new List<Medico>
//    {
//        new Medico("Dr. João Silva", "12345-SP", "Cardiologia"),
//        new Medico("Dra. Maria Oliveira", "67890-RJ", "Pediatria"),
//        new Medico("Dr. Pedro Santos", "11223-MG", "Ortopedia"),
//        new Medico("Dra. Ana Costa", "44556-BA", "Dermatologia"),
//        new Medico("Dr. Carlos Souza", "77889-PR", "Neurologia"),
//        new Medico("Dra. Paula Lima", "99001-SC", "Ginecologia"),
//        new Medico("Dr. Ricardo Alves", "22334-ES", "Psiquiatria"),
//        new Medico("Dra. Fernanda Ribeiro", "55667-PE", "Oftalmologia")
//    };

//    foreach (var medico in medicos)
//    {
//        var resultado = medicoServico.Adicionar(medico);
//        if (resultado.IsFailed)
//        {
//            Console.WriteLine($"Erro ao adicionar médico {medico.Nome}: {string.Join(", ", resultado.Errors)}");
//        }
//    }

//    var atividades = new List<Atividade>
//    {
//        new Atividade(1, DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), new List<Medico> { medicos[0] }, TipoAtividade.Consulta),
//        new Atividade(2, DateTime.Now.AddHours(3), DateTime.Now.AddHours(5), new List<Medico> { medicos[1] }, TipoAtividade.Cirurgia),
//        new Atividade(3, DateTime.Now.AddHours(6), DateTime.Now.AddHours(7), new List<Medico> { medicos[2] }, TipoAtividade.Consulta),
//        new Atividade(4, DateTime.Now.AddHours(8), DateTime.Now.AddHours(9), new List<Medico> { medicos[3] }, TipoAtividade.Cirurgia),
//        new Atividade(5, DateTime.Now.AddHours(10), DateTime.Now.AddHours(12), new List<Medico> { medicos[4] }, TipoAtividade.Consulta),
//        new Atividade(6, DateTime.Now.AddHours(13), DateTime.Now.AddHours(14), new List<Medico> { medicos[5] }, TipoAtividade.Cirurgia),
//        new Atividade(7, DateTime.Now.AddHours(15), DateTime.Now.AddHours(16), new List<Medico> { medicos[6] }, TipoAtividade.Consulta),
//        new Atividade(8, DateTime.Now.AddHours(17), DateTime.Now.AddHours(19), new List<Medico> { medicos[7] }, TipoAtividade.Cirurgia)
//    };

//    foreach (var atividade in atividades)
//    {
//        var resultado = atividadeServico.Adicionar(atividade);
//        if (resultado.IsFailed)
//        {
//            Console.WriteLine($"Erro ao adicionar atividade: {string.Join(", ", resultado.Errors)}");
//        }
//    }
//}

app.Run();
        }
    }
}