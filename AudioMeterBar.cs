namespace AudioPlayer;

internal sealed class AudioMeterBar : Control
{
    private float _level;

    public AudioMeterBar()
    {
        DoubleBuffered = true;
        BackColor = Color.FromArgb(18, 21, 24);
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
    }

    public void SetLevel(float level)
    {
        _level = Math.Clamp(level, 0f, 1f);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var g = e.Graphics;
        var bounds = ClientRectangle;
        g.Clear(BackColor);

        if (bounds.Width <= 2 || bounds.Height <= 2)
        {
            return;
        }

        var redHeight = Math.Max(8, bounds.Height / 5);
        var yellowHeight = Math.Max(10, bounds.Height / 5);
        var greenHeight = Math.Max(1, bounds.Height - redHeight - yellowHeight);
        using var redBrush = new SolidBrush(Color.FromArgb(75, 31, 31));
        using var yellowBrush = new SolidBrush(Color.FromArgb(59, 53, 30));
        using var greenBrush = new SolidBrush(Color.FromArgb(21, 54, 43));
        g.FillRectangle(redBrush, 0, 0, bounds.Width, redHeight);
        g.FillRectangle(yellowBrush, 0, redHeight, bounds.Width, yellowHeight);
        g.FillRectangle(greenBrush, 0, redHeight + yellowHeight, bounds.Width, greenHeight);

        var fillHeight = (int)Math.Round(bounds.Height * _level);
        var fillTop = bounds.Bottom - fillHeight;
        using var fillBrush = new SolidBrush(_level > 0.82f
            ? Color.FromArgb(229, 91, 84)
            : _level > 0.62f
                ? Color.FromArgb(224, 174, 67)
                : Color.FromArgb(86, 214, 142));
        g.FillRectangle(fillBrush, 5, fillTop, Math.Max(1, bounds.Width - 10), fillHeight);

        using var borderPen = new Pen(Color.FromArgb(91, 101, 108));
        g.DrawRectangle(borderPen, 0, 0, bounds.Width - 1, bounds.Height - 1);
    }
}
