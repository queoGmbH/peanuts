using System.Drawing;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Thumbnailing {
    public class DrawingImageInputWrapper {
        private readonly Image _image;
        private readonly string _outputFilename;

        public DrawingImageInputWrapper(Image image, string outputFilename) {
            _image = image;
            _outputFilename = outputFilename;
        }

        public Image Image {
            get { return _image; }
        }

        public string OutputFilename {
            get { return _outputFilename; }
        }
    }
}