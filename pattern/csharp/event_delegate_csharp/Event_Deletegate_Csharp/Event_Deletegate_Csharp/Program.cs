using System;

namespace Event_Deletegate_Csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.CancelKeyPress += (s, e) => Console.WriteLine("test");
            Student student1 = new Student(1, "Quoc", "le do");
            student1.nameChange += Student1_nameChange; 
            student1.setName("Dung");

            Student student2 = new Student(2, "Toan", "le do");
            student2.nameChange += (s,e)=> Console.WriteLine("Student 2 change from Toan to " + e.getName());
            student2.setName("My");
        }

        private static void Student1_nameChange(object sender, NameChangeEventArgs e)
        {
            Student student = (Student)sender;
            Console.WriteLine("Sender: " + student.getName());
            Console.WriteLine("EventArgs: " + e.getName());
        }
    }
}
