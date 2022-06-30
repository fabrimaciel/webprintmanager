using WebPrintManager;

Thread.Sleep(2000);

var printManager = new PrintManager();

printManager.ConnectionChanged += (sender, e) =>
{
    if (printManager.IsConnected)
    {
        Task.Factory.StartNew(async () =>
        {
            await Task.Delay(1000);
            var printers = await printManager.GetPrinters(default);

            var printer = printers.FirstOrDefault(f => f.StartsWith("EPSON", StringComparison.InvariantCultureIgnoreCase));

            if (printer != null)
            {
                var escPos = new WebPrintManager.Epson.EscPosPrinter(System.Text.Encoding.Default);
                using (var outputStream = new MemoryStream())
                {
                    await escPos.TestPrinter(outputStream, default);
                    escPos.FullPaperCut();
                    await escPos.PrintDocument(outputStream, default);

                    var installedPrinter = new InstalledPrinter(printer);

                    var info = await printManager.GetPrinter(installedPrinter, default);
                    await printManager.Purge(installedPrinter, default);

                    var job = await printManager.RawPrint(
                        installedPrinter,
                        new PrintDocumentInfo
                        {
                            Name = "Test",
                        },
                        outputStream.ToArray(),
                        default);

                    await job.Refresh(default);
                }
            }
        });
    }
};

Task.Factory.StartNew(
    async () =>
    {
        await printManager.Start("localhost", 24080);
    });

Console.WriteLine("Started");
Console.ReadKey();
