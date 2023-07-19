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
using System.Windows.Shapes;
using WordGenius.Desktop.Entities.Sentences;
using WordGenius.Desktop.Entities.Words;
using WordGenius.Desktop.Interfaces.Sentences;
using WordGenius.Desktop.Interfaces.Words;
using WordGenius.Helpers;
using WordGenius.Repositories.Sentences;

namespace WordGenius.Desktop.Windows.Words
{
    /// <summary>
    /// Interaction logic for CreateSentence.xaml
    /// </summary>
    public partial class CreateSentence : Window
    {
        public readonly ISentenceRepository _sentenceRepository;

        public long word_id { get; set; }

        public CreateSentence()
        {
            InitializeComponent();
            _sentenceRepository = new SentenceRepository();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InformationBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("");
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            var sentence = GetDateUI();
            if (sentence != null)
            {
                var result = await _sentenceRepository.CreateAsync(sentence);
                if (result > 0)
                {
                    this.Close();
                }
            }
        }

        private Sentence GetDateUI()
        {
            if (sentenceTb.Text != string.Empty)
            {
                Sentence sentence = new Sentence();
                sentence.WordsId = word_id;
                sentence.SentenceText = sentenceTb.Text;

                return sentence;
            }
            return null;
        }
    }
}
