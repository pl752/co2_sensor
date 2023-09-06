using System.IO.Ports;
using System.Text;

namespace co2_sensor {
  public partial class Form1 : Form {
    readonly SerialPort port = new("COM8", 9600, Parity.None, 8, StopBits.One) {
      ReadTimeout = 1000
    };
    readonly byte[] tx_data = { 0xff, 0x01, 0x86, 0x00, 0x00, 0x00, 0x00, 0x00, 0x79 };
    readonly byte[] rx_data = new byte[9];
    readonly StringBuilder sb = new();

    public Form1() {
      port.DataReceived += sp_DataReceived;
      port.Open();
      InitializeComponent();
    }

    private void timer1_Tick(object sender, EventArgs e) {
      try {
        port.Write(tx_data, 0, tx_data.Length);
      }
      catch
      {
        try
        {
          port.Close();
        }
        catch { }
        port.Open();
      }
    }

    void sp_DataReceived(object sender, SerialDataReceivedEventArgs e) {
      Thread.Sleep(100);
      try {
        int r = port.Read(rx_data, 0, 9);
        if (r == 9) BeginInvoke(() => label_upd());
      }
      catch {
        port.ReadExisting();
      }
    }

    void label_upd() {
      unchecked {
        byte crc = 0;
        for (int i = 0; i < 8; i++) { crc += rx_data[i]; }
        crc = (byte)(0xff - crc);
        if (crc == rx_data[8]) {
          sb.Append(256 * rx_data[2] + rx_data[3]);
          sb.Append(" PPM CO2  |  ");
          sb.Append(rx_data[4] - 40);
          sb.Append("Ct");
          label1.Text = sb.ToString();
          sb.Clear();
        }
        else {
          label1.Text = "!";
        }
      }
    }
  }
}