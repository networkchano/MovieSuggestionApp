using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieSuggestionApp
{
    public partial class MainForm : Form
    {
        // Controls
        private TextBox txtSearch;
        private Button btnSearchOnline, btnSuggest, btnAddMovie, btnSettings, btnToggleTheme;
        private FlowLayoutPanel pnlMovies, pnlFavorites;
        private Panel pnlDetails;
        private PictureBox picPosterLarge;
        private Label lblTitle, lblGenre, lblYear, lblRating;
        private Button btnAddFavorite, btnRemoveFavorite;

        private bool darkMode = true;
        private List<Movie> allMovies = new List<Movie>();

        // Path to placeholder image
        private string placeholderPath = "Images\\placeholder.png";

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Movie Suggestion App - Netflix Style";
            this.ClientSize = new Size(1280, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9);
            this.BackColor = Color.FromArgb(18, 18, 18);

            // Top navigation
            txtSearch = new TextBox() { Location = new Point(20, 20), Size = new Size(400, 30) };
            // Manual placeholder
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Text = "Search movies...";
            txtSearch.GotFocus += (s, e) =>
            {
                if (txtSearch.Text == "Search movies...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };
            txtSearch.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "Search movies...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            btnSearchOnline = new Button() { Text = "Search Online", Location = new Point(430, 20), Size = new Size(120, 30), FlatStyle = FlatStyle.Flat };
            btnSuggest = new Button() { Text = "Suggest Movie", Location = new Point(560, 20), Size = new Size(120, 30), FlatStyle = FlatStyle.Flat };
            btnAddMovie = new Button() { Text = "Add Movie", Location = new Point(690, 20), Size = new Size(120, 30), FlatStyle = FlatStyle.Flat };
            btnSettings = new Button() { Text = "Settings", Location = new Point(820, 20), Size = new Size(120, 30), FlatStyle = FlatStyle.Flat };
            btnToggleTheme = new Button() { Text = "Light Mode", Location = new Point(950, 20), Size = new Size(120, 30), FlatStyle = FlatStyle.Flat };
            btnToggleTheme.Click += BtnToggleTheme_Click;

            this.Controls.AddRange(new Control[] { txtSearch, btnSearchOnline, btnSuggest, btnAddMovie, btnSettings, btnToggleTheme });

            // Main movie carousel
            pnlMovies = new FlowLayoutPanel()
            {
                Location = new Point(20, 70),
                Size = new Size(1240, 400),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.FromArgb(18, 18, 18)
            };
            this.Controls.Add(pnlMovies);

            // Details panel
            pnlDetails = new Panel()
            {
                Location = new Point(20, 480),
                Size = new Size(1240, 180),
                BackColor = Color.FromArgb(30, 30, 30)
            };
            this.Controls.Add(pnlDetails);

            picPosterLarge = new PictureBox() { Location = new Point(10, 10), Size = new Size(150, 225), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
            lblTitle = new Label() { Location = new Point(180, 10), Size = new Size(1040, 30), ForeColor = Color.WhiteSmoke, Font = new Font("Segoe UI", 14, FontStyle.Bold), Text = "Title" };
            lblGenre = new Label() { Location = new Point(180, 50), Size = new Size(1040, 20), ForeColor = Color.WhiteSmoke, Text = "Genre" };
            lblYear = new Label() { Location = new Point(180, 80), Size = new Size(1040, 20), ForeColor = Color.WhiteSmoke, Text = "Year" };
            lblRating = new Label() { Location = new Point(180, 110), Size = new Size(1040, 20), ForeColor = Color.WhiteSmoke, Text = "Rating" };
            btnAddFavorite = new Button() { Location = new Point(180, 140), Size = new Size(120, 30), Text = "Add Favorite", FlatStyle = FlatStyle.Flat };
            btnRemoveFavorite = new Button() { Location = new Point(310, 140), Size = new Size(150, 30), Text = "Remove Favorite", FlatStyle = FlatStyle.Flat };

            pnlDetails.Controls.AddRange(new Control[] { picPosterLarge, lblTitle, lblGenre, lblYear, lblRating, btnAddFavorite, btnRemoveFavorite });

            // Favorites carousel
            pnlFavorites = new FlowLayoutPanel()
            {
                Location = new Point(20, 670),
                Size = new Size(1240, 120),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.FromArgb(18, 18, 18)
            };
            this.Controls.Add(pnlFavorites);

            this.Load += MainForm_Load;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await LoadMoviesAsync();
        }

        private void BtnToggleTheme_Click(object sender, EventArgs e)
        {
            if (darkMode)
            {
                this.BackColor = Color.WhiteSmoke;
                pnlMovies.BackColor = Color.WhiteSmoke;
                pnlDetails.BackColor = Color.LightGray;
                pnlFavorites.BackColor = Color.WhiteSmoke;
                darkMode = false;
                btnToggleTheme.Text = "Dark Mode";
            }
            else
            {
                this.BackColor = Color.FromArgb(18, 18, 18);
                pnlMovies.BackColor = Color.FromArgb(18, 18, 18);
                pnlDetails.BackColor = Color.FromArgb(30, 30, 30);
                pnlFavorites.BackColor = Color.FromArgb(18, 18, 18);
                darkMode = true;
                btnToggleTheme.Text = "Light Mode";
            }
        }

        // Load image from URL with file-based placeholder
        private async Task<Image> LoadImageFromUrlAsync(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var data = await client.GetByteArrayAsync(url);
                    using (var ms = new System.IO.MemoryStream(data))
                    {
                        return Image.FromStream(ms);
                    }
                }
            }
            catch
            {
                // File-based placeholder
                if (System.IO.File.Exists(placeholderPath))
                    return Image.FromFile(placeholderPath);
                else
                    return new Bitmap(150, 200); // fallback empty image
            }
        }

        private async Task LoadMoviesAsync()
        {
            allMovies.Add(new Movie { Title = "Interstellar", Year = "2014", Genre = "Sci-Fi", Rating = 8.6, PosterUrl = "https://image.tmdb.org/t/p/w500/rAiYTfKGqDCRIIqo664sY9XZIvQ.jpg" });
            allMovies.Add(new Movie { Title = "The Dark Knight", Year = "2008", Genre = "Action", Rating = 9.0, PosterUrl = "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg" });
            allMovies.Add(new Movie { Title = "Spirited Away", Year = "2001", Genre = "Animation", Rating = 8.6, PosterUrl = "https://image.tmdb.org/t/p/w500/dL11DBPcRhWWnJcFXl9A07MrqTI.jpg" });
            allMovies.Add(new Movie { Title = "The Shawshank Redemption", Year = "1994", Genre = "Drama", Rating = 9.3, PosterUrl = "https://image.tmdb.org/t/p/w500/q6y0Go1tsGEsmtFryDOJo3dEmqu.jpg" });
            allMovies.Add(new Movie { Title = "The Godfather", Year = "1972", Genre = "Crime / Drama", Rating = 9.2, PosterUrl = "https://image.tmdb.org/t/p/w500/iVZ3JAcAjmguGPnRNfWFOtLHOuY.jpg" });
            allMovies.Add(new Movie { Title = "Fight Club", Year = "1999", Genre = "Drama", Rating = 8.8, PosterUrl = "https://image.tmdb.org/t/p/w500/bptfVGEQuv6vDTIMVCHjJ9Dz8PX.jpg" });
            allMovies.Add(new Movie { Title = "12 Angry Men", Year = "1957", Genre = "Drama", Rating = 9.0, PosterUrl = "https://image.tmdb.org/t/p/w500/3W0v956XxSG5xgm7LB6qu8ExYJ2.jpg" });
            allMovies.Add(new Movie { Title = "The Lord of the Rings: The Return of the King", Year = "2003", Genre = "Adventure / Fantasy", Rating = 9.0, PosterUrl = "https://image.tmdb.org/t/p/w500/rCzpDGLbOoPwLjy3OAm5NUPOTrC.jpg" });
            allMovies.Add(new Movie { Title = "Schindler's List", Year = "1993", Genre = "Biography / Drama", Rating = 9.0, PosterUrl = "https://image.tmdb.org/t/p/w500/c8Ass7acuOe4za6DhSattE359gr.jpg" });
            allMovies.Add(new Movie { Title = "Pulp Fiction", Year = "1994", Genre = "Crime / Drama", Rating = 8.8, PosterUrl = "https://image.tmdb.org/t/p/w500/d5iIlFn5s0ImszYzBPb8JPIfbXD.jpg" });
            allMovies.Add(new Movie { Title = "The Lord of the Rings: The Fellowship of the Ring", Year = "2001", Genre = "Adventure / Fantasy", Rating = 8.9, PosterUrl = "https://image.tmdb.org/t/p/w500/6oom5QYQ2yQTMJIbnvbkBL9cHo6.jpg" });
            allMovies.Add(new Movie { Title = "Forrest Gump", Year = "1994", Genre = "Drama / Romance", Rating = 8.8, PosterUrl = "https://image.tmdb.org/t/p/w500/saHP97rTPS5eLmrLQEcANmKrsFl.jpg" });

            // Add more movies as needed...

            foreach (var movie in allMovies)
            {
                movie.PosterImage = await LoadImageFromUrlAsync(movie.PosterUrl);

                Panel moviePanel = new Panel() { Size = new Size(150, 250), Margin = new Padding(12) };
                PictureBox poster = new PictureBox()
                {
                    Size = new Size(150, 200),
                    Image = movie.PosterImage,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Cursor = Cursors.Hand
                };
                Label title = new Label() { Text = movie.Title, Dock = DockStyle.Bottom, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.WhiteSmoke };
                poster.Click += (s, e) => ShowMovieDetails(movie);

                moviePanel.Controls.Add(poster);
                moviePanel.Controls.Add(title);
                pnlMovies.Controls.Add(moviePanel);
            }

        }

        private void ShowMovieDetails(Movie movie)
        {
            lblTitle.Text = movie.Title;
            lblGenre.Text = "Genre: " + movie.Genre;
            lblYear.Text = "Year: " + movie.Year;
            lblRating.Text = "Rating: " + movie.Rating.ToString();
            picPosterLarge.Image = movie.PosterImage;
        }

        private Random rnd = new Random();

        private void btnSuggest_Click(object sender, EventArgs e)
        {
            if (allMovies.Count == 0) return;

            int index = rnd.Next(allMovies.Count);
            Movie suggested = allMovies[index];
            ShowMovieDetails(suggested);

            // Optional: highlight the suggested movie in the carousel
            MessageBox.Show($"We suggest: {suggested.Title} ({suggested.Year})", "Movie Suggestion", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      


        public class Movie
        {
            public string Title { get; set; }
            public string Year { get; set; }
            public string Genre { get; set; }
            public double Rating { get; set; }
            public string PosterUrl { get; set; }
            public Image PosterImage { get; set; }
        }
    }
}
