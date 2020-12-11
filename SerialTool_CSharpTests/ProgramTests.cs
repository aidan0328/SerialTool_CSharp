using NUnit.Framework;
using SerialTool_CSharp;

namespace SerialTool_CSharp.Tests {
    [TestFixture()]
    public class ProgramTests {
        [Test()]
        public void MainTest() {
            Program.Main();
        }
    }
}