using System.ComponentModel;

namespace AudioPlayer;

internal sealed class VerticalVolumeBar : Control
{
    private const float MaxValue = 5f;
    private float _value = 1f;
    private bool _dragging;

    public VerticalVolumeBar()
    {
        DoubleBuffered = true;
        Cursor = Cursors.Hand;
        BackColor = Color.FromArgb(18, 21, 24);
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
    }

    public event EventHandler<float>? ValueChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public float Value
    {
        get => _value;
        set
        {
            var next = Math.Clamp(value, 0f, MaxValue);
            if (Math.Abs(_value - next) < 0.001f)
            {
                return;
            }

            _value = next;
            Invalidate();
            ValueChanged?.Invoke(this, _value);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var g = e.Graphics;
        var bounds = ClientRectangle;
        g.Clear(BackColor);

        using var borderPen = new Pen(Color.FromArgb(91, 101, 108));
        using var trackBrush = new SolidBrush(Color.FromArgb(38, 45, 52));
        using var fillBrush = new SolidBrush(Color.FromArgb(92, 160, 222));
        using var textBrush = new SolidBrush(Color.FromArgb(230, 236, 240));

        var track = Rectangle.Inflate(bounds, -10, -8);
        g.FillRectangle(trackBrush, track);

        var fillHeight = (int)Math.Round(track.Height * (_value / MaxValue));
        g.FillRectangle(fillBrush, track.Left, track.Bottom - fillHeight, track.Width, fillHeight);
        g.DrawRectangle(borderPen, track);
        g.DrawString(_value.ToString("0.00"), Font, textBrush, new RectangleF(0, 2, bounds.Width, 18), new StringFormat { Alignment = StringAlignment.Center });
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == MouseButtons.Left)
        {
            _dragging = true;
            Capture = true;
            SetFromY(e.Y);
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_dragging)
        {
            SetFromY(e.Y);
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button == MouseButtons.Left)
        {
            _dragging = false;
            Capture = false;
            SetFromY(e.Y);
        }
    }

    private void SetFromY(int y)
    {
        var trackTop = 8;
        var trackBottom = Math.Max(trackTop + 1, Height - 8);
        Value = (1f - ((float)Math.Clamp(y, trackTop, trackBottom) - trackTop) / (trackBottom - trackTop)) * MaxValue;
    }
}
