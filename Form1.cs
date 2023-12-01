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
        // globale Liste - vorsicht vor unübersichtlichkeit. kann halt jeder drauf zugreifen
        List<String> myControlNamesGuid = new List<String>();  // wir merken uns die Namen der Controls mithilfe von GUID
        List<WallStreetBets>? wsb = ReadAndDeserializeApiResponse(); //? default wert ist in dieser liste
        SortBy mySortBy = SortBy.NoOfComments;  // Default Wert

        private string? headerCoiceText; // globale Eigenschaft

        public Form1()  // Programm startet mit akutellen Werten 
        {
            InitializeComponent();

            //List<WallStreetBets> wsb = ReadAndDeserializeApiResponse(); //liste abrufen deserialisiert
            //wsb = ReadAndDeserializeApiResponse(); //liste abrufen deserialisiert

            // wsb = wsb.OrderBy(x => x.Ticker).ToList();          

            GenerateGUIControls(); // liste wurde übergeben an die Methode
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GenerateGUIControls();
        }

        //auswertung nach dem enum-wert
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
        /// parameter: Elemente werden ausgelesen um die Elemente/Bilder zu ersetllen
        /// </summary>
        /// <param name="wsb"></param>
       //private void GenerateGUIControls(List<WallStreetBets> wsb) // vorher diese Liste reingegeben!Liste wird hier verwendet
        private void GenerateGUIControls()  // automatisch die globale Liste
        {
            RemoveControls();
            //Liste sortieren VOR der Anzeige!!
            sortWsb();


            /*
                         * Liste durchlaufen
                         * in jeder Liste einen Control/Label hinzufügen
                         * startposition 15 15
                         * das erste control 100 p breit, das 2.. mit +
                         * nach unten feste höhe 75 pixel
                         * nach jedem Durchlauf mit y runter gehen (x ist fest)
                         * 
                         */
            int xPos = 50;
            int yPos = 100;
            int maxProgressBarValue = getMaxProgressBarValue(wsb);
            // Hier die Werte ändern

            // create Header Labels    
            GenerateLabel(xPos, yPos, 1, "KÜRZEL", 80);
            GenerateLabel(xPos, yPos, 3, "SCORE", 90); //Anzahl Kommentare
            GenerateLabel(xPos, yPos, 5, "STIER", 90); // Label erzeugen Bulli/Bear
            GenerateLabel(xPos, yPos, 7, "BÄR", 90); // Label erzeugen Bulli/Bear
            GenerateLabel(xPos, yPos, 9, "N° KOMMENTARE", 190); //   Sentiment-Score  i: 1 benamte Parameter

            // create labels/pic/ProgressBar with values // mit wsb werden die Controls generiert
            yPos += 50;
            foreach (WallStreetBets item in wsb) //hier wird die globale liste verwendet. Liste wsb als Grundlage um die labels zu erstellen
            {
                // Kürzel
                GenerateLabel(xPos, yPos, 1, item.Ticker, 80);

                // SentimentScore
                if (item.SentimentScore != null)
                {
                    GenerateLabel(xPos, yPos, 3, item.SentimentScore.ToString(), 90);
                }
                else
                {
                    GenerateLabel(xPos, yPos, 3, " - ", 90);
                }

                // Label(String) erzeugen STIER | BÄR
                if (item.Sentiment != null)
                {
                    GenerateLabel(xPos, yPos, 5, item.Sentiment.ToString(), 90);   // xPos + 50 muss nicht, da sich die GenerateLabel schon darum kümmert mit xPos * i
                }
                else
                {
                    GenerateLabel(xPos, yPos, 5, " - ", 90);
                }

                // Pic-Inhalt füllen
                if (item.SentimentScore <= 0)
                {

                    GeneratePicBox(xPos, yPos, 7, @"..\..\..\res\Bear.png", 90);
                }
                else
                {
                    GeneratePicBox(xPos, yPos, 7, @"..\..\..\res\Bull.png", 90);

                }

                //Anzahl Kommentare
                GenerateLabel(xPos, yPos, 9, item.NoOfComments.ToString(), 90); // Label erzeugt und mit aktuellem Wert der wsb gefüllt 

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
        /// Generiert das Label
        /// </summary>
        /// <param name="xPos">Positionswert für x</param>
        /// <param name="yPos">Positionswert für y</param>
        /// <param name="i">index für die Reihenfolge der Labels</param>
        /// <param name="displayText">angezeigter Wert IM Label</param>
        private void GenerateLabel(int xPos, int yPos, int i, string displayText, int width)
        {
            string name = System.Guid.NewGuid().ToString(); //GUID global unique identifier. GUID-Objekt kommt zurück
            Label lbl = new Label();
            lbl.Name = name;
            lbl.Location = new Point(xPos * i, yPos);
            lbl.Width = width;
            // lbl.Font.Size = new Size("9pt");   ????
            lbl.Height = 29;
            lbl.Visible = true;
            lbl.Text = displayText; //wert wird hier übergeben
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.Click += label1_Click;
            lbl.ForeColor = Color.White; // ForeColor this HIer die from
            lbl.BackColor = Color.FromArgb(255, 10, 10, 0);
            this.Controls.Add(lbl); // label wird der Form hinzugefügt
            this.myControlNamesGuid.Add(lbl.Name); // hier wird der Name der Liste hinzugefügt
            // unsere label haben keine Namen
        }
        private void GeneratePicBox(int xPos, int yPos, int i, string path, int width)
        {
            string name = System.Guid.NewGuid().ToString(); //GUID global unique identifier. GUID-Objekt kommt zurück    
            PictureBox pBox = new PictureBox();
            pBox.Name = name;
            pBox.Location = new Point(xPos * i, yPos);
            pBox.Width = width;
            pBox.Height = 20;
            pBox.Visible = true;
            pBox.SizeMode = PictureBoxSizeMode.Zoom;
            pBox.Image = Image.FromFile(path);
            //ForeColor = Color.White;
            //BackColor = Color.Black;
            this.Controls.Add(pBox);
            this.myControlNamesGuid.Add(pBox.Name); // hier wird der Name der Liste hinzugefügt
        }

        private void GenerateHeaderLabel(int xPos, int yPos, int i, string displayText, int width)
        {
            string name = System.Guid.NewGuid().ToString(); //GUID global unique identifier. GUID-Objekt kommt zurück
            Label lbl = new Label();
            lbl.Name = name;
            lbl.Location = new Point(xPos * i, yPos);
            lbl.Width = width;
            lbl.Height = 29;
            lbl.Visible = true;
            lbl.Text = displayText; //wert wird hier übergeben
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Controls.Add(lbl);
            this.myControlNamesGuid.Add(lbl.Name); // hier wird der Name der Liste hinzugefügt
        }

        private void GenerateProgressBar(int xPos, int yPos, int valueProgressBar, int maxValue) // read Only
        {
            string name = System.Guid.NewGuid().ToString(); //GUID global unique identifier. GUID-Objekt kommt zurück
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
            this.Controls.Add(pB);  // this ist hier das Objekt, in welchem Kontext ich mich bewege. Hier Forms!!!
            this.myControlNamesGuid.Add(pB.Name); // hier wird der Name der Liste hinzugefügt
        }

        private void GenerateTextBox(int xPos, int yPos)
        {
            for (int i = 1; i < 4; i++)
            {
                TextBox tb = new TextBox();
                string name = System.Guid.NewGuid().ToString(); //GUID global unique identifier. GUID-Objekt kommt zurück
                tb.Name = name;
                tb.Location = new Point(xPos * i, yPos);
                tb.Width = 50;
                tb.Height = 29;
                tb.Visible = true;
                tb.Text = "H";
                this.Controls.Add(tb);  // this ist hier das Objekt, in welchem Kontext ich mich bewege. Hier Forms!!!
                this.myControlNamesGuid.Add(tb.Name); // hier wird der Name der Liste hinzugefügt

            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #endregion

        /// <summary>
        /// Liste wird abgerufen.
        /// Api wird angefragt. Werte der API werden verarbeitet
        /// </summary>
        /// <param name="dateTimeAsString">wenn leer dann aktuellste Daten, wenn Datum gesetzt, dann Daten entsprechden datum</param>
        /// <returns>gibt deserialisierte Liste zurück, mit der weitergearbeitet wird</returns>
        public static List<WallStreetBets>? ReadAndDeserializeApiResponse(String dateTimeAsString = "")  // RückgabeTyp !!! Liste   !!! datum mit übergeben nullable? default ist leer
        {
            //Hier wird eine Instanz der HttpClient-Klasse erstellt und mit einer Basisadresse konfiguriert.
            //Die Basisadresse wird auf "https://uselessfacts.jsph.pl" <-- Beispiel gesetzt.  //https://tradestie.com/api/v1/apps/reddit?date=2022-04-03
            //Diese Adresse wird als Grundlage für alle nachfolgenden Anfragen verwendet.
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("https://tradestie.com")
            };

            //Mit der GetAsync-Methode wird eine asynchrone HTTP-GET-Anfrage an die angegebene Ressource gesendet.
            //In diesem Fall wird die Ressource "/api/v2/facts/random?language=de" auf der Basisadresse angefordert.
            //Die Result-Eigenschaft wird verwendet, um auf das Ergebnis zu warten und
            //die Antwort in der Variable response zu speichern.
            // BaseAdresse wird ergänzt mit dem Bereich der DB ???

            string path = "";
            if (dateTimeAsString != "")
            {
                path = "api/v1/apps/reddit?date=" + dateTimeAsString;  // aus response kommt text zurück  http.response.massage. kein string
            }
            else
            {
                path = "api/v1/apps/reddit";
            }

            HttpResponseMessage response = client.GetAsync(path).Result; //Header gibt das AKTUELLE Datum zurück. Header hat NICHTS mit CONTENT zu tun

            //Der Inhalt der Antwort wird asynchron als Zeichenfolge gelesen.
            //Das .Result wird erneut verwendet, um auf das Ergebnis zu warten
            //und die Zeichenfolge in der Variable content zu speichern.
            var content = response.Content.ReadAsStringAsync().Result;  // Rückgabe ist eine Liste (das ist aber API-spezifisch). deswegen Liste zur verfügung stellen

            //Der Inhalt der Antwort wird dann mithilfe von JsonSerializer
            //in ein Objekt des Typs UselessFact deserialisiert.
            //WallStreetBets ist eine von uns angelegte Klasse,
            //die die Struktur der empfangenen JSON-Daten repräsentiert.

            // wir gekommen nicht nur 1 Element zurück sondern eine Liste. Typ Liste von Objekten
            List<WallStreetBets>? post = JsonSerializer.Deserialize<List<WallStreetBets>>(content);  //vorher diese Liste reingegeben! // hier wird die Liste mit Magie über Deserializer gefüllt. durch ANnotateion in Klasse .... liste gefüllt

            return post;  // deserialisierte Liste aus den Json-Daten, die abgerufen wurden
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            wsb = ReadAndDeserializeApiResponse(GetPickedDate());
            GenerateGUIControls(); // das control generieren
        }

        private string GetPickedDate()
        {
            return dateTimePicker1.Value.ToString("yyyy-MM-dd");
        }

        private void RemoveControls()
        {
            // Liste durchgehen anstelle von controls.
            foreach (string name in myControlNamesGuid) // hilfsliste wird durchgegangen, um damit die Controls aus der FORM zu löschen
                                                        // in der HIlfsliste sind aber noch alle vorhanden
            {
                Control myControl = this.Controls.Find(name, true)[0];  // true: soll alle durchsuchen. bekommen nun ein cotnrol zurück
                if (myControl != null)
                {
                    this.Controls.Remove(myControl);  // Controls sind alle die es gibt
                }
                //myControlNamesGuid.Remove(name); //  HIlfsliste leeren. jeden Eintrag einzeln aus der Liste entfernen - IN DER SCLEIFE
                //alternativ neue Liste erstellen              

            }
            //ganze liste neu erstellen
            //List<String> myControlNamesGuid = new List<String>();  // diese lsite gibt es schon. deswgen überschreiben in nächster zeilte
            myControlNamesGuid = new List<String>();  //hier alternativ neue Liste erstellen 


            //foreach (Control control in this.Controls) // Controls ist eine feste Eigenschaft der Form
            //{
            //    if (control.Name == "button1" || control.Name == "dateTimePicker1")
            //    {
            //        continue;
            //    }
            //    this.Controls.Remove(control);
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RemoveControls();

        }

        private void label1_Click(object sender, EventArgs e) //MUSS void bleiben, da diese sonst nicht mehr an Button gebunden
        {
            //MessageBox.Show(this.Text); //this ist die Form
            //MessageBox.Show(((Label)sender).Text);  //sender zum einem Label gecastet. Dann auf den Text von dem gecasteten Label zugegriffen // z.B. Kürzel
            // anhand des Textes in einer Methode auslesen, wonach sortiert werden soll
            // headerCoiceText = sender.ToString(); // System.Windows.Forms.Label, Text: KÜRZEL
            string headerCoiceText = ((Label)sender).Text;
            MessageBox.Show(headerCoiceText);  // KÜRZEL

            // Zuordnung geklickter Wert welcher Enum-Wert
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

                    //default:  Nicht unbedingt notwendig. nur wenn dieser Sinn ergibt
                    //    mySortBy = SortBy.NoOfComments;
                    //    break               
            }
            GenerateGUIControls();


            // aus dem Event können niemals werte zurückgegeben werden. 
            // hier können lediglich Methoden (Bsp.) aufgerufen werden, die Werte in irgendetwas anderes reinpacken

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
