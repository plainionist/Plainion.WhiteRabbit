using System;
using System.Windows.Forms;

namespace Plainion.WhiteRabbit.Presentation
{
    internal class Recorder
    {
        private readonly Timer myElapsedTimer = null;
        private DateTime myStartTime;
        private int myElapsedSeconds = 0;
        private readonly Channel myChannel = null;

        public Recorder(Channel channel)
        {
            myChannel = channel;

            myElapsedTimer = new Timer();
            myElapsedTimer.Enabled = false;
            myElapsedTimer.Interval = 1000;
            myElapsedTimer.Tick += myElapsedTimer_OnTick;
        }

        public DateTime StartTime => myStartTime;

        public DateTime StopTime => myStartTime + Elapsed;

        public TimeSpan Elapsed => new(0, 0, myElapsedSeconds);

        public void Start()
        {
            Start(DateTime.Now);
        }

        public void Start(DateTime start)
        {
            myStartTime = start;
            myElapsedSeconds = (int)(DateTime.Now - start).TotalSeconds;

            myChannel.OnTimeElapsedChanged(Elapsed);

            myElapsedTimer.Enabled = true;
        }

        public void Stop()
        {
            myElapsedTimer.Enabled = false;
        }

        private void myElapsedTimer_OnTick(object sender, EventArgs e)
        {
            ++myElapsedSeconds;

            myChannel.OnTimeElapsedChanged(Elapsed);
        }
    }
}
