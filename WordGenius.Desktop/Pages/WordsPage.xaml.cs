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
using WordGenius.Desktop.Components.Sentences;
using WordGenius.Desktop.Components.Words;
using WordGenius.Desktop.Entities.Words;
using WordGenius.Desktop.Interfaces.Sentences;
using WordGenius.Desktop.Interfaces.Words;
using WordGenius.Desktop.Repository.Words;
using WordGenius.Desktop.Utils;
using WordGenius.Desktop.Windows.Words;
using WordGenius.Repositories.Sentences;

namespace WordGenius.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for WordsPage.xaml
    /// </summary>
    public partial class WordsPage : Page
    {
        private readonly WordRepository _wordRepository;

        private readonly SentenceRepository _sentenceRepository;

        public Word myWord { get; set; }

        public WordsPage()
        {
            InitializeComponent();
            this._wordRepository = new WordRepository();
            this._sentenceRepository = new SentenceRepository();
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
                PageSize = 50
            };
            var words = await _wordRepository.GetAllAsync(paginationParams);

            foreach (var word in words)
            {
                WordControl wordControl = new WordControl();
                wordControl.SetData(word);
                wordControl.Refresh = RefreshAsync;
                wordControl.AlterBorder = AlterBorder;
                wrpWords.Children.Add(wordControl);
            }
        }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshAsync();
        }


        private async void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string search = tbSearch.Text;

                wrpWords.Children.Clear();
                var words = await _wordRepository.SearchAsync(search);

                foreach (var word in words)
                {
                    WordControl wordControl = new WordControl();
                    wordControl.SetData(word);

                    wrpWords.Children.Add(wordControl);
                }
            }
        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                await RefreshAsync();
            }
        }

        private async void btnBackSentence_Click(object sender, RoutedEventArgs e)
        {
            SecondaryBorder.Visibility = Visibility.Collapsed;
            MainBorder.Visibility = Visibility.Visible;
            await RefreshAsync();
        }

        private async void btnCreateSentence_Click(object sender, RoutedEventArgs e)
        {
            CreateSentence createSentence = new CreateSentence();
            createSentence.word_id = myWord.Id;
            createSentence.ShowDialog();
            await RefreshSentenceAsync();
        }

        public async void AlterBorder(Word word)
        {
            myWord = word;
            MainBorder.Visibility = Visibility.Collapsed;
            SecondaryBorder.Visibility = Visibility.Visible;
            await RefreshSentenceAsync();

        }

        private async Task RefreshSentenceAsync()
        {
            stackSentences.Children.Clear();
            var sentences = await _sentenceRepository.GetAllWordsSentenceAsync(myWord.Id);
            foreach (var sentence in sentences)
            {
                SentencesControl sentencesControl = new SentencesControl();
                sentencesControl.SetData(sentence);
                sentencesControl.wordId = myWord.Id;
                sentencesControl.Refresh = RefreshSentenceAsync;
                stackSentences.Children.Add(sentencesControl);
            }
        }
    }
}
