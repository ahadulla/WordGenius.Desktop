using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WordGenius.Desktop.Entities.Translator;

namespace WordGenius.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for TranslatePage.xaml
    /// </summary>
    public partial class TranslatePage : Page
    {
        public TranslatePage()
        {
            InitializeComponent();
        }


        private void replaceButton_Click(object sender, RoutedEventArgs e)
        {
            string temp = brFromLb.Content.ToString()!;
            brFromLb.Content = brToLb.Content;
            brToLb.Content = temp;
        }

        private void copyTextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (translateTextTb.Text != null)
            {
                string text = translateTextTb.Text.ToString()!;
                Clipboard.SetText(text);
            }
        }

        public static async Task<string> Translate(string from, string to, string text)
        {
            try
            {
                string body;
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://google-translate1.p.rapidapi.com/language/translate/v2"),
                    Headers =  {
                             { "X-RapidAPI-Key", "9335dd2676msh24b28885b1584cep13a9e1jsn9d7e47c44a6c" },
                             { "X-RapidAPI-Host", "google-translate1.p.rapidapi.com" },
                            },
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                 {
                     { "q", $"{text}" },
                     { "target", $"{to}" },
                     { "source", $"{from}" },
                 }),
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    body = await response.Content.ReadAsStringAsync();
                }
                return body;
            }
            catch
            {
                return null;
            }
        }

        private async void startTranslateButton_Click(object sender, RoutedEventArgs e)
        {
            string from = brFromLb.Content.ToString()!;
            string to = brToLb.Content.ToString()!;
            if (from == "Uzbek")
            {
                from = "uz";
                to = "en";
            }
            else
            {
                from = "en";
                to = "uz";
            }
            if (fromText.Text != null)
            {
                string text = fromText.Text;
                string JsonContent = await Translate(from, to, text);
                if(JsonContent != null)
                {
                    TranslationResponse transtation = JsonConvert.DeserializeObject<TranslationResponse>(JsonContent);
                    translateTextTb.Text = transtation.data.translations[0].translatedText;
                }
                else
                {
                    MessageBox.Show("May have too much data or no internet connection");
                }
               
            }
        }
    }
}
