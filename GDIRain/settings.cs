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
    public partial class settings : Form
    {
        public settings(Color oldRain, Color oldBG, bool usingWind, bool usingLightning, int rainWeight, bool activeResize)
        {
            InitializeComponent();
            newBG = oldBG;
            newRain = oldRain;
            newWind = usingWind;
            newLightning = usingLightning;

            checkBox1.Checked = usingLightning;
            checkBox2.Checked = usingWind;
            panel1.BackColor = oldRain;
            panel2.BackColor = oldBG;

            numericUpDown1.Value = rainWeight;
            newRainWeight = rainWeight;

            newActiveResize = activeResize;
            checkBox3.Checked = activeResize;
        }
        
        public Color newBG { get; set; }
        public Color newRain { get; set; }
        public bool newWind { get; set; }
        public bool newLightning { get; set; }
        public int newRainWeight { get; set; }
        public bool newActiveResize { get; set; }

        private void settings_Load(object sender, EventArgs e)
        {
            ToolTip helpTT = new ToolTip();
            helpTT.ShowAlways = true;
            helpTT.SetToolTip(label4, "Lower is more rain. (Spawn drop every X pixels)");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setRainColor.AnyColor = true;
            setRainColor.SolidColorOnly = true;
            setRainColor.Color = newRain;

            if (setRainColor.ShowDialog() == DialogResult.OK)
            {
                newRain = setRainColor.Color;
                panel1.BackColor = newRain;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetBackColor.AnyColor = true;
            SetBackColor.SolidColorOnly = true;
            SetBackColor.Color = newBG;

            if (SetBackColor.ShowDialog() == DialogResult.OK)
            {
                newBG = SetBackColor.Color;
                panel2.BackColor = newBG;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            newLightning = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            newWind = checkBox2.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            newRainWeight = (int)numericUpDown1.Value;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            newActiveResize = checkBox3.Checked;
        }
    }
}
