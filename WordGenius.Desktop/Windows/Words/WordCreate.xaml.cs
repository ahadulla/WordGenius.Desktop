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
using WordGenius.Desktop.Entities.Words;
using WordGenius.Desktop.Interfaces.Words;
using WordGenius.Desktop.Repository.Words;
using WordGenius.Helpers;

namespace WordGenius.Desktop.Windows.Words
{
    /// <summary>
    /// Interaction logic for WordCreate.xaml
    /// </summary>
    public partial class WordCreate : Window
    {
        private readonly IWordsRepository _wordsRepository;

        public WordCreate()
        {
            InitializeComponent();
            this._wordsRepository = new WordRepository();
        }

        private void InformationBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("");
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var word = GetDateUI();
            var result = await _wordsRepository.CreateAsync(word);
            if (result > 0)
            {
                MessageBox.Show("Successful");
                this.Close();
            }
        }

        private Word GetDateUI()
        {
            Word word = new Word();
            word.Text = uzbekWordTb.Text;
            word.Translate = englishWordTb.Text;
            word.Discription = descriptionTb.Text;
            word.Is_Remember = false;
            word.CreateAt = TimeHelper.GetDateTime();
            word.UpdateAt = TimeHelper.GetDateTime();

            return word;
        }
    }
}
