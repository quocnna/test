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
    public class NetworkServer
    {
        #region Inner Class
        [Serializable]
        private class MessageStop { }

        #endregion

        public NetworkServer(TestModel model, int port = 7373)
        {
            Model = model;
            _Port = port;
        }

        private int _Port;
        private IPAddress _IP;
        private TcpListener _Listener;
        private Thread _ThreadRun;
        private HashSet<string> _UserSessions = new HashSet<string>();
        public readonly TestModel Model;

        public void Start()
        {
            if (_ThreadRun == null)
                _ThreadRun = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        _IP = Dns.GetHostAddresses(Dns.GetHostName()).Where(e => e.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
                        _Listener = new TcpListener(_IP, _Port);
                        _Listener.Start();

                        execute();
                    }
                    finally
                    {
                        _Listener.Stop();
                        MessageBox.Show("Service is stoped", "Sharing file");
                    }
                }));

            if (_ThreadRun.ThreadState != ThreadState.Running)
                _ThreadRun.Start();
            MessageBox.Show("Service is started", "Sharing file");
        }
        public void Stop()
        {
            if (_ThreadRun != null)
                using (TcpClient socket = new TcpClient())
                {
                    socket.Connect(_IP, _Port);
                    using (NetworkStream stream = socket.GetStream())
                    {
                        IFormatter formatter = new BinaryFormatter();
                        using (MemoryStream memory = new MemoryStream())
                        {
                            formatter.Serialize(memory, new MessageStop());
                            byte[] buffer = memory.ToArray();
                            stream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            _ThreadRun = null;
        }

        private void execute()
        {
            while (true)
            {
                TcpClient client = _Listener.AcceptTcpClient();
                using (NetworkStream stream = client.GetStream())
                {
                    while (!stream.DataAvailable)
                        Thread.Sleep(5);

                    IFormatter formatter = new BinaryFormatter();
                    try
                    {
                        object o = formatter.Deserialize(stream);
                        if (o is MessageStop)
                            return;

                        MessageBase message = o as MessageBase;
                        if (message != null && (message.Command == CommandType.Login || _UserSessions.Contains(message.Id ?? "")))
                            switch (message.Command)
                            {
                                case CommandType.Login: reply(stream, processLogin(message as MessageLogin)); break;
                                case CommandType.GetLast: reply(stream, processGetLast(message as MessageGetLast)); break;
                                case CommandType.GetTestCase: reply(stream, processGetTestCase(message as MessageGetTestCase)); break;
                                case CommandType.AddTestCase: reply(stream, processAddTestCase(message as MessageAddTestCase)); break;
                                case CommandType.AddTestStep: reply(stream, processAddTestStep(message as MessageAddTestStep)); break;
                            }
                        stream.Close();
                    }
                    catch
                    {
                    }
                }
            }
        }
        public void reply(NetworkStream stream, object value)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memory = new MemoryStream();
            formatter.Serialize(memory, value);
            byte[] data = memory.ToArray();
            stream.Write(data, 0, data.Length);
        }

        private ReplyLogin processLogin(MessageLogin message)
        {
            //Check
            //Network user = _Users.Where(e => string.Compare(e.UserName, message.UserName, true) == 0 && e.Password == message.Password).FirstOrDefault();
            string id = Guid.NewGuid().ToString(); //string.Empty;
            _UserSessions.Add(id);
            //if (user != null)
            //{
            //    id = user.SessionId;
            //    _UserSessions.Add(id);
            //}
            return new ReplyLogin(id);
        }
        private ReplyGetLast processGetLast(MessageGetLast message)
        {
            ReplyGetLast res = new ReplyGetLast();
            foreach (TestCase e in Model.TestCases)
                processGetLast(e, message, new Dictionary<string, TestItemWarp>(), res.TestCases, res.Steps);

            return res;
        }
        private void processGetLast(TestCase tc, MessageGetLast message, Dictionary<string, TestItemWarp> caches, List<TestItemWarp> testCaseChangedSet, List<TestItemWarp> testStepChangedSet)
        {
            RecordStatus recordStatus = message.ExistedTestCases.ContainsKey(tc.Id) ? message.ExistedTestCases[tc.Id] : RecordStatus.New;
            if (recordStatus != RecordStatus.Unchanged)
            {
                TestItemWarp warp = new TestItemWarp(tc, recordStatus);
                caches[tc.Id] = warp;
                if (tc.Parent != null && caches.ContainsKey(tc.Parent.Id))
                {
                    TestItemWarp parent = caches[tc.Parent.Id];
                    if (parent.Children == null)
                        parent.Children = new List<TestItemWarp>();
                    parent.Children.Add(warp);
                }
                else
                    testCaseChangedSet.Add(warp);
            }

            foreach (TestStep step in tc.Steps)
            {
                recordStatus = message.ExistedSteps.ContainsKey(step.Id) ? message.ExistedSteps[step.Id] : RecordStatus.New;
                if (recordStatus != RecordStatus.Unchanged)
                    testStepChangedSet.Add(new TestItemWarp(step, recordStatus));
            }

            foreach (TestCase child in tc.Children)
                processGetLast(child, message, caches, testCaseChangedSet, testStepChangedSet);
        }

        private TestCase processGetTestCase(MessageGetTestCase message)
        {
            return new TestCase();
        }

        private string processAddTestCase(MessageAddTestCase message)
        {
            return null;
        }

        private string processAddTestStep(MessageAddTestStep message)
        {
            return null;
        }
    }
}
