namespace MovieSuggestionApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearchOnline = new System.Windows.Forms.Button();
            this.btnSuggest = new System.Windows.Forms.Button();
            this.btnAddMovie = new System.Windows.Forms.Button();
            this.btnToggleTheme = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.lstMovies = new System.Windows.Forms.ListBox();
            this.picPoster = new System.Windows.Forms.PictureBox();
            this.lblDetails = new System.Windows.Forms.Label();
            this.btnAddFavorite = new System.Windows.Forms.Button();
            this.btnRemoveFavorites = new System.Windows.Forms.Button();
            this.lstFavorites = new System.Windows.Forms.ListBox();
            this.pnlRow = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.picPoster)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(722, 375);
            this.txtSearch.Multiline = true;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(535, 80);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnSearchOnline
            // 
            this.btnSearchOnline.Location = new System.Drawing.Point(47, 154);
            this.btnSearchOnline.Name = "btnSearchOnline";
            this.btnSearchOnline.Size = new System.Drawing.Size(250, 123);
            this.btnSearchOnline.TabIndex = 1;
            this.btnSearchOnline.Text = "Search Online";
            this.btnSearchOnline.UseVisualStyleBackColor = true;
            // 
            // btnSuggest
            // 
            this.btnSuggest.Location = new System.Drawing.Point(47, 332);
            this.btnSuggest.Name = "btnSuggest";
            this.btnSuggest.Size = new System.Drawing.Size(250, 123);
            this.btnSuggest.TabIndex = 2;
            this.btnSuggest.Text = "Suggest";
            this.btnSuggest.UseVisualStyleBackColor = true;
            this.btnSuggest.Click += new System.EventHandler(this.btnSuggest_Click);
            // 
            // btnAddMovie
            // 
            this.btnAddMovie.Location = new System.Drawing.Point(47, 522);
            this.btnAddMovie.Name = "btnAddMovie";
            this.btnAddMovie.Size = new System.Drawing.Size(250, 123);
            this.btnAddMovie.TabIndex = 3;
            this.btnAddMovie.Text = "Add Movie";
            this.btnAddMovie.UseVisualStyleBackColor = true;
            // 
            // btnToggleTheme
            // 
            this.btnToggleTheme.Location = new System.Drawing.Point(2683, 25);
            this.btnToggleTheme.Name = "btnToggleTheme";
            this.btnToggleTheme.Size = new System.Drawing.Size(250, 123);
            this.btnToggleTheme.TabIndex = 4;
            this.btnToggleTheme.Text = "THEME";
            this.btnToggleTheme.UseVisualStyleBackColor = true;
            this.btnToggleTheme.Click += new System.EventHandler(this.btnToggleTheme_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(2401, 25);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(250, 123);
            this.btnSettings.TabIndex = 5;
            this.btnSettings.Text = "SETTINGS";
            this.btnSettings.UseVisualStyleBackColor = true;
            // 
            // lstMovies
            // 
            this.lstMovies.FormattingEnabled = true;
            this.lstMovies.ItemHeight = 31;
            this.lstMovies.Location = new System.Drawing.Point(1449, 222);
            this.lstMovies.Name = "lstMovies";
            this.lstMovies.Size = new System.Drawing.Size(393, 128);
            this.lstMovies.TabIndex = 6;
            // 
            // picPoster
            // 
            this.picPoster.Location = new System.Drawing.Point(558, 12);
            this.picPoster.Name = "picPoster";
            this.picPoster.Size = new System.Drawing.Size(866, 338);
            this.picPoster.TabIndex = 7;
            this.picPoster.TabStop = false;
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Location = new System.Drawing.Point(1511, 445);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(92, 32);
            this.lblDetails.TabIndex = 8;
            this.lblDetails.Text = "label1";
            // 
            // btnAddFavorite
            // 
            this.btnAddFavorite.Location = new System.Drawing.Point(1996, 265);
            this.btnAddFavorite.Name = "btnAddFavorite";
            this.btnAddFavorite.Size = new System.Drawing.Size(226, 136);
            this.btnAddFavorite.TabIndex = 9;
            this.btnAddFavorite.Text = "FAVORITE";
            this.btnAddFavorite.UseVisualStyleBackColor = true;
            this.btnAddFavorite.Click += new System.EventHandler(this.btnAddFavorite_Click);
            // 
            // btnRemoveFavorites
            // 
            this.btnRemoveFavorites.Location = new System.Drawing.Point(2256, 265);
            this.btnRemoveFavorites.Name = "btnRemoveFavorites";
            this.btnRemoveFavorites.Size = new System.Drawing.Size(226, 136);
            this.btnRemoveFavorites.TabIndex = 10;
            this.btnRemoveFavorites.Text = "REMOVE FAVORITE";
            this.btnRemoveFavorites.UseVisualStyleBackColor = true;
            // 
            // lstFavorites
            // 
            this.lstFavorites.FormattingEnabled = true;
            this.lstFavorites.ItemHeight = 31;
            this.lstFavorites.Location = new System.Drawing.Point(2018, 154);
            this.lstFavorites.Name = "lstFavorites";
            this.lstFavorites.Size = new System.Drawing.Size(389, 97);
            this.lstFavorites.TabIndex = 11;
            // 
            // pnlRow
            // 
            this.pnlRow.Location = new System.Drawing.Point(412, 564);
            this.pnlRow.Name = "pnlRow";
            this.pnlRow.Size = new System.Drawing.Size(1250, 496);
            this.pnlRow.TabIndex = 12;
            this.pnlRow.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlRow_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2945, 1684);
            this.Controls.Add(this.pnlRow);
            this.Controls.Add(this.lstFavorites);
            this.Controls.Add(this.btnRemoveFavorites);
            this.Controls.Add(this.btnAddFavorite);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.picPoster);
            this.Controls.Add(this.lstMovies);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnToggleTheme);
            this.Controls.Add(this.btnAddMovie);
            this.Controls.Add(this.btnSuggest);
            this.Controls.Add(this.btnSearchOnline);
            this.Controls.Add(this.txtSearch);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picPoster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearchOnline;
        private System.Windows.Forms.Button btnSuggest;
        private System.Windows.Forms.Button btnAddMovie;
        private System.Windows.Forms.Button btnToggleTheme;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.ListBox lstMovies;
        private System.Windows.Forms.PictureBox picPoster;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Button btnAddFavorite;
        private System.Windows.Forms.Button btnRemoveFavorites;
        private System.Windows.Forms.ListBox lstFavorites;
        private System.Windows.Forms.FlowLayoutPanel pnlRow;
    }

}

