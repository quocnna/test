using System;
namespace ObserverDelegate
{
    public class Client2: IObserve
    {
        public void recieved(string ms)
        {
            Console.WriteLine("Client2 send ms: "+ ms);
        }
    }
}
