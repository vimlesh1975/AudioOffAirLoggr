using System.ComponentModel;

namespace AudioPlayer;

internal sealed class TimelineRulerControl : Control
{
    private TimeSpan _duration = TimeSpan.Zero;

    public TimelineRulerControl()
    {
        DoubleBuffered = true;
        BackColor = Color.FromArgb(30, 35, 40);
        ForeColor = Color.FromArgb(205, 215, 222);
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TimeSpan Duration
    {
        get => _duration;
        set
        {
            var next = value < TimeSpan.Zero ? TimeSpan.Zero : value;
            if (_duration == next)
            {
                return;
            }

            _duration = next;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var g = e.Graphics;
        g.Clear(BackColor);

        if (Width < 20 || Height < 12 || _duration.TotalSeconds <= 0)
        {
            return;
        }

        using var tickPen = new Pen(Color.FromArgb(105, 119, 129));
        using var textBrush = new SolidBrush(ForeColor);
        using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };

        var totalSeconds = _duration.TotalSeconds;
        var tickSeconds = ChooseTickSeconds(totalSeconds);
        for (var seconds = 0d; seconds <= totalSeconds + 0.001; seconds += tickSeconds)
        {
            var x = (float)(seconds / totalSeconds * (Width - 1));
            var major = Math.Abs(seconds % 60) < 0.001 || tickSeconds >= 60;
            var tickHeight = major ? 10 : 6;
            g.DrawLine(tickPen, x, 0, x, tickHeight);

            if (major)
            {
                g.DrawString(FormatRulerTime(TimeSpan.FromSeconds(seconds)), Font, textBrush, new RectangleF(x - 34, tickHeight + 1, 68, Height - tickHeight - 1), format);
            }
        }
    }

    private static double ChooseTickSeconds(double totalSeconds)
    {
        if (totalSeconds <= 120)
        {
            return 10;
        }

        if (totalSeconds <= 600)
        {
            return 30;
        }

        return 60;
    }

    private static string FormatRulerTime(TimeSpan time)
    {
        return time.TotalHours >= 1
            ? $"{(int)time.TotalHours}:{time.Minutes:00}"
            : $"{(int)time.TotalMinutes}:00";
    }
}
