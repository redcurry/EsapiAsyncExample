using System;
using System.Threading;
using System.Windows.Input;
using EsapiAsyncExample.EsapiService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using VMS.TPS.Common.Model.Types;

namespace EsapiAsyncExample
{
    /// <summary>
    /// Represents the view model for the main window.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IMeanDoseCalculator _meanDoseCalculator;

        private bool _isCalculationInProgress;

        private CancellationTokenSource _cancellation;

        /// <summary>
        /// Initializes an instance of this class with the given mean dose calculator object.
        /// </summary>
        /// <param name="meanDoseCalculator">The mean dose calculator to use.</param>
        public MainViewModel(IMeanDoseCalculator meanDoseCalculator)
        {
            _meanDoseCalculator = meanDoseCalculator;
            _isCalculationInProgress = false;

            CalculateOrCancelMeanDoseCommand = new RelayCommand(CalculateOrCancelMeanDose);
        }

        public ICommand CalculateOrCancelMeanDoseCommand { get; }

        private double _progress;
        public double Progress
        {
            get { return _progress; }
            set { Set(ref _progress, value); }
        }

        private DoseValue _meanDose;
        public DoseValue MeanDose
        {
            get { return _meanDose; }
            set { Set(ref _meanDose, value); }
        }

        private void CalculateOrCancelMeanDose()
        {
            if (_isCalculationInProgress)
            {
                CancelMeanDose();
            }
            else
            {
                CalculateMeanDose();
            }
        }

        private async void CalculateMeanDose()
        {
            _isCalculationInProgress = true;

            var progress = new Progress<double>(UpdateProgress);
            _cancellation = new CancellationTokenSource();

            try
            {
                MeanDose = await _meanDoseCalculator
                    .CalculateForActivePlanSetupAsync(progress, _cancellation.Token);
            }
            catch (OperationCanceledException)
            {
                MeanDose = DoseValue.UndefinedDose();
            }

            ResetProgress();

            _isCalculationInProgress = false;
        }

        private void CancelMeanDose()
        {
            _cancellation.Cancel();
        }

        private void UpdateProgress(double p)
        {
            Progress = p;
        }

        private void ResetProgress()
        {
            UpdateProgress(0.0);
        }
    }
}