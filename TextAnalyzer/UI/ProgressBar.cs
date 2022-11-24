namespace TextAnalyzer;

using System;
using System.Text;
using System.Threading;

public class ProgressBar : IDisposable, IProgress<double>
{
    private const int BlockCount = 10;
    private readonly TimeSpan animationInterval = TimeSpan.FromSeconds(1.0 / 8);
    private const string Animation = @"|/-\";

    private readonly Timer _timer;

    private double _currentProgress;
    private string _currentText = string.Empty;
    private bool _disposed;
    private int _animationIndex;

    public ProgressBar()
    {
        _timer = new Timer(TimeHandler);
        if (!Console.IsOutputRedirected)
        {
            ResetTimer();
        }
    }

    public void Report(double value)
    {
        value = Math.Max(0, Math.Min(1, value));
        Interlocked.Exchange(ref _currentProgress, value);
    }

    private void TimeHandler(object? state)
    {
        lock (_timer)
        {
            if (_disposed) return;

            int progressBlockCount = (int)(_currentProgress * BlockCount);
            int percent = (int)(_currentProgress * 100);
            string text = string.Format("[{0}{1}] {2,3}% {3}",
                new string('#', progressBlockCount), new string('-', BlockCount - progressBlockCount),
                percent,
                Animation[_animationIndex++ % Animation.Length]);
            UpdateText(text);

            ResetTimer();
        }
    }

    private void UpdateText(string text)
    {
        int commonPrefixLength = 0;
        int commonLength = Math.Min(_currentText.Length, text.Length);
        while (commonPrefixLength < commonLength && text[commonPrefixLength] == _currentText[commonPrefixLength])
        {
            commonPrefixLength++;
        }

        StringBuilder outputBuilder = new StringBuilder();
        outputBuilder.Append('\b', _currentText.Length - commonPrefixLength);

        outputBuilder.Append(text.Substring(commonPrefixLength));

        int overlapCount = _currentText.Length - text.Length;
        if (overlapCount > 0)
        {
            outputBuilder.Append(' ', overlapCount);
            outputBuilder.Append('\b', overlapCount);
        }

        Console.Write(outputBuilder);
        _currentText = text;
    }

    private void ResetTimer()
    {
        _timer.Change(animationInterval, TimeSpan.FromMilliseconds(-1));
    }

    public void Dispose()
    {
        lock (_timer)
        {
            _disposed = true;
            UpdateText(string.Empty);
        }
    }
}