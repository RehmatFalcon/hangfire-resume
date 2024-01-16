using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

if (builder.Configuration["Driver"] == "PSQL")
{
    builder.Services.AddHangfire(config =>
    {
        config.UsePostgreSqlStorage(otp =>
        {
            var connectionString = builder.Configuration.GetConnectionString("PsqlHangfireConnection");
            otp.UseNpgsqlConnection(connectionString: connectionString);
        }, new PostgreSqlStorageOptions()
        {
            InvisibilityTimeout = TimeSpan.FromMinutes(2)
        });
    });
}
else
{
    builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions()
        {
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(2)
        }));
}

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHangfireDashboard();

app.Run();