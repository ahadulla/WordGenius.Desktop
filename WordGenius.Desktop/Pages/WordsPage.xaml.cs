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
using WordGenius.Desktop.Components.Words;
using WordGenius.Desktop.Interfaces.Words;
using WordGenius.Desktop.Repository.Words;
using WordGenius.Desktop.Utils;
using WordGenius.Desktop.Windows.Words;

namespace WordGenius.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for WordsPage.xaml
    /// </summary>
    public partial class WordsPage : Page
    {
        private readonly WordRepository _wordRepository;

        public WordsPage()
        {
            InitializeComponent();
            this._wordRepository = new WordRepository();
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            WordCreate wordCreate = new WordCreate();
            wordCreate.ShowDialog();
            await RefreshAsync();
        }

        private async Task RefreshAsync()
        {
            wrpWords.Children.Clear();
            var paginationParams = new PagenationParams()
            {
                PageNumber = 1,
                PageSize = 30
            };
            var words = await _wordRepository.GetAllAsync(paginationParams);

            foreach (var word in words)
            {
                WordControl courseViewUserControl = new WordControl();
                courseViewUserControl.SetData(word);
                wrpWords.Children.Add(courseViewUserControl);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshAsync();
        }
    }
}
