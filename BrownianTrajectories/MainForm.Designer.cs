namespace BrownianTrajectories
{
  partial class MainForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.моделированиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.залипалкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize) (this.chart1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 31);
            this.chart1.Name = "chart1";
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.EmptyPointStyle.Color = System.Drawing.Color.Black;
            series1.LabelBackColor = System.Drawing.SystemColors.ActiveBorder;
            series1.LabelBorderColor = System.Drawing.SystemColors.ActiveBorder;
            series1.LabelBorderWidth = 3;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(645, 433);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(663, 403);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 61);
            this.button1.TabIndex = 1;
            this.button1.Text = "Запуск ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.моделированиеToolStripMenuItem, this.залипалкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(810, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // моделированиеToolStripMenuItem
            // 
            this.моделированиеToolStripMenuItem.Name = "моделированиеToolStripMenuItem";
            this.моделированиеToolStripMenuItem.Size = new System.Drawing.Size(136, 24);
            this.моделированиеToolStripMenuItem.Text = "Моделирование";
            this.моделированиеToolStripMenuItem.Click += new System.EventHandler(this.моделированиеToolStripMenuItem_Click);
            // 
            // залипалкаToolStripMenuItem
            // 
            this.залипалкаToolStripMenuItem.Name = "залипалкаToolStripMenuItem";
            this.залипалкаToolStripMenuItem.Size = new System.Drawing.Size(94, 24);
            this.залипалкаToolStripMenuItem.Text = "Залипалка";
            this.залипалкаToolStripMenuItem.Click += new System.EventHandler(this.залипалкаToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(810, 475);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize) (this.chart1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem моделированиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem залипалкаToolStripMenuItem;

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;

        #endregion
    }
}