using System;

namespace Plainion.WhiteRabbit.Presentation
{
    public class Channel
    {
        public Action<TimeSpan> OnTimeElapsedChanged { get; set; }
    }
}
