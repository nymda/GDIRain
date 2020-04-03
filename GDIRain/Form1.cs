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
            int newX = rnd.Next(0, bg.Width);
            int newZ = rnd.Next(0, 10);
            int newlen = rnd.Next(5, 15);
            drops.Add(new droplet(newY, newX, newZ, newlen));
        }

        private void drawFrame_Tick(object sender, EventArgs e)
        {
            spawnDrop();
            spawnDrop();
            g.FillRectangle(Brushes.Gray, 0, 0, bg.Width, bg.Height);
            for(int i = 0; i < drops.Count; i++)
            {
                int drawX = drops[i].x;
                int drawY = drops[i].y;
                int length = drops[i].length;
                drawY = bg.Height - drawY;
                if(drawY < bg.Height - 1)
                {
                    g.DrawLine(thick, drawX, drawY, drawX, drawY - length);
                    drops[i].y -= (1 * drops[i].z) * 2;
                }

            }
            pictureBox1.Image = bg;
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
