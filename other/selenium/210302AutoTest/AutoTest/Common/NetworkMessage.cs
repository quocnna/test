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
    internal enum CommandType { Login, GetLast, GetTestCase, AddTestCase, AddTestStep }

    #region TestItemWarp

    [Serializable]
    internal class TestItemWarp
    {
        public TestItemWarp(TestItemBase testItem, RecordStatus status)
        {
            this.TestItem = testItem;
            this.RecordStatus = status;
        }
        public RecordStatus RecordStatus;
        public TestItemBase TestItem;

        public List<TestItemWarp> Children;
    }

    #endregion

    #region Messages

    [Serializable]
    internal class MessageBase
    {
        public MessageBase(CommandType command, string id)
        {
            this.Command = command;
            this.Id = id;
        }
        public readonly CommandType Command;
        public readonly string Id;
    }
    [Serializable]
    internal class MessageBase<T> : MessageBase
    {
        public MessageBase(CommandType command, string id, T value)
            : base(command, id)
        {
            Value = value;
        }

        public T Value;
    }
    [Serializable]
    internal class MessageLogin : MessageBase
    {
        public MessageLogin(string id, string userName, string password)
            : base(CommandType.Login, id)
        {
            this.UserName = userName;
            this.Password = password;
        }

        public string UserName;
        public string Password;
    }

    [Serializable]
    internal class ReplyLogin
    {
        public ReplyLogin(string sessionId)
        {
            this.sessionId = sessionId;
        }

        public readonly string sessionId;
    }
    [Serializable]
    internal class MessageGetLast : MessageBase
    {
        public MessageGetLast(string sessionId, Dictionary<string, RecordStatus> existedTestCaseIds, Dictionary<string, RecordStatus> existedSteps)
            : base(CommandType.GetLast, sessionId)
        {
            this.ExistedTestCases = existedTestCaseIds;
            this.ExistedSteps = existedSteps;
        }

        public Dictionary<string, RecordStatus> ExistedTestCases;
        public Dictionary<string, RecordStatus> ExistedSteps;
    }
    [Serializable]
    internal class ReplyGetLast
    {
        public List<TestItemWarp> TestCases = new List<TestItemWarp>();
        public List<TestItemWarp> Steps = new List<TestItemWarp>();
    }

    internal class MessageGetTestCase : MessageBase
    {
        public MessageGetTestCase(string sessionId)
            : base(CommandType.GetTestCase, sessionId)
        {
        }
    }
    internal class MessageAddTestCase : MessageBase<TestCase>
    {
        public MessageAddTestCase(string sessionId, TestCase value)
            : base(CommandType.AddTestCase, sessionId, value)
        {
        }
    }
    internal class MessageAddTestStep : MessageBase<TestStep>
    {
        public MessageAddTestStep(string sessionId, TestStep value)
            : base(CommandType.AddTestStep, sessionId, value)
        {
        }
    }

    #endregion
}
