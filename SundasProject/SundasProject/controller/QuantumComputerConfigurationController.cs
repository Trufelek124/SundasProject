using SundasProject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace SundasProject.controller
{

    public class QuantumComputerConfigurationController
    {

        private const string DATABASE_FILE = "Database.xml";
        private const string PULSE_IMG_FILE = "pulse.png";

        private string FormatDouble(double value) => value == Math.Floor(value)
            ? value.ToString("F1")
            : value.ToString("G");

        List<Property> Properties = new List<Property>();

        public ObservableCollection<Property> GetProperties()
        {

            return new ObservableCollection<Property>(Properties);
        }

        public void InitializeDb()
        {
            if (!System.IO.File.Exists(DATABASE_FILE))
            {
                CreateEmptyDatabaseFile();
            }
        }

        public ObservableCollection<Property> LoadPropertiesFromXml()
        {
            
            try
            {
                if (System.IO.File.Exists(DATABASE_FILE))
                {
                    var xdoc = XDocument.Load(DATABASE_FILE)!;
                    foreach (var element in xdoc.Root!.Elements("Property"))
                    {
                        Properties.Add(new Property
                        {
                            Wavelength = double.TryParse(element.Element("Wavelength")?.Value, out var wavelength) ? wavelength : 0,
                            Path = double.TryParse(element.Element("Path")?.Value, out var path) ? path : 0,
                            Phase = double.TryParse(element.Element("Phase")?.Value, out var phase) ? phase : 0,
                            Intensity = double.TryParse(element.Element("Intensity")?.Value, out var intensity) ? intensity : 0,
                            Duration = double.TryParse(element.Element("Duration")?.Value, out var duration) ? duration : 0,
                            Center = double.TryParse(element.Element("Center")?.Value, out var center) ? center : 0,
                            Bandwidth = double.TryParse(element.Element("Bandwidth")?.Value, out var bandwidth) ? bandwidth : 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return GetProperties();
        }
        public BitmapImage RunSimulation(double wavelength, double path, double phase, double intensity, double duration, double center, double bandwidth)
        {
            try
            {
                // Remove previously generated plot image (if any)
                if (System.IO.File.Exists(PULSE_IMG_FILE))
                {
                    System.IO.File.Delete(PULSE_IMG_FILE);
                    return null;
                }

                var juliaScript = $@"
                using QuantumOptics
                using Plots

                function simulate_pulse(wavelength::Float64, path::Float64, phase::Float64, intensity::Float64,
                    duration::Float64, center::Float64, bandwidth::Float64)

                    T = range(0, stop=duration, length=1000)
                    E = zeros(ComplexF64, length(T))
                    E .= intensity .* exp.(-((T .- center).^2) ./ (2 * bandwidth^2))
                    E .*= exp.(1im .* (2pi/wavelength .* (T.*path) .+ phase))
                    p = plot(xlabel=\""Time (s)\"", ylabel=\""Intensity / Phase\"", title=\""Laser Pulse Simulation\"")
                    plot!(T, abs.(E), label=\""Intensity\"", linewidth=2)
                    plot!(T, angle.(E), label=\""Phase\"", linewidth=2, linestyle=:dash)
                    savefig(p, \""{PULSE_IMG_FILE}\"")
                end

                simulate_pulse({FormatDouble(wavelength)}, {FormatDouble(path)}, {FormatDouble(phase)}, {FormatDouble(intensity)}, {FormatDouble(duration)}, {FormatDouble(center)}, {FormatDouble(bandwidth)})
                    ";

                // Run Julia script
                var juliaProcess = new Process();
                juliaProcess.StartInfo.FileName = "julia";
                juliaProcess.StartInfo.Arguments = $"-e \"{juliaScript}\"";
                juliaProcess.StartInfo.UseShellExecute = false;
                juliaProcess.StartInfo.CreateNoWindow = true;
                juliaProcess.StartInfo.RedirectStandardError = true;
                juliaProcess.Start();

                var error = juliaProcess.StandardError.ReadToEnd();

                juliaProcess.WaitForExit();

                // If there is an error, show a message box with the error message
                /*
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show($"An error occurred while running the simulation (Please Try again): \n{error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                */

                // Load the image into memory and then set the source of the PlotImage
                var bitmap = new BitmapImage();
                using (var stream = new MemoryStream(File.ReadAllBytes(PULSE_IMG_FILE)))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                }
                bitmap.Freeze(); // This is necessary to avoid cross thread operations

                return bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        public void CreateNewProperty(double Wavelength,
            double Path,
            double Phase,
            double Intensity,
            double Duration,
            double Center,
            double Bandwidth) {


            var property = new Property
            {
                Wavelength = Wavelength,
                Path = Path,
                Phase = Phase,
                Intensity = Intensity,
                Duration = Duration,
                Center = Center,
                Bandwidth = Bandwidth
            };
            Properties.Add(property);
            SavePropertiesToXml();
        }

        public void DeleteProperty(Property property) { 
            Properties.Remove(property);
            SavePropertiesToXml();
        }


        private void CreateEmptyDatabaseFile()
        {
            try
            {
                var xdoc = new XDocument();
                var root = new XElement("Properties");
                xdoc.Add(root);
                xdoc.Save(DATABASE_FILE);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SavePropertiesToXml()
        {
            try
            {
                var xdoc = new XDocument();
                var root = new XElement("Properties");
                xdoc.Add(root);

                foreach (var property in Properties)
                {
                    var element = new XElement("Property");
                    element.Add(new XElement("Wavelength", property.Wavelength));
                    element.Add(new XElement("Path", property.Path));
                    element.Add(new XElement("Phase", property.Phase));
                    element.Add(new XElement("Intensity", property.Intensity));
                    element.Add(new XElement("Duration", property.Duration));
                    element.Add(new XElement("Center", property.Center));
                    element.Add(new XElement("Bandwidth", property.Bandwidth));
                    root.Add(element);
                }

                xdoc.Save(DATABASE_FILE);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
