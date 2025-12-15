using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieSuggestionApp
{
    public partial class MainForm : Form
    {
        private List<Movie> _movies;
        private List<int> _watchlist = new();
        private Movie? _currentMovie;
        private Random _random = new();
        private string? _selectedGenre = null;
        private List<Button> _genreButtons = new();
        private static readonly HttpClient httpClient = new HttpClient();

        public MainForm()
        {
            InitializeComponent();
            _movies = MovieRepository.GetMovies();

            InitializeGenreButtons();
            UpdateWatchlistCount();
            ApplyDarkTheme();
        }

        private void ApplyDarkTheme()
        {
            this.BackColor = Color.FromArgb(10, 10, 18);
            this.ForeColor = Color.White;

            pnlCard.BackColor = Color.FromArgb(20, 20, 25);
            pnlCard.Visible = false;

            pnlDetails.BackColor = Color.Transparent;

            picPoster.BackColor = Color.FromArgb(30, 30, 35);

            btnSuggest.BackColor = Color.FromArgb(220, 38, 38);
            btnSuggest.ForeColor = Color.White;
            btnSuggest.FlatStyle = FlatStyle.Flat;
            btnSuggest.FlatAppearance.BorderSize = 0;

            lblHeader.ForeColor = Color.White;

            btnWatchlist.FlatStyle = FlatStyle.Flat;
            btnWatchlist.FlatAppearance.BorderSize = 0;
        }

        private void InitializeGenreButtons()
        {
            var genres = _movies.SelectMany(m => m.Genre).Distinct().OrderBy(g => g).ToList();

            // "All Genres" button
            CreateGenreButton("All Genres", true);

            // Individual genre buttons
            foreach (var genre in genres)
            {
                CreateGenreButton(genre, false);
            }
        }

        private void CreateGenreButton(string text, bool isDefault)
        {
            var btn = new Button();
            btn.Text = text;
            btn.AutoSize = true;
            btn.Padding = new Padding(10, 5, 10, 5);
            btn.Cursor = Cursors.Hand;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;

            if (isDefault)
            {
                SetGenreButtonActive(btn, true);
            }
            else
            {
                SetGenreButtonActive(btn, false);
            }

            btn.Click += GenreButton_Click;
            flowGenres.Controls.Add(btn);
            _genreButtons.Add(btn);
        }

        private void SetGenreButtonActive(Button btn, bool active)
        {
            if (active)
            {
                btn.BackColor = Color.FromArgb(220, 38, 38);
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.FromArgb(220, 38, 38);
            }
            else
            {
                btn.BackColor = Color.Transparent;
                btn.ForeColor = Color.Gray;
                btn.FlatAppearance.BorderColor = Color.Gray;
            }
        }

        private void GenreButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button clickedBtn)
            {
                // Reset all buttons
                foreach (var btn in _genreButtons)
                {
                    SetGenreButtonActive(btn, false);
                }

                // Activate clicked button
                SetGenreButtonActive(clickedBtn, true);

                if (clickedBtn.Text == "All Genres")
                {
                    _selectedGenre = null;
                }
                else
                {
                    _selectedGenre = clickedBtn.Text;
                }
            }
        }

        private void btnSuggest_Click(object sender, EventArgs e)
        {
            var pool = _movies;

            if (!string.IsNullOrEmpty(_selectedGenre))
            {
                pool = _movies.Where(m => m.Genre.Contains(_selectedGenre)).ToList();
            }

            if (pool.Count == 0) pool = _movies;

            Cursor = Cursors.WaitCursor;
            System.Threading.Thread.Sleep(400);
            Cursor = Cursors.Default;

            _currentMovie = pool[_random.Next(pool.Count)];
            DisplayMovie(_currentMovie);
        }

        private async void DisplayMovie(Movie movie)
        {
            pnlCard.Visible = true;
            lblTitle.Text = movie.Title;
            lblRating.Text = $"{movie.Rating}/10";
            lblMeta.Text = $"{movie.Year} • {movie.Runtime} • {movie.Director}";
            lblPlot.Text = movie.Plot;
            lblGenres.Text = string.Join(" • ", movie.Genre);

            // Load poster image
            await LoadPosterAsync(movie.PosterUrl);

            UpdateButtonState();
        }

        private async Task LoadPosterAsync(string url)
        {
            try
            {
                // Show loading placeholder
                picPoster.Image = null;
                picPoster.BackColor = Color.FromArgb(30, 30, 35);

                if (string.IsNullOrEmpty(url))
                {
                    return;
                }

                // Download image
                var imageBytes = await httpClient.GetByteArrayAsync(url);
                using (var ms = new System.IO.MemoryStream(imageBytes))
                {
                    var image = Image.FromStream(ms);

                    // Update UI on the main thread
                    if (picPoster.InvokeRequired)
                    {
                        picPoster.Invoke(new Action(() => picPoster.Image = image));
                    }
                    else
                    {
                        picPoster.Image = image;
                    }
                }
            }
            catch (Exception ex)
            {
                // If image fails to load, show placeholder
                picPoster.Image = null;
                picPoster.BackColor = Color.FromArgb(40, 40, 50);
                Console.WriteLine($"Error loading poster: {ex.Message}");
            }
        }

        private void btnWatchlist_Click(object sender, EventArgs e)
        {
            if (_currentMovie == null) return;

            if (_watchlist.Contains(_currentMovie.Id))
            {
                _watchlist.Remove(_currentMovie.Id);
            }
            else
            {
                _watchlist.Add(_currentMovie.Id);
            }

            UpdateButtonState();
            UpdateWatchlistCount();
        }

        private void UpdateButtonState()
        {
            if (_currentMovie == null) return;

            if (_watchlist.Contains(_currentMovie.Id))
            {
                btnWatchlist.Text = "✓ In Watchlist";
                btnWatchlist.BackColor = Color.FromArgb(40, 40, 50);
            }
            else
            {
                btnWatchlist.Text = "+ Add to Watchlist";
                btnWatchlist.BackColor = Color.FromArgb(220, 38, 38);
            }
        }

        private void UpdateWatchlistCount()
        {
            lblWatchlistCount.Text = $"Watchlist: {_watchlist.Count}";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}