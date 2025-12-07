using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MovieSuggestionApp.MainForm;

namespace MovieSuggestionApp
{
    public partial class MainForm : Form
    {
        // Controls
        private TextBox txtSearch;
        private Button btnSearchOnline, btnSuggest, btnAddMovie, btnSettings, btnToggleTheme;
        private FlowLayoutPanel pnlMovies, pnlFavorites;
        private Panel pnlDetails, topBar;
        private PictureBox picPosterLarge;
        private Label lblTitle, lblGenre, lblYear, lblRating;
        private Button btnAddFavorite, btnRemoveFavorite;

        // Data
        private List<Movie> allMovies = new List<Movie>();
        private List<Movie> favoriteMovies = new List<Movie>();
        private Movie currentMovie = null;
        private readonly string placeholderPath = Path.Combine("Images", "placeholder.png");

        // UI / theme
        private bool darkMode = true;
        private AnimationManager animator;

        public MainForm()
        {
            InitializeComponent();
            animator = new AnimationManager(this);
            LoadSampleMovies();
            _ = PopulateCarouselAsync(); // fire and forget load posters
        }

        private void InitializeComponent()
        {
            // form
            this.Text = "Movie Suggestion App";
            this.ClientSize = new Size(1280, 820);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.DoubleBuffered = true;

            // top bar (separate to allow gradient/animation)
            topBar = new Panel()
            {
                Location = new Point(8, 8),
                Size = new Size(1264, 56),
                BackColor = Color.FromArgb(20, 20, 20)
            };
            this.Controls.Add(topBar);

            // Search box
            txtSearch = new TextBox()
            {
                Location = new Point(12, 14),
                Size = new Size(420, 28),
                ForeColor = Color.Gray,
                Text = "🔎 Search movies..."
            };
            txtSearch.GotFocus += (s, e) =>
            {
                if (txtSearch.Text.StartsWith("🔎")) { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; }
            };
            txtSearch.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔎 Search movies..."; txtSearch.ForeColor = Color.Gray; }
            };
            topBar.Controls.Add(txtSearch);

            // Buttons with simple icons (Unicode) so no external assets required
            btnSearchOnline = CreateTopButton("Search Online", "🔍", new Point(440, 12));
            btnSuggest = CreateTopButton("Suggest", "🎲", new Point(560, 12));
            btnAddMovie = CreateTopButton("Add", "＋", new Point(680, 12));
            btnSettings = CreateTopButton("Settings", "⚙", new Point(800, 12));
            btnToggleTheme = CreateTopButton("Dark Mode", "🌙", new Point(920, 12));

            topBar.Controls.AddRange(new Control[] { btnSearchOnline, btnSuggest, btnAddMovie, btnSettings, btnToggleTheme });

            // Main movie row
            pnlMovies = new FlowLayoutPanel()
            {
                Location = new Point(8, 72),
                Size = new Size(1264, 420),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(12)
            };
            this.Controls.Add(pnlMovies);

            // thin divider
            var divider = new Panel()
            {
                Location = new Point(8, 502),
                Size = new Size(1264, 6),
                BackColor = Color.FromArgb(60, Color.Gray)
            };
            this.Controls.Add(divider);

            // Details panel (bottom)
            pnlDetails = new Panel()
            {
                Location = new Point(8, 518),
                Size = new Size(1264, 280),
                BackColor = Color.FromArgb(40, 40, 40),
                Padding = new Padding(14)
            };
            this.Controls.Add(pnlDetails);

            picPosterLarge = new PictureBox()
            {
                Location = new Point(18, 20),
                Size = new Size(160, 240),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(60, 60, 60)
            };
            pnlDetails.Controls.Add(picPosterLarge);

            lblTitle = new Label()
            {
                Location = new Point(200, 20),
                Size = new Size(1040, 40),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Text = "Title"
            };
            pnlDetails.Controls.Add(lblTitle);

