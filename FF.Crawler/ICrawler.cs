using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Crawler
{
    public interface ICrawler
    {
            // public void ParseSearchFile(string filePath, FF.DataModel.CRAWL_URLS cr);
            void Parse(string filePath);
            List<string> ParseSearch(string strSearch);
        
    }
}
