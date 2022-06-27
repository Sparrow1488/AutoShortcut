using Sparrow.Video.Shortcuts.Builders;

namespace Sparrow.Video.Tests
{
    public class CommandBuilderTests
    {
        [Test]
        public void Test1()
        {
            var expired = "-i {projectPath} -some command -save-to {savePath}";
            var buildResult = new CommandBuilder()
                                       .Insert("-i {projectPath}")
                                       .Insert("-some command")
                                       .InsertLast("-save-to {savePath}")
                                       .BuildCommand();
            StringAssert.AreEqualIgnoringCase(expired, buildResult);
        }
    }
}