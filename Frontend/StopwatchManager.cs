using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace Frontend
{
    public static class StopwatchManager
    {
        public static Stopwatch CreateStopWatch()
        {
            return new Stopwatch();
        }

        public static void StopShowAndResetStopWatchTime(Stopwatch stopwatch, string operation)
        {
            stopwatch.Stop();

            var ms = stopwatch.ElapsedMilliseconds;
            var seconds = ms * 0.001f;
            MessageBox.Show($"{operation}: {ms.ToString(CultureInfo.InvariantCulture)}ms ({seconds.ToString(CultureInfo.InvariantCulture)}s)");

            stopwatch.Reset();
        }
    }
}