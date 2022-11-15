using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest.Data
{
    [Serializable]
    public abstract class TestItemBase : NotifyPropertyChangedBase
    {
        public TestItemBase()
        {
            this.Status = TestStatus.Ready;
        }

        public int LogRowIndex;

        protected string _Id;
        public string Id
        {
            get
            {
                if (_Id == null)
                    _Id = Guid.NewGuid().ToString();
                return _Id;
            }
        }

        protected string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    onPropertyChange("Title");
                    RecordStatus = RecordStatus.Changed;
                }
            }
        }

        protected int _Index;
        public int Index
        {
            get { return _Index; }
            set
            {
                if (_Index != value)
                {
                    _Index = value;
                    onPropertyChange("Index");
                    RecordStatus = RecordStatus.Changed;
                }
            }
        }

        protected TestStatus _Status;
        public TestStatus Status
        {
            get { return _Status; }
            set
            {
                if (_Status == TestStatus.Fail && (value != TestStatus.Blocked && value != TestStatus.Ready))
                    return;

                _Status = value;
                if (value == TestStatus.Blocked)
                    StatusImage = "pack://application:,,,/Images/lock.png";
                else if (value == TestStatus.Ready)
                    StatusImage = "pack://application:,,,/Images/ready.png";
                else if (value == TestStatus.Executing)
                    StatusImage = "pack://application:,,,/Images/process.png";
                else if (value == TestStatus.Pass)
                    StatusImage = "pack://application:,,,/Images/pass.png";
                else if (value == TestStatus.Fail)
                    StatusImage = "pack://application:,,,/Images/fail.png";
                else if (value == TestStatus.Interrupt)
                    StatusImage = "pack://application:,,,/Images/error.png";

                onPropertyChange("Status");
                onPropertyChange("StatusImage");

                if (value != TestStatus.Blocked && value != TestStatus.Ready && value != TestStatus.Interrupt)
                {
                    if (this.Parent != null && this.Parent.Status != TestStatus.Blocked && this.Parent.Status != TestStatus.Fail)
                        this.Parent.Status = value;
                }
            }
        }
        public string StatusImage { get; private set; }

        protected TestCase _Parent;
        public TestCase Parent
        {
            get { return _Parent; }
            set
            {
                if (_Parent != value)
                {
                    _Parent = value;
                    RecordStatus = RecordStatus.Changed;
                }
            }
        }

        protected RecordStatus _RecordStatus;
        public RecordStatus RecordStatus
        {
            get { return _RecordStatus; }
            set
            {
                if (value != RecordStatus.Changed || _RecordStatus == RecordStatus.Unchanged)
                    _RecordStatus = value;
            }
        }

        protected override void onPropertyChange(string propertyName)
        {
            base.onPropertyChange(propertyName);
        }

        public abstract void OnVariableNameChanged(string oldValue, string newValue);

        public override bool Equals(object obj)
        {
            return base.Equals(obj) || (obj is TestItemBase && string.Equals(this.Id, (obj as TestItemBase).Id));
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
