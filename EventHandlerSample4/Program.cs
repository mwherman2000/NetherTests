// https://www.codeproject.com/Articles/11541/The-Simplest-C-Events-Example-Imaginable

using System;
using System.Threading.Tasks;

namespace EventHandlerSample4
{
    public class Metronome
    {
        public delegate void TickEventHandler(Metronome m, EventArgs e);
        public event TickEventHandler OnTickEventHandlerPtr;
        public EventArgs e = null;

        public async Task Start()
        {
            while (true)
            {
                Console.Write(".");
                //System.Threading.Thread.Sleep(1000);
                await Task.Delay(5000);
                if (OnTickEventHandlerPtr != null)
                {
                    OnTickEventHandlerPtr(this, e);
                }
            }
        }
    }

    public class Listener
    {
        int listenerId = 0;

        public void Subscribe(Metronome m, int lId)
        {
            listenerId = lId;
            m.OnTickEventHandlerPtr += new Metronome.TickEventHandler(HeardHandler);
        }

        private void HeardHandler(Metronome m, EventArgs e)
        {
            System.Console.WriteLine("HEARD IT " + listenerId.ToString());
        }

    }

    class Test
    {
        static void Main()
        {
            Metronome m = new Metronome();
            Task mt = m.Start();

            Listener l1 = new Listener();
            Listener l2 = new Listener();

            l1.Subscribe(m, 1);
            l2.Subscribe(m, 2);

            mt.Wait();

            Console.ReadLine();
        }
    }
}