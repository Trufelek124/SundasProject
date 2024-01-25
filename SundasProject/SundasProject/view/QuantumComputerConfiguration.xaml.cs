using SundasProject.controller;
using SundasProject.model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SundasProject.view
{
    /// <summary>
    /// Logika interakcji dla klasy QuantumComputerConfiguration.xaml
    /// </summary>
    public partial class QuantumComputerConfiguration : Page
    {

        public QuantumComputerConfigurationController quantumComputerConfigurationController;
        public ObservableCollection<Property> Properties = new ObservableCollection<Property>();

        [System.Text.RegularExpressions.GeneratedRegex("[^0-9.Ee-]+")]
        private static partial System.Text.RegularExpressions.Regex OnlyDigits();


        public QuantumComputerConfiguration()
        {
            InitializeComponent();
            quantumComputerConfigurationController = new QuantumComputerConfigurationController();

            try
            {
                quantumComputerConfigurationController.InitializeDb();

                Properties = quantumComputerConfigurationController.LoadPropertiesFromXml();
                PropertiesDataGrid.ItemsSource = Properties;
                SimulateButton.IsEnabled = DeleteButton.IsEnabled = false;
                RefreshProperties();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PropertiesDataGrid.SelectedItem is Property property &&
                    double.TryParse(WavelengthTextBox.Text, out var wl) &&
                    double.TryParse(PathTextBox.Text, out var pth) &&
                    double.TryParse(PhaseTextBox.Text, out var phs) &&
                    double.TryParse(IntensityTextBox.Text, out var inte) &&
                    double.TryParse(DurationTextBox.Text, out var dur) &&
                    double.TryParse(CenterTextBox.Text, out var cntr) &&
                    double.TryParse(BandwidthTextBox.Text, out var bw))
                {

                    quantumComputerConfigurationController.CreateNewProperty(wl,
                        pth,
                        phs,
                        inte,
                        dur,
                        cntr,
                        bw
                        );


                    RefreshProperties();
                    PropertiesDataGrid.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter valid numbers.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PropertiesDataGrid.SelectedItem is Property property)
                {
                    quantumComputerConfigurationController.DeleteProperty(property);
                    RefreshProperties();
                    PropertiesDataGrid.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e) { }


        private void SimulateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (double.TryParse(WavelengthTextBox.Text, out var wavelength) &&
                    double.TryParse(PathTextBox.Text, out var path) &&
                    double.TryParse(PhaseTextBox.Text, out var phase) &&
                    double.TryParse(IntensityTextBox.Text, out var intensity) &&
                    double.TryParse(DurationTextBox.Text, out var duration) &&
                    double.TryParse(CenterTextBox.Text, out var center) &&
                    double.TryParse(BandwidthTextBox.Text, out var bandwidth))
                {
                    var bitmap = quantumComputerConfigurationController.RunSimulation(wavelength, path, phase, intensity, duration, center, bandwidth);

                    PlotImage.Source = bitmap;
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter valid numbers.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void PropertiesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SimulateButton.IsEnabled = DeleteButton.IsEnabled = PropertiesDataGrid.SelectedItem != null;

                if (PropertiesDataGrid.SelectedItem is not Property property) return;

                WavelengthTextBox.Text = property.Wavelength.ToString();
                PathTextBox.Text = property.Path.ToString();
                PhaseTextBox.Text = property.Phase.ToString();
                IntensityTextBox.Text = property.Intensity.ToString();
                DurationTextBox.Text = property.Duration.ToString();
                CenterTextBox.Text = property.Center.ToString();
                BandwidthTextBox.Text = property.Bandwidth.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) => e.Handled = !IsTextAllowed(e.Text);

        private void NumberValidationPasting(object sender, DataObjectPastingEventArgs e)
        {
            try
            {
                if (!e.DataObject.GetDataPresent(typeof(string)))
                {
                    e.CancelCommand();
                    return;
                }

                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static bool IsTextAllowed(string text) => !OnlyDigits().IsMatch(text);


        private void RefreshProperties() {
            quantumComputerConfigurationController.GetProperties();
        }

    }
}
