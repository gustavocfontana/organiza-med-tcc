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
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Services
            builder.Services.AddDbContext<OrganizaMedDbContext>();
            builder.Services.AddScoped<IRepositorioMedicos, RepositorioMedicos>();
            builder.Services.AddScoped<MedicosServico>();
            builder.Services.AddScoped<IRepositorioAtividades, RepositorioAtividades>();
            builder.Services.AddScoped<AtividadesServico>();
            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
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
                });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Autenticacao/Login";
                options.AccessDeniedPath = "/Autenticacao/AcessoNegado";
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

            #region DadosIniciais
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrganizaMedDbContext>();
    context.Database.EnsureCreated();

    if (!context.Medicos.Any())
    {
        var medicos = new List<Medico>
        {
            new Medico("Dr. João Silva", "12345-SP", "Cardiologia"),
            new Medico("Dra. Maria Souza", "67890-SP", "Neurologia"),
            new Medico("Dr. Carlos Pereira", "11223-SP", "Ortopedia"),
            new Medico("Dra. Ana Lima", "44556-SP", "Dermatologia"),
            new Medico("Dr. Pedro Gomes", "77889-SP", "Pediatria"),
            new Medico("Dra. Julia Santos", "99001-SP", "Geriatria"),
            new Medico("Dr. Marcos Alves", "22334-SP", "Ginecologia"),
            new Medico("Dra. Paula Ferreira", "55667-SP", "Oftalmologia"),
            new Medico("Dr. Ricardo Almeida", "88990-SP", "Oncologia"),
            new Medico("Dra. Vanessa Costa", "33445-SP", "Psiquiatria")
        };

        context.Medicos.AddRange(medicos);
        await context.SaveChangesAsync(); 
    }

    if (!context.Atividades.Any())
    {
        var medicos = context.Medicos.ToList();
        var atividades = new List<Atividade>
        {
            new Atividade(id: 0, DateTime.Now.AddHours(-3), DateTime.Now.AddHours(-2), new List<Medico> { medicos[0] }, TipoAtividade.Consulta),
            new Atividade(id: 0, DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-1), new List<Medico> { medicos[1] }, TipoAtividade.Cirurgia),
            new Atividade(id: 0, DateTime.Now.AddHours(-5), DateTime.Now.AddHours(-4), new List<Medico> { medicos[2] }, TipoAtividade.Consulta),
            new Atividade(id: 0, DateTime.Now.AddHours(-4), DateTime.Now.AddHours(-3), new List<Medico> { medicos[3] }, TipoAtividade.Cirurgia),
            new Atividade(id: 0, DateTime.Now.AddHours(-6), DateTime.Now.AddHours(-5), new List<Medico> { medicos[4] }, TipoAtividade.Consulta),
            new Atividade(id: 0, DateTime.Now.AddHours(-1), DateTime.Now, new List<Medico> { medicos[5] }, TipoAtividade.Cirurgia),
            new Atividade(id: 0, DateTime.Now.AddHours(-8), DateTime.Now.AddHours(-7), new List<Medico> { medicos[6] }, TipoAtividade.Consulta),
            new Atividade(id: 0, DateTime.Now.AddHours(-7), DateTime.Now.AddHours(-6), new List<Medico> { medicos[7] }, TipoAtividade.Cirurgia),
            new Atividade(id: 0, DateTime.Now.AddHours(-9), DateTime.Now.AddHours(-8), new List<Medico> { medicos[8] }, TipoAtividade.Consulta),
            new Atividade(id: 0, DateTime.Now.AddHours(-10), DateTime.Now.AddHours(-9), new List<Medico> { medicos[9] }, TipoAtividade.Cirurgia)
        };

        context.Atividades.AddRange(atividades);
        await context.SaveChangesAsync();
    }
}
#endregion

            app.Run();
        }
    }
}