using AMEML;
using AMEML.Classes;
using AMEML.Helpers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;


internal class Program
{
    public static ProxyServer proxyServer = new();

    static void CleanUp(object sender, EventArgs e)
    {
        proxyServer.RestoreOriginalProxySettings();
        Console.CursorVisible = true;
        Console.ResetColor();
    }

    static async Task Main(string[] args)
    {
        Console.CursorVisible = false;
        AppDomain.CurrentDomain.ProcessExit += CleanUp;

        CNSL.Info("If a dialog pops up that asks you to install a certificate press \"yes\"");

        proxyServer.BeforeResponse += OnBeforeResponse;

        var explicitEndPoint = new ExplicitProxyEndPoint(System.Net.IPAddress.Any, 8000);
        proxyServer.AddEndPoint(explicitEndPoint);

        proxyServer.Start();
        proxyServer.SetAsSystemProxy(explicitEndPoint, ProxyProtocolType.AllHttp);

        Console.Clear();
        Console.WriteLine(new String('-', Console.WindowWidth));
        Console.WriteLine("\n   Welcome to AMEML (Amazon Music Export My Library) v1.0\n");
        Console.WriteLine(new String('-', Console.WindowWidth));
        Console.WriteLine("\n\n");

        CNSL.Info("Proxy server started");
        CNSL.Info("", "Please go into the Amazon Music and click on your profile then on \"Settings\", scroll down and click on \"Reload Library\"");

        while (true) { }
    }

    static async Task OnBeforeResponse(object sender, SessionEventArgs e)
    {
        var proc = Process.GetProcessById(e.HttpClient.ProcessId.Value);
        if (proc == null || proc.ProcessName.ToLower() != "amazon music") return;

        var url = e.HttpClient.Request.RequestUri.ToString();
        if (url.Trim() != "https://www.amazon.de/cirrus/v3/sync") return;

        byte[] bodyBytes = await e.GetResponseBody();
        string bodyString = Encoding.UTF8.GetString(bodyBytes);
        var body = JsonConvert.DeserializeObject<Snapshot>(bodyString);

        if (body == null || body.snapshotURL == null) return;
        CNSL.Info("Catched sync request", "");
        e.Ok(bodyBytes, e.HttpClient.Response.Headers);

        string format = await CNSL.ConsoleMenu("Choose format for file", "CSV", "JSON", "CSV (FreeYourMusic friendly)");

        CNSL.Info("", "Stopping Proxy...");
        proxyServer.Stop();
        CNSL.Info("Restoring settings...");
        proxyServer.RestoreOriginalProxySettings();

        CNSL.Info("Getting infos... ");
        string exe = Assembly.GetExecutingAssembly().Location;
        string? docPath = Path.GetDirectoryName(exe);
        docPath ??= AppDomain.CurrentDomain.BaseDirectory;
        docPath ??= Directory.GetCurrentDirectory();
        if (docPath == null) Environment.Exit(1);
        string downloadPath = Path.Combine(docPath, DateTime.Now.ToString("dd-MM-yyyyTHH-mm") + (format == "JSON" ? ".json" : ".csv"));

        DecompWebClient client = new();
        Uri downloadUri = new(body.snapshotURL);
        CNSL.Info("", "Downloading snapshot...");

        if (format != "CSV")
        {
            string content = client.DownloadString(downloadUri);
            List<Dictionary<string, object>> csv = Converter.ParseCSV(content);

            CNSL.Info("", $"Converting {csv.Count} songs to {format}...");
            CNSL.Info("Depending on the count of songs and your PC this will take some time");
            string data = content;

            if (format == "JSON") data = Converter.ToJSON(csv);
            else if (format == "CSV (FreeYourMusic friendly)")
            {
                for (int i = 0; i < csv.Count; i++)
                {
                    csv[i] = new()
                    {
                        { "name", csv[i].First(t => t.Key == "title").Value },
                        { "artist", csv[i].First(t => t.Key == "artistName").Value },
                        { "album", csv[i].First(t => t.Key == "albumName").Value },
                    };
                }
                data = Converter.ToCSV(csv);
            }
            File.WriteAllText(downloadPath, data);
        }
        else client.DownloadFile(downloadUri, downloadPath);

        CNSL.Info("", "Snapshot saved to " + downloadPath);

        CNSL.Info("Press any key to exit...");
        Console.ReadKey();
        Environment.Exit(0);
        Process.GetCurrentProcess().Kill();
    }
}
