using Azure.Identity;

class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        /*
        if (builder.Environment.IsProduction())
        {
            builder.Configuration.AddAzureKeyVault(new Uri("https://keyvaultnswi152.vault.azure.net"), new Azure.Identity.DefaultAzureCredential());
        }
        */

        // Add Application Insights
        // specify the key in APPLICATIONINSIGHTS_CONNECTION_STRING environment variable
        builder.Services.AddApplicationInsightsTelemetry();

        // Add services to the container.
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}