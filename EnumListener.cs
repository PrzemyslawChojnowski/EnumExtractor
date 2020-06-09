using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumExtractor
{
    public class EnumListener : EnumBaseListener
    {
        EnumParser parser;
        ParsedEnum currentlyParsingEnum;

        public List<ParsedEnum> ParsedEnums { get; set; }

        public EnumListener(EnumParser _parser)
        {
            parser = _parser;
            ParsedEnums = new List<ParsedEnum>();
        }

        public override void EnterEnumDeclaration(EnumParser.EnumDeclarationContext context)
        {
            currentlyParsingEnum = new ParsedEnum();
            currentlyParsingEnum.EnumName = Char.ToLowerInvariant(context.ID().ToString()[0]) + context.ID().ToString().Substring(1) + "Dict";
            currentlyParsingEnum.ParsedEnumContent.AppendLine("const " + currentlyParsingEnum.EnumName + " = {");
        }

        public override void ExitEnumDeclaration(EnumParser.EnumDeclarationContext context)
        {
            currentlyParsingEnum.ParsedEnumContent.AppendLine("};\n");
            currentlyParsingEnum.ParsedEnumContent.AppendLine("export default " + currentlyParsingEnum.EnumName + ";");
            ParsedEnums.Add(currentlyParsingEnum);
        }

        public override void EnterAssign(EnumParser.AssignContext context)
        {
            ITokenStream tokens = (ITokenStream)parser.InputStream;
            String args;

            if (context.expr() != null)
                args = tokens.GetText(context.expr());
            else
                args = currentlyParsingEnum.ParsedEnumIds.Count().ToString();

            string name = Char.ToLowerInvariant(context.ID().ToString()[0]) + context.ID().ToString().Substring(1);

            currentlyParsingEnum.ParsedEnumIds.Add(Int32.Parse(args));
            currentlyParsingEnum.ParsedEnumContent.AppendLine("\t" + name + ": " + args + ",");
        }
    }
}
