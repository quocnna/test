using System;
namespace ObserverDelegate
{
    public class Client1: IObserve
    {
        public void recieved(string ms)
        {
            Console.WriteLine("Client 1 send message: " + ms);
        }
    }
}
