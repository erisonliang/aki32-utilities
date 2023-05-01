using Microsoft.Identity.Client;

namespace Aki32Utilities.Azure.B2CAuth.FromConsoleViaGlyph;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            RunAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    private static async Task RunAsync()
    {
        SampleConfiguration config = SampleConfiguration.ReadFromJsonFile("appsettings.json");
        var appConfig = config.PublicClientApplicationOptions;
        var app = PublicClientApplicationBuilder.CreateWithApplicationOptions(appConfig)
                                                .Build();
        var httpClient = new HttpClient();

        MyInformation myInformation = new MyInformation(app, httpClient, config.MicrosoftGraphBaseEndpoint);
        await myInformation.DisplayMeAndMyManagerAsync();
    }
}
