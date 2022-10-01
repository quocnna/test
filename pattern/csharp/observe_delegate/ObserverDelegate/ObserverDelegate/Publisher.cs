using System;
namespace ObserverDelegate
{
    public class Publisher
    {
        public delegate void sendMS(String ms);
        public event sendMS send;


        public void sendMessage(String ms)
        {
            Console.WriteLine("Publisher send ms");
            send.Invoke(ms);
        }
    }
}
