using System.IO;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.ResourceManagement {
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class FileMessageProviderTest:ServiceBaseTest {
        public IMessageProvider MailMessageProvider { get; set; }

        [Test]
        public void TestLoadResource() {
            string renderedMessage = MailMessageProvider.RenderMessage("test", new ModelMap());

            Assert.IsNotNull(renderedMessage);
            StringAssert.StartsWith("Subject: Testbetreff", renderedMessage);
        }

        [Test]
        public void TestRenderMessageWithoutCulture() {
            string renderedMessage = MailMessageProvider.RenderMessage("TestWithoutCulture", new ModelMap());

            Assert.IsNotNull(renderedMessage);
            StringAssert.StartsWith("Subject: TestWithoutCulture", renderedMessage);
        }

        [Test]
        public void TestRenderNotExistingResource() {
            Assert.Throws<FileNotFoundException>(() => MailMessageProvider.RenderMessage("TestNotExistingResource", new ModelMap()));
        }
    }
}