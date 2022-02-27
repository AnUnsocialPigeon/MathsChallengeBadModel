using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            if (SYBox == null || EYBox == null || TABox == null || PLBox == null || SeriesCollection == null) return;

            if (!int.TryParse(SYBox.Text.ToString(), out int StartYear) || !int.TryParse(EYBox.Text.ToString(), out int EndYear) ||
                !float.TryParse(TABox.Text.ToString(), out StartVal) ||
                !float.TryParse(PLBox.Text.ToString(), out Labour) || !float.TryParse(ABox.Text, out a) || !float.TryParse(OffsetBox.Text, out Offset) ||
                EndYear < StartYear) return;

            string[] Months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            List<string> XAxisLabels = new List<string>();
            SeriesCollection[0].Values.Clear();

            float max = 100 - Labour;
            #endregion

            for (int i = StartYear; i <= EndYear; i++) {
                XAxisLabels.Add(i.ToString());
                //for (int j = 0; j < Months.Length; j++) {
                //    XAxisLabels.Add($"{i} {Months[j]}");
                //    //SeriesCollection[0].Values.Add((double)(((i - StartYear) * 12) + j));
                //}

                SeriesCollection[0].Values.Add(MathsStuff2(i - Offset, max));
            }
            Labels = XAxisLabels.ToArray();
            
        }
        private void Button_Click(object sender, RoutedEventArgs e) {
            UpdateGraph();
        }
        private double x(float year) => 0.25f * year;
        private double Clamp(double d, double Max) => d > Max ? Max : d;

        private double MathsStuff(float y, double max) {
            double ans = max - (max * (2 / (1 + Math.Pow(1.15f, 0.3f * y))));
            return ans;
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
