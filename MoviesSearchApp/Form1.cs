using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace MoviesSearchApp
{
    public partial class Form1 : Form
    {
        private const string apiKey = "fae137e";
        public Form1()
        {
            InitializeComponent();
        }

        private void txtMovie(object sender, EventArgs e)
        {

        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string movieTitle = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(movieTitle))
            {
                MessageBox.Show("Please enter a title movie: ");
                return;
            }

            await SearchMovieAsync(movieTitle);

        }

        private async Task SearchMovieAsync(string title)
        {
            string apiUrl = $"https://www.omdbapi.com/?t={Uri.EscapeDataString(title)}&apikey={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    JObject movie = JObject.Parse(json);

                    if (movie["Response"].ToString() == "false")
                    {
                        MessageBox.Show("Movie is not found");
                        return;
                    }

                    lblTitle.Text = "Title: " + (string)movie["Title"];
                    lblYear.Text = "Year: " + (string)movie["Year"];
                    lblGenre.Text = "Genre: " + (string)movie["Genre"];
                    lblRating.Text = "IMDB Rating: " + (string)movie["imdbRating"];

                    string posterUrl = (string)movie["Poster"];
                    if (!string.IsNullOrEmpty(posterUrl) && posterUrl != "N/A")
                    {

                        pictureBox1.Load(posterUrl);

                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }


                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }


            }
        }

    }
}
