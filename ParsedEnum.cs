using System.Collections.Generic;
using System.Text;

namespace EnumExtractor
{
    public class ParsedEnum
    {
        public ParsedEnum()
        {
            ParsedEnumContent = new StringBuilder();
            ParsedEnumIds = new List<int>();
        }

        public string EnumName { get; set; }
        public StringBuilder ParsedEnumContent { get; set; }
        public List<int> ParsedEnumIds { get; set; }
    }
}
