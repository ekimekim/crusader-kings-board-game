namespace CKBlazor.Services
{
    public class TimerService
    {
        public event Action? OnTimerElapsed;

        private Timer timer;

        public TimerService()
        {
            timer = new Timer(TimerElapsed);
            timer.Change(0, (long)TimeSpan.FromSeconds(30).TotalMilliseconds);
        }

        private void TimerElapsed(object? state)
        {
            OnTimerElapsed?.Invoke();
        }
    }
}
