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
using WordGenius.Desktop.Pages;
using WordGenius.Desktop.Repositories.Results;
using WordGenius.Desktop.Repository.Words;

namespace WordGenius.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ResultRepository _resultRepository;

        private readonly WordRepository _wordRepository;

        public MainWindow()
        {
            InitializeComponent();
            HomePage homePage = new HomePage();
            PageNavigator.Content = homePage;
            this._resultRepository = new ResultRepository();
            this._wordRepository = new WordRepository();
        }

        private void brDragble_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimizedBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaxsimizedBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaxsimizedBtn.Style = (Style)FindResource("MaximazedButton");
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaxsimizedBtn.Style = (Style)FindResource("MaximazedButton2");

            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void rbHome_Click(object sender, RoutedEventArgs e)
        {
            HomePage homePage = new HomePage();
            PageNavigator.Content = homePage;
        }

        private void rbtranslate_Click(object sender, RoutedEventArgs e)
        {
            TranslatePage translatePage = new TranslatePage();
            PageNavigator.Content = translatePage;
        }

        private void rbAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutPage aboutPage = new AboutPage();
            PageNavigator.Content = aboutPage;
        }

        private void rbWords_Click(object sender, RoutedEventArgs e)
        {
            WordsPage wordsPage = new WordsPage();
            PageNavigator.Content = wordsPage;
        }

        private void rbTest_Click(object sender, RoutedEventArgs e)
        {
            TestPage testPage = new TestPage();
            PageNavigator.Content = testPage;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var resultList = await _resultRepository.GetAllFinishedStep1Async();

                if (resultList.Count > 0)
                {
                    var result = await _wordRepository.UpdateIsRememberAllAsync(resultList);
                }


                resultList = await _resultRepository.GetAllFinishedStep2Async();

                if (resultList.Count > 0)
                {
                    var result = await _wordRepository.UpdateIsRememberAllAsync(resultList);
                }

                resultList = await _resultRepository.GetAllFinishedStep3Async();

                if (resultList.Count > 0)
                {
                    var result = await _wordRepository.UpdateIsRememberAllAsync(resultList);
                }

            }
            catch
            {
            }
            

        }
    }
}
