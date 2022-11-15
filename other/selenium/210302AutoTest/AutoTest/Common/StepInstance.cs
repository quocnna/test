using AutoTest.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTest.Core
{
    #region Exceptions

    public class ExceptionBase : Exception
    {
        public ExceptionBase(string message = null)
        {
            _Message = message;
        }
        private string _Message;
        public override string Message
        {
            get
            {

                return _Message == null ? string.Empty : _Message;
            }
        }
    }
    public class PassException : ExceptionBase
    {
        public PassException(string message) : base(message) { }
    }
    public class FailException : ExceptionBase
    {
        public FailException(string message) : base(message) { }
    }
    public class ExitStepException : ExceptionBase
    {
        public ExitStepException(string message) : base(message) { }
    }
    public class ExitTestCaseException : ExceptionBase
    {
        public ExitTestCaseException(string message) : base(message) { }
    }
    public class ExitAndStopException : ExceptionBase
    {
        public ExitAndStopException(string message) : base(message) { }
    }
    public class JumpException : ExceptionBase
    {
        public JumpException(int distance): base()
        {
            this.Distance = distance;
        }
        public int Distance;
    }
    public class EndStepException : ExceptionBase
    {
        public EndStepException() : base() { }
    }

    #endregion

    #region StepInstance

    public class StepInstance : IStepInstance
    {
        public StepInstance(ITableMemory data, IMemory stepData, Action<string> createCheckPoint)
        {
            Data = data;
            StepData = stepData;
            _CreateCheckPoint = createCheckPoint;
        }

        #region Fields

        private Action<string> _CreateCheckPoint;

        #endregion

        #region IStepInstance

        public bool CancelAction;
        public object Result {get; set;}
        public virtual ITableMemory Data { get; private set; }
        public virtual IMemory StepData { get; private set; }

        public void Pass(string message = null)
        {
            throw new PassException(message);
        }
        public void Fail(string message = null)
        {
            throw new FailException(message);
        }

        public void ExitCurrentStep(string message = null)
        {
            throw new ExitStepException(message);
        }
        public void ExitCurrentTestCase(string message = null)
        {
            throw new ExitTestCaseException(message);
        }
        public void ExitAndStop(string message = null)
        {
            throw new ExitAndStopException(message);
        }
        public void ExitToAfterRunning()
        {
            CancelAction = true;
        }

        public void Jump(int distance)
        {
            throw new JumpException(distance);
        }

        public void Sleep(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
        }

        public void Log(params object[] values)
        {
            Core.Log.Write(values);
        }

        public void CreateCheckPoint(string title = null)
        {
            if (_CreateCheckPoint != null)
                _CreateCheckPoint(title);
        }

        #endregion
    }

    #endregion
}
