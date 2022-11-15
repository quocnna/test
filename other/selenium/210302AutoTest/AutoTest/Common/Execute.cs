using AutoTest.Data;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AutoTest.Core
{
    public class Execute
    {
        #region Fields

        private TestStep _EndStep;
        private TestModel _Model;
        public static IntPtr MainWindowHandler;

        #endregion

        public void Compile(IEnumerable<TestCase> testCases, out string Error, out bool isBefore)
        {
            Error = null;
            isBefore = true;
            Stack<TestCase> stack = new Stack<TestCase>();
            try
            {
                if (testCases != null)
                    foreach (TestCase e in testCases)
                        compile(e, stack);
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                    Error = "Stop";
                else
                {
                    while (stack.Count > 0)
                        stack.Pop().IsExpanded = true;
                    Error = ex.Message.Substring(1);
                    isBefore = ex.Message[0] == '0';
                }
            }
        }
        private void compile(TestCase tc, Stack<TestCase> stack)
        {
            stack.Push(tc);
            if (tc.Status != TestStatus.Blocked)
                tc.Status = TestStatus.Ready;

            if (!tc.Children.IsEmpty())
                foreach (TestCase e in tc.Children)
                    compile(e, stack);

            IEnumerable<TestStep> steps = tc.Steps;
            if (!steps.IsEmpty())
                foreach (var step in steps)
                {
                    step.Status = TestStatus.Ready;
                    string error = step.CompileBeforeRunning();
                    if (!error.IsEmpty())
                    {
                        step.IsSelected = true;
                        throw new Exception("0" + error);
                    }

                    error = step.CompileAfterRunning();
                    if (!error.IsEmpty())
                    {
                        step.IsSelected = true;
                        throw new Exception("1" + error);
                    }
                }

            stack.Pop();
        }

        public void RunStep(TestModel model, TestStep step)
        {
            if (step == null)
                throw new Exception("There are no selected Step to run");

            Run(model, step, step);
        }
        public void RunTestCase(TestModel model, TestCase tc)
        {
            if (tc == null)
                throw new Exception("There are no selected Test Case to run");

            init(model, null);

            MemoryManager memory = new MemoryManager(new GlobalMemory(model.GlobalVariables));
            memory.PushBackToRoot(tc.Parent);
            runTestCase(tc, memory);
        }
        public void Run(TestModel model, TestStep startStep, TestStep endStep)
        {
            if (model.TestCases.IsEmpty())
                return;

            init(model, endStep);
            int start = 0;

            GlobalMemory globalMemory = new GlobalMemory(model.GlobalVariables);

            if (startStep != null)
            {
                MemoryManager memory = new MemoryManager(globalMemory);
                memory.PushBackToRoot(startStep.Parent);

                TestCase tc = startStep.Parent;
                runTestCase(tc, memory, startStep);

                while (tc.Parent != null)
                {
                    int i = tc.Parent.Children.IndexOf(tc);
                    for (int j = i + 1; j < tc.Parent.Children.Count; j++)
                        runTestCase(tc.Parent.Children[j], memory);

                    if (tc.Parent != null)
                        tc = tc.Parent;
                    else
                        break;
                }

                start = model.TestCases.IndexOf(tc) + 1;
            }

            for (int i = start; i < model.TestCases.Count; i++)
                runTestCase(model.TestCases[i], new MemoryManager(globalMemory));
        }

        private void runTestCase(TestCase tc, MemoryManager memory, TestStep start = null)
        {
            if (tc.Status == TestStatus.Blocked)
                return;

            tc.IsExpanded = true;
            tc.LogRowIndex = Log.LineCount;

            Log.WriteSeparate();
            Log.Write("BEGIN TESTCASE ", tc.Title);
            memory.Push(tc);

            if (!tc.Children.IsEmpty())
                foreach (TestCase e in tc.Children)
                    if (e.Status != TestStatus.Blocked)
                        runTestCase(e, memory);

            bool jumpBack = false;
            if (!tc.Steps.IsEmpty())
                for (int i = 0; i < tc.Steps.Count; i++)
                {
                    jumpBack = false;
                    TestStep step = tc.Steps[i];
                    try
                    {
                        if (start != null)
                        {
                            if (!object.Equals(start, step))
                                continue;
                            else
                                start = null;
                        }

                        runStep(step, memory);
                    }
                    catch (ExitTestCaseException ex)
                    {
                        Log.Write("Exit TestCase: ", ex.Message);
                        break;
                    }
                    catch (JumpException ex)
                    {
                        jumpBack = ex.Distance < 1;
                        Log.Write("Jump ", "(" + ex.Distance + "): ", ex.Message);
                        i = Math.Max(-1, i + ex.Distance - 1);
                    }
                    finally
                    {
                        if (!jumpBack && object.Equals(step, _EndStep))
                            throw new EndStepException();
                    }
                }

            if (tc.Status == TestStatus.Executing)
                tc.Status = TestStatus.Pass;

            memory.Pop();
            Log.Write("END TESTCASE ", tc.Title);
        }
        private void runStep(TestStep step, MemoryManager memory)
        {
            if (step.Status == TestStatus.Blocked)
                return;

            step.LogRowIndex = Log.LineCount;
            Log.WriteEmptyLine();
            Log.Write("BEGIN STEP ", step.Title);

            step.Status = TestStatus.Executing;
            Dictionary<string, Value> stepParas = step.GetStepParas();
            StepDataMemory stepData = new StepDataMemory(stepParas, memory);
            StepInstance instance = new StepInstance(memory, stepData, (title) => { createCheckPoint(title, step, memory, stepData); });

            try
            {
                Log.Write("[Before Running...]");
                step.ExecuteBeforeRunning(instance);

                if (!instance.CancelAction)
                {
                    Log.Write("[Execute Action...]");
                    if (step.Action is ICallTestAction)
                        runFunction(step, stepParas, stepData);
                    else if (step.Action != null)
                        step.Action.Execute(instance);
                }

                Log.Write("[After Running...]");
                step.ExecuteAfterRunning(instance);

                if (step.Status == TestStatus.Executing)
                    step.Status = TestStatus.Pass;
            }
            catch (Exception ex)
            {
                ex = ex.InnerException ?? ex;
                if (ex is PassException)
                {
                    step.Status = TestStatus.Pass;
                    Log.Write("Pass: ", ex.Message);
                }
                else if (ex is FailException)
                {
                    step.Status = TestStatus.Fail;
                    Log.Write("Fail: ", ex.Message);
                }
                else if (ex is ExitStepException)
                {
                    Log.Write("EXIT STEP: ", ex.Message);
                }
                else
                    throw ex;
            }
            finally
            {
                if (step.Status != TestStatus.Pass && step.Status != TestStatus.Fail)
                    step.Status = TestStatus.Interrupt;

                Log.Write("END STEP ", step.Title);
            }

        }
        private void runFunction(TestStep step, Dictionary<string, Value> stepParas, StepDataMemory stepData)
        {
            ICallTestAction action = step.Action as ICallTestAction;
            TestCase testCase = TestModel.AllFunctions.FirstOrDefault(e => e.Id == action.CallTestCaseId);
            if (testCase == null)
                throw new Exception("try to call not existed test case");

            TestCase temp = testCase;
            do
            {
                temp.IsExpanded = true;
                temp = temp.Parent;
            }
            while (temp != null);

            MemoryManager newMemory = new MemoryManager(stepData.Memory.GlobalData);
            newMemory.PushBackToRoot(testCase);
            List<string> keys = new List<string>();
            foreach (var e in stepParas)
            {
                keys.Add(e.Key);
                newMemory[e.Key] = stepData[e.Key];
            }

            Log.PaddingLevel++;

            runTestCase(testCase, newMemory);
            step.Status = testCase.Status;
            foreach (string key in keys)
                stepData[key] = newMemory[key];

            testCase.IsExpanded = false;
            Log.PaddingLevel--;
        }

        private void init(TestModel model, TestStep endStep)
        {
            _Model = model;
            _EndStep = endStep;

            _Model.CheckPoints.Clear();

            if (Directory.Exists(model.CheckPointsFolder))
                Directory.Delete(model.CheckPointsFolder, true);
            Thread.Sleep(500);
            Directory.CreateDirectory(model.CheckPointsFolder);

        }
        private void createCheckPoint(string title, TestStep step, MemoryManager memory, StepDataMemory stepData)
        {
            CheckPoint chkPoint = new CheckPoint();
            chkPoint.Title = title.IsEmpty()? ("Step: " + step.Title) : title;

            StringBuilder sb = new StringBuilder();
            if (!step.Parent.Data.IsEmpty())
            {
                sb.AppendLine("TESTCASE PARAMETERS");
                foreach (var e in step.Parent.Data)
                {
                    sb.AppendFormat("\n{0}={1}", e.Name, e.Value, memory[e.Name]);
                    Value val = memory[e.Name];
                    if ((e.Value ?? "").ToString() != val)
                        sb.Append("=> " + (string)val);
                }
            }

            if (!stepData.IsEmpty())
            {
                sb.AppendLine();
                sb.AppendLine("STEP PARAMETERS");
                foreach (var e in stepData)
                {
                    sb.AppendFormat("\n{0}={1}", e.Key, e.Value, stepData[e.Key]);
                    Value val = stepData[e.Key];
                    if ((e.Value ?? "").ToString() != val)
                        sb.Append("=> " + (string)val);
                }
            }

            chkPoint.LogData = sb.ToString().Trim();

            if (TestModel.MainWindowHandle != IntPtr.Zero)
            {
                chkPoint.ApplicationImageFileName = string.Format("App{0}.jpg", _Model.CheckPoints.Count);
                string path = _Model.CheckPointsFolder + "\\" + chkPoint.ApplicationImageFileName;
                Utility.CaptureWindowToFile(TestModel.MainWindowHandle, path, ImageFormat.Jpeg);
            }

            _Model.CheckPoints.Add(chkPoint);
        }
    }
}
