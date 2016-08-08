using EsapiAsyncExample;
using EsapiAsyncExample.EsapiService;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS
{
    public class Script
    {
        public void Execute(ScriptContext scriptContext)
        {
            // The ESAPI worker needs to be created in the main thread
            var esapiWorker = new EsapiWorker(scriptContext);

            // Create and show the main window on a separate thread
            ConcurrentStaThreadRunner.Run(() =>
            {
                var meanDoseCalculator = new MeanDoseCalculator(esapiWorker);
                var viewModel = new MainViewModel(meanDoseCalculator);
                var mainWindow = new MainWindow(viewModel);
                mainWindow.ShowDialog();
            });
        }
    }
}