            lblGenre = new Label() { Location = new Point(200, 66), Size = new Size(600, 20), ForeColor = Color.LightGray, Text = "Genre" };
            pnlDetails.Controls.Add(lblGenre);

            lblYear = new Label() { Location = new Point(200, 92), Size = new Size(200, 20), ForeColor = Color.LightGray, Text = "Year" };
            pnlDetails.Controls.Add(lblYear);

            lblRating = new Label() { Location = new Point(200, 118), Size = new Size(200, 20), ForeColor = Color.LightGray, Text = "Rating" };
            pnlDetails.Controls.Add(lblRating);

            btnAddFavorite = new Button()
            {
                Text = "♥ Add to Favorites",
                Location = new Point(200, 160),
                Size = new Size(160, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 20, 60),
                ForeColor = Color.White
            };
            btnAddFavorite.FlatAppearance.BorderSize = 0;
            btnAddFavorite.Cursor = Cursors.Hand;
            pnlDetails.Controls.Add(btnAddFavorite);

            btnRemoveFavorite = new Button()
            {
                Text = "Remove Favorite",
                Location = new Point(372, 160),
                Size = new Size(160, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(90, 90, 90),
                ForeColor = Color.White
            };
            btnRemoveFavorite.FlatAppearance.BorderSize = 0;
            btnRemoveFavorite.Cursor = Cursors.Hand;
            pnlDetails.Controls.Add(btnRemoveFavorite);

            // Favorites carousel (bottom small posters)
            pnlFavorites = new FlowLayoutPanel()
            {
                Location = new Point(8, 806),
                Size = new Size(1264, 100),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Visible = true
            };
            // Note: form height is 820; adjust if you want favorites visible - we'll keep it off-screen by default
            // If you want visible, move pnlFavorites up and increase form height.

            // Events
            btnToggleTheme.Click += BtnToggleTheme_Click;
            btnSuggest.Click += BtnSuggest_Click;
            btnAddFavorite.Click += BtnAddFavorite_Click;
            btnRemoveFavorite.Click += BtnRemoveFavorite_Click;
            btnSearchOnline.Click += BtnSearchOnline_Click;
            btnAddMovie.Click += BtnAddMovie_Click;
            btnSettings.Click += BtnSettings_Click;

            // Initial theme
            ApplyTheme(darkMode ? Theme.Dark : Theme.Light, immediate: true);
        }

