using System.IO.Ports;

namespace co2_sensor;

internal readonly record struct CO2_result(byte Status, short PPM, sbyte Temp);

internal class MZ19_sensor : IDisposable
{
  private object _lock = new();
  private bool disposed = false;
  readonly SerialPort port;
  readonly byte[] tx_data_read_CO2 = { 0xff, 0x01, 0x86, 0x00, 0x00, 0x00, 0x00, 0x00, 0x79 };
  readonly byte[] rx_data = new byte[9];
  int data_count = 0;
  bool running = false;
  AutoResetEvent _port_read_finish = new(false);

  public MZ19_sensor(string portname)
  {
    port = new(portname, 9600, Parity.None, 8, StopBits.One)
    {
      ReadTimeout = 1000
    };
    port.DataReceived += sp_DataReceived;
    port.Open();
  }

  private bool StartCommand(byte[] tx_data)
  {
    try
    {
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
      return false;
    }
    return true;
  }

  private CO2_result Process_CO2()
  {
    if(data_count != 9)
      return new(2, 0, 0);
    unchecked
    {
      byte crc = 0;
      for (int i = 0; i < 8; i++) { crc += rx_data[i]; }
      crc = (byte)(0xff - crc);
      if (crc != rx_data[8])
        return new(3, 0, 0);
    }
    return new(0, (short)(256 * rx_data[2] + rx_data[3]), (sbyte)(rx_data[4] - 40));
  }

  public CO2_result ReadCO2()
  {
    if(disposed) return new(5, 0, 0);
    lock (_lock)
    {
      if (running) return new(4, 0, 0);
      running = true;
    }
    if(!StartCommand(tx_data_read_CO2))
    {
      running = false;
      return new(1, 0, 0);
    }
    _port_read_finish.WaitOne();
    lock(_lock)
    {
      running = false;
      return Process_CO2();
    }
  }

  public Task<CO2_result> ReadCO2Async() => Task.Run(ReadCO2);

  void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
  {
    for (int i = 0; i < 100 && port.BytesToRead < 9; ++i)
    {
      Thread.Sleep(10);
    }
    try
    {
      data_count = port.Read(rx_data, 0, 9);
    }
    catch
    {
      port.ReadExisting();
      data_count = 0;
    }
    _port_read_finish.Set();
  }

  public void Dispose()
  {
    _Dispose();
    GC.SuppressFinalize(this);
  }

  private void _Dispose()
  {
    if (disposed) return;
    if (running) _port_read_finish.Set();
    port.Dispose();
    disposed = true;
  }

  ~MZ19_sensor()
  {
    _Dispose();
  }
}
