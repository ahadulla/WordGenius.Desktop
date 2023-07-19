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
using WordGenius.Desktop.Interfaces.Sentences;
using WordGenius.Desktop.ViewModels;
using WordGenius.Repositories.Sentences;

namespace WordGenius.Desktop.Windows.Sentences
{
    /// <summary>
    /// Interaction logic for UpdateSentence.xaml
    /// </summary>
    public partial class UpdateSentence : Window
    {
        private readonly ISentenceRepository _sentenceRepository;

        public SentenceViewModel mySentence { get; set; }

        public long wordId { get; set; }


        public UpdateSentence()
        {
            InitializeComponent();
            _sentenceRepository = new SentenceRepository();
        }

        public void SetData(SentenceViewModel obj)
        {
            mySentence = obj;
            sentenceTb.Text = obj.sentence;
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
                var result = await _sentenceRepository.UpdateAsync(mySentence.Id, sentence);
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
                sentence.WordsId = wordId;
                sentence.SentenceText = sentenceTb.Text;

                return sentence;
            }
            return null;
        }
    }
}
