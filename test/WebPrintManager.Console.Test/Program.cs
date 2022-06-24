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
                var escPos = new WebPrintManager.Epson.EscPosPrinter(printManager, printer, System.Text.Encoding.Default);
                await escPos.TestPrinter(default);
                escPos.FullPaperCut();
                await escPos.PrintDocument(default);
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
