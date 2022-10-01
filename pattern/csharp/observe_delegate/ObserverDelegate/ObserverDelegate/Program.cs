using System;

namespace ObserverDelegate
{
    class Program
    {
        static void Main(string[] args)
        {
            Publisher publisher = new Publisher();
            Client1 client1 = new Client1();
            publisher.send += client1.recieved;
            Client2 client2 = new Client2();
            publisher.send += client2.recieved;
            publisher.send += e => Console.WriteLine("Test Client3 message: " + e);
            publisher.sendMessage("ABC");
        }
    }
}
