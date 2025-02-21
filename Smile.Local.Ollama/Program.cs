using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Smile.Local.Ollama.Business.Services;
using Smile.Local.Ollama.Business.Services.Interfaces;
using Smile.Local.Ollama.Data;

namespace Smile.Local.Ollama
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IDBContext, DBContext>();
            builder.Services.AddTransient<IOllamaService, OllamaService>();
            builder.Services.AddTransient<IPdfDocumentService, PdfDocumentService>();
            builder.Services.AddTransient<IWordDocumentService, WordDocumentService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
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
