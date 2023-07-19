﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WordGenius.Desktop.Entities.Test;
using WordGenius.Desktop.Entities.Words;

namespace WordGenius.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for StartTestPage.xaml
    /// </summary>
    public partial class StartTestPage : Page
    {
        public Test myTest { get; set; }

        public Action nextBtnTrue { get; set; }

        public Action<int> setCorrect { get; set; }

        public Action<Test> AddIncorrect { get; set; }

        public Action<Test> AddCorrect { get; set; }

        public StartTestPage()
        {
            InitializeComponent();
        }

        public void SetDatauzb(Test test)
        {
            myTest = test;

            TbCount.Text = test.count.ToString() + "/" + test.maxcount.ToString();
            TbQuestion.Text = test.word.Text;
            List<string> list = new List<string>();
            list.Add(test.word.Translate);
            list.Add(test.errorAns1);
            list.Add(test.errorAns2);
            list.Add(test.errorAns3);
            //List<string> list = new List<string>() {"a","b","c","d"};
            list = Shuffle(list);
            A.Content = list[0].ToString();
            B.Content = list[1].ToString();
            C.Content = list[2].ToString();
            D.Content = list[3].ToString();
        }

        public void SetDataeng(Test test)
        {
            myTest = test;

            TbCount.Text = test.count.ToString() + "/" + test.maxcount.ToString();
            TbQuestion.Text = test.word.Translate;
            List<string> list = new List<string>();
            list.Add(test.word.Text);
            list.Add(test.errorAns1);
            list.Add(test.errorAns2);
            list.Add(test.errorAns3);

            list = Shuffle(list);
            A.Content = list[0].ToString();
            B.Content = list[1].ToString();
            C.Content = list[2].ToString();
            D.Content = list[3].ToString();
        }

        public async void SetDataSount(Test test)
        {
            myTest = test;
            TbQuestion.Visibility = Visibility.Collapsed;
            playSoundBtn.Visibility = Visibility.Visible;
            TbCount.Text = test.count.ToString() + "/" + test.maxcount.ToString();
            TbQuestion.Text = test.word.Translate;
            List<string> list = new List<string>();
            list.Add(test.word.Text);
            list.Add(test.errorAns1);
            list.Add(test.errorAns2);
            list.Add(test.errorAns3);

            list = Shuffle(list);
            A.Content = list[0].ToString();
            B.Content = list[1].ToString();
            C.Content = list[2].ToString();
            D.Content = list[3].ToString();

            await Task.Run(() => SpeechAsync(myTest.word.Translate.ToString()));

        }


        public List<string> Shuffle(List<string> list)
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

        private async void playSoundBtn_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => SpeechAsync(myTest.word.Translate.ToString()));
        }

        public void SpeechAsync(string text)
        {
            string metin = $"{text}";

            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new System.Globalization.CultureInfo("uz-Latn-UZ"));

            synthesizer.Speak(metin);

        }

        private void A_Click(object sender, RoutedEventArgs e)
        {
            nextBtnTrue();
            B.IsEnabled = false;
            C.IsEnabled = false;
            D.IsEnabled = false;

            

            if (A.Content == myTest.word.Text || A.Content == myTest.word.Translate)
            {
                setCorrect(1);
                AddCorrect(myTest);
                A.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03774C"));
            }
            else
            {
                setCorrect(0);
                checkker();
                AddIncorrect(myTest);
                A.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C14843"));
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            nextBtnTrue();
            A.IsEnabled = false;
            C.IsEnabled = false;
            D.IsEnabled = false;

           

            if (B.Content == myTest.word.Text || B.Content == myTest.word.Translate)
            {
                setCorrect(1);
                AddCorrect(myTest);
                B.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03774C"));
            }
            else
            {
                setCorrect(0);
                checkker();
                AddIncorrect(myTest);
                B.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C14843"));
            }

        }

        private void C_Click(object sender, RoutedEventArgs e)
        {
            nextBtnTrue();
            A.IsEnabled = false;
            B.IsEnabled = false;
            D.IsEnabled = false;

            

            if (C.Content == myTest.word.Text || C.Content == myTest.word.Translate)
            {
                setCorrect(1);
                AddCorrect(myTest);
                C.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03774C"));
            }
            else
            {
                setCorrect(0);
                checkker();
                AddIncorrect(myTest);
                C.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C14843"));
            }

        }

        private void D_Click(object sender, RoutedEventArgs e)
        {
            nextBtnTrue();
            A.IsEnabled = false;
            B.IsEnabled = false;
            C.IsEnabled = false;

            

            if (D.Content == myTest.word.Text || D.Content == myTest.word.Translate)
            {
                setCorrect(1);
                AddCorrect(myTest);
                D.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03774C"));
                
            }
            else
            {
                setCorrect(0);
                checkker();
                AddIncorrect(myTest);
                D.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C14843"));
                
            }

        }


        public void checkker()
        {
            if (A.Content == myTest.word.Text || A.Content == myTest.word.Translate)
            {
                A.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03774C"));
            }
            else if (B.Content == myTest.word.Text || B.Content == myTest.word.Translate)
            {
                B.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03774C"));
            }
            else if (C.Content == myTest.word.Text || C.Content == myTest.word.Translate)
            {
                C.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03774C"));
            }
            else if (D.Content == myTest.word.Text || D.Content == myTest.word.Translate)
            {
                D.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03774C"));
            }
        }
    }
}
