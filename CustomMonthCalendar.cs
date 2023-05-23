using System.Drawing;
using System.Windows.Forms;

public class CustomMonthCalendar : MonthCalendar
{
    private Font customFont;

    public CustomMonthCalendar()
    {
        // Define a fonte personalizada com o tamanho desejado
        customFont = new Font(Font.FontFamily, 14f);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Define a fonte personalizada para desenhar o calendário
        e.Graphics.DrawString(
            Text,
            customFont,
            SystemBrushes.ControlText,
            ClientRectangle,
            new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });
    }
}
