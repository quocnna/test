using System;

namespace Event_Delegate
{
    public delegate void NameChange(String name);

    class Program
    {
        static void Main(string[] args)
        {
            Student student1 = new Student(1,"Quoc", "Le Do");
            student1.nameChange += Student1_nameChange;
            student1.setName("Dung");

            Student student2 = new Student(2, "Toan", "Le Do");
            student2.nameChange += e => Console.WriteLine("Student change is: "+ e);
            student2.setName("My");
        }

        private static void Student1_nameChange(string name)
        {
            Console.WriteLine("Student change is " + name);
        }
    }
}
