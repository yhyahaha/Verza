using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public struct ScrapingRect
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Value { get; set; }
        public int TabIndex { get; set; }

    }
}
