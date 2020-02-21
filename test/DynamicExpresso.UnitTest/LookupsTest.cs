using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicExpresso.UnitTest
{
    [TestFixture]
    public class LookupsTest
    {
        delegate Dictionary<int, string> LookupFunc(string name, params object[] parameters);

        [Test]
        public void LookupTest()
        {
            Interpreter interpreter = new Interpreter();

            interpreter.SetFunction("lookup", new LookupFunc(Lookup));

            string exp = $"lookup(\"test\", 1,5,6,26,32, \"in1\", \"in2\", \"in3\")";

            object lookupResult = interpreter.Eval($"{exp}");

            interpreter.SetVariable("testLookupResult", lookupResult);

            Assert.AreEqual("Test 5 parameter", interpreter.Eval($"testLookupResult[5]").ToString());
        }

        private Dictionary<int, string> Lookup(string name, params object[] parameters)
        {
            var inParameters = parameters.Where(x => x.GetType() == typeof(string)).Select(x => x.ToString());
            var outParameters = parameters.Where(x => x.GetType() == typeof(int)).Select(x => Convert.ToInt32(x));

            Dictionary<int, string> lookupMockResult = new Dictionary<int, string>();

            foreach (var outParameter in outParameters)
            {
                lookupMockResult.Add(outParameter, $"Test {outParameter} parameter");
            }

            return lookupMockResult;
        }
    }
}
