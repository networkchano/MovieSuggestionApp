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
        private FlowLayoutPanel flowAllMovies;
        private Button btnViewWatchlist;
        private Button btnBackToMain;
        private bool _isViewingWatchlist = false;

        public MainForm()
        {
            InitializeComponent();
            _movies = MovieRepository.GetMovies();

            // Set fullscreen mode
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = false;

            // Initialize the all movies panel
            InitializeAllMoviesPanel();
            InitializeWatchlistButton();
            InitializeBackButton();

            InitializeGenreButtons();
            UpdateWatchlistCount();
            ApplyDarkTheme();

            // Adjust layout for fullscreen
            this.Resize += MainForm_Resize;
            AdjustLayoutForFullscreen();

            // Display all movies initially
            DisplayAllMoviesByGenre();
        }

        private void InitializeAllMoviesPanel()
        {
            flowAllMovies = new FlowLayoutPanel();
            flowAllMovies.AutoScroll = true;
            flowAllMovies.FlowDirection = FlowDirection.TopDown;
            flowAllMovies.WrapContents = false;
            flowAllMovies.BackColor = Color.Transparent;
            this.Controls.Add(flowAllMovies);
        }

        private void InitializeWatchlistButton()
        {
            btnViewWatchlist = new Button();
            btnViewWatchlist.Text = "📋 View Watchlist";
            btnViewWatchlist.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnViewWatchlist.BackColor = Color.FromArgb(220, 38, 38);
            btnViewWatchlist.ForeColor = Color.White;
            btnViewWatchlist.FlatStyle = FlatStyle.Flat;
            btnViewWatchlist.FlatAppearance.BorderSize = 0;
            btnViewWatchlist.Cursor = Cursors.Hand;
            btnViewWatchlist.Width = 200;
            btnViewWatchlist.Height = 50;
            btnViewWatchlist.Click += BtnViewWatchlist_Click;
            this.Controls.Add(btnViewWatchlist);
            btnViewWatchlist.BringToFront();
        }

        private void InitializeBackButton()
        {
            btnBackToMain = new Button();
            btnBackToMain.Text = "← Back to Main";
            btnBackToMain.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnBackToMain.BackColor = Color.FromArgb(60, 60, 70);
            btnBackToMain.ForeColor = Color.White;
            btnBackToMain.FlatStyle = FlatStyle.Flat;
            btnBackToMain.FlatAppearance.BorderSize = 0;
            btnBackToMain.Cursor = Cursors.Hand;
            btnBackToMain.Width = 200;
            btnBackToMain.Height = 50;
            btnBackToMain.Visible = false;
            btnBackToMain.Click += BtnBackToMain_Click;
            this.Controls.Add(btnBackToMain);
            btnBackToMain.BringToFront();
        }

        private void BtnViewWatchlist_Click(object sender, EventArgs e)
        {
            ShowWatchlist();
        }

        private void BtnBackToMain_Click(object sender, EventArgs e)
        {
            GoBackToMain();
        }

        private void ShowWatchlist()
        {
            _isViewingWatchlist = true;
            flowAllMovies.Controls.Clear();
            flowAllMovies.Visible = true;
            pnlCard.Visible = false;
            btnBackToMain.Visible = true;

            // Hide genre buttons and suggest button when viewing watchlist
            flowGenres.Visible = false;
            btnSuggest.Visible = false;

            var watchlistMovies = _movies.Where(m => _watchlist.Contains(m.Id)).ToList();

            if (watchlistMovies.Count == 0)
            {
                var lblEmpty = new Label();
                lblEmpty.Text = "Your watchlist is empty.\nAdd movies to your watchlist to see them here!";
                lblEmpty.Font = new Font("Segoe UI", 18F, FontStyle.Regular);
                lblEmpty.ForeColor = Color.Gray;
                lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
                lblEmpty.AutoSize = false;
                lblEmpty.Width = flowAllMovies.Width - 40;
                lblEmpty.Height = 200;
                lblEmpty.Padding = new Padding(20);
                flowAllMovies.Controls.Add(lblEmpty);
            }
            else
            {
                // Title
                var lblTitle = new Label();
                lblTitle.Text = $"My Watchlist ({watchlistMovies.Count} movies)";
                lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
                lblTitle.ForeColor = Color.White;
                lblTitle.AutoSize = false;
                lblTitle.Width = flowAllMovies.Width - 40;
                lblTitle.Height = 60;
                lblTitle.Padding = new Padding(10, 10, 0, 0);
                flowAllMovies.Controls.Add(lblTitle);

                // Movie grid
                var movieFlow = new FlowLayoutPanel();
                movieFlow.FlowDirection = FlowDirection.LeftToRight;
                movieFlow.WrapContents = true;
                movieFlow.AutoScroll = false;
                movieFlow.Width = flowAllMovies.Width - 40;
                movieFlow.Height = flowAllMovies.Height - 100;
                movieFlow.BackColor = Color.Transparent;
                movieFlow.Padding = new Padding(10, 0, 10, 10);

                foreach (var movie in watchlistMovies)
                {
                    CreateWatchlistMovieCard(movieFlow, movie);
                }

                flowAllMovies.Controls.Add(movieFlow);
            }
        }

        private void CreateWatchlistMovieCard(FlowLayoutPanel parent, Movie movie)
        {
            var card = new Panel();
            card.Width = 200;
            card.Height = 340;
            card.BackColor = Color.FromArgb(30, 30, 35);
            card.Cursor = Cursors.Hand;
            card.Margin = new Padding(10);
            card.Tag = movie;

            // Poster
            var pic = new PictureBox();
            pic.Width = 200;
            pic.Height = 250;
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.BackColor = Color.FromArgb(40, 40, 50);
            pic.Location = new Point(0, 0);
            card.Controls.Add(pic);

            // Load poster asynchronously
            LoadMovieCardPoster(pic, movie.PosterUrl);

            // Title
            var lblTitle = new Label();
            lblTitle.Text = movie.Title;
            lblTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Width = 200;
            lblTitle.Height = 40;
            lblTitle.Location = new Point(0, 250);
            card.Controls.Add(lblTitle);

            // Remove button
            var btnRemove = new Button();
            btnRemove.Text = "✕ Remove";
            btnRemove.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnRemove.ForeColor = Color.White;
            btnRemove.BackColor = Color.FromArgb(220, 38, 38);
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Width = 180;
            btnRemove.Height = 30;
            btnRemove.Location = new Point(10, 300);
            btnRemove.Cursor = Cursors.Hand;
            btnRemove.Click += (s, e) => {
                _watchlist.Remove(movie.Id);
                UpdateWatchlistCount();
                ShowWatchlist(); // Refresh the watchlist view
            };
            card.Controls.Add(btnRemove);

            // Rating badge
            var lblRating = new Label();
            lblRating.Text = $"★ {movie.Rating}";
            lblRating.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRating.ForeColor = Color.White;
            lblRating.BackColor = Color.FromArgb(220, 38, 38);
            lblRating.AutoSize = true;
            lblRating.Padding = new Padding(5, 2, 5, 2);
            lblRating.Location = new Point(5, 5);
            card.Controls.Add(lblRating);
            lblRating.BringToFront();

            // Click event to show full details
            card.Click += (s, e) => ShowMovieDetails(movie);
            pic.Click += (s, e) => ShowMovieDetails(movie);
            lblTitle.Click += (s, e) => ShowMovieDetails(movie);

            parent.Controls.Add(card);
        }

        private void GoBackToMain()
        {
            _isViewingWatchlist = false;
            pnlCard.Visible = false;
            flowAllMovies.Visible = true;
            btnBackToMain.Visible = false;
            flowGenres.Visible = true;
            btnSuggest.Visible = true;
            DisplayAllMoviesByGenre();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            AdjustLayoutForFullscreen();
        }

        private void AdjustLayoutForFullscreen()
        {
            int screenWidth = this.ClientSize.Width;
            int screenHeight = this.ClientSize.Height;

            // Header - centered at top
            lblHeader.Width = screenWidth;
            lblHeader.Location = new Point(0, 40);
            lblHeader.Height = 80;

            // View Watchlist button - top right
            btnViewWatchlist.Location = new Point(screenWidth - 220, 50);

            // Back to Main button - top left
            btnBackToMain.Location = new Point(20, 50);

            // Genre buttons - centered below header
            int genreFlowWidth = Math.Min(1000, screenWidth - 100);
            flowGenres.Width = genreFlowWidth;
            flowGenres.Location = new Point((screenWidth - genreFlowWidth) / 2, 140);
            flowGenres.Height = 80;

            // Suggest button - centered
            int btnSuggestWidth = 400;
            btnSuggest.Width = btnSuggestWidth;
            btnSuggest.Location = new Point((screenWidth - btnSuggestWidth) / 2, 240);
            btnSuggest.Height = 70;

            // All movies panel - below suggest button
            int moviesWidth = Math.Min(1400, screenWidth - 100);
            flowAllMovies.Width = moviesWidth;
            flowAllMovies.Height = screenHeight - 380;
            flowAllMovies.Location = new Point((screenWidth - moviesWidth) / 2, 330);

            // Movie card panel - centered with responsive sizing (hidden by default)
            int cardWidth = Math.Min(1200, screenWidth - 100);
            int cardHeight = Math.Min(600, screenHeight - 450);
            pnlCard.Width = cardWidth;
            pnlCard.Height = cardHeight;
            pnlCard.Location = new Point((screenWidth - cardWidth) / 2, 330);

            // Adjust poster and details within card
            if (pnlCard.Visible)
            {
                picPoster.Location = new Point(30, 30);
                picPoster.Width = 280;
                picPoster.Height = cardHeight - 60;

                pnlDetails.Location = new Point(330, 30);
                pnlDetails.Width = cardWidth - 380;
                pnlDetails.Height = cardHeight - 60;

                // Adjust button position within details
                btnWatchlist.Width = pnlDetails.Width - 40;
                btnWatchlist.Location = new Point(20, pnlDetails.Height - 80);

                // Adjust plot label size
                lblPlot.Width = pnlDetails.Width - 40;
                lblPlot.Height = pnlDetails.Height - 260;
            }

            // Watchlist count - bottom right
            lblWatchlistCount.Location = new Point(screenWidth - 200, screenHeight - 50);
        }

        private void DisplayAllMoviesByGenre()
        {
            flowAllMovies.Controls.Clear();
            flowAllMovies.Visible = true;
            pnlCard.Visible = false;

            // Get movies to display based on selected genre
            var moviesToDisplay = _movies;
            if (!string.IsNullOrEmpty(_selectedGenre))
            {
                moviesToDisplay = _movies.Where(m => m.Genre.Contains(_selectedGenre)).ToList();
            }

            // Group movies by genre
            var genreGroups = moviesToDisplay
                .SelectMany(m => m.Genre.Select(g => new { Genre = g, Movie = m }))
                .GroupBy(x => x.Genre)
                .OrderBy(g => g.Key);

            if (!string.IsNullOrEmpty(_selectedGenre))
            {
                // If a specific genre is selected, just show that genre
                var filteredGroup = genreGroups.FirstOrDefault(g => g.Key == _selectedGenre);
                if (filteredGroup != null)
                {
                    CreateGenreSection(filteredGroup.Key, filteredGroup.Select(x => x.Movie).Distinct().ToList());
                }
            }
            else
            {
                // Show all genres
                foreach (var genreGroup in genreGroups)
                {
                    var moviesInGenre = genreGroup.Select(x => x.Movie).Distinct().ToList();
                    CreateGenreSection(genreGroup.Key, moviesInGenre);
                }
            }
        }

        private void CreateGenreSection(string genreName, List<Movie> movies)
        {
            // Genre title
            var lblGenreTitle = new Label();
            lblGenreTitle.Text = genreName;
            lblGenreTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblGenreTitle.ForeColor = Color.White;
            lblGenreTitle.AutoSize = false;
            lblGenreTitle.Width = flowAllMovies.Width - 40;
            lblGenreTitle.Height = 50;
            lblGenreTitle.Padding = new Padding(10, 10, 0, 0);
            flowAllMovies.Controls.Add(lblGenreTitle);

            // Horizontal scroll panel for movies
            var movieFlow = new FlowLayoutPanel();
            movieFlow.FlowDirection = FlowDirection.LeftToRight;
            movieFlow.WrapContents = false;
            movieFlow.AutoScroll = true;
            movieFlow.Width = flowAllMovies.Width - 40;
            movieFlow.Height = 320;
            movieFlow.BackColor = Color.Transparent;
            movieFlow.Padding = new Padding(10, 0, 10, 10);

            foreach (var movie in movies)
            {
                CreateMovieCard(movieFlow, movie);
            }

            flowAllMovies.Controls.Add(movieFlow);
        }

        private void CreateMovieCard(FlowLayoutPanel parent, Movie movie)
        {
            var card = new Panel();
            card.Width = 200;
            card.Height = 300;
            card.BackColor = Color.FromArgb(30, 30, 35);
            card.Cursor = Cursors.Hand;
            card.Margin = new Padding(5);
            card.Tag = movie;

            // Poster
            var pic = new PictureBox();
            pic.Width = 200;
            pic.Height = 250;
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.BackColor = Color.FromArgb(40, 40, 50);
            pic.Location = new Point(0, 0);
            card.Controls.Add(pic);

            // Load poster asynchronously
            LoadMovieCardPoster(pic, movie.PosterUrl);

            // Title overlay
            var lblTitle = new Label();
            lblTitle.Text = movie.Title;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.FromArgb(200, 20, 20, 25);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Width = 200;
            lblTitle.Height = 50;
            lblTitle.Location = new Point(0, 250);
            card.Controls.Add(lblTitle);

            // Rating badge
            var lblRating = new Label();
            lblRating.Text = $"★ {movie.Rating}";
            lblRating.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRating.ForeColor = Color.White;
            lblRating.BackColor = Color.FromArgb(220, 38, 38);
            lblRating.AutoSize = true;
            lblRating.Padding = new Padding(5, 2, 5, 2);
            lblRating.Location = new Point(5, 5);
            card.Controls.Add(lblRating);
            lblRating.BringToFront();

            // Watchlist indicator if movie is in watchlist
            if (_watchlist.Contains(movie.Id))
            {
                var lblInWatchlist = new Label();
                lblInWatchlist.Text = "✓";
                lblInWatchlist.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                lblInWatchlist.ForeColor = Color.White;
                lblInWatchlist.BackColor = Color.FromArgb(34, 197, 94);
                lblInWatchlist.Width = 30;
                lblInWatchlist.Height = 30;
                lblInWatchlist.TextAlign = ContentAlignment.MiddleCenter;
                lblInWatchlist.Location = new Point(165, 5);
                card.Controls.Add(lblInWatchlist);
                lblInWatchlist.BringToFront();
            }

            // Click event to show full details
            card.Click += (s, e) => ShowMovieDetails(movie);
            pic.Click += (s, e) => ShowMovieDetails(movie);
            lblTitle.Click += (s, e) => ShowMovieDetails(movie);

            parent.Controls.Add(card);
        }

        private async void LoadMovieCardPoster(PictureBox pic, string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) return;

                var imageBytes = await httpClient.GetByteArrayAsync(url);
                using (var ms = new System.IO.MemoryStream(imageBytes))
                {
                    var image = Image.FromStream(ms);
                    if (pic.InvokeRequired)
                    {
                        pic.Invoke(new Action(() => pic.Image = image));
                    }
                    else
                    {
                        pic.Image = image;
                    }
                }
            }
            catch
            {
                // Silently fail
            }
        }

        private void ShowMovieDetails(Movie movie)
        {
            _currentMovie = movie;
            flowAllMovies.Visible = false;
            btnBackToMain.Visible = true;
            DisplayMovie(movie);
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
            btnSuggest.Font = new Font("Segoe UI", 16F, FontStyle.Bold);

            lblHeader.ForeColor = Color.White;
            lblHeader.Font = new Font("Segoe UI", 32F, FontStyle.Bold);

            btnWatchlist.FlatStyle = FlatStyle.Flat;
            btnWatchlist.FlatAppearance.BorderSize = 0;
            btnWatchlist.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            lblWatchlistCount.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
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
            btn.Padding = new Padding(15, 8, 15, 8);
            btn.Cursor = Cursors.Hand;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            btn.Margin = new Padding(5);

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

                // Refresh the movie display
                DisplayAllMoviesByGenre();
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
            flowAllMovies.Visible = false;
            btnBackToMain.Visible = true;
            DisplayMovie(_currentMovie);
            AdjustLayoutForFullscreen();
        }

        private async void DisplayMovie(Movie movie)
        {
            pnlCard.Visible = true;
            lblTitle.Text = movie.Title;
            lblRating.Text = $"★ {movie.Rating}";
            lblMeta.Text = $"{movie.Year} • {movie.Runtime} • {movie.Director}";
            lblPlot.Text = movie.Plot;
            lblGenres.Text = string.Join(" • ", movie.Genre);

            // Adjust layout after making card visible
            AdjustLayoutForFullscreen();

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
            lblWatchlistCount.Text = $"Watchlist: {_watchlist.Count} movies";
            btnViewWatchlist.Text = $"📋 View Watchlist ({_watchlist.Count})";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AdjustLayoutForFullscreen();
        }

        // Add keyboard shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Application.Exit();
                return true;
            }
            else if (keyData == Keys.Back || keyData == Keys.B)
            {
                if (pnlCard.Visible || _isViewingWatchlist)
                {
                    GoBackToMain();
                    return true;
                }
            }
            else if (keyData == Keys.W && (Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                // Ctrl+W to view watchlist
                ShowWatchlist();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}