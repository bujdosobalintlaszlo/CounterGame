using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsztaliAlkalmazasok_szorgalmi1
{
    public partial class Form1 : Form
    {
        private Dictionary<string, Action> gombFunkciok;
        private int nehezseg;
        private Random r = new Random();
        private List<Button> gombok = new List<Button>();
        private int aktualisIndex = 1;
        private int pontszam = 0;
        private int ido;
        private Timer idozito;
        private int hiba;
        Label Lhiba = new Label();
        Label LIDo = new Label();
        private int perc;
        Label LPont = new Label();
        FlowLayoutPanel FInfoBar = new FlowLayoutPanel();

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            EventDictionaryba();
            this.Text = "Számolós játék";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FokepernyoGen();
            
        }
        private void Restart(object o, EventArgs e)
        {
            pontszam = 0;
            aktualisIndex = 1;
            gombok = new List<Button> { };
            LIDo = new Label();
            ido = 0;
            hiba = 0;
            FokepernyoGen();
        }
        private void FokepernyoGen()
        {
            this.Controls.Clear();
            FlowLayoutPanel FNehezseg = FlowPanelGeneral(FlowDirection.LeftToRight);

            GombokPanelhezAd(FNehezseg, 3, "Könnyű mód", "Közepes mód", "Nehéz mód");

            FlowLayoutPanel FInditas = FlowPanelGeneral(FlowDirection.TopDown);
            Button BStart = GombGeneralas("Indítás", 50, 100);
            FInditas.Controls.Add(BStart);

            FlowLayoutPanel FMain = FlowPanelGeneral(FlowDirection.TopDown);
            FNehezseg.Anchor = AnchorStyles.None;
            FInditas.Anchor = AnchorStyles.None;
            FMain.Anchor = AnchorStyles.None;
            this.Controls.Add(FMain);
            FMain.Controls.Add(FNehezseg);
            FMain.Controls.Add(FInditas);
            FMain.Location = new Point((this.ClientSize.Width - FMain.Width) / 2, (this.ClientSize.Height - FMain.Height) / 2);
            EventetGombhozAdNehezsegek(FNehezseg);
            EventetGombhozAdInditas(BStart, "Indítás");
            FMain.BackColor = Color.FromArgb(0, 255, 255, 255);

        }
        private FlowLayoutPanel FlowPanelGeneral(FlowDirection direction)
        {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.FlowDirection = direction;
            panel.AutoSize = true;
            panel.Dock = DockStyle.None;
            panel.Anchor = AnchorStyles.None;
            return panel;
        }

        private void GombokPanelhezAd(FlowLayoutPanel panel, int count, params string[] buttonTexts)
        {
            for (int i = 0; i < count && i < buttonTexts.Length; i++)
            {
                Button button = GombGeneralas(buttonTexts[i], 50, 100);
                button.Name = buttonTexts[i];
                panel.Controls.Add(button);
            }
        }
        private Button GombGeneralas(string text, int magassag, int szelesseg)
        {
            Button button = new Button();
            button.Text = text;
            button.Height = magassag;
            button.Width = szelesseg;
            button.Anchor = AnchorStyles.None;
            button.Name = text;
            return button;
        }
        private void EventDictionaryba()
        {
            gombFunkciok = new Dictionary<string, Action>
            {
                { "Könnyű mód", Konnyu },
                { "Közepes mód", Kozepes },
                { "Nehéz mód", Nehez },
                { "Indítás", Inditas }
            };
        }
        private void EventetGombhozAdNehezsegek(FlowLayoutPanel panel)
        {
            foreach (Button button in panel.Controls)
            {
                if (gombFunkciok.TryGetValue(button.Text, out Action function))
                {
                    button.Click += (sender, e) => function();
                }
            }
        }
        private void EventetGombhozAdInditas(Button button, string buttonText)
        {
            if (gombFunkciok.TryGetValue(buttonText, out Action function))
            {
                button.Click += (sender, e) => function();
            }
            button.Enabled = false;
        }

        private Button GombGeneralasHover(string text, int magassag, int szelesseg, int uniqueID)
        {
            Button button = new Button();
            button.Text = text;
            button.Height = magassag;
            button.Width = szelesseg;
            button.Anchor = AnchorStyles.None;
            button.Name = text;
            button.MouseEnter += EltuntetHaJo;
            button.Tag = uniqueID;
            return button;
        }

        private void Konnyu()
        {
            nehezseg = 0;
            InditasGombEngedelyezese();
        }

        private void Kozepes()
        {
            nehezseg = 1;
            InditasGombEngedelyezese();
        }

        private void Nehez()
        {
            nehezseg = 2;
            InditasGombEngedelyezese();
        }
        private void InditasGombEngedelyezese()
        {
            Button inditasGomb = (Button)Controls.Find("Indítás", true).FirstOrDefault(b => b.Name == "Indítás");
            if (inditasGomb != null && !inditasGomb.Enabled)
            {
                inditasGomb.Enabled = true;
            }
        }
        private void Inditas()
        {
            this.Controls.Clear();
            Button backgroundButton = new Button();
            backgroundButton.Text = "Indítás";
            backgroundButton.Font = new Font(backgroundButton.Font.FontFamily, 40);
            backgroundButton.BackColor = Color.Transparent;
            backgroundButton.FlatStyle = FlatStyle.Flat;
            backgroundButton.FlatAppearance.BorderSize = 0;
            backgroundButton.Dock = DockStyle.Fill;
            backgroundButton.Click += KezddElJatekot;
            this.Controls.Add(backgroundButton);
            //this.BackgroundImage.Dispose();
        }

        private void KezddElJatekot(object o, EventArgs e)
        {
            if (nehezseg == 0)
            {
                this.Controls.Clear();
                this.Enabled = false;
                InfoSavGen();
                HibaNGen();
                KonnyuGen();
            }
            else if (nehezseg == 1)
            {
                this.Controls.Clear();
                this.Enabled = false;
                InfoSavGen();
                HibaNGen();
                KozepesGen();
            }
            else if (nehezseg == 2)
            {
                this.Controls.Clear();
                this.Enabled = false;
                InfoSavGen();
                HibaNGen();
                NehezGen();
            }
        }
        private void HibaNGen()
        {
            Lhiba = new Label();
            Lhiba.Text = $"Hibák száma:{hiba}";
            FInfoBar.Controls.Add(Lhiba);
        }
        private void InfoSavGen()
        {
            FInfoBar = new FlowLayoutPanel();
            PontokMutatoGen(FInfoBar);
            this.Controls.Add(FInfoBar);
            FInfoBar.BackColor = Color.FromArgb(0, 255, 255, 255);
        }
        
        private void PontokMutatoGen(FlowLayoutPanel F)
        {
            LPont = new Label();
            LPont.Text = $"Pontszám: {pontszam}";
            F.Controls.Add(LPont);
        }
        private void KonnyuGen()
        {
            int buttonWidth = 50;
            int buttonHeight = 50;
            int minDistance = 30;

            for (int i = 1; i <= 10; i++)
            {
                Button gomb = GombGeneralasHover(i.ToString(), buttonWidth, buttonHeight, i);
                int x, y;
                do
                {
                    x = r.Next(Math.Max(0, this.ClientSize.Width - buttonWidth + 1));
                    y = r.Next(Math.Max(0, this.ClientSize.Height - buttonHeight + 1));
                } while (EllenorzottHely(x, y, buttonWidth, buttonHeight, minDistance, FInfoBar) ||
                         GombokEllenorzottHely(x, y, buttonWidth, buttonHeight, minDistance));

                gomb.Location = new Point(x, y);
                this.Controls.Add(gomb);
                gombok.Add(gomb);
            }

            idozito = new Timer();
            idozito.Interval = 1000;
            idozito.Tick += (object o, EventArgs e) => {
                this.Enabled = true;
                ido++;

                if (ido >= 60)
                {
                    ido = 0;
                    PercNoveles();
                }

                FrissitIdoLabel();
            };

            FInfoBar.Controls.Add(LIDo);
            idozito.Start();
        }

        private void PercNoveles()
        {
            if (perc > 59 && ido > 59)
            {
                this.Close();
            }
            perc++;
        }
        private void FrissitIdoLabel()
        {
            LIDo.Text = $"{PercFormat(perc)}:{ido:D2}";
        }

        private string PercFormat(int perc)
        {
            return perc.ToString("D2");
        }

        private void KozepesGen()
        {
            int buttonWidth = 40;
            int buttonHeight = 40;
            int minDistance = 15;

            for (int i = 1; i <= 40; i++)
            {
                Button gomb = GombGeneralasHover(i.ToString(), buttonWidth, buttonHeight, i);
                int x, y;
                do
                {
                    x = r.Next(Math.Max(0, this.ClientSize.Width - buttonWidth + 1));
                    y = r.Next(Math.Max(0, this.ClientSize.Height - buttonHeight + 1));
                } while (EllenorzottHely(x, y, buttonWidth, buttonHeight, minDistance, FInfoBar) ||
                         GombokEllenorzottHely(x, y, buttonWidth, buttonHeight, minDistance));

                gomb.Location = new Point(x, y);
                this.Controls.Add(gomb);
                gombok.Add(gomb);
            }

            idozito = new Timer();
            idozito.Interval = 1000;
            idozito.Tick += (object o, EventArgs e) => {
                ido++;
                this.Enabled = true;
                if (ido >= 60)
                {
                    ido = 0;
                    PercNoveles();
                }

                FrissitIdoLabel();
            };

            FInfoBar.Controls.Add(LIDo);
            idozito.Start();
        }

        private void NehezGen()
        {
            int buttonWidth = 30;
            int buttonHeight = 30;
            int minDistance = 5;

            for (int i = 1; i <= 50; i++)
            {
                Button gomb = GombGeneralasHover(i.ToString(), buttonWidth, buttonHeight, i);
                int x, y;
                do
                {
                    x = r.Next(Math.Max(0, this.ClientSize.Width - buttonWidth + 1));
                    y = r.Next(Math.Max(0, this.ClientSize.Height - buttonHeight + 1));
                } while (EllenorzottHely(x, y, buttonWidth, buttonHeight, minDistance, FInfoBar) ||
                         GombokEllenorzottHely(x, y, buttonWidth, buttonHeight, minDistance));

                gomb.Location = new Point(x, y);
                this.Controls.Add(gomb);
                gombok.Add(gomb);
            }

            idozito = new Timer();
            idozito.Interval = 1000;
            idozito.Tick += (object o, EventArgs e) => {
                ido++;
                this.Enabled = true;
                if (ido >= 60)
                {
                    ido = 0;
                    PercNoveles();
                }

                FrissitIdoLabel();
            };

            FInfoBar.Controls.Add(LIDo);
            idozito.Start();
        }


        private async void EltuntetHaJo(object sender, EventArgs e)
        {
            Button aktualisGomb = (Button)sender;
            int expectedIndex = (int)aktualisGomb.Tag;
            if (expectedIndex == aktualisIndex)
            {

                if (aktualisIndex == gombok.Count)
                {
                    pontszam += 5;
                    this.Controls.Clear();
                    Button BVege = new Button();
                    BVege.Height = this.ClientSize.Height;
                    BVege.Width = this.ClientSize.Width;
                    BVege.Font = new Font(BVege.Font.FontFamily, 40);
                    BVege.BackColor = Color.Transparent;
                    BVege.FlatStyle = FlatStyle.Flat;
                    BVege.FlatAppearance.BorderSize = 0;
                    BVege.Dock = DockStyle.Fill;
                    BVege.Text = $"Pontszám: {pontszam}\nA játékot {perc:D2}p:{ido:D2}mp alatt teljesítetted!\nHibák száma:{hiba}\nSzint: {GetSzintString(nehezseg)}.";
                    BVege.Click += Restart;
                    this.Controls.Add(BVege);
                    idozito.Stop();
                }
                else
                {
                    aktualisIndex++;
                    aktualisGomb.Visible = false;
                    pontszam += 5;
                    LPont.Text = $"Pontszám: {pontszam}";
                }
            }
            else
            {
                aktualisGomb.BackColor = Color.Red;
                await Task.Delay(100);
                aktualisGomb.BackColor = SystemColors.ButtonFace;
                if (pontszam > 2)
                {
                    pontszam -= 2;
                    LPont.Text = $"Pontszám: {pontszam}";
                }
                LPont.Text = $"Pontszám: {pontszam}";
                hiba++;
                Lhiba.Text = $"Hibák száma: {hiba}";

            }
        }

        private string GetSzintString(int nehezseg)
        {
            switch (nehezseg)
            {
                case 0:
                    return "Könnyű";
                case 1:
                    return "Közepes";
                case 2:
                    return "Nehéz";
                default:
                    return "Ismeretlen";
            }
        }
        private bool EllenorzottHely(int x, int y, int width, int height, int minDistance, Control container)
        {
            foreach (Control control in container.Controls)
            {
                if (control is Button)
                {
                    Rectangle existingButtonRect = new Rectangle(control.Location, control.Size);
                    Rectangle newButtonRect = new Rectangle(x, y, width, height);

                    if (existingButtonRect.IntersectsWith(newButtonRect) ||
                        Math.Abs(x - control.Location.X) < minDistance ||
                        Math.Abs(y - control.Location.Y) < minDistance)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool GombokEllenorzottHely(int x, int y, int width, int height, int minDistance)
        {
            foreach (Button existingButton in gombok)
            {
                Rectangle existingButtonRect = new Rectangle(existingButton.Location, existingButton.Size);
                Rectangle newButtonRect = new Rectangle(x, y, width, height);

                if (existingButtonRect.IntersectsWith(newButtonRect) ||
                    Math.Abs(x - existingButton.Location.X) < minDistance ||
                    Math.Abs(y - existingButton.Location.Y) < minDistance)
                {
                    return true;
                }
            }
            return false;
        }
    }
}