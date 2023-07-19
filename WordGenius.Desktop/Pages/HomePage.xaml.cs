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
using WordGenius.Desktop.Repository.Words;
using WordGenius.Desktop.Utils;
using WordGenius.Repositories.Sentences;

namespace WordGenius.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {

        private readonly WordRepository _wordRepository;
        
        private readonly SentenceRepository _sentenceRepository;


        public HomePage()
        {
            InitializeComponent();
            this._wordRepository = new WordRepository();

            this._sentenceRepository = new SentenceRepository();
        }

        public void SetDataChart(List<double> nums , List<string> strings )
        {
            SetChart.Values.Clear();
            foreach (var item in nums)
            {
                SetChart.Values.Add(item);
            }

            DateLabel.Labels = strings.ToArray();


        }


        public void SetData(int AllCount , int TodayCount , int SentenceCount)
        {
            AllNumberLb.Content = AllCount;

            todayNumberLb.Content = TodayCount;

            SentenceNumberLb.Content = SentenceCount;
        }

        public async void ReadDate()
        {
            var dates = await _wordRepository.GetAllCreatedDayAsync();

            var countAll = await _wordRepository.CountAsync();

            var countToday = await _wordRepository.CountTodayAsync();

            var counSentence = await _sentenceRepository.CountAsync();

            List<string> strings = new List<string>();

            List<double> values = new List<double>();

            foreach (var date in dates)
            {
                string day = date.date.Day.ToString(); 
                string month = date.date.Month.ToString();
                strings.Add(day+"/"+month);
                values.Add(double.Parse(date.Ids.ToString()));
            }
            strings.Reverse();
            values.Reverse();
            SetDataChart(values, strings);
            SetData(countAll, countToday, counSentence);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ReadDate();
        }


    }
}
