using System.Text;

namespace co2_sensor;
public partial class Form1 : Form
{
  readonly MZ19_sensor sensor = new("COM8");
  readonly StringBuilder sb = new();

  StatAccumulator hourPPM, dayPPM, hourTemp, dayTemp;

  public Form1()
  {
    hourPPM = new(720);
    dayPPM = new(24);
    hourTemp = new(720);
    dayTemp = new(24);
    InitializeComponent();
    AvgLabUpdate();
  }

  private async void timer1_Tick(object sender, EventArgs e)
  {
    CO2_result result = await sensor.ReadCO2Async();
    BeginInvoke(label_upd, result);
  }

  void label_upd(CO2_result result)
  {
    if (result.Status == 0)
    {
      hourPPM.Push(result.PPM);
      hourTemp.Push(result.Temp);
      sb.Append(result.PPM);
      sb.Append(" PPM CO2  |  ");
      sb.Append(result.Temp);
      sb.Append("Ct");
      label1.Text = sb.ToString();
      sb.Clear();
    }
    else
    {
      label1.Text = "!";
      hourPPM.Push(hourPPM.Average);
      hourTemp.Push(hourTemp.Average);
    }
    AvgLabUpdate();
  }

  void AvgLabUpdate()
  {
    sb.Append("Avg: ");
    sb.Append(hourPPM.Average);
    sb.Append(" PPM  |  ");
    sb.Append(hourTemp.Average);
    sb.Append("Ct");
    AvgLabel.Text = sb.ToString();
    sb.Clear();
  }
}