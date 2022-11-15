using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AutoTest.Data
{
    #region TestStatus

    [Serializable]
    public enum TestStatus
    {
        [XmlEnum(Name = "Stop")]
        Ready = 0,
        [XmlEnum(Name = "Executing")]
        Executing = 1,
        [XmlEnum(Name = "Blocked")]
        Blocked = 2,
        [XmlEnum(Name = "Pass")]
        Pass = 3,
        [XmlEnum(Name = "Fail")]
        Fail = 4,
        [XmlEnum(Name = "Interrupt")]
        Interrupt
    }

    #endregion

    #region RecordStatus

    [Serializable]
    public enum RecordStatus
    {
        New = 1,
        Unchanged = 2,
        Changed = 4,
        Delete = 8
    }

    #endregion

    #region IMemory

    public interface IMemory : IEnumerable<KeyValuePair<string, Value>>
    {
        Value this[string variableName] { get; set; }
    }

    #endregion

    #region ITableMemory

    public interface ITableMemory : IMemory
    {
        bool MoveNext(string variableName);
        void MoveFirst(string variableName);
        void MoveLast(string variableName);
        int GetCurrentRowIndex(string variableName);
        int GetTotalRecords(string variableName);
        Value this[string variableName, int rowIndex] { get; set; }
    }

    #endregion

    #region IStepInstance

    public interface IStepInstance
    {
        object Result { get; set; }
        ITableMemory Data { get; }
        IMemory StepData { get; }

        void Pass(string message = null);
        void Fail(string message = null);

        void ExitCurrentStep(string message = null);
        void ExitCurrentTestCase(string message = null);
        void ExitAndStop(string message = null);
        void ExitToAfterRunning();

        void Jump(int step);
        void Sleep(int millisecondsTimeout);
        void Log(params object[] values);

        void CreateCheckPoint(string title = null);
    }

    #endregion

    #region ICallTestAction
    public interface ICallTestAction
    {
        string CallTestCaseId { get; }
    }

    #endregion

    #region IPersistent

    public interface IPersistent
    {
        XElement SaveXml();
        void LoadXml(XElement parentNode);
    }

    #endregion
}
