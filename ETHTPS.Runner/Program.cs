using Konsole;

using Newtonsoft.Json;

namespace ETHTPS.Runner
{
    internal class Program
    {
        private static Style _DEFAULT_STYLE = Style.WhiteOnRed;
        private const string _STARTUP_CONFIG = "Startup.json";

        static async Task Main(string[] args)
        {
            if (!File.Exists(_STARTUP_CONFIG))
            {
                await Console.Out.WriteLineAsync($"{_STARTUP_CONFIG} not found");
                return;
            }
            StartupConfig? config = JsonConvert.DeserializeObject<StartupConfig>(File.ReadAllText(_STARTUP_CONFIG));
            if (config == null || config?.Executables?.Length == 0)
            {
                await Console.Out.WriteLineAsync("Nothing to do");
                return;
            }
            if (config?.Executables == null || config.Executables.Length == 0)
            {
                Console.WriteLine("No executables specified");
                return;
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Service[] services = config.Executables.Select(x => new Service(x.Name ?? throw new ArgumentNullException("Name"), Path.Combine(config.BaseDirectory ?? string.Empty, x.Directory ?? string.Empty), x.Executable ?? "", x.Arguments ?? Array.Empty<string>()))
                                                   .ToArray();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            var size = Window.GetHostWidthHeight.Invoke();
            Console.Clear();
            WindowSettings settings = new()
            {
                Width = size.width,
                Height = size.height,
                SX = 0,
                SY = 0
            };

            var w = Window.OpenBox("Services", settings);
            var consoles = w.SplitColumns(services.Select(x => new Split(size.width / services.Length - 1, x.Name)).ToArray()).ToList();
            var windowedServices = services.Select((s, i) => new WindowedService(s, consoles[i])).ToList();
            var exit = false;
            Console.CancelKeyPress += async (sender, e) =>
            {
                e.Cancel = true;
                await Task.WhenAll(windowedServices.Select(s => Task.Run(() => s.Kill())));
                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                Environment.Exit(0);
            };
            await Task.WhenAll(windowedServices.Select(s =>
            {
                try
                {
                    return Task.Run(() => s.Start());
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{s.Name} failed to start: {e}");
                    return Task.FromException(e);
                }
            }));
            while (windowedServices.Any(x => x.Running) && !exit)
            {
                await Task.Delay(-1);
            }
        }
    }
}