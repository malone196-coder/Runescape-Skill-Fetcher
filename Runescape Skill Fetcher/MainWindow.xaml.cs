using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.VisualBasic.FileIO;
using System.IO;


namespace Runescape_Skill_Fetcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public class PlayerStats
        { 
            public string agility = "";
            public string attack = "";
            public string construction = "";
            public string cooking = "";
            public string crafting = "";
            public string defence = "";
            public string farming = "";
            public string firemaking = "";
            public string fishing = "";
            public string fletching = "";
            public string herblore = "";
            public string hitpoints = "";
            public string hunter = "";
            public string magic = "";
            public string mining = "";
            public string prayer = "";
            public string ranged = "";
            public string runecrafting = "";
            public string sailing = "";
            public string slayer = "";
            public string smithing = "";
            public string strength = "";
            public string thieving = "";
            public string woodcutting = "";
        }

        public class ApiClient 
        {
            private static readonly HttpClient client = new HttpClient();

            public static async Task<string> GetPlayerStats(string username)
            {
                // URL-encode the username to handle spaces and special characters
                string encoded = Uri.EscapeDataString(username ?? string.Empty);
                string url = $"https://secure.runescape.com/m=hiscore_oldschool/index_lite.ws?player={encoded}";
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseBody;
                

        
            }
        }

        public void PlayerUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            string username = PlayerUsername.Text;
        }

        public class HiscoreEntry
        {
            public int Rank { get; set; }
            public int Level { get; set; }
            public int Experience { get; set; }
        }

        public async void FetchStatsButton_Click(object sender, RoutedEventArgs e)
        {
            string username = PlayerUsername.Text;

            var button = sender as Button;
            try
            {
                if (button != null) button.IsEnabled = false;
                await FetchStats(username);
            }
            finally
            {
                if (button != null) button.IsEnabled = true;
            }
        }

        public async Task FetchStats(string username)
        {
            try
            {

                string stats = await ApiClient.GetPlayerStats(username);
                // Display CSV data in the PlayerStatsDisplay
                using (StringReader stringReader = new StringReader(stats))

                using (TextFieldParser parser = new TextFieldParser(stringReader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    parser.HasFieldsEnclosedInQuotes = false;

                    while (!parser.EndOfData)

                    {
                        string[] fields = parser.ReadFields();
                       
                    }
                }


                PlayerStatsDisplay_Text(stats);
            }
            catch (HttpRequestException ex)
            {
                // Show HTTP errors in the PlayerStatsDisplay as text
                PlayerStatsDisplay_Text($"Error fetching player stats: {ex.Message}");
            }
            catch (Exception ex)
            {
                PlayerStatsDisplay_Text($"Unexpected error: {ex.Message}");
            }
        }

        public void PlayerStatsDisplay_Text(string stats) => PlayerStatsDisplay.Document = new FlowDocument(new Paragraph(new Run(stats)));
    }
}