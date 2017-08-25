using FluentAssertions;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {

    [TestFixture]
    public class DocumentRepositoryTest {
        

        [Test]
        public void TestOnlyFilename() {
            
            DocumentRepository documentRepository = new DocumentRepository("C:\\Temp\\ImmoTest\\", null, "C:\\Temp\\ImmoUploadTest");
            string filename = "bild1.jpg";
            string fullFileName = documentRepository.GetFullFileName(filename);
            fullFileName.ShouldBeEquivalentTo("C:\\Temp\\ImmoTest\\" + filename);
        }

        [Test]
        public void TestOnlyFilenameWithPath() {
            DocumentRepository documentRepository = new DocumentRepository("C:\\Temp\\ImmoTest\\", null, "C:\\Temp\\ImmoUploadTest");
            string filename = "c:\\Temp\\bild1.jpg";
            string fullFileName = documentRepository.GetFullFileName(filename);
            fullFileName.ShouldBeEquivalentTo("C:\\Temp\\ImmoTest\\bild1.jpg");
        }

        [Test]
        public void TestOnlyFilenameUpload() {
            DocumentRepository documentRepository = new DocumentRepository("C:\\Temp\\ImmoTest\\", null, "C:\\Temp\\ImmoUploadTest");
            string filename = "bild1.jpg";
            string fullFileName = documentRepository.GetFullTempFileName(filename);
            fullFileName.ShouldBeEquivalentTo("C:\\Temp\\ImmoUploadTest\\bild1.jpg");
        }

        [Test]
        public void TestFilenameUploadWithPath() {
            DocumentRepository documentRepository = new DocumentRepository("C:\\Temp\\ImmoTest\\", null, "C:\\Temp\\ImmoUploadTest");
            string filename = "c:\\Temp\\bild1.jpg";
            string fullFileName = documentRepository.GetFullTempFileName(filename);
            fullFileName.ShouldBeEquivalentTo("C:\\Temp\\ImmoUploadTest\\bild1.jpg");
        }
    }
}