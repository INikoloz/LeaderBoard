using LeaderBoard.Abstraction.Repositories;
using LeaderBoard.Application.Abstract;
using LeaderBoard.Application.Concrete;
using LeaderBoard.DAL;
using LeaderBoard.DAL.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace LeaderBoard.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserScoreRepository, ScoreRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IScoreService, UserScoreService>();
            services.AddScoped<DBContext>();

            string connectionString = Configuration.GetConnectionString("DefaultConnection:ConnectionString");
            services.AddTransient<IDbConnection>(sp => new SqlConnection(connectionString));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
