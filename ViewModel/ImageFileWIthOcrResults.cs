using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace ViewModel
{
    class ImageFileWithOcrResults
    {
        public ImageFileWithOcrResults()
        {
            this.filePath = string.Empty;
            this.ocrAngle = 0.0;
            this.scrapingRects = new List<ScrapingRect>();
        }
        
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set
            {
                if (value == filePath) return;
                this.filePath = value;
            }
        }

        private double ocrAngle;
        public double OcrAngle
        {
            get { return ocrAngle; }
            set
            {
                if (value == ocrAngle) return;
                this.ocrAngle = value;
            }
        }

        private IList<ScrapingRect> scrapingRects;            
        public IList<ScrapingRect> ScrapingRects
        {
            get { return scrapingRects; }
        }
    }
}
