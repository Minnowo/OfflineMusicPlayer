using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MusicPlayer.Helpers;
using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;

namespace MusicPlayer.Controls
{

    public partial class ProgressSlider : UserControl
    {

        public delegate void ValueChanged(int value);
        public event ValueChanged ValueChange;

        public Color SliderColor 
        {
            get 
            {
                return sliderColor;
            }
            set
            {
                sliderColor = value;
                Invalidate();
            }
        }
        public int Value
        {
            get
            {
                if (sliderMaxSize.Width == 0)
                    return 0;
                return (int)((double)displayValueRect.Width / (double)sliderMaxSize.Width * 100d).Clamp(0, 100);
            }
            set
            {
                displayValueRect = new Rectangle(BorderSize, BorderSize, ((value.Clamp(0,100) * sliderMaxSize.Width) / 100).Clamp(0, sliderMaxSize.Width), ClientSize.Height - BorderSize * 2);
                Invalidate();
            }
        }

        public int BorderSize
        {
            get
            {
                return borderSize;
            }
            set
            {
                borderSize = value;
                displayValueRect = new Rectangle(BorderSize, value, (Value * sliderMaxSize.Width) / 100, ClientSize.Height - value * 2);
                sliderMaxSize = new Size(ClientSize.Width - value * 2, ClientSize.Height - value * 2);
                Invalidate();
            }
        }

        private Color sliderColor;
        private int borderSize;

        private bool isLeftClicking = false;
        private Rectangle displayValueRect;
        private Size sliderMaxSize;
        public ProgressSlider()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            this.ClientSize = new Size(104, 25);
            displayValueRect = new Rectangle(2, 2, 50, ClientSize.Height - 4 );
            sliderMaxSize = new Size(ClientSize.Width - 4, ClientSize.Height - 4);
            BackColor = Color.Black;
            this.SliderColor = Color.Red;

        }

        private void OnValueChanged()
        {
            if(ValueChange != null)
            {
                ValueChange(Value);
            }
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            displayValueRect = new Rectangle(BorderSize, BorderSize, (Value * sliderMaxSize.Width) / 100, ClientSize.Height - BorderSize * 2);
            sliderMaxSize = new Size(ClientSize.Width - borderSize * 2, ClientSize.Height - BorderSize * 2);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isLeftClicking)
            {
                displayValueRect.Width = (int)Math.Round((double)e.X).Clamp(0, sliderMaxSize.Width);

                OnValueChanged();
                Invalidate();
            }

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    isLeftClicking = false;
                    break;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    isLeftClicking = true;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (SolidBrush br = new SolidBrush(this.sliderColor))
            {
                g.FillRectangle(br, this.displayValueRect);

            }
        }
    }

}
