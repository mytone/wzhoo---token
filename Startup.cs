using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Cors;
using wzhoo.Models;
using wzhoo.Controllers;
using static wzhoo.LoginRequestDTO;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

namespace wzhoo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }
               


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //���jwt��֤��
            services.AddControllers();
            services.Configure<TokenManagement>(Configuration.GetSection("tokenConfig"));
            var token = Configuration.GetSection("tokenConfig").Get<TokenManagement>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
               
                //Token Validation Parameters
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    //��ȡ������Ҫʹ�õ�Microsoft.IdentityModel.Tokens.SecurityKey����ǩ����֤��
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    //��ȡ������һ��System.String������ʾ��ʹ�õ���Ч�����߼����ҵķ����ߡ�
                    ValidIssuer = token.Issuer,
                    //��ȡ������һ���ַ��������ַ�����ʾ�����ڼ�����Ч���ڷ������ƵĹ��ڡ�
                    ValidAudience = token.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
                
            });

            services.AddCors(options => {
                options.AddPolicy("any", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });

            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IUserService, UserService>();        


            
            // ip limit deal //////////////////////////////
            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            // �����ĵ�����������3.x���breaking change��Ҫ����
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTimedJob();

            // file upload update time : 2020-10-17
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 268435456;
            });
                       
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }   
            
            //ip limit deal ///////////////////////////////////
            app.UseIpRateLimiting();


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseTimedJob();

            app.UseCors("any");

            // file upload update time: 2020-10-7
            app.UseStaticFiles();
                       
        }     


        
    }

}
