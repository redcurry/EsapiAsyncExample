using System;
using System.Threading;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.Types;

namespace EsapiAsyncExample.EsapiService
{
    /// <summary>
    /// Provides an interface for calculating the mean dose of the active plan.
    /// </summary>
    public interface IMeanDoseCalculator
    {
        /// <summary>
        /// Calculates the mean dose of the active plan asynchronously.
        /// </summary>
        /// <param name="progress">The object used to report progress updates.</param>
        /// <param name="cancellation">The object used to provide cancellation.</param>
        /// <returns>
        /// The started Task associated with this operation,
        /// which can be used to obtain the result as a DoseValue.
        /// </returns>
        Task<DoseValue> CalculateForActivePlanSetupAsync(
            IProgress<double> progress, CancellationToken cancellation);
    }
}
