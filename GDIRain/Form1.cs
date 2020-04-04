using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDIRain
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Random rnd = new Random();
        public List<droplet> drops = new List<droplet> { };
        public Bitmap bg;
        public Graphics g;

        public Color rainCol = Color.LightBlue;
        public Color bgCol = Color.Gray;

        public Brush brushBG = Brushes.Gray;
        public Pen thick = new Pen(Brushes.LightBlue, 2);

        public int flashOpacity = 255;
        public bool flashing = false;
        public bool flashSwitch = false;

        public bool useWind = true;
        public bool useLignting = false;

        public int modifyerOnWind = 3;

        public int divValSpawn = 50;

        public bool activeResizing = true;

        private void Form1_Load(object sender, EventArgs e)
        {
            initialise();
            spawnDropsByWinWidth();    
            drawFrame.Start();
            lightningFlash.Interval = rnd.Next(3000, 10000);
            lightningFlash.Start();
        }

        public void initialise()
        {
            bg = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(bg);
        }

        public void beginLightning()
        {
            if (useLignting) { flashing = true; }
        }

        private void drawFrame_Tick(object sender, EventArgs e)
        {
            spawnDropsByWinWidth();
            g.FillRectangle(brushBG, 0, 0, bg.Width, bg.Height);
            for(int i = 0; i < drops.Count; i++)
            {
                int drawX = drops[i].x;
                int drawY = drops[i].y;
                int length = drops[i].length;
                drawY = bg.Height - drawY;
                g.DrawLine(thick, drawX, drawY, drawX + modifyerOnWind, drawY - length);
                drops[i].y -= (1 * drops[i].z);
                drops[i].x -= modifyerOnWind;
            }

            if (flashing)
            {
                if(flashOpacity > 0)
                {
                    Color flashColor = Color.FromArgb(flashOpacity, 255, 255, 255);
                    g.FillRectangle(new SolidBrush(flashColor), 0, 0, bg.Width, bg.Height);
                    flashOpacity -= 4;
                }
                if(flashOpacity <= 0)
                {
                    flashOpacity = 255;
                    flashing = false;
                }
            }

            pictureBox1.Image = bg;
            drops = cullDrops();
        }

        public void spawnDropsByWinWidth()
        {
            int cdrop = bg.Width / divValSpawn;
            for(int i = 0; i < cdrop; i++)
            {
                spawnDrop();
            }
        }

        public void spawnDrop()
        {
            int newY = bg.Height;
            int newX = rnd.Next(0, bg.Width + (bg.Width / 4));
            int newZ = rnd.Next(5, 20);
            int newlen = rnd.Next(5, 15);
            drops.Add(new droplet(newY, newX, newZ, newlen));
        }

        public List<droplet> cullDrops()
        {
            List<droplet> ret = new List<droplet> { };
            foreach(droplet d in drops)
            {
                if(d.y > 0 && d.y < bg.Height)
                {
                    ret.Add(d);
                }
            }
            return ret;
        }

        private void main_resized(object sender, EventArgs e)
        {
            this.Text = "Rain | [S] Settings | " + this.Width + " x " + this.Height;
            if (activeResizing)
            {
                bg = new Bitmap(this.Width, this.Height);
                g = Graphics.FromImage(bg);
            }
        }

        private void main_kdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.S)
            {
                using (var form = new settings(rainCol, bgCol, useWind, useLignting, divValSpawn, activeResizing))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        rainCol = form.newRain;

                        bgCol = form.newBG;

                        useWind = form.newWind;
                        if (useWind){ modifyerOnWind = 3; }
                        else{ modifyerOnWind = 0; }

                        useLignting = form.newLightning;

                        brushBG = new SolidBrush(bgCol);
                        thick = new Pen(rainCol, 2);

                        divValSpawn = form.newRainWeight;

                        activeResizing = form.newActiveResize;
                        if (activeResizing)
                        {
                            bg = new Bitmap(this.Width, this.Height);
                            g = Graphics.FromImage(bg);
                        }
                    }
                }
            }
        }

        private void lightningFlash_Tick(object sender, EventArgs e)
        {
            beginLightning();
            lightningFlash.Interval = rnd.Next(3000, 30000);
        }
    }

    public class droplet
    {
        public int y { get; set; }
        public int x { get; set; }
        public int z { get; set; }
        public int length { get; set; }

        public droplet(int yPos, int xPos, int zPos, int len)
        {
            y = yPos;
            x = xPos;
            z = zPos;
            length = len;
        }
    }
}
