using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SwaggerOptions = MicroRabbit.Transfer.Api.Options.SwaggerOptions;
using Infra.IoC;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using MicroRabbit.Transfer.Data.Context;

namespace MicroRabbit.Transfer.Api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<TransferDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TransferDbConnection"));
            }
            );

            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new OpenApiInfo {
                    Version = "v1",
                        Title = "Transfer Microservice",
                        Description = "A simple example ASP.NET Core Web API",
                        TermsOfService = new Uri ("https://www.facebook.com/"),
                        Contact = new OpenApiContact {
                            Name = "Shayne Boyer",
                                Url = new Uri ("https://twitter.com/spboyer"),
                                Email = string.Empty,

                        },
                        License = new OpenApiLicense {
                            Name = "Use under LICX",
                                Url = new Uri ("https://example.com/license"),
                        }
                });

                c.AddSecurityDefinition ("Bearer", new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the bearer scheme",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement (new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string> ()
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine (AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments (xmlPath);

            });

            services.AddMediatR (typeof (Startup));
            services.AddControllers ();
            RegisterServices(services);

        }
        private void RegisterServices (IServiceCollection services) {
            DependencyContainer.RegisterServices (services);

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseAuthorization ();
            var swaggerOptions = new SwaggerOptions ();
            Configuration.GetSection (nameof (SwaggerOptions)).Bind (swaggerOptions);

            app.UseSwagger (option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

            app.UseSwaggerUI (option => {
                option.SwaggerEndpoint (swaggerOptions.UiEndpoint, swaggerOptions.Description);
                option.InjectStylesheet ("/swagger/custom.css");
            });
            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}