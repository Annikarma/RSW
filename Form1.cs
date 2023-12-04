using System.Drawing.Text;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;

namespace RSW
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// remember the names of the controls using GUID
        /// </summary>
        List<String> myControlNamesGuid = new List<String>();
        List<WallStreetBets>? wsb = ReadAndDeserializeApiResponse();

        /// <summary>
        /// Default Wert: SortBy No Kommentare
        /// </summary>
        SortBy mySortBy = SortBy.NoOfComments;

        private string? headerCoiceText;

        public Form1()
        {
            InitializeComponent();
            GenerateGUIControls();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GenerateGUIControls();
        }

        private void sortWsb()
        {
            switch (mySortBy)
            {
                case SortBy.Ticker:
                    wsb = wsb.OrderBy(x => x.Ticker).ToList();
                    break;
                case SortBy.NoOfComments:
                    wsb = wsb.OrderByDescending(x => x.NoOfComments).ToList();  // descending
                    break;
                case SortBy.Score:
                    wsb = wsb.OrderBy(x => x.SentimentScore).ToList();
                    break;
                case SortBy.Sentiment:
                    wsb = wsb.OrderBy(x => x.Sentiment).ToList();
                    break;
                case SortBy.SentimentStier:
                    wsb = wsb.OrderByDescending(x => x.Sentiment).ToList();
                    break;
            }

        }

        /// <summary>
        /// First Step: Clean the window: RemoveControls()
        /// Second Step: Sort the List
        /// Third Step: Create Header Labels
        /// Fourth Step: Go through the List and create dynamic labels, pic, ProgressBar with values.
        /// The x-value increases with index i
        /// After each run, increase y-value. Repetition with Step 4 till the End of List
        /// </summary>
        private void GenerateGUIControls()
        {
            RemoveControls();
            sortWsb();

            int xPos = 50;
            int yPos = 100;
            int maxProgressBarValue = getMaxProgressBarValue(wsb);

            // create Header Labels    
            GenerateLabel(xPos, yPos, 1, "KÜRZEL", 80);
            GenerateLabel(xPos, yPos, 3, "SCORE", 90);
            GenerateLabel(xPos, yPos, 5, "STIER", 90);
            GenerateLabel(xPos, yPos, 7, "BÄR", 90);
            GenerateLabel(xPos, yPos, 9, "N° KOMMENTARE", 190);

            yPos += 50;

            foreach (WallStreetBets item in wsb)
            {
                GenerateLabel(xPos, yPos, 1, item.Ticker, 80);

                if (item.SentimentScore != null)
                {
                    GenerateLabel(xPos, yPos, 3, item.SentimentScore.ToString(), 90);
                }
                else
                {
                    GenerateLabel(xPos, yPos, 3, " - ", 90);
                }

                if (item.Sentiment != null)
                {
                    GenerateLabel(xPos, yPos, 5, item.Sentiment.ToString(), 90);   
                }
                else
                {
                    GenerateLabel(xPos, yPos, 5, " - ", 90);
                }

                if (item.SentimentScore <= 0)
                {

                    GeneratePicBox(xPos, yPos, 7, @"..\..\..\res\Bear.png", 90);
                }
                else
                {
                    GeneratePicBox(xPos, yPos, 7, @"..\..\..\res\Bull.png", 90);

                }

                GenerateLabel(xPos, yPos, 9, item.NoOfComments.ToString(), 90); 

                GenerateProgressBar(xPos, yPos, item.NoOfComments, maxProgressBarValue);

                yPos += 40;
            }

            pictureBox1.SendToBack();
        }

        private int getMaxProgressBarValue(List<WallStreetBets> wsb)
        {
            int maxValue = 0;
            foreach (WallStreetBets w in wsb)
            {

                if (w.NoOfComments > maxValue)
                {
                    maxValue = w.NoOfComments;
                }
            }
            return maxValue;
        }
        #region Region: Sichtbare Items auf der Oberlfäche generieren und mit Werten befüllen 


        /// <summary>
        /// Generate the Label
        /// </summary>
        /// <param name="xPos">Positionvalue for x</param>
        /// <param name="yPos">Positionvalue for y</param>
        /// <param name="i">index für die Reihenfolge der Labels</param>
        /// <param name="displayText">displayed value for Label</param>
        private void GenerateLabel(int xPos, int yPos, int i, string displayText, int width)
        {
            string name = System.Guid.NewGuid().ToString(); //GUID global unique identifier. GUID-Objekt kommt zurück
            Label lbl = new Label();
            lbl.Name = name;
            lbl.Location = new Point(xPos * i, yPos);
            lbl.Width = width;
            lbl.Height = 29;
            lbl.Visible = true;
            lbl.Text = displayText;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.Click += label1_Click;
            lbl.ForeColor = Color.White;
            lbl.BackColor = Color.FromArgb(255, 10, 10, 0);
            this.Controls.Add(lbl);
            this.myControlNamesGuid.Add(lbl.Name);
        }
        private void GeneratePicBox(int xPos, int yPos, int i, string path, int width)
        {
            string name = System.Guid.NewGuid().ToString();
            PictureBox pBox = new PictureBox();
            pBox.Name = name;
            pBox.Location = new Point(xPos * i, yPos);
            pBox.Width = width;
            pBox.Height = 20;
            pBox.Visible = true;
            pBox.SizeMode = PictureBoxSizeMode.Zoom;
            pBox.Image = Image.FromFile(path);
            this.Controls.Add(pBox);
            this.myControlNamesGuid.Add(pBox.Name);
        }

        private void GenerateHeaderLabel(int xPos, int yPos, int i, string displayText, int width)
        {
            string name = System.Guid.NewGuid().ToString();
            Label lbl = new Label();
            lbl.Name = name;
            lbl.Location = new Point(xPos * i, yPos);
            lbl.Width = width;
            lbl.Height = 29;
            lbl.Visible = true;
            lbl.Text = displayText;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Controls.Add(lbl);
            this.myControlNamesGuid.Add(lbl.Name);
        }

        private void GenerateProgressBar(int xPos, int yPos, int valueProgressBar, int maxValue)
        {
            string name = System.Guid.NewGuid().ToString();
            ProgressBar pB = new ProgressBar
            {
                Name = name,
                Location = new Point(xPos + 500, yPos),
                Width = 400,
                Height = 25,
                Visible = true,
                Maximum = maxValue,
                Value = valueProgressBar,
            };
            pB.BringToFront();
            this.Controls.Add(pB);
            this.myControlNamesGuid.Add(pB.Name);
        }

        private void GenerateTextBox(int xPos, int yPos)
        {
            for (int i = 1; i < 4; i++)
            {
                TextBox tb = new TextBox();
                string name = System.Guid.NewGuid().ToString();
                tb.Name = name;
                tb.Location = new Point(xPos * i, yPos);
                tb.Width = 50;
                tb.Height = 29;
                tb.Visible = true;
                tb.Text = "H";
                this.Controls.Add(tb);
                this.myControlNamesGuid.Add(tb.Name);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        #endregion

        /// <summary>
        /// list is retrieved.
        /// Api is requested. API values are processed
        /// </summary>
        /// <param name="dateTimeAsString">if empty then curently data. if date set then data corresponding to date</param>
        /// <returns>eturns deserialized list to continue working with</returns>
        public static List<WallStreetBets>? ReadAndDeserializeApiResponse(String dateTimeAsString = "")
        {

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("https://tradestie.com")
            };

            string path = "";
            if (dateTimeAsString != "")
            {
                path = "api/v1/apps/reddit?date=" + dateTimeAsString;
            }
            else
            {
                path = "api/v1/apps/reddit";
            }

            HttpResponseMessage response = client.GetAsync(path).Result;

            var content = response.Content.ReadAsStringAsync().Result;


            List<WallStreetBets>? post = JsonSerializer.Deserialize<List<WallStreetBets>>(content);

            return post;
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            wsb = ReadAndDeserializeApiResponse(GetPickedDate());
            GenerateGUIControls();
        }

        private string GetPickedDate()
        {
            return dateTimePicker1.Value.ToString("yyyy-MM-dd");
        }

        private void RemoveControls()
        {
            // Go through list instead of controls
            foreach (string name in myControlNamesGuid)
            {
                Control myControl = this.Controls.Find(name, true)[0];
                if (myControl != null)
                {
                    this.Controls.Remove(myControl);
                }
            }

            myControlNamesGuid = new List<String>();
        }

        /// <summary>
        /// Assign clicked value to enum value
        /// </summary>
        /// <param name="sender">label</param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            string headerCoiceText = ((Label)sender).Text;

            switch (headerCoiceText)
            {
                case "KÜRZEL":
                    mySortBy = SortBy.Ticker;
                    break;
                case "SCORE":
                    mySortBy = SortBy.Score;
                    break;

                case "STIER":
                    mySortBy = SortBy.SentimentStier;
                    break;

                case "BÄR":
                    mySortBy = SortBy.Sentiment;
                    break;

                case "N° KOMMENTARE":
                    mySortBy = SortBy.NoOfComments;
                    break;
            }

            GenerateGUIControls();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
