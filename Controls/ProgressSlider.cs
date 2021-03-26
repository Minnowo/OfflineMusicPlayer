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

    public partial class ProgressSlider1 : UserControl
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
        public ProgressSlider1()
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

    public partial class ProgressSlider : TrackBar
    {
        public ProgressSlider()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            Maximum = 100;
            TickStyle = TickStyle.Both;
            TickFrequency = 0;
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!TrackBarRenderer.IsSupported)
            {
                this.Parent.Text = "CustomTrackBar Disabled";
                return;
            }

            this.Parent.Text = "CustomTrackBar Enabled";
            Rectangle trackRectangle = new Rectangle();
            trackRectangle.X = ClientRectangle.X + 2;
            trackRectangle.Y = ClientRectangle.Y + 28;
            trackRectangle.Width = ClientRectangle.Width - 4;
            trackRectangle.Height = 4;
            
            TrackBarRenderer.DrawHorizontalTrack(e.Graphics,
                trackRectangle);
            TrackBarRenderer.DrawHorizontalThumb(e.Graphics,
                new Rectangle(0,0,40,40), System.Windows.Forms.VisualStyles.TrackBarThumbState.Hot);

            base.OnPaint(e);
        }

        /*protected override void OnCreateControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            if (Parent != null)
                BackColor = Color.Transparent;

            base.OnCreateControl();
        }*/
        /*protected override void OnMouseUp(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    isClicking = false;
                    break;
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    isClicking = true;
                    break;
            }
            base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isClicking)
            {
                this.Value = (int)Math.Round(((double)e.X / (double)ClientSize.Width) * 100).Clamp(0, 100);
                Console.WriteLine((int)Math.Round((double)e.X / (double)ClientSize.Width) );
            }
            base.OnMouseMove(e);
        }*/

        #region Component Designer generated code
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }

    class CustomTrackBar : Control
    {
        private int numberTicks = 10;
        private Rectangle trackRectangle = new Rectangle();
        private Rectangle ticksRectangle = new Rectangle();
        private Rectangle thumbRectangle = new Rectangle();
        private int currentTickPosition = 0;
        private float tickSpace = 0;
        private bool thumbClicked = false;
        private TrackBarThumbState thumbState =
            TrackBarThumbState.Normal;

        public CustomTrackBar(int ticks = 10, Size trackBarSize = new Size())
        {
            this.Location = new Point(10, 10);
            this.Size = trackBarSize;
            this.numberTicks = ticks;
            this.BackColor = Color.DarkCyan;
            this.DoubleBuffered = true;

            // Calculate the initial sizes of the bar, 
            // thumb and ticks.
            SetupTrackBar();
        }

        // Calculate the sizes of the bar, thumb, and ticks rectangle.
        private void SetupTrackBar()
        {
            if (!TrackBarRenderer.IsSupported)
                return;

            using (Graphics g = this.CreateGraphics())
            {
                // Calculate the size of the track bar.
                trackRectangle.X = ClientRectangle.X + 2;
                trackRectangle.Y = ClientRectangle.Y + 28;
                trackRectangle.Width = ClientRectangle.Width - 4;
                trackRectangle.Height = 4;

                // Calculate the size of the rectangle in which to 
                // draw the ticks.
                ticksRectangle.X = trackRectangle.X + 4;
                ticksRectangle.Y = trackRectangle.Y - 8;
                ticksRectangle.Width = trackRectangle.Width - 8;
                ticksRectangle.Height = 4;

                tickSpace = ((float)ticksRectangle.Width - 1) /
                    ((float)numberTicks - 1);

                // Calculate the size of the thumb.
                thumbRectangle.Size =
                    TrackBarRenderer.GetTopPointingThumbSize(g,
                    TrackBarThumbState.Normal);

                thumbRectangle.X = CurrentTickXCoordinate();
                thumbRectangle.Y = trackRectangle.Y - 8;
            }
        }

        private int CurrentTickXCoordinate()
        {
            if (tickSpace == 0)
            {
                return 0;
            }
            else
            {
                return ((int)Math.Round(tickSpace) *
                    currentTickPosition);
            }
        }

        // Draw the track bar.
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!TrackBarRenderer.IsSupported)
            {
                this.Parent.Text = "CustomTrackBar Disabled";
                return;
            }

            this.Parent.Text = "CustomTrackBar Enabled";
            TrackBarRenderer.DrawHorizontalTrack(e.Graphics,
                trackRectangle);
            TrackBarRenderer.DrawTopPointingThumb(e.Graphics,
                thumbRectangle, thumbState);
            TrackBarRenderer.DrawHorizontalTicks(e.Graphics,
                ticksRectangle, numberTicks, EdgeStyle.Raised);
        }

        // Determine whether the user has clicked the track bar thumb.
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!TrackBarRenderer.IsSupported)
                return;

            if (this.thumbRectangle.Contains(e.Location))
            {
                thumbClicked = true;
                thumbState = TrackBarThumbState.Pressed;
            }

            this.Invalidate();
        }

        // Redraw the track bar thumb if the user has moved it.
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!TrackBarRenderer.IsSupported)
                return;

            if (thumbClicked == true)
            {
                if (e.Location.X > trackRectangle.X &&
                    e.Location.X < (trackRectangle.X +
                    trackRectangle.Width - thumbRectangle.Width))
                {
                    thumbClicked = false;
                    thumbState = TrackBarThumbState.Hot;
                    this.Invalidate();
                }

                thumbClicked = false;
            }
        }

        // Track cursor movements.
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!TrackBarRenderer.IsSupported)
                return;

            // The user is moving the thumb.
            if (thumbClicked == true)
            {
                // Track movements to the next tick to the right, if 
                // the cursor has moved halfway to the next tick.
                if (currentTickPosition < numberTicks - 1 &&
                    e.Location.X > CurrentTickXCoordinate() +
                    (int)(tickSpace))
                {
                    currentTickPosition++;
                }

                // Track movements to the next tick to the left, if 
                // cursor has moved halfway to the next tick.
                else if (currentTickPosition > 0 &&
                    e.Location.X < CurrentTickXCoordinate() -
                    (int)(tickSpace / 2))
                {
                    currentTickPosition--;
                }

                thumbRectangle.X = CurrentTickXCoordinate();
            }

            // The cursor is passing over the track.
            else
            {
                thumbState = thumbRectangle.Contains(e.Location) ?
                    TrackBarThumbState.Hot : TrackBarThumbState.Normal;
            }

            Invalidate();
        }
    }
}
