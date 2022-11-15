using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace AutoTest.ExternalFunction
{
    internal abstract class ItemWarp<T>
    {
        public ItemWarp(T value, string name, string key)
        {
            this.Value = value;
            this.Name = name;
            this.Key = key;
        }

        public string Name { get; private set; }
        public string Key { get; private set; }

        public T Value { get; private set; }

        public override string ToString()
        {
            return this.Key;
        }
    }
    internal class AssemblyWarp : ItemWarp<Assembly>
    {
        public AssemblyWarp(Assembly assembly) : base(assembly, assembly.GetName().Name, assembly.FullName)
        {
            this.Classes = new Dictionary<string, ClassWarp>();
        }

        public Dictionary<string, ClassWarp> Classes { get; private set; }
    }
    internal class ClassWarp : ItemWarp<Type>
    {
        public ClassWarp(string name, Type type) :base(type, string.IsNullOrEmpty(name) ? type.Name : name, type.FullName)
        {
            this.Functions = new Dictionary<string, FunctionWarp>();

            foreach (var meth in type.GetMethods())
            {
                AutoTestAttribute att = meth.GetCustomAttribute(typeof(AutoTestAttribute)) as AutoTestAttribute;
                if (att != null)
                {
                    FunctionWarp func = new FunctionWarp(att.FriendlyName, meth);
                    this.Functions[func.Key] = func;
                }
            }
        }

        public Dictionary<string, FunctionWarp> Functions { get; private set; }
    }
    internal class FunctionWarp : ItemWarp<MethodInfo>
    {
        public FunctionWarp(string name, MethodInfo meth) : base(meth, string.IsNullOrEmpty(name) ? meth.Name : name, meth.Name)
        {
            this.Paras = new List<string>();

            foreach (var para in meth.GetParameters())
                this.Paras.Add(para.Name);
        }

        public List<string> Paras { get; private set; }

        public string GetMethodDescription()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ParameterInfo p in this.Value.GetParameters())
                sb.AppendFormat(", {0} {1}", getTypeName(p.ParameterType), p.Name);
            if (sb.Length > 0)
                sb.Remove(0, 2);
            return string.Format("{0} {1}({2})",
                getTypeName(this.Value.ReturnType),
                this.Value.Name,
                sb);
        }
        private static string getTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Type t in type.GetGenericArguments())
                    sb.AppendFormat(", {0}", getTypeName(t));

                if (sb.Length > 0)
                    sb.Remove(0, 2);

                return string.Format("{0}<{1}>", type.Name.Substring(0, type.Name.IndexOf('`')), sb);
            }
            return type.Name;
        }
    }

    public class AutoTestAttribute : Attribute
    {
        public AutoTestAttribute(string friendlyName = null)
        {
            this.FriendlyName = friendlyName;
        }

        public string FriendlyName;
    }

    [Serializable, AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ExternalLibAttribute : AutoTestAttribute
    {
        public ExternalLibAttribute(string friendlyName = null) : base(friendlyName) { }
    }

    [Serializable, AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class UnitTestAttribute : AutoTestAttribute
    {
        public UnitTestAttribute(string friendlyName = null) : base(friendlyName) { }
    }

    [Serializable]
    public class AssertFailedException : Exception
    {
        public AssertFailedException()
        {
        }

        internal AssertFailedException(string message)
            : base(message)
        {
            _Message = message;
        }

        internal AssertFailedException(string message, Exception inner)
            : base(message, inner)
        {
            _Message = message;
        }

        private string _Message;
        public override string Message
        {
            get
            {
                return _Message;
            }
        }
    }
}
