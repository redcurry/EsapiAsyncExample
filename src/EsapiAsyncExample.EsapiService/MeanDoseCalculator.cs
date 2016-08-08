using System;
using System.Threading;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.Types;

namespace EsapiAsyncExample.EsapiService
{
    /// <summary>
    /// Represents an object that calculates the mean dose of the active plan.
    /// </summary>
    public class MeanDoseCalculator : IMeanDoseCalculator
    {
        private readonly EsapiWorker _esapiWorker;

        /// <summary>
        /// Initializes an instance of this class with the given EsapiWorker.
        /// </summary>
        /// <param name="esapiWorker">
        /// The EsapiWorker used to run operations asynchronously on the ESAPI thread.
        /// </param>
        public MeanDoseCalculator(EsapiWorker esapiWorker)
        {
            _esapiWorker = esapiWorker;
        }

        /// <summary>
        /// Implements <seealso cref="IMeanDoseCalculator.CalculateForActivePlanSetupAsync" />
        /// </summary>
        public Task<DoseValue> CalculateForActivePlanSetupAsync(
            IProgress<double> progress, CancellationToken cancellation)
        {
            return _esapiWorker.RunAsync(scriptContext =>
            {
                var dose = scriptContext.PlanSetup.Dose;
                var meanDose = new DoseValue(0.0, dose.DoseMax3D.Unit);

                for (int z = 0; z < dose.ZSize; z++)
                {
                    cancellation.ThrowIfCancellationRequested();
                    progress.Report((double)(z + 1) / dose.ZSize);

                    var buffer = new int[dose.XSize, dose.YSize];
                    dose.GetVoxels(z, buffer);

                    for (int x = 0; x < dose.XSize; x++)
                        for (int y = 0; y < dose.YSize; y++)
                            meanDose += dose.VoxelToDoseValue(buffer[x, y]);
                }

                return meanDose / (dose.XSize * dose.YSize * dose.ZSize);
            });
        }
    }
}