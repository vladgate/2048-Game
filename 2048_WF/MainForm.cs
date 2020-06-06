using _2048_Core.Abstract;
using _2048_Core.Presenter;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace _2048_WF
{
    public partial class MainForm : Form, IMainView
    {
        private Label[,] _labels;
        private Font _imageFontLarge/*, myFont2, myFont3*/;
        private Font _imageFontMedium;
        private const int LABEL_SIZE = 50;
        private const int WIDTH_CORRECTION = 40;
        private const int HEIGHT_CORRECTION = 140;


        public MainForm()
        {
            InitializeComponent();
            _imageFontLarge = new Font("Times New Roman", 16f, FontStyle.Bold);
            _imageFontMedium = new Font("Times New Roman", 14f, FontStyle.Bold);
        }

        #region FormEvents
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitClick?.Invoke(sender, new ExitGameEventArgs(false));
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitClick?.Invoke(sender, new ExitGameEventArgs(true));
        }
        private void mainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownHandler(sender, e);
        }
        private void mainPanel_MouseUp(object sender, MouseEventArgs e)
        {
            MouseUpHandler(sender, e);
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutClick?.Invoke(sender, e);
        }
        private void classic4x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGameClick?.Invoke(sender, new GameParametersEventArgs(4, 4));
        }
        private void big5x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGameClick?.Invoke(sender, new GameParametersEventArgs(5, 5));
        }

        private void resetHighscoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetHighscore?.Invoke(sender, e);
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            BackClick?.Invoke(sender, e);
        }
        private void btnRestart_Click(object sender, EventArgs e)
        {
            RestartClick?.Invoke(sender, e);
        }
        private void MouseUpHandler(object sender, MouseEventArgs e)
        {
            MouseUpView?.Invoke(sender, new MousePositionEventArgs(e.X, e.Y));
        }
        private void MouseDownHandler(object sender, MouseEventArgs e)
        {
            MouseDownView?.Invoke(sender, new MousePositionEventArgs(e.X, e.Y));
        }

        #endregion


        #region IMainView
        public event EventHandler ResetHighscore;
        public event EventHandler AboutClick;
        public event EventHandler RestartClick;
        public event EventHandler BackClick;
        public event EventHandler<ExitGameEventArgs> ExitClick;
        public event EventHandler<MousePositionEventArgs> MouseDownView;
        public event EventHandler<MousePositionEventArgs> MouseUpView;
        public event EventHandler<GameParametersEventArgs> NewGameClick;

        public uint CurrentScore
        {
            set => lblScore.Text = value.ToString();
        }

        public uint HighScore
        {
            set => lblHighScore.Text = value.ToString();
        }

        public void DrawGameField(int width, int height)
        {
            _mainPanel.Controls.Clear();
            //_mainPanel.Width = LABEL_SIZE * width;
            //_mainPanel.Height = LABEL_SIZE * height;
            _labels = new Label[width, height];

            byte shift = 2;
            switch (width)
            {
                case 3: shift = 12; break;
                case 4: shift = 8; break;
                case 5: shift = 5; break;
            }
            //int labeSize = (_mainPanel.Width - shift) / width - shift;
            _mainPanel.Width = LABEL_SIZE * width + shift * (width + 1);
            _mainPanel.Height = LABEL_SIZE * height + shift * (height + 1);
            Width = _mainPanel.Width + WIDTH_CORRECTION;
            Height = _mainPanel.Height + HEIGHT_CORRECTION;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Label lbl = new Label();
                    //lbl.Name = "label_" + i + "_" + j;
                    lbl.Width = lbl.Height = LABEL_SIZE;
                    lbl.Location = new Point((j) * LABEL_SIZE + shift * (j + 1), (i) * LABEL_SIZE + shift * (i + 1));
                    lbl.BackColor = SystemColors.ScrollBar;
                    lbl.Font = _imageFontLarge;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.MouseDown += MouseDownHandler;
                    lbl.MouseUp += MouseUpHandler;
                    _mainPanel.Controls.Add(lbl);
                    _labels[i, j] = lbl;
                }
            }
        }

        public void Set0(int i, int j)
        {
            _labels[i, j].Text = "";
            _labels[i, j].BackColor = SystemColors.ScrollBar;
            _labels[i, j].Font = _imageFontLarge;
        }

        public void Set2(int i, int j)
        {
            _labels[i, j].Text = "2";
            _labels[i, j].BackColor = SystemColors.ControlLight;
            _labels[i, j].ForeColor = SystemColors.Desktop;
            _labels[i, j].Font = _imageFontLarge;
        }

        public void Set4(int i, int j)
        {
            _labels[i, j].Text = "4";
            _labels[i, j].BackColor = SystemColors.Info;
            _labels[i, j].ForeColor = SystemColors.Desktop;
            _labels[i, j].Font = _imageFontLarge;
        }

        public void Set8(int i, int j)
        {
            _labels[i, j].Text = "8";
            _labels[i, j].BackColor = Color.SandyBrown;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontLarge;
        }

        public void Set16(int i, int j)
        {
            _labels[i, j].Text = "16";
            _labels[i, j].BackColor = Color.Chocolate;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontLarge;
        }

        public void Set32(int i, int j)
        {
            _labels[i, j].Text = "32";
            _labels[i, j].BackColor = Color.Tomato;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontLarge;
        }

        public void Set64(int i, int j)
        {
            _labels[i, j].Text = "64";
            _labels[i, j].BackColor = Color.Red;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontLarge;
        }

        public void Set128(int i, int j)
        {
            _labels[i, j].Text = "128";
            _labels[i, j].BackColor = Color.Goldenrod;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontLarge;
        }

        public void Set256(int i, int j)
        {
            _labels[i, j].Text = "256";
            _labels[i, j].BackColor = Color.Goldenrod;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontLarge;
        }

        public void Set512(int i, int j)
        {
            _labels[i, j].Text = "512";
            _labels[i, j].BackColor = Color.Goldenrod;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontLarge;
        }

        public void Set1024(int i, int j)
        {
            _labels[i, j].Text = "1024";
            _labels[i, j].BackColor = Color.Goldenrod;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontMedium;
        }

        public void Set2048(int i, int j)
        {
            _labels[i, j].Text = "2048";
            _labels[i, j].BackColor = Color.Goldenrod;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontMedium;
        }

        public void Set4096(int i, int j)
        {
            _labels[i, j].Text = "4096";
            _labels[i, j].BackColor = SystemColors.Desktop;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontMedium;
        }

        public void Set8192(int i, int j)
        {
            _labels[i, j].Text = "8192";
            _labels[i, j].BackColor = SystemColors.Desktop;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontMedium;
        }

        public void Set16384(int i, int j)
        {
            _labels[i, j].Text = "16384";
            _labels[i, j].BackColor = SystemColors.Desktop;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontMedium;
        }

        public void Set32768(int i, int j)
        {
            _labels[i, j].Text = "32768";
            _labels[i, j].BackColor = SystemColors.Desktop;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontMedium;
        }

        public void Set65536(int i, int j)
        {
            _labels[i, j].Text = "65536";
            _labels[i, j].BackColor = SystemColors.Desktop;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontMedium;
        }

        public void Set131072(int i, int j)
        {
            _labels[i, j].Text = "131072";
            _labels[i, j].BackColor = SystemColors.Desktop;
            _labels[i, j].ForeColor = SystemColors.ButtonHighlight;
            _labels[i, j].Font = _imageFontMedium;
        }

        #endregion

    }
}
