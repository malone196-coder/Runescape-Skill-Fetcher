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
using System.Diagnostics;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper.Configuration.Attributes;


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

        public record Spy(string Level);
        
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
            //This part of the code is going to assign to the string 'stats' the results from ApiClient.GetPlayerStats
            //Then it is going to parse through the CSV data and assign to strings for each skill. May need to clean up later.
            try
            {

                string stats = await ApiClient.GetPlayerStats(username);
                

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    MissingFieldFound = null,
                };

                using var reader = new StringReader(stats);
                using var csv = new CsvReader(reader, config);

                var levels = new List<int>();
                while (csv.Read())
                {
                    var level = csv.GetField<int>(1);
                    levels.Add(level);
                    

                }
                
                string agility = $"Agility: {levels[17] }";
                string attack = $"Attack: {levels[1]}";
                string construction = $"Construction: {levels[23]}";
                string cooking = $"Cooking: {levels[8]}";
                string crafting = $"Crafting {levels[13]}";
                string defence = $"Defence: {levels[2]}";
                string farming = $"Farming: {levels[20]}";
                string firemaking = $"Firemaking: {levels[12]}";
                string fishing = $"Fishing: {levels[11]}";
                string fletching = $"Fletching: {levels[10]}";
                string herblore = $"Herblore: {levels[16]}";
                string hitpoints = $"Hitpoints: {levels[4]}";
                string hunter = $"Hunter: {levels[22]}";
                string magic = $"Magic: {levels[7]}";
                string mining = $"Mining: {levels[15]}";
                string prayer = $"Prayer: {levels[6]}";
                string ranged = $"Ranged: {levels[5]}";
                string runecrafting = $"Runecrafting: {levels[21]}";
                string sailing = $"Sailing: {levels[24]}";
                string slayer = $"Slayer: {levels[19]}";
                string smithing = $"Smithing: {levels[14]}";
                string strength = $"Strength: {levels[3]}";
                string thieving = $"Thieving: {levels[18]}";
                string woodcutting = $"Woodcutting: {levels[9]}";
                
                //the following is used to set up the RichTextBoxDisplay
                string[] levelsStringDisplayArray = { agility, attack, construction, cooking, crafting, defence, farming, firemaking, fishing, fletching, herblore, hitpoints, hunter,
                                                      magic, mining, prayer, ranged, runecrafting, sailing, slayer, smithing, strength, thieving, woodcutting};



                string levelsStringDisplay = $"{levelsStringDisplayArray[1]}\t{levelsStringDisplayArray[11]}\t{levelsStringDisplayArray[14]}\n" +
                                             $"{levelsStringDisplayArray[21]}\t{levelsStringDisplayArray[0]}\t{levelsStringDisplayArray[20]}\n" +
                                             $"{levelsStringDisplayArray[5]}\t{levelsStringDisplayArray[10]}\t{levelsStringDisplayArray[8]}\n" +
                                             $"{levelsStringDisplayArray[16]}\t{levelsStringDisplayArray[22]}\t{levelsStringDisplayArray[3]}\n" +
                                             $"{levelsStringDisplayArray[15]}\t{levelsStringDisplayArray[4]}\t{levelsStringDisplayArray[7]}\n" +
                                             $"{levelsStringDisplayArray[13]}\t{levelsStringDisplayArray[9]}\t{levelsStringDisplayArray[23]}\n" +
                                             $"{levelsStringDisplayArray[17]}\t{levelsStringDisplayArray[19]}\t{levelsStringDisplayArray[6]}\n" +
                                             $"{levelsStringDisplayArray[2]}\t{levelsStringDisplayArray[12]}\t{levelsStringDisplayArray[18]}"
                ;
                PlayerStatsDisplay_Text(levelsStringDisplay);
                





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