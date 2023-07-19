using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WordGenius.Desktop.Entities.Results;
using WordGenius.Desktop.Entities.Test;
using WordGenius.Desktop.Entities.Words;
using WordGenius.Desktop.Helpers;
using WordGenius.Desktop.Interfaces.Results;
using WordGenius.Desktop.Repositories.Results;
using WordGenius.Desktop.Repository.Words;
using WordGenius.Helpers;

namespace WordGenius.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        private readonly WordRepository _wordRepository;

        private readonly IResultRepository _resultRepository;

        public Task<List<Test>> tests;

        List<Test> incorrectTest = new List<Test>();

        List<Test> correctTest = new List<Test>();

        public int i { get; set; }

        public int correct { get; set; } = 0;

        public int incorrect { get; set; } = 0;

        public bool numberIs { get; set; } = true;

        public TestPage()
        {
            InitializeComponent();
            this._wordRepository = new WordRepository();
            this._resultRepository = new ResultRepository();
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
                StartTestPage testPage = new StartTestPage();

                if (n.Content == "START")
                {
                    Starttt(int.Parse(secund.Text));
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
                }
                else
                {
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

                }
                testPage.AddCorrect = AddCorrectTest;
                testPage.AddIncorrect = AddIncorrectTest;
                testPage.nextBtnTrue = nextBtnTrue;
                testPage.setCorrect = setCorrect;
                PageNavigator.Content = testPage;
                nextBtn.Content = "NEXT QUESTION";
                nextBtn.IsEnabled = false;
            }

        }

        public void AddIncorrectTest(Test test)
        {
            incorrectTest.Add(test);
        }

        public async void AddCorrectTest(Test test)
        {
            correctTest.Add(test);
            try
            {
                var Dbresult = await _wordRepository.UpdateCorrectCountAsync(test.word.Id);

                var DbResult1 = await _wordRepository.UpdateIsRememberAsync(test.word.Id);

                if (DbResult1 > 0)
                {
                    Result result = new Result();
                    result.WordsId = test.word.Id;
                    result.Step1 = TimeHelper.GetDateTime();

                    var DbResult2 = await _resultRepository.CreateAsync(result);

                }
            }
            catch
            {
            }
        }

        public void nextBtnTrue()
        {
            nextBtn.IsEnabled = true;
        }

        public void setCorrect(int a)
        {
            if (a == 1)
            {
                correct++;
            }
            else
            {
                incorrect++;
            }
        }

        private async void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            i = correct = incorrect = 0;

            secund.Text = (int.Parse(numberTb.Text) * 6).ToString();

            if (rbsound.IsChecked == true || rbEngUz.IsChecked == true)
            {
                if (rbAllday.IsChecked == true)
                {
                    tests = CreateTest(int.Parse(numberTb.Text), 1, 1);
                }
                else if (rbToday.IsChecked == true)
                {
                    tests = CreateTest(int.Parse(numberTb.Text), 1, 0);
                }
            }
            else
            {
                if (rbAllday.IsChecked == true)
                {
                    tests = CreateTest(int.Parse(numberTb.Text), 0, 1);
                }
                else if (rbToday.IsChecked == true)
                {
                    tests = CreateTest(int.Parse(numberTb.Text), 0, 0);
                }
            }
            if ((await tests).Count > 0)
            {
                nextBtn.Content = "START";
                nextBtn.Visibility = Visibility.Visible;
                Secundomer.Visibility = Visibility.Visible;
                StecGenerate.Visibility = Visibility.Collapsed;
            }
        }

        //public async Task Metod1(int n)
        //{
        //    for (int i = n - 1; i >= 0; i--)
        //    {
        //        await Task.Delay(1000);
        //        secund.Text = i.ToString();
        //        if (i == 0 || incorrect + correct == int.Parse(numberTb.Text))
        //        {
        //            await Task.Delay(500);
        //            AlterPage();

        //        }
        //    }
        //}

        CancellationTokenSource cts;

        public async Task Metod1(int n)
        {
            for (int i = n - 1; i >= 0; i--)
            {
                try
                {
                    await Task.Delay(1000, cts.Token);
                    secund.Text = i.ToString();
                    if (i == 0 || incorrect + correct == int.Parse(numberTb.Text))
                    {
                        await Task.Delay(500);
                        AlterPage();
                        nextBtnTrue();
                        cts.Cancel();
                    }
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }


        public void AlterPage()
        {
            nextBtn.Visibility = Visibility.Collapsed;
            ResultPage resultPage = new ResultPage();
            resultPage.SetData(correct.ToString(), incorrect.ToString(), incorrectTest, correctTest);
            PageNavigator.Content = resultPage;
            Secundomer.Visibility = Visibility.Collapsed;
            StecGenerate.Visibility = Visibility.Visible;
        }

        private async void Starttt(int n)
        {
            cts = new CancellationTokenSource();
            Task task1 = Metod1(n);
            await task1;
        }


        public async Task<List<Test>> CreateTest(int n, int a, int day)
        {
            Randomm rn = new Randomm();

            List<Test> tests = new List<Test>();

            IList<Word> words;

            var count = await _wordRepository.CountAsync();
            count = rn.Next(0, count - 30);

            if (day == 1)
            {
                words = await _wordRepository.GetAllNMemoAsync(count);
            }
            else
            {
                words = await _wordRepository.GetAllTodayAsync();
            }
            int stop = words.Count;
            if (stop < 10)
            {
                MessageBox.Show("Not enough words entered");
                return new List<Test>();
            }

            if (stop < n)
            {
                n = stop;
            }

            words = Shuffle(words);

            for (int i = 0; i < n; i++)
            {
                var son = rn.GenerateRandomNumbers(0, stop, i);

                Test test = new Test();
                test.word = words[i];
                if (a == 1)
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


        public IList<Word> Shuffle(IList<Word> list)
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DefoultTestPage defoultTestPage = new DefoultTestPage();
            PageNavigator.Content = defoultTestPage;
        }
    }
}
