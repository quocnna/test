using System;
namespace Event_Deletegate_Csharp
{
    public class Student
    {
        public event EventHandler<NameChangeEventArgs> nameChange;
        private int id;
        private String name;
        private String address;

        public Student()
        {
        }

        public Student(int id, String name, String address)
        {
            this.id = id;
            this.name = name;
            this.address = address;
        }

        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
            nameChange?.Invoke(this, new NameChangeEventArgs(name));
        }
    }

    public class NameChangeEventArgs: EventArgs
    {
        private String name;
        public void setName(String name) {
            this.name = name;    
        }
        public String getName()
        {
            return name;
        }

        public NameChangeEventArgs(String name)
        {
            this.name = name;
        }
    }
}
