using System;
using System.Threading;

namespace STANC
{
    public class ServerHeartbeatMonitor
    {
        private readonly int checkIntervalMillis;
        private readonly int timeoutPeriodMillis;
        private readonly Action timeoutCallback;
        private object lastHeartbeatTimeLock = new object();
        private DateTimeOffset lastHeartbeatTime;

        private DateTimeOffset LastHeartbeatTime
        {
            set
            {
                lock (lastHeartbeatTimeLock)
                {
                    lastHeartbeatTime = value;
                }
            }
            get
            {
                lock (lastHeartbeatTimeLock)
                {
                    return lastHeartbeatTime;
                }
            }
        }

        private Timer checkTimer { get; set; }

        public ServerHeartbeatMonitor(int checkIntervalMillis, int timeoutPeriodMillis, Action timeoutCallback)
        {
            this.checkIntervalMillis = checkIntervalMillis;
            this.timeoutPeriodMillis = timeoutPeriodMillis;
            this.timeoutCallback = timeoutCallback;
        }

        public void Start()
        {
            LastHeartbeatTime = DateTimeOffset.UtcNow;
            checkTimer = new Timer(CheckTimeout, null, 0, checkIntervalMillis);
            
        }

        public void Stop()
        {
            checkTimer?.Dispose();
        }

        public void RegisterHeartbeat()
        {
            LastHeartbeatTime = DateTimeOffset.UtcNow;
        }

        private void CheckTimeout(object state)
        {
            if ((DateTimeOffset.UtcNow - LastHeartbeatTime).TotalMilliseconds > timeoutPeriodMillis)
            {
                Stop();
                timeoutCallback?.Invoke();
            }
        }

    }
}
