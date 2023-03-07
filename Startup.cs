using System.Text;
using System.Text.Json.Serialization;
using BlogV6.Data;
using BlogV6.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace BlogV6
{

    public class Startup
    {
        // requires using Microsoft.Extensions.Configuration;
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var keyInBytes = Encoding.ASCII.GetBytes(Configuration["JwtKey"]); // Chave definida na aplicação
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyInBytes),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                }
            );

            // Services Controllers
            services.AddControllers().ConfigureApiBehaviorOptions(
                options => options.SuppressModelStateInvalidFilter = true
            )
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault; // Quando tiver objeto nulo não renderiza na tela
            });

            // Adicionar cache
            services.AddMemoryCache();

            // Adicionar compressão
            services.AddResponseCompression(options =>
                {
                    options.Providers.Add<GzipCompressionProvider>();
                    options.EnableForHttps = true;
                    //x.Providers.Add<BrotliCompressionProvider>() // Outra forma de compressão
                    //x.Providers.Add<CustomCompressionProvider>() // Outra forma de compressão
                }
            );

            // Configurando compressão
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Fastest;
            });

            // IDependency Injection
            services.AddDbContext<BlogDataContext>();
            services.AddTransient<TokenService>();
            services.AddTransient<EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCompression();
            app.UseStaticFiles(); // Upload de imagens, CSS, js...

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