        // helper to create consistent top buttons
        private Button CreateTopButton(string text, string icon, Point location)
        {
            var b = new Button()
            {
                Text = $"{icon}  {text}",
                Location = location,
                Size = new Size(112, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            // hover
            b.MouseEnter += (s, e) => b.BackColor = Color.FromArgb(60, 60, 60);
            b.MouseLeave += (s, e) => b.BackColor = Color.FromArgb(40, 40, 40);
            return b;
        }

        // ----------- THEME / COLORS & ANIMATION -------------
        private enum Theme { Dark, Light }

        private void BtnToggleTheme_Click(object sender, EventArgs e)
        {
            darkMode = !darkMode;
            var target = darkMode ? Theme.Dark : Theme.Light;
            ApplyTheme(target, immediate: false);
            btnToggleTheme.Text = darkMode ? "🌙  Dark Mode" : "🔆  Light Mode";
        }

        private void ApplyTheme(Theme target, bool immediate)
        {
            // color palette
            Color bg = (target == Theme.Dark) ? Color.FromArgb(18, 18, 18) : Color.FromArgb(245, 245, 245);
            Color top = (target == Theme.Dark) ? Color.FromArgb(15, 15, 15) : Color.FromArgb(230, 230, 230);
            Color panel = (target == Theme.Dark) ? Color.FromArgb(30, 30, 30) : Color.FromArgb(240, 240, 240);
            Color text = (target == Theme.Dark) ? Color.White : Color.FromArgb(30, 30, 30);

            if (immediate)
            {
                this.BackColor = bg;
                topBar.BackColor = top;
                pnlDetails.BackColor = panel;
                lblTitle.ForeColor = text;
                lblGenre.ForeColor = (target == Theme.Dark) ? Color.LightGray : Color.Gray;
                lblYear.ForeColor = (target == Theme.Dark) ? Color.LightGray : Color.Gray;
                lblRating.ForeColor = (target == Theme.Dark) ? Color.LightGray : Color.Gray;
                foreach (Control c in pnlMovies.Controls) c.Invalidate();
            }
            else
            {
                // animate color transition
                animator.StartColorTransition(this, this.BackColor, bg, 300);
                animator.StartColorTransition(topBar, topBar.BackColor, top, 300);
                animator.StartColorTransition(pnlDetails, pnlDetails.BackColor, panel, 300);
                // small text color change without animation
                lblTitle.ForeColor = text;
                lblGenre.ForeColor = (target == Theme.Dark) ? Color.LightGray : Color.Gray;
                lblYear.ForeColor = (target == Theme.Dark) ? Color.LightGray : Color.Gray;
                lblRating.ForeColor = (target == Theme.Dark) ? Color.LightGray : Color.Gray;
            }
        }

        // ----------- LOAD & POPULATE POSTERS -------------
        private void LoadSampleMovies()
        {
            // Use direct image URLs (tmdb) or valid .jpg links
            allMovies.Add(new Movie("Interstellar", "2014", "Sci-Fi", 8.6, "https://image.tmdb.org/t/p/w500/rAiYTfKGqDCRIIqo664sY9XZIvQ.jpg"));
            allMovies.Add(new Movie("The Dark Knight", "2008", "Action", 9.0, "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg"));
            allMovies.Add(new Movie("Spirited Away", "2001", "Animation", 8.6, "https://image.tmdb.org/t/p/w500/dL11DBPcRhWWnJcFXl9A07MrqTI.jpg"));
            allMovies.Add(new Movie("The Shawshank Redemption", "1994", "Drama", 9.3, "https://image.tmdb.org/t/p/w500/q6y0Go1tsGEsmtFryDOJo3dEmqu.jpg"));
            allMovies.Add(new Movie("The Godfather", "1972", "Crime / Drama", 9.2, "https://image.tmdb.org/t/p/w500/iVZ3JAcAjmguGPnRNfWFOtLHOuY.jpg"));
            allMovies.Add(new Movie("Pulp Fiction", "1994", "Crime / Drama", 8.8, "https://image.tmdb.org/t/p/w500/d5iIlFn5s0ImszYzBPb8JPIfbXD.jpg"));
            allMovies.Add(new Movie("Fight Club", "1999", "Drama", 8.8, "https://image.tmdb.org/t/p/w500/bptfVGEQuv6vDTIMVCHjJ9Dz8PX.jpg"));
            allMovies.Add(new Movie("Forrest Gump", "1994", "Drama / Romance", 8.8, "https://image.tmdb.org/t/p/w500/saHP97rTPS5eLmrLQEcANmKrsFl.jpg"));
            // ... add more if you like
        }

        private async Task PopulateCarouselAsync()
        {
            pnlMovies.Controls.Clear();
            foreach (var movie in allMovies)
            {
                // create card
                var card = CreatePosterCard(movie);
                pnlMovies.Controls.Add(card);

                // asynchronously load the image and set (fade/zoom handled in card)
                _ = LoadAndSetPosterAsync(movie, card);
                // small delay to create staggered effect
                await Task.Delay(60);
            }
        }

        private Panel CreatePosterCard(Movie movie)
        {
            Panel panel = new Panel()
            {
                Size = new Size(180, 260),
                Margin = new Padding(8),
                BackColor = Color.Transparent,
                Tag = movie
            };

            var pic = new PictureBox()
            {
                Size = new Size(160, 220),
                Location = new Point(10, 6),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(60, 60, 60),
                Cursor = Cursors.Hand,
                Tag = movie
            };

            // title overlay
            var lbl = new Label()
            {
                Text = movie.Title,
                Location = new Point(10, 230),
                Size = new Size(160, 28),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(120, Color.Black)
            };

            // hover effects
            pic.MouseEnter += (s, e) =>
            {
                animator.StartScale(pic, 1.05f, 180);
                pic.BorderStyle = BorderStyle.FixedSingle;
            };
            pic.MouseLeave += (s, e) =>
            {
                animator.StartScale(pic, 1.0f, 160);
                pic.BorderStyle = BorderStyle.None;
            };

            pic.Click += (s, e) => ShowMovieDetails((Movie)pic.Tag);

            panel.Controls.Add(pic);
            panel.Controls.Add(lbl);
            return panel;
        }

        private async Task LoadAndSetPosterAsync(Movie movie, Panel card)
        {
            PictureBox pic = card.Controls[0] as PictureBox;
            Image img = null;
            try
            {
                img = await LoadImageFromUrlAsync(movie.PosterUrl);
            }
            catch { img = null; }

            if (img == null && File.Exists(placeholderPath))
                img = Image.FromFile(placeholderPath);
            else if (img == null)
                img = new Bitmap(160, 220);

            movie.PosterImage = img;

            // set image with fade
            animator.FadeInPicture(pic, img, 220);
        }

        private async Task<Image> LoadImageFromUrlAsync(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var data = await client.GetByteArrayAsync(url);
                    using (var ms = new MemoryStream(data))
                    {
                        return Image.FromStream(ms);
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        // ----------- DETAILS / FAVORITES / SUGGEST -------------
        private void ShowMovieDetails(Movie movie)
        {
            if (movie == null) return;
            currentMovie = movie;
            lblTitle.Text = movie.Title;
            lblGenre.Text = "Genre: " + movie.Genre;
            lblYear.Text = "Year: " + movie.Year;
            lblRating.Text = "Rating: " + movie.Rating.ToString("0.0");

            if (movie.PosterImage != null)
                animator.FadeInPicture(picPosterLarge, movie.PosterImage, 260);

            // optional: slide-in effect for details panel (subtle)
            animator.SlideInFromBottom(pnlDetails, 220);
        }

        private void BtnAddFavorite_Click(object sender, EventArgs e)
        {
            if (currentMovie == null) return;
            if (favoriteMovies.Contains(currentMovie)) return;
            favoriteMovies.Add(currentMovie);
            AddFavoriteCard(currentMovie);
        }

        private void BtnRemoveFavorite_Click(object sender, EventArgs e)
        {
            if (currentMovie == null) return;
            if (!favoriteMovies.Contains(currentMovie)) return;
            favoriteMovies.Remove(currentMovie);
            // remove from UI by matching Title (simple)
            foreach (Control c in pnlFavorites.Controls)
            {
                if (c.Tag is Movie m && m.Title == currentMovie.Title)
                {
                    pnlFavorites.Controls.Remove(c);
                    break;
                }
            }
        }

        private void AddFavoriteCard(Movie movie)
        {
            Panel p = new Panel() { Size = new Size(100, 140), Margin = new Padding(6), Tag = movie };
            PictureBox pb = new PictureBox() { Size = new Size(100, 140), SizeMode = PictureBoxSizeMode.Zoom, Image = movie.PosterImage, Cursor = Cursors.Hand };
            pb.Click += (s, e) => ShowMovieDetails(movie);
            p.Controls.Add(pb);
            pnlFavorites.Controls.Add(p);
            // animate small pop

        }

        private void BtnSuggest_Click(object sender, EventArgs e)
        {
            if (allMovies.Count == 0) return;
            var idx = new Random().Next(allMovies.Count);
            ShowMovieDetails(allMovies[idx]);
            // gentle UI hint
            using (var toast = new Form() { Size = new Size(320, 60), StartPosition = FormStartPosition.Manual, FormBorderStyle = FormBorderStyle.None, BackColor = Color.FromArgb(22, 22, 22), Opacity = 0.9 })
            {
                toast.Location = new Point(this.Left + 20, this.Top + 80);
                var l = new Label() { Text = $"Suggested: {allMovies[idx].Title}", ForeColor = Color.White, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
                toast.Controls.Add(l);
                toast.Show();
                var t = new Timer { Interval = 1400 };
                t.Tick += (s, ev) => { t.Stop(); toast.Close(); };
                t.Start();
                Application.DoEvents();
            }
        }

        // placeholder actions for other top buttons (extend as needed)
        private void BtnSearchOnline_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Search: {txtSearch.Text}", "Search");
        }
        private void BtnAddMovie_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add movie clicked (placeholder).", "Add Movie");
        }
        private void BtnSettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Settings clicked (placeholder).", "Settings");
        }

        // ----------------- Helper classes -------------------
        public class Movie
        {
            public string Title { get; set; }
            public string Year { get; set; }
            public string Genre { get; set; }
            public double Rating { get; set; }
            public string PosterUrl { get; set; }
            public Image PosterImage { get; set; }

            public Movie() { }
            public Movie(string t, string y, string g, double r, string u) { Title = t; Year = y; Genre = g; Rating = r; PosterUrl = u; }
        }

        // ----------------- Animation Manager -------------------
        // Lightweight animation helper using timers to do fades, slides, scales & color lerps
        public class AnimationManager
        {
            private Control owner;
            public AnimationManager(Control ownerControl) { owner = ownerControl; }

            // Fade-in a PictureBox image
            public void FadeInPicture(PictureBox pb, Image img, int durationMs = 250)
            {
                if (pb == null || img == null) return;
                pb.Image = new Bitmap(pb.Width, pb.Height);
                var bmp = new Bitmap(pb.Width, pb.Height);
                var gtemp = Graphics.FromImage(bmp); gtemp.Clear(pb.BackColor); gtemp.Dispose();
                pb.Image = bmp;

                float opacity = 0f;
                Timer t = new Timer { Interval = 15 };
                int steps = Math.Max(1, durationMs / t.Interval);
                float step = 1f / steps;

                t.Tick += (s, e) =>
                {
                    opacity += step;
                    if (opacity >= 1f) opacity = 1f;
                    // draw image with opacity onto bitmap
                    using (var tmp = new Bitmap(pb.Width, pb.Height))
                    using (var g = Graphics.FromImage(tmp))
                    {
                        g.Clear(pb.BackColor);
                        var cm = new System.Drawing.Imaging.ColorMatrix { Matrix33 = opacity };
                        var ia = new System.Drawing.Imaging.ImageAttributes();
                        ia.SetColorMatrix(cm, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);
                        g.DrawImage(img, new Rectangle(0, 0, pb.Width, pb.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                        // swap
                        pb.Image = (Image)tmp.Clone();
                    }

                    if (opacity >= 1f) t.Stop();
                };
                t.Start();
            }

            // Slide panel in from bottom
            public void SlideInFromBottom(Control panel, int durationMs = 220)
            {
                if (panel == null) return;
                int start = panel.Top + 30;
                int target = panel.Top;
                panel.Top = start;
                Timer t = new Timer { Interval = 12 };
                int steps = Math.Max(1, durationMs / t.Interval);
                float step = (start - target) / (float)steps;
                t.Tick += (s, e) =>
                {
                    panel.Top = Math.Max(target, (int)(panel.Top - Math.Ceiling(step)));
                    if (panel.Top <= target) t.Stop();
                };
                t.Start();
            }

            // Scale animation for control (uses transform by resizing - approximate)
            public void StartScale(Control ctrl, float scale, int durationMs = 160)
            {

            }

            // helper scale (quick, not high-fidelity transform)
            public void Scale(Control ctrl, float fromScale, float toScale, int durationMs = 200)
            {
                if (ctrl == null) return;
                var original = ctrl.Size;
                Timer t = new Timer { Interval = 12 };
                int steps = Math.Max(1, durationMs / t.Interval);
                float step = (toScale - fromScale) / steps;
                float current = fromScale;
                t.Tick += (s, e) =>
                {
                    current += step;
                    int w = (int)(original.Width * current);
                    int h = (int)(original.Height * current);
                    ctrl.Size = new Size(Math.Max(1, w), Math.Max(1, h));
                    if ((step > 0 && current >= toScale) || (step < 0 && current <= toScale)) t.Stop();
                };
                t.Start();
            }

            // convenience wrapper for quick scale


            // simple color transition on a control's BackColor
            public void StartColorTransition(Control ctrl, Color from, Color to, int durationMs = 240)
            {
                if (ctrl == null) return;
                Timer t = new Timer { Interval = 12 };
                int steps = Math.Max(1, durationMs / t.Interval);
                int step = 0;
                t.Tick += (s, e) =>
                {
                    step++;
                    float p = Math.Min(1f, step / (float)steps);
                    ctrl.BackColor = LerpColor(from, to, p);
                    if (p >= 1f) t.Stop();
                };
                t.Start();
            }

            private static Color LerpColor(Color a, Color b, float t)
            {
                int r = (int)(a.R + (b.R - a.R) * t);
                int g = (int)(a.G + (b.G - a.G) * t);
                int bl = (int)(a.B + (b.B - a.B) * t);
                int alpha = (int)(a.A + (b.A - a.A) * t);
                return Color.FromArgb(alpha, r, g, bl);
            }

            // helper to compute a percent-based scale starting value (approx)

        }
    }


namespace MoviePicker
    {
        public partial class Form1 : Form
        {
            private List<Movie> allMovies;

            public Form1()
            {
       
                LoadMovies();
                PopulateMovies(allMovies);
                SetupUI();
            }

            // Load sample movies
            private void LoadMovies()
            {
                allMovies = new List<Movie>()
            {
                new Movie { Title = "The Shawshank Redemption", Year = "1994", Genre = "Drama", Rating = 9.3, PosterUrl = "https://www.impawards.com/1994/shawshank_redemption_ver1.html" },
                new Movie { Title = "The Godfather", Year = "1972", Genre = "Crime / Drama", Rating = 9.2, PosterUrl = "https://www.impawards.com/1972/godfather_ver1.html" },
                new Movie { Title = "Inception", Year = "2010", Genre = "Sci-Fi / Action", Rating = 8.8, PosterUrl = "https://www.impawards.com/2010/inception_ver1.html" },
                new Movie { Title = "Pulp Fiction", Year = "1994", Genre = "Crime / Drama", Rating = 8.9, PosterUrl = "https://www.impawards.com/1994/pulp_fiction_ver1.html" },
                new Movie { Title = "Interstellar", Year = "2014", Genre = "Sci-Fi / Adventure", Rating = 8.6, PosterUrl = "https://www.impawards.com/2014/interstellar_ver1.html" },
                new Movie { Title = "The Dark Knight", Year = "2008", Genre = "Action / Crime", Rating = 9.0, PosterUrl = "https://www.impawards.com/2008/dark_knight.html" },
                new Movie { Title = "Forrest Gump", Year = "1994", Genre = "Drama / Romance", Rating = 8.8, PosterUrl = "https://www.impawards.com/1994/forrest_gump_ver1.html" },
                new Movie { Title = "Fight Club", Year = "1999", Genre = "Drama", Rating = 8.8, PosterUrl = "https://www.impawards.com/1999/fight_club_ver1.html" },
                new Movie { Title = "The Matrix", Year = "1999", Genre = "Sci-Fi / Action", Rating = 8.7, PosterUrl = "https://www.impawards.com/1999/matrix_ver1.html" },
                new Movie { Title = "Gladiator", Year = "2000", Genre = "Action / Drama", Rating = 8.5, PosterUrl = "https://www.impawards.com/2000/gladiator_ver1.html" }
            };
            }

            // Setup basic UI controls
            private TextBox txtGenre;
            private NumericUpDown numRating;
            private ComboBox cmbYear;
            private Button btnFilter;
            private FlowLayoutPanel flowPanel;

            private void SetupUI()
            {
                this.Text = "Movie Picker";
                this.Size = new Size(900, 600);

                Label lblGenre = new Label() { Text = "Genre:", Location = new Point(20, 20) };
                txtGenre = new TextBox() { Location = new Point(70, 18), Width = 120 };

                Label lblRating = new Label() { Text = "Min Rating:", Location = new Point(210, 20) };
                numRating = new NumericUpDown() { Location = new Point(290, 18), Minimum = 0, Maximum = 10, DecimalPlaces = 1, Increment = 0.1M, Value = 0 };

                Label lblYear = new Label() { Text = "Year:", Location = new Point(390, 20) };
                cmbYear = new ComboBox() { Location = new Point(440, 18), Width = 100 };
                cmbYear.Items.Add("All");
                cmbYear.Items.AddRange(allMovies.Select(m => m.Year).Distinct().ToArray());
                cmbYear.SelectedIndex = 0;

                btnFilter = new Button() { Text = "Filter", Location = new Point(560, 16) };
                btnFilter.Click += BtnFilter_Click;

                flowPanel = new FlowLayoutPanel() { Location = new Point(20, 60), Size = new Size(840, 480), AutoScroll = true };

                this.Controls.Add(lblGenre);
                this.Controls.Add(txtGenre);
                this.Controls.Add(lblRating);
                this.Controls.Add(numRating);
                this.Controls.Add(lblYear);
                this.Controls.Add(cmbYear);
                this.Controls.Add(btnFilter);
                this.Controls.Add(flowPanel);
            }

            private void BtnFilter_Click(object sender, EventArgs e)
            {
                string genre = txtGenre.Text.ToLower();
                double minRating = (double)numRating.Value;
                string year = cmbYear.SelectedItem.ToString();

                var filtered = allMovies.Where(m =>
                    m.Genre.ToLower().Contains(genre) &&
                    m.Rating >= minRating &&
                    (year == "All" || m.Year == year)
                ).ToList();

                PopulateMovies(filtered);
            }

            // Display movies in FlowLayoutPanel
            private void PopulateMovies(List<Movie> movies)
            {
                flowPanel.Controls.Clear();

                foreach (var movie in movies)
                {
                    Panel panel = new Panel() { Width = 150, Height = 250, Margin = new Padding(10), BorderStyle = BorderStyle.FixedSingle };

                    PictureBox pb = new PictureBox()
                    {
                        Size = new Size(140, 180),
                        Location = new Point(5, 5),
                        SizeMode = PictureBoxSizeMode.StretchImage
                    };

                    try
                    {
                        pb.Load(movie.PosterUrl); // load poster from URL
                    }
                    catch
                    {
                        pb.BackColor = Color.Gray;
                    }

                    Label lblTitle = new Label() { Text = movie.Title, Location = new Point(5, 190), Width = 140, Height = 20 };
                    Label lblYear = new Label() { Text = movie.Year, Location = new Point(5, 210), Width = 140, Height = 15 };
                    Label lblRating = new Label() { Text = $"⭐ {movie.Rating}", Location = new Point(5, 225), Width = 140, Height = 15 };

                    panel.Controls.Add(pb);
                    panel.Controls.Add(lblTitle);
                    panel.Controls.Add(lblYear);
                    panel.Controls.Add(lblRating);

                    flowPanel.Controls.Add(panel);
                }

            }
        }
    }
}

