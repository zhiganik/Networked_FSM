using System;
using System.Threading.Tasks;

namespace Shakhtarsk.Runtime
{
    public class ActionTimer
    {
        private readonly float _targetTime;
        
        private bool _isRunning;

        public bool IsRunning => _isRunning;

        public event Action OnTimerStart;
        public event Action OnTimerStop;

        public ActionTimer(float time)
        {
            _targetTime = time;
            _isRunning = false;
        }

        public async void Launch()
        {
            _isRunning = true;
            OnTimerStart?.Invoke();
            await RunTimer();
        }

        private async Task RunTimer()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _targetTime && _isRunning)
            {
                await Task.Delay(10);
                elapsedTime += 0.01f;
            }
            
            _isRunning = false;
            OnTimerStop?.Invoke();
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Reset()
        {
            _isRunning = false;
        }
    }
}