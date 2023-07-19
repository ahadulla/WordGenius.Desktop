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
using WordGenius.Desktop.Repository.Words;
using WordGenius.Helpers;

namespace WordGenius.Desktop.Windows.Words
{
    /// <summary>
    /// Interaction logic for WordUpdate.xaml
    /// </summary>
    /// 



    public partial class WordUpdate : Window
    {


        private readonly WordRepository _wordRepository;

        public long Id { get; set; }

        public WordUpdate()
        {
            InitializeComponent();
            this._wordRepository = new WordRepository();

        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var word = GetDateUI();
            if (word != null)
            {
                var result = await _wordRepository.UpdateAsync(Id,word);
                if (result > 0)
                {
                    this.Close();
                }
            }
        }

        private Word GetDateUI()
        {
            if (uzbekWordTb.Text != string.Empty && englishWordTb.Text != string.Empty)
            {
                Word word = new Word();
                word.Text = uzbekWordTb.Text;
                word.Translate = englishWordTb.Text;
                word.Discription = descriptionTb.Text;
                word.UpdateAt = TimeHelper.GetDateTime();
                return word;
            }
            return null;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InformationBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("");
        }
    }
}
