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
            this.lblHeader = new System.Windows.Forms.Label();
            this.flowGenres = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSuggest = new System.Windows.Forms.Button();
            this.pnlCard = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblRating = new System.Windows.Forms.Label();
            this.lblGenres = new System.Windows.Forms.Label();
            this.lblMeta = new System.Windows.Forms.Label();
            this.lblPlot = new System.Windows.Forms.Label();
            this.btnWatchlist = new System.Windows.Forms.Button();
            this.lblWatchlistCount = new System.Windows.Forms.Label();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();

            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = false;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeader.Location = new System.Drawing.Point(0, 20);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(800, 50);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "CINEMOJO";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // flowGenres
            // 
            this.flowGenres.Location = new System.Drawing.Point(50, 80);
            this.flowGenres.Name = "flowGenres";
            this.flowGenres.Size = new System.Drawing.Size(700, 60);
            this.flowGenres.TabIndex = 1;
            this.flowGenres.AutoScroll = true;
            this.flowGenres.WrapContents = false; // Horizontal scroll if needed
            this.flowGenres.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;

            // 
            // btnSuggest
            // 
            this.btnSuggest.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnSuggest.Location = new System.Drawing.Point(250, 160);
            this.btnSuggest.Name = "btnSuggest";
            this.btnSuggest.Size = new System.Drawing.Size(300, 50);
            this.btnSuggest.TabIndex = 2;
            this.btnSuggest.Text = "Suggest a Movie";
            this.btnSuggest.UseVisualStyleBackColor = true;
            this.btnSuggest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSuggest.Click += new System.EventHandler(this.btnSuggest_Click);

            // 
            // pnlCard
            // 
            this.pnlCard.Controls.Add(this.btnWatchlist);
            this.pnlCard.Controls.Add(this.lblPlot);
            this.pnlCard.Controls.Add(this.lblMeta);
            this.pnlCard.Controls.Add(this.lblGenres);
            this.pnlCard.Controls.Add(this.lblRating);
            this.pnlCard.Controls.Add(this.lblTitle);
            this.pnlCard.Location = new System.Drawing.Point(100, 230);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Size = new System.Drawing.Size(600, 320);
            this.pnlCard.TabIndex = 3;

            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(460, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Movie Title";

            // 
            // lblRating
            // 
            this.lblRating.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblRating.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRating.ForeColor = System.Drawing.Color.White;
            this.lblRating.Location = new System.Drawing.Point(500, 20);
            this.lblRating.Name = "lblRating";
            this.lblRating.Size = new System.Drawing.Size(80, 40);
            this.lblRating.TabIndex = 1;
            this.lblRating.Text = "8.5";
            this.lblRating.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblGenres
            // 
            this.lblGenres.AutoSize = true;
            this.lblGenres.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblGenres.ForeColor = System.Drawing.Color.Gray;
            this.lblGenres.Location = new System.Drawing.Point(25, 70);
            this.lblGenres.Name = "lblGenres";
            this.lblGenres.Size = new System.Drawing.Size(100, 19);
            this.lblGenres.TabIndex = 2;
            this.lblGenres.Text = "Action • Sci-Fi";

            // 
            // lblMeta
            // 
            this.lblMeta.AutoSize = true;
            this.lblMeta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMeta.Location = new System.Drawing.Point(25, 100);
            this.lblMeta.Name = "lblMeta";
            this.lblMeta.Size = new System.Drawing.Size(200, 19);
            this.lblMeta.TabIndex = 3;
            this.lblMeta.Text = "2010 • 148 min • Christopher Nolan";

            // 
            // lblPlot
            // 
            this.lblPlot.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblPlot.Location = new System.Drawing.Point(25, 140);
            this.lblPlot.Name = "lblPlot";
            this.lblPlot.Size = new System.Drawing.Size(550, 100);
            this.lblPlot.TabIndex = 4;
            this.lblPlot.Text = "Plot description goes here...";

            // 
            // btnWatchlist
            // 
            this.btnWatchlist.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnWatchlist.Location = new System.Drawing.Point(25, 250);
            this.btnWatchlist.Name = "btnWatchlist";
            this.btnWatchlist.Size = new System.Drawing.Size(550, 45);
            this.btnWatchlist.TabIndex = 5;
            this.btnWatchlist.Text = "Add to Watchlist";
            this.btnWatchlist.UseVisualStyleBackColor = true;
            this.btnWatchlist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWatchlist.Click += new System.EventHandler(this.btnWatchlist_Click);

            // 
            // lblWatchlistCount
            // 
            this.lblWatchlistCount.AutoSize = true;
            this.lblWatchlistCount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWatchlistCount.Location = new System.Drawing.Point(650, 580);
            this.lblWatchlistCount.Name = "lblWatchlistCount";
            this.lblWatchlistCount.Size = new System.Drawing.Size(83, 19);
            this.lblWatchlistCount.TabIndex = 4;
            this.lblWatchlistCount.Text = "Watchlist: 0";

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 620);
            this.Controls.Add(this.lblWatchlistCount);
            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.btnSuggest);
            this.Controls.Add(this.flowGenres);
            this.Controls.Add(this.lblHeader);
            this.Name = "MainForm";
            this.Text = "CineMojo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.FlowLayoutPanel flowGenres;
        private System.Windows.Forms.Button btnSuggest;
        private System.Windows.Forms.Panel pnlCard;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblRating;
        private System.Windows.Forms.Label lblGenres;
        private System.Windows.Forms.Label lblMeta;
        private System.Windows.Forms.Label lblPlot;
        private System.Windows.Forms.Button btnWatchlist;
        private System.Windows.Forms.Label lblWatchlistCount;
    }
}
