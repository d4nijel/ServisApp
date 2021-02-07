using System;
using System.IO;
using jsreport.AspNetCore;
using jsreport.Binary;
using jsreport.Local;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using ServisApp.Data;
using ServisApp.ScheduledJobs;
using ServisApp.SendEmails;

namespace ServisApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<MojContext>(options => options.UseSqlServer(Configuration.GetConnectionString("localDS")));

            services.AddMvc();

            services.AddJsReport(new LocalReporting()
                .UseBinary(JsReportBinary.GetBinary())
                .AsUtility()
                .Create());

            var emailNotificationMetadata = Configuration.GetSection("EmailNotificationMetadata").Get<EmailNotificationMetadata>();

            services.AddSingleton(emailNotificationMetadata);
            services.AddSingleton<IJobFactory, CustomQuartzJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<NotificationJob>();

            //"0 0 12 * * ?" Every day at noon - 12pm
            //"0 * * ? * *"  Every minute
            //0 / 10 * *** ? Every 10 sec
            services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(NotificationJob), "Email Notification Job", "0 0 12 * * ?"));
            services.AddHostedService<CustomQuartzHostedService>();

            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();

            app.UseSession();

            app.UseStaticFiles();

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "PDFFiles")),
                RequestPath = "/PDFFiles"
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "MyArea",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Autentifikacija}/{action=Index}/{id?}");
            });
        }
    }
}
