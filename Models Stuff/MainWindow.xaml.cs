using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Models_Stuff {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public float Internet;
        public float Labour;
        public float StartVal;
        public float a;
        public float Offset;


        public MainWindow() {
            InitializeComponent();
            SeriesCollection = new SeriesCollection {
                new LineSeries
                {
                    Title = "% of remote workers",
                    Values = new ChartValues<double> { },
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))
                    
                },
            };
            YFormatter = value => value.ToString();
            DataContext = this;
        }

        private void TextChanged(object sender, TextChangedEventArgs e) {
        }


        private void UpdateGraph() {
            #region Setup
            // Null checks for instantiation
            if (SYBox == null || EYBox == null || TABox == null || PLBox == null || SeriesCollection == null) return;

            // data entry initiation checks.
            if (!int.TryParse(SYBox.Text.ToString(), out int StartYear) || !int.TryParse(EYBox.Text.ToString(), out int EndYear) ||
                !float.TryParse(TABox.Text.ToString(), out StartVal) ||
                !float.TryParse(PLBox.Text.ToString(), out Labour) || !float.TryParse(ABox.Text, out a) || !float.TryParse(OffsetBox.Text, out Offset) ||
                EndYear < StartYear) return;

            // Label setups
            string[] Months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            List<string> XAxisLabels = new List<string>();
            SeriesCollection[0].Values.Clear();

            float max = 100 - Labour;
            #endregion

            // Iterating through the data.
            for (int i = StartYear; i <= EndYear; i++) {
                XAxisLabels.Add(i.ToString());
                SeriesCollection[0].Values.Add(MathsStuff2(i - Offset, max));
            }
            Labels = XAxisLabels.ToArray();
            
        }


        private void Button_Click(object sender, RoutedEventArgs e) {
            UpdateGraph();
        }

        /// <summary>
        /// The used function
        /// </summary>
        /// <param name="y"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private double MathsStuff2(float y, double max) => max - ((max - StartVal) * Math.Pow(a, y));
    }
}
