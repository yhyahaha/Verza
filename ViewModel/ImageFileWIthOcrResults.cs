using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace ViewModel
{
    public class ImageFileWithOcrResults
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

        private List<ScrapingRect> scrapingRects;            
        public List<ScrapingRect> ScrapingRects
        {
            get { return scrapingRects; }
        }

        public void ReadScrapingRectsFromTemplate(string templatePath)
        {
            List<ScrapingRect> rects = new List<ScrapingRect>();

            using (StreamReader reader = new StreamReader(templatePath))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    ScrapingRect rect = new ScrapingRect();

                    var str = reader.ReadLine();
                    string[] token = str.Split(',');

                    rect.Id = int.Parse(token[0]);
                    rect.Name = token[1];
                    rect.Left = double.Parse(token[2]);
                    rect.Top = double.Parse(token[3]);
                    rect.Width = double.Parse(token[4]);
                    rect.Height = double.Parse(token[5]);
                    rect.Value = string.Empty;
                    rect.TabIndex = int.Parse(token[6]);

                    rects.Add(rect);
                }
            }
            this.scrapingRects = rects;
        }

        public void SetScrapingRectValue(int rectId,string resultValue)
        {
            if (rectId >= this.scrapingRects.Count) return;
            string s = this.scrapingRects[rectId].Value;
            scrapingRects[rectId].Value = "aa";
        }
    }
}
