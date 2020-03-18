using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back_End
{
    class Dictionary
    {
        public static HashSet<string> GetBadExtensions()
        {
            return new HashSet<string>() { ".jpg", ".png", ".svg", ".pdf", ".gif", ".css", ".js", ".xml" };
        }

        public static HashSet<string> GetfilterWords()
        {
            return new HashSet<string>() { "a", "act", "ago", "am", "an", "and", "any", "are", "as", "at", "can", "get",
                                                       "go", "got", "in", "is", "it", "its", "may", "may", "my", "no", "on", "one",
                                                       "or", "say", "so", "to", "the", "yet"};
        }
    }
}
