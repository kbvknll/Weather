using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Weather.Data;

namespace Weather
{
    public partial class MainWindow : Window
    {
        private List<DateTemperature> _datetemperatures;

        public MainWindow()
        {
            InitializeComponent();

            _datetemperatures = Data.DataContext.DateTemperarures.ToList();
            ListWeather.ItemsSource = _datetemperatures;
        }

        private void ViewMI_Click(object sender, RoutedEventArgs e)
        {
            DateTemperature selectedDate = (DateTemperature)ListWeather.SelectedItem;
            MessageBox.Show($"Day: {selectedDate.Day} \nInfo: {selectedDate.Info}");
        }

        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var filter = _datetemperatures;

            var selectedFilter = (ComboBoxItem)FilterCB.SelectedItem;

            if (selectedFilter == null)
                return;

            switch (selectedFilter.Content.ToString())
            {
                case "Positive":
                    filter = filter.Where(x => x.Tempa > 0).ToList();
                    break;
                case "Negative":
                    filter = filter.Where(x => x.Tempa < 0).ToList();
                    break;
                case "All":
                    filter = _datetemperatures;
                    break;
            }

            ListWeather.ItemsSource = filter;
            ListWeather.Items.Refresh();
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSortOption = (ComboBoxItem)SortCB.SelectedItem;

            switch (selectedSortOption.Content.ToString())
            {
                case "Sort by Date Asc":
                    _datetemperatures = _datetemperatures.OrderBy(x => x.Day).ToList();
                    break;
                case "Sort by Date Desc":
                    _datetemperatures = _datetemperatures.OrderByDescending(x => x.Day).ToList();
                    break;
                case "Sort by Temperature Asc":
                    _datetemperatures = _datetemperatures.OrderBy(x => x.Tempa).ToList();
                    break;
                case "Sort by Temperature Desc":
                    _datetemperatures = _datetemperatures.OrderByDescending(x => x.Tempa).ToList();
                    break;
            }

            ListWeather.ItemsSource = _datetemperatures;
            ListWeather.Items.Refresh();
        }


        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            ListWeather.ItemsSource = _datetemperatures;
            ListWeather.Items.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dpDay.SelectedDate.HasValue && double.TryParse(txtTemperature.Text, out double temperature))
            {
                string dayString = dpDay.SelectedDate.Value.ToString("d"); 
                DateTemperature newData = new DateTemperature(dayString, "Info", temperature);
                _datetemperatures.Add(newData);
                ListWeather.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please enter a valid date and temperature.");
            }
        }

        private void Button_Details_Click(object sender, RoutedEventArgs e)
        {
            double averageTemperature = _datetemperatures.Average(dt => dt.Tempa);
            double maxTemperature = _datetemperatures.Max(dt => dt.Tempa);
            double minTemperature = _datetemperatures.Min(dt => dt.Tempa);

            var temperatureGroups = _datetemperatures
                .GroupBy(dt => dt.Tempa)
                .OrderByDescending(g => g.Count())
                .ToList();

            var anomalies = _datetemperatures
                .Zip(_datetemperatures.Skip(1), (prev, next) => (prev, next))
                .Where(pair => Math.Abs(pair.next.Tempa - pair.prev.Tempa) >= 20)
                .Select(pair => (pair.prev, pair.next));

            string anomalyDrop = anomalies.Where(pair => pair.next.Tempa < pair.prev.Tempa).Select(pair => $"{pair.prev.Day} -> {pair.next.Day}").FirstOrDefault();
            string anomalyRise = anomalies.Where(pair => pair.next.Tempa > pair.prev.Tempa).Select(pair => $"{pair.prev.Day} -> {pair.next.Day}").FirstOrDefault();

            var dialog = new System.Windows.Window
            {
                Title = "Статистика температуры",
                Content = new StackPanel
                {
                    Children =
                    {
                        new Label { Content = "Средняя температура: " + averageTemperature.ToString("F1") + "°C" },
                        new Label { Content = "Максимальная температура: " + maxTemperature.ToString("F1") + "°C" },
                        new Label { Content = "Минимальная температура: " + minTemperature.ToString("F1") + "°C" },
                        new Label { Content = "Аномальный спад температуры: " + (anomalyDrop != null ? anomalyDrop : "None") },
                        new Label { Content = "Аномальный подъем температуры: " + (anomalyRise != null ? anomalyRise : "None") },
                        new Label { Content = "Количество дней с одинаковой температурой:" },
                        new ListView { ItemsSource = temperatureGroups.Select(g => new { Temperature = g.Key, Days = string.Join(", ", g.Select(dt => dt.Day)) }), DisplayMemberPath = "Days" }
                    }
                },
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            dialog.Owner = this;
            dialog.ShowDialog();
        }
    }
}