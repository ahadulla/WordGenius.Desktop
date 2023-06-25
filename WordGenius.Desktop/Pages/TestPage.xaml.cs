using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WordGenius.Desktop.Components.Test;
using WordGenius.Desktop.Entities.Test;
using WordGenius.Desktop.Entities.Words;
using WordGenius.Desktop.Helpers;
using WordGenius.Desktop.Repository.Words;
using WordGenius.Desktop.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WordGenius.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        private readonly WordRepository _wordRepository;

        public Task<List<Test>> tests ;

        public int i { get; set; }

        public int correct { get; set; } = 0;

        public int incorrect { get; set; } = 0;

        public TestPage()
        {
            InitializeComponent();
            this._wordRepository = new WordRepository();
        }

        private void plusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(numberTb.Text) < 30)
            {
                int temp = int.Parse(numberTb.Text) + 5;
                numberTb.Text = temp.ToString();
            }
        }

        private void minusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(numberTb.Text) > 10)
            {
                int temp = int.Parse(numberTb.Text) - 5;
                numberTb.Text = temp.ToString();
            }
        }

        private async void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (i < int.Parse(numberTb.Text))
            {
                Button n = (Button)sender;
                if (n.Content == "START")
                {
                    Starttt(int.Parse(secund.Text));
                    StartTestPage testPage = new StartTestPage();
                    if((rbsound.IsChecked == true))
                    {
                        testPage.SetDataSount((await tests)[i++]);
                    }
                    else if(rbEngUz.IsChecked == true)
                    {
                        testPage.SetDataeng((await tests)[i++]);
                    }
                    else
                    {
                        testPage.SetDatauzb((await tests)[i++]);
                    }
                    testPage.nextBtnTrue = nextBtnTrue;
                    testPage.setCorrect = setCorrect;
                    testPage.AlterPage = AlterPage;
                    PageNavigator.Content = testPage;
                }
                else
                {
                    StartTestPage testPage = new StartTestPage();
                    if ((rbsound.IsChecked == true))
                    {
                        testPage.SetDataSount((await tests)[i++]);
                    }
                    else if (rbEngUz.IsChecked == true)
                    {
                        testPage.SetDataeng((await tests)[i++]);
                    }
                    else
                    {
                        testPage.SetDatauzb((await tests)[i++]);
                    }
                    testPage.nextBtnTrue = nextBtnTrue;
                    testPage.setCorrect = setCorrect;
                    testPage.AlterPage = AlterPage;
                    PageNavigator.Content = testPage;
                }
                nextBtn.Content = "NEXT QUESTION";
                nextBtn.IsEnabled = false;
            }
            
        }

        public void nextBtnTrue()
        {
            nextBtn.IsEnabled = true;
        }

        public void setCorrect(int a)
        {
            if(a==1)
            {
                correct++;
            }
            else
            {
                incorrect++;
            }
        }

        private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            i = correct = incorrect = 0;

            secund.Text = (int.Parse(numberTb.Text) * 6).ToString();
            nextBtn.Content = "START";
            nextBtn.Visibility = Visibility.Visible;
            Secundomer.Visibility = Visibility.Visible;
            StecGenerate.Visibility = Visibility.Collapsed;
            if(rbsound.IsChecked == true || rbEngUz.IsChecked == true)
            {
                tests = CreateTest(int.Parse(numberTb.Text),1);
            }
            else
            {
                tests = CreateTest(int.Parse(numberTb.Text), 0);
            }
        }

        public async Task Metod1(int n)
        {
            for (int i = n; i >=0; i--)
            {
                await Task.Delay(1000);
                secund.Text = i.ToString();
                if (i == 0)
                {
                    nextBtn.Visibility = Visibility.Collapsed;
                    ResultPage resultPage = new ResultPage();
                    resultPage.SetData(correct.ToString(), incorrect.ToString());
                    PageNavigator.Content = resultPage;
                    Secundomer.Visibility = Visibility.Collapsed;
                    StecGenerate.Visibility = Visibility.Visible;
                }

            }
        }

        public void AlterPage()
        {
                nextBtn.Visibility = Visibility.Collapsed;
                ResultPage resultPage = new ResultPage();
                resultPage.SetData(correct.ToString(), incorrect.ToString());
                PageNavigator.Content = resultPage;
                Secundomer.Visibility = Visibility.Collapsed;
                StecGenerate.Visibility = Visibility.Visible;
        }

        private async void Starttt(int n)
        {
            Task task1 = Metod1(n);
            await task1;
        }


        public async Task<List<Test>> CreateTest(int n, int a)
        {
            Randomm rn = new Randomm();

            List<Test> tests = new List<Test>();


            var count = await _wordRepository.CountAsync();
            count = rn.Next(0, count-30);

            var words = await _wordRepository.GetAllNMemoAsync(count);
            int stop = words.Count;
            

            for (int i = 0; i < n; i++)
            {
                var son = rn.GenerateRandomNumbers(0, stop, i);

                Test test = new Test();
                test.word = words[i];
                if(a == 1)
                {
                    test.errorAns1 = words[son[0]].Text;
                    test.errorAns2 = words[son[1]].Text;
                    test.errorAns3 = words[son[2]].Text;
                }
                else
                {
                    test.errorAns1 = words[son[0]].Translate;
                    test.errorAns2 = words[son[1]].Translate;
                    test.errorAns3 = words[son[2]].Translate;
                }

                test.count = i + 1;
                test.maxcount = n;

                tests.Add(test);
            }

            return tests;
        }

        public List<Word> Shuffle(List<Word> list)
        {
            var random = new Random();
            list = list.OrderBy(x => random.Next()).ToList();

            return list;
        }

        public List<Word> Shuffle2(List<Word> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

    }
}
