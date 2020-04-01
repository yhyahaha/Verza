using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpOcrForWpfLibrary
{
    class OcrLanguages
    {
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        public OcrLanguages()
        {
            dictionary.Add("日本語", "ja");
            dictionary.Add("英語", "en-us");
        }

        public string GetLanguage(string tag)
        {
            var t = dictionary.Where(x => x.Value == tag).Select(x => x.Key).FirstOrDefault();
            return t ?? string.Empty;
        }

        public string GetTag(string language)
        {
            string tag = "";
            dictionary.TryGetValue(language, out tag);
            return tag;
        }
       
    }

}
