using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AutoTest.ExternalFunction
{
    public static class Assert
    {
        internal static void HandleFail(string assertionName, string message, params object[] parameters)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(message))
            {
                if (parameters == null)
                {
                    str = message;
                }
                else
                {
                    str = string.Format(CultureInfo.CurrentCulture, message, parameters);
                }
            }
            throw new AssertFailedException((assertionName ?? "") + " " + str);
        }

        public static void AreEqual(object expected, object actual)
        {
            AreEqual(expected, actual, string.Empty, null);
        }

        public static void AreEqual<T>(T expected, T actual)
        {
            AreEqual<T>(expected, actual, string.Empty, null);
        }

        public static void AreEqual(double expected, double actual, double delta)
        {
            AreEqual(expected, actual, delta, string.Empty, null);
        }

        public static void AreEqual(object expected, object actual, string message)
        {
            AreEqual(expected, actual, message, null);
        }

        public static void AreEqual(float expected, float actual, float delta)
        {
            AreEqual(expected, actual, delta, string.Empty, null);
        }

        public static void AreEqual(string expected, string actual, bool ignoreCase)
        {
            AreEqual(expected, actual, ignoreCase, string.Empty, (object[])null);
        }

        public static void AreEqual<T>(T expected, T actual, string message)
        {
            AreEqual<T>(expected, actual, message, null);
        }

        public static void AreEqual(double expected, double actual, double delta, string message)
        {
            AreEqual(expected, actual, delta, message, null);
        }

        public static void AreEqual(object expected, object actual, string message, params object[] parameters)
        {
            AreEqual<object>(expected, actual, message, parameters);
        }

        public static void AreEqual(float expected, float actual, float delta, string message)
        {
            AreEqual(expected, actual, delta, message, null);
        }

        public static void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture)
        {
            AreEqual(expected, actual, ignoreCase, culture, string.Empty, null);
        }

        public static void AreEqual(string expected, string actual, bool ignoreCase, string message)
        {
            AreEqual(expected, actual, ignoreCase, message, (object[])null);
        }

        public static void AreEqual<T>(T expected, T actual, string message, params object[] parameters)
        {
            if (!object.Equals(expected, actual))
                HandleFail("Assert.AreEqual", message, parameters);
        }

        public static void AreEqual(double expected, double actual, double delta, string message, params object[] parameters)
        {
            if (Math.Abs((double)(expected - actual)) > delta)
                HandleFail("Assert.AreEqual", message, parameters);
        }

        public static void AreEqual(float expected, float actual, float delta, string message, params object[] parameters)
        {
            if (Math.Abs((float)(expected - actual)) > delta)
                HandleFail("Assert.AreEqual", message, parameters);
        }

        public static void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture, string message)
        {
            AreEqual(expected, actual, ignoreCase, culture, message, null);
        }

        public static void AreEqual(string expected, string actual, bool ignoreCase, string message, params object[] parameters)
        {
            AreEqual(expected, actual, ignoreCase, CultureInfo.InvariantCulture, message, parameters);
        }

        public static void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture, string message, params object[] parameters)
        {
            CheckParameterNotNull(culture, "Assert.AreEqual", "culture", string.Empty, new object[0]);
            if (string.Compare(expected, actual, ignoreCase, culture) != 0)
                HandleFail("Assert.AreEqual", message, parameters);
        }

        public static void AreNotEqual(object notExpected, object actual)
        {
            AreNotEqual(notExpected, actual, string.Empty, null);
        }

        public static void AreNotEqual<T>(T notExpected, T actual)
        {
            AreNotEqual<T>(notExpected, actual, string.Empty, null);
        }

        public static void AreNotEqual(double notExpected, double actual, double delta)
        {
            AreNotEqual(notExpected, actual, delta, string.Empty, null);
        }

        public static void AreNotEqual(object notExpected, object actual, string message)
        {
            AreNotEqual(notExpected, actual, message, null);
        }

        public static void AreNotEqual(float notExpected, float actual, float delta)
        {
            AreNotEqual(notExpected, actual, delta, string.Empty, null);
        }

        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase)
        {
            AreNotEqual(notExpected, actual, ignoreCase, string.Empty, (object[])null);
        }

        public static void AreNotEqual<T>(T notExpected, T actual, string message)
        {
            AreNotEqual<T>(notExpected, actual, message, null);
        }

        public static void AreNotEqual(double notExpected, double actual, double delta, string message)
        {
            AreNotEqual(notExpected, actual, delta, message, null);
        }

        public static void AreNotEqual(object notExpected, object actual, string message, params object[] parameters)
        {
            AreNotEqual<object>(notExpected, actual, message, parameters);
        }

        public static void AreNotEqual(float notExpected, float actual, float delta, string message)
        {
            AreNotEqual(notExpected, actual, delta, message, null);
        }

        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture)
        {
            AreNotEqual(notExpected, actual, ignoreCase, culture, string.Empty, null);
        }

        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message)
        {
            AreNotEqual(notExpected, actual, ignoreCase, message, (object[])null);
        }

        public static void AreNotEqual<T>(T notExpected, T actual, string message, params object[] parameters)
        {
            if (object.Equals(notExpected, actual))
                HandleFail("Assert.AreNotEqual", message, parameters);
        }

        public static void AreNotEqual(double notExpected, double actual, double delta, string message, params object[] parameters)
        {
            if (Math.Abs((double)(notExpected - actual)) <= delta)
                HandleFail("Assert.AreNotEqual", message, parameters);
        }

        public static void AreNotEqual(float notExpected, float actual, float delta, string message, params object[] parameters)
        {
            if (Math.Abs((float)(notExpected - actual)) <= delta)
                HandleFail("Assert.AreNotEqual", message, parameters);
        }

        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture, string message)
        {
            AreNotEqual(notExpected, actual, ignoreCase, culture, message, null);
        }

        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message, params object[] parameters)
        {
            AreNotEqual(notExpected, actual, ignoreCase, CultureInfo.InvariantCulture, message, parameters);
        }

        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture, string message, params object[] parameters)
        {
            CheckParameterNotNull(culture, "Assert.AreNotEqual", "culture", string.Empty, new object[0]);
            if (string.Compare(notExpected, actual, ignoreCase, culture) == 0)
                HandleFail("Assert.AreNotEqual", message, parameters);
        }

        public static void AreNotSame(object notExpected, object actual)
        {
            AreNotSame(notExpected, actual, string.Empty, null);
        }

        public static void AreNotSame(object notExpected, object actual, string message)
        {
            AreNotSame(notExpected, actual, message, null);
        }

        public static void AreNotSame(object notExpected, object actual, string message, params object[] parameters)
        {
            if (object.ReferenceEquals(notExpected, actual))
            {
                HandleFail("Assert.AreNotSame", message, parameters);
            }
        }

        public static void AreSame(object expected, object actual)
        {
            AreSame(expected, actual, string.Empty, null);
        }

        public static void AreSame(object expected, object actual, string message)
        {
            AreSame(expected, actual, message, null);
        }

        public static void AreSame(object expected, object actual, string message, params object[] parameters)
        {
            if (!object.ReferenceEquals(expected, actual))
                HandleFail("Assert.AreSame", message, parameters);
        }

        internal static void CheckParameterNotNull(object param, string assertionName, string parameterName, string message, params object[] parameters)
        {
            if (param == null)
                HandleFail(assertionName, message, parameters);
        }

        public static void Fail()
        {
            Fail(string.Empty, null);
        }

        public static void Fail(string message)
        {
            Fail(message, null);
        }

        public static void Fail(string message, params object[] parameters)
        {
            HandleFail("Assert.Fail", message, parameters);
        }

        public static void IsFalse(bool condition)
        {
            IsFalse(condition, string.Empty, null);
        }

        public static void IsFalse(bool condition, string message)
        {
            IsFalse(condition, message, null);
        }

        public static void IsFalse(bool condition, string message, params object[] parameters)
        {
            if (condition)
            {
                HandleFail("Assert.IsFalse", message, parameters);
            }
        }

        public static void IsInstanceOfType(object value, Type expectedType)
        {
            IsInstanceOfType(value, expectedType, string.Empty, null);
        }

        public static void IsInstanceOfType(object value, Type expectedType, string message)
        {
            IsInstanceOfType(value, expectedType, message, null);
        }

        public static void IsInstanceOfType(object value, Type expectedType, string message, params object[] parameters)
        {
            if (expectedType == null || !expectedType.IsInstanceOfType(value))
                HandleFail("Assert.IsInstanceOfType", message, parameters);
        }

        public static void IsNotInstanceOfType(object value, Type wrongType)
        {
            IsNotInstanceOfType(value, wrongType, string.Empty, null);
        }

        public static void IsNotInstanceOfType(object value, Type wrongType, string message)
        {
            IsNotInstanceOfType(value, wrongType, message, null);
        }

        public static void IsNotInstanceOfType(object value, Type wrongType, string message, params object[] parameters)
        {
            if (wrongType == null || ((value != null) && wrongType.IsInstanceOfType(value)))
                HandleFail("Assert.IsNotInstanceOfType", message, parameters);
        }

        public static void IsNotNull(object value)
        {
            IsNotNull(value, string.Empty, null);
        }

        public static void IsNotNull(object value, string message)
        {
            IsNotNull(value, message, null);
        }

        public static void IsNotNull(object value, string message, params object[] parameters)
        {
            if (value == null)
            {
                HandleFail("Assert.IsNotNull", message, parameters);
            }
        }

        public static void IsNull(object value)
        {
            IsNull(value, string.Empty, null);
        }

        public static void IsNull(object value, string message)
        {
            IsNull(value, message, null);
        }

        public static void IsNull(object value, string message, params object[] parameters)
        {
            if (value != null)
            {
                HandleFail("Assert.IsNull", message, parameters);
            }
        }

        public static void IsTrue(bool condition)
        {
            IsTrue(condition, string.Empty, null);
        }

        public static void IsTrue(bool condition, string message)
        {
            IsTrue(condition, message, null);
        }

        public static void IsTrue(bool condition, string message, params object[] parameters)
        {
            if (!condition)
            {
                HandleFail("Assert.IsTrue", message, parameters);
            }
        }

        public static string ReplaceNullChars(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            List<int> list = new List<int>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\0')
                {
                    list.Add(i);
                }
            }
            if (list.Count <= 0)
            {
                return input;
            }
            StringBuilder builder = new StringBuilder(input.Length + list.Count);
            int startIndex = 0;
            foreach (int num3 in list)
            {
                builder.Append(input.Substring(startIndex, num3 - startIndex));
                builder.Append(@"\0");
                startIndex = num3 + 1;
            }
            builder.Append(input.Substring(startIndex));
            return builder.ToString();
        }
    }
}
