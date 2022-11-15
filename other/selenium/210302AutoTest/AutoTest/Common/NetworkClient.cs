using AutoTest.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections.Specialized;


namespace AutoTest.Core
{
    public class NetworkClient
    {
        public NetworkClient(string ipServerToConnect, int portServerToConnect = 7373)
        {
            _IpServerToConnect = ipServerToConnect;
            _PortServerToConnect = portServerToConnect;
        }

        private string _IpServerToConnect;
        private int _PortServerToConnect;
        private string _SessionId;

        public string Login(string userName, string password)
        {
            try
            {
                ReplyLogin reply = getData<ReplyLogin>(new MessageLogin(null, userName, password));
                _SessionId = reply.sessionId;
                return !string.IsNullOrEmpty(_SessionId) ? null : "Username or password invalid.";
            }
            catch
            {
            }
            return "Can't connect to server";
        }
        public void GetLast(TestModel model)
        {
            Dictionary<string, RecordStatus> existedTestCases = new Dictionary<string, RecordStatus>();
            Dictionary<string, RecordStatus> existedSteps = new Dictionary<string, RecordStatus>();
            Dictionary<string, TestItemBase> existedItems = new Dictionary<string, TestItemBase>();

            Action<TestCase> buildMessage = null;
            buildMessage = (tc) =>
                {
                    existedItems[tc.Id] = tc;
                    if (tc.RecordStatus !=  RecordStatus.New)
                        existedTestCases[tc.Id] = tc.RecordStatus;

                    foreach (TestStep e in tc.Steps)
                    {
                        existedItems[e.Id] = e;
                        if (e.RecordStatus != RecordStatus.New)
                            existedSteps[e.Id] = e.RecordStatus;
                    }

                    foreach (TestCase child in tc.Children)
                        buildMessage(child);
                };
            MessageGetLast message = new MessageGetLast(_SessionId, existedTestCases, existedSteps);

            try
            {
                ReplyGetLast reply = getData<ReplyGetLast>(message);

                #region Update TestCases

                Action<List<TestItemWarp>> updateTestCase = null;
                updateTestCase = (items) =>
                     {
                         foreach (TestItemWarp e in items)
                         {
                             TestCase tc = e.TestItem as TestCase;

                             if (e.RecordStatus == RecordStatus.New)
                             {
                                 IList list = getTestCaseCollection(tc, model, existedItems);
                                 list.Insert(tc.Index, tc);
                             }
                             else if (e.RecordStatus == RecordStatus.Changed)
                             {
                                 TestCase old = existedItems[tc.Id] as TestCase;
                                 if (old != null)
                                 {
                                     ObservableCollection<TestCase> list = getTestCaseCollection(old, model, existedItems);
                                     if (old.Parent != tc.Parent)
                                     {
                                         list.Remove(old);

                                         list = getTestCaseCollection(tc, model, existedItems);
                                         list.Insert(tc.Index, tc);
                                     }
                                     else if (old.Index != tc.Index)
                                             list.Move(old.Index, tc.Index);

                                     list[tc.Index] = tc;
                                 }
                             }

                             if (e.Children != null)
                                 updateTestCase(e.Children);

                             tc.RecordStatus = RecordStatus.Unchanged;
                         }
                     };

                updateTestCase(reply.TestCases);

                #endregion

                #region Update Steps

                foreach (TestItemWarp e in reply.Steps)
                {
                    TestStep step = e.TestItem as TestStep;
                    TestCase parent = existedItems.ContainsKey(step.Parent.Id) ? existedItems[step.Parent.Id] as TestCase : null;
                    if (parent == null)
                        continue;

                    if (e.RecordStatus == RecordStatus.New)
                        parent.Steps.Insert(step.Index, step);
                    else if (e.RecordStatus == RecordStatus.Changed && existedItems.ContainsKey(step.Id))
                    {
                        TestStep old = existedItems[step.Id] as TestStep;

                        if (old.Parent.Id != step.Parent.Id && existedItems.ContainsKey(old.Parent.Id))
                        {
                            (existedItems[old.Parent.Id] as TestCase).Steps.RemoveAt(old.Index);
                            parent.Steps.Insert(step.Index, step);
                        }
                        else
                        {
                            if (old.Index != step.Index)
                                parent.Steps.Move(old.Index, step.Index);
                            parent.Steps[step.Index] = step;
                        }
                    }

                    step.RecordStatus = RecordStatus.Unchanged;
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Last Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private T getData<T>(MessageBase message)
        {
            try
            {
                using (TcpClient socket = new TcpClient())
                {
                    socket.Connect(_IpServerToConnect, _PortServerToConnect);

                    using (NetworkStream stream = socket.GetStream())
                    {
                        sendMessage(stream, message);
                        return receiveMessage<T>(stream);
                    }
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                MessageBox.Show("Can't connect to server", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return default(T);
            }
        }
        public void sendMessage(NetworkStream stream, object value)
        {
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream memory = new MemoryStream())
            {
                formatter.Serialize(memory, value);
                byte[] buffer = memory.ToArray();
                stream.Write(buffer, 0, buffer.Length);
            }
        }
        private T receiveMessage<T>(NetworkStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream memory = new MemoryStream())
            {
                byte[] buffer = new byte[1024];
                int i = 0;
                while ((i = stream.Read(buffer, 0, buffer.Length)) > 0)
                    memory.Write(buffer, 0, i);
                memory.Seek(0, SeekOrigin.Begin);

                return (T)formatter.Deserialize(memory);
            }
        }

        private ObservableCollection<TestCase> getTestCaseCollection(TestCase tc, TestModel model, Dictionary<string, TestItemBase> allITems)
        {
            TestCase parent = tc.Parent == null || !allITems.ContainsKey(tc.Parent.Id) ? null : allITems[tc.Parent.Id] as TestCase;
            return parent == null ? model.TestCases : parent.Children;
        }
    }
}
