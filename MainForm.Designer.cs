using System;
using System.Windows.Forms;
using System.Drawing;

namespace MovieSuggestionApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblHeader = new Label();
            flowGenres = new FlowLayoutPanel();
            btnSuggest = new Button();
            pnlCard = new Panel();
            picPoster = new PictureBox();
            pnlDetails = new Panel();
            btnWatchlist = new Button();
            lblPlot = new Label();
            lblMeta = new Label();
            lblGenres = new Label();
            lblRating = new Label();
            lblTitle = new Label();
            lblWatchlistCount = new Label();
            pnlCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPoster).BeginInit();
            pnlDetails.SuspendLayout();
            SuspendLayout();
           
            lblHeader.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblHeader.Location = new Point(0, 27);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(1029, 67);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "CINE  SMU";
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
           
            flowGenres.AutoScroll = true;
            flowGenres.Location = new Point(114, 107);
            flowGenres.Margin = new Padding(3, 4, 3, 4);
            flowGenres.Name = "flowGenres";
            flowGenres.Size = new Size(800, 80);
            flowGenres.TabIndex = 1;
            flowGenres.WrapContents = false;
           
            btnSuggest.Cursor = Cursors.Hand;
            btnSuggest.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnSuggest.Location = new Point(343, 213);
            btnSuggest.Margin = new Padding(3, 4, 3, 4);
            btnSuggest.Name = "btnSuggest";
            btnSuggest.Size = new Size(343, 67);
            btnSuggest.TabIndex = 2;
            btnSuggest.Text = "Suggest a Movie";
            btnSuggest.UseVisualStyleBackColor = true;
            btnSuggest.Click += btnSuggest_Click;
           
            pnlCard.Controls.Add(picPoster);
            pnlCard.Controls.Add(pnlDetails);
            pnlCard.Location = new Point(57, 307);
            pnlCard.Margin = new Padding(3, 4, 3, 4);
            pnlCard.Name = "pnlCard";
            pnlCard.Size = new Size(914, 480);
            pnlCard.TabIndex = 3;
           
            picPoster.Location = new Point(23, 27);
            picPoster.Margin = new Padding(3, 4, 3, 4);
            picPoster.Name = "picPoster";
            picPoster.Size = new Size(229, 400);
            picPoster.SizeMode = PictureBoxSizeMode.Zoom;
            picPoster.TabIndex = 0;
            picPoster.TabStop = false;
           
            pnlDetails.Controls.Add(btnWatchlist);
            pnlDetails.Controls.Add(lblPlot);
            pnlDetails.Controls.Add(lblMeta);
            pnlDetails.Controls.Add(lblGenres);
            pnlDetails.Controls.Add(lblRating);
            pnlDetails.Controls.Add(lblTitle);
            pnlDetails.Location = new Point(274, 27);
            pnlDetails.Margin = new Padding(3, 4, 3, 4);
            pnlDetails.Name = "pnlDetails";
            pnlDetails.Size = new Size(617, 427);
            pnlDetails.TabIndex = 1;
           
            btnWatchlist.Cursor = Cursors.Hand;
            btnWatchlist.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnWatchlist.Location = new Point(17, 340);
            btnWatchlist.Margin = new Padding(3, 4, 3, 4);
            btnWatchlist.Name = "btnWatchlist";
            btnWatchlist.Size = new Size(583, 60);
            btnWatchlist.TabIndex = 5;
            btnWatchlist.Text = "+ Add to Watchlist";
            btnWatchlist.UseVisualStyleBackColor = true;
            btnWatchlist.Click += btnWatchlist_Click;
            
            lblPlot.Font = new Font("Segoe UI", 10F);
            lblPlot.Location = new Point(17, 160);
            lblPlot.Name = "lblPlot";
            lblPlot.Size = new Size(583, 160);
            lblPlot.TabIndex = 4;
            lblPlot.Text = "Plot description goes here...";
           
            lblMeta.AutoSize = true;
            lblMeta.Font = new Font("Segoe UI", 10F);
            lblMeta.Location = new Point(17, 107);
            lblMeta.Name = "lblMeta";
            lblMeta.Size = new Size(281, 23);
            lblMeta.TabIndex = 3;
            lblMeta.Text = "2010 • 148 min • Christopher Nolan";
           
            lblGenres.AutoSize = true;
            lblGenres.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblGenres.ForeColor = Color.Gray;
            lblGenres.Location = new Point(17, 67);
            lblGenres.Name = "lblGenres";
            lblGenres.Size = new Size(112, 23);
            lblGenres.TabIndex = 2;
            lblGenres.Text = "Action • Sci-Fi";
           
            lblRating.BackColor = Color.FromArgb(220, 38, 38);
            lblRating.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblRating.ForeColor = Color.White;
            lblRating.Location = new Point(503, 0);
            lblRating.Name = "lblRating";
            lblRating.Size = new Size(91, 53);
            lblRating.TabIndex = 1;
            lblRating.Text = "8.5";
            lblRating.TextAlign = ContentAlignment.MiddleCenter;
            
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(11, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(457, 53);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Movie Title";
            
            lblWatchlistCount.AutoSize = true;
            lblWatchlistCount.Font = new Font("Segoe UI", 10F);
            lblWatchlistCount.Location = new Point(857, 813);
            lblWatchlistCount.Name = "lblWatchlistCount";
            lblWatchlistCount.Size = new Size(97, 23);
            lblWatchlistCount.TabIndex = 4;
            lblWatchlistCount.Text = "Watchlist: 0";
            
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1029, 867);
            Controls.Add(lblWatchlistCount);
            Controls.Add(pnlCard);
            Controls.Add(btnSuggest);
            Controls.Add(flowGenres);
            Controls.Add(lblHeader);
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cine SMU";
            Load += MainForm_Load;
            pnlCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picPoster).EndInit();
            pnlDetails.ResumeLayout(false);
            pnlDetails.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.FlowLayoutPanel flowGenres;
        private System.Windows.Forms.Button btnSuggest;
        private System.Windows.Forms.Panel pnlCard;
        private System.Windows.Forms.PictureBox picPoster;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblRating;
        private System.Windows.Forms.Label lblGenres;
        private System.Windows.Forms.Label lblMeta;
        private System.Windows.Forms.Label lblPlot;
        private System.Windows.Forms.Button btnWatchlist;
        private System.Windows.Forms.Label lblWatchlistCount;
    }
}
