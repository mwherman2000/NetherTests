// Reference: https://msdn.microsoft.com/en-us/library/system.eventargs(v=vs.80).aspx

using System;

namespace EventHanderSample2
{
    public class FireAlarmEventEnabledClass
    {
        // Events are handled with delegates, so we must establish a FireEventHandler
        // as a delegate:
        public delegate void FireAlarmEventHandler(object sender, FireAlarmEventArgs fe);

        // Now, create a public event "FireEvent" whose type is our FireEventHandler delegate. 
        public event FireAlarmEventHandler FireAlarmEventHandlerPtr;

        // This will be the starting point of our event-- it will create FireEventArgs,
        // and then raise the event, passing FireEventArgs. 
        public void ActivateFireAlarmHandlerTrigger(string room, int ferocity)
        {
            FireAlarmEventArgs fireArgs = new FireAlarmEventArgs(room, ferocity);

            // Now, raise the event by invoking the delegate. Pass in 
            // the object that initated the event (this) as well as FireEventArgs. 
            // The call must match the signature of FireEventHandler.
            FireAlarmEventHandlerPtr(this, fireArgs);
        }
    }

    class FireAlarmHandlerListener
    {
        // Create a FireAlarm to handle and raise the fire events. 
        public FireAlarmHandlerListener(FireAlarmEventEnabledClass fireAlarm)
        {
            // Add a delegate containing the ExtinguishFire function to the class'
            // event so that when FireAlarm is raised, it will subsequently execute 
            // ExtinguishFire.
            fireAlarm.FireAlarmEventHandlerPtr += new FireAlarmEventEnabledClass.FireAlarmEventHandler(ExtinguishFire);
        }

        // This is the function to be executed when a fire event is raised. 
        void ExtinguishFire(object sender, FireAlarmEventArgs fe)
        {
            Console.WriteLine("\nThe ExtinguishFire function was called by {0} with ferocity {1}.", 
                sender.ToString(), fe.ferocity.ToString());

            // Now, act in response to the event.
            if (fe.ferocity < 2)
                Console.WriteLine("This fire in the {0} is no problem.  I'm going to pour some water on it.", fe.room);
            else if (fe.ferocity < 5)
                Console.WriteLine("I'm using FireExtinguisher to put out the fire in the {0}.", fe.room);
            else
                Console.WriteLine("The fire in the {0} is out of control.  I'm calling the fire department!", fe.room);
        }
    }

    public class FireEventTest
    {
        public static void Main()
        {
            // Create an instance of the class that will be firing an event.
            FireAlarmEventEnabledClass myFireAlarm = new FireAlarmEventEnabledClass();

            // Create an instance of the class that will be handling the event. Note that 
            // it receives the class that will fire the event as a parameter. 
            FireAlarmHandlerListener myFireHandlerListener = new FireAlarmHandlerListener(myFireAlarm);

            //use our class to raise a few events and watch them get handled
            myFireAlarm.ActivateFireAlarmHandlerTrigger("Kitchen", 3);
            myFireAlarm.ActivateFireAlarmHandlerTrigger("Study", 1);
            myFireAlarm.ActivateFireAlarmHandlerTrigger("Porch", 5);

            Console.ReadLine();
        }
    }

    public class FireAlarmEventArgs : EventArgs
    {
        public FireAlarmEventArgs(string room, int ferocity)
        {
            this.room = room;
            this.ferocity = ferocity;
        }

        public string room;
        public int ferocity;
    }

}
