using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest.Data
{
    public class User
    {
        private string _Status = string.Empty;
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private string _UserName = string.Empty;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private string _Password = string.Empty;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private string _IP = string.Empty;
        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }

        public string SessionId
        {
            get;
            set;
        }
    }
}
