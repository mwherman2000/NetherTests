// Reference: https://msdn.microsoft.com/en-us/library/system.eventargs(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-2

using System;

namespace EventHandlerSample1
{
    class Program
    {
        static void Main(string[] args)
        {
            CounterEventEnabledClass c = new CounterEventEnabledClass(new Random().Next(10));
            c.ThresholdReachedHandlerPtr += ThresholdReachedHandler;

            Console.WriteLine("press 'a' key to increase total");
            while (Console.ReadKey(true).KeyChar == 'a')
            {
                Console.WriteLine("adding one");
                c.Add(1);
            }

            Console.ReadLine();
        }

        static void ThresholdReachedHandler(object sender, ThresholdReachedEventArgs e)
        {
            Console.WriteLine("The threshold of {0} was reached at {1}.", e.Threshold, e.TimeReached);
            //Environment.Exit(0);
        }
    }

    class CounterEventEnabledClass
    {
        private int threshold;
        private int total;

        public CounterEventEnabledClass(int passedThreshold)
        {
            threshold = passedThreshold;
        }

        public void Add(int x)
        {
            total += x;
            if (total >= threshold)
            {
                ThresholdReachedEventArgs args = new ThresholdReachedEventArgs();
                args.Threshold = threshold;
                args.TimeReached = DateTime.Now;
                OnThresholdReachedHandlerTrigger(args);
            }
        }

        protected virtual void OnThresholdReachedHandlerTrigger(ThresholdReachedEventArgs e)
        {
            EventHandler<ThresholdReachedEventArgs> handler = ThresholdReachedHandlerPtr;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<ThresholdReachedEventArgs> ThresholdReachedHandlerPtr;
    }

    public class ThresholdReachedEventArgs : EventArgs
    {
        public int Threshold { get; set; }
        public DateTime TimeReached { get; set; }
    }
}