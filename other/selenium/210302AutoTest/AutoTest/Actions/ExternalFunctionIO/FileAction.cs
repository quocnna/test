using AutoTest.Data;
using AutoTest.ExternalFunction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTest.ExternalFunctionIO
{
    [ExternalLib("File Action")]
    public class FileAction
    {
        [ExternalLib]
        public static string ReadAllText(Value path)
        {
            string strPath = (string) path;
            return File.Exists(strPath) ? File.ReadAllText(strPath) : string.Empty;
        }

        [ExternalLib]
        public static string[] ReadAllLines(Value path)
        {
            string strPath = (string)path;
            return File.Exists(strPath) ? File.ReadAllLines(strPath) : new string[0];
        }
    }
}
