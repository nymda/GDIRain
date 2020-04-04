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
        public Pen thick = new Pen(Brushes.LightBlue, 2);

        private void Form1_Load(object sender, EventArgs e)
        {
            initialise();

            for(int i = 0; i < 50; i++)
            {
                spawnDrop();
            }

            drawFrame.Start();
        }

        public void initialise()
        {
            bg = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(bg);
        }

        public void spawnDrop()
        {
            int newY = bg.Height;
            int newX = rnd.Next(0, bg.Width + (bg.Width / 4));
            int newZ = rnd.Next(5, 20);
            int newlen = rnd.Next(5, 15);
            drops.Add(new droplet(newY, newX, newZ, newlen));
        }

        private void drawFrame_Tick(object sender, EventArgs e)
        {
            spawnDropsByWinWidth();
            g.FillRectangle(Brushes.Gray, 0, 0, bg.Width, bg.Height);
            for(int i = 0; i < drops.Count; i++)
            {
                int drawX = drops[i].x;
                int drawY = drops[i].y;
                int length = drops[i].length;
                drawY = bg.Height - drawY;
                g.DrawLine(thick, drawX, drawY, drawX + 3, drawY - length);
                drops[i].y -= (1 * drops[i].z);
                drops[i].x -= 3;
            }
            pictureBox1.Image = bg;
            drops = cullDrops();
        }

        public void spawnDropsByWinWidth()
        {
            int cdrop = this.Width / 50;
            for(int i = 0; i < cdrop; i++)
            {
                spawnDrop();
            }
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
            bg = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(bg);
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
