namespace co2_sensor;

internal struct StatAccumulator
{
  readonly Queue<double> _seq;
  public double Average { get; private set; } = 0.0;
  public readonly int Count => _seq.Count;
  int _max_len;

  public StatAccumulator(int max_len)
  {
    _max_len = max_len;
    _seq = new Queue<double>();
  }

  public StatAccumulator(int max_len, IEnumerable<double> data)
  {
    _max_len = max_len;
    _seq = new Queue<double>(data);
    Average = _seq.Average();
  }

  public void Push(double value)
  {
    if (_seq.Count < _max_len)
    {
      _seq.Enqueue(value);
      double len = _seq.Count;
      Average *= (len - 1.0) / len;
      Average += value / len;
    }
    else
    {
      double old = _seq.Dequeue();
      _seq.Enqueue(value);
      Average += (value - old) / _seq.Count;
    }
  }

  public readonly IReadOnlyCollection<double> GetSequence() => _seq;
}
