namespace RSW
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnUpdate = new Button();
            dateTimePicker1 = new DateTimePicker();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.LimeGreen;
            btnUpdate.Font = new Font("Roboto Cn", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnUpdate.ForeColor = SystemColors.ButtonHighlight;
            btnUpdate.Location = new Point(307, 15);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(115, 30);
            btnUpdate.TabIndex = 2;
            btnUpdate.Text = "LADEN";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CalendarFont = new Font("Roboto Lt", 9F, FontStyle.Bold, GraphicsUnit.Point);
            dateTimePicker1.CalendarMonthBackground = SystemColors.Menu;
            dateTimePicker1.Font = new Font("Roboto Lt", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dateTimePicker1.Location = new Point(49, 16);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(242, 26);
            dateTimePicker1.TabIndex = 1;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.BullImage;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.ErrorImage = null;
            pictureBox1.Location = new Point(3, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1612, 896);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.Black;
            ClientSize = new Size(1616, 913);
            Controls.Add(btnUpdate);
            Controls.Add(dateTimePicker1);
            Controls.Add(pictureBox1);
            MaximumSize = new Size(1634, 960);
            MinimumSize = new Size(1634, 960);
            Name = "Form1";
            Text = "WallStreetBets";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblNumberComment;
        private TextBox tbnumberOfComment;
        private TextBox tbSentiment;
        private Label lblSentiment;
        private ProgressBar progBarNumberOfComments;
        private TextBox tbTicker;
        private Label lblTicker;
        private DataGridView dataGridView1;
        private Label d;
        private PictureBox pictureBox1;
        private Button btnUpdate;
        private DateTimePicker dateTimePicker1;
    }
}