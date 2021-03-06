[assembly: System.CLSCompliant(false)]
namespace TraceURL.TEST
{
    using NUnit.Framework;

    [TestFixture]
    public class TestClass
    {
        [TestCaseSource(nameof(TestSourceString))]
        public void Test_0(string input, int expected)
        {
            StringProcessor sp = new ();

            int actual = sp.IsTimedOut(input);

            Assert.That(actual, Is.EqualTo(expected));
        }

        public static System.Collections.Generic.IEnumerable<object> TestSourceString
        {
            get
            {
                yield return new object[] { "Request timed out.\n, Request timed out.\n, Request timed out.\n, Request timed out.", 4 };
                yield return new object[] { "asdasdasd", 0 };
                yield return new object[] { "Request timed out.", 1 };
                yield return new object[] { "asdRequest timed out.asd", 1 };
            }
        }
    }
}
