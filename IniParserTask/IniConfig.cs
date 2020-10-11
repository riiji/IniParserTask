namespace IniParserTask
{
    public class IniConfig
    {
        public char CommentarySymbol { get; set; } = ';';
        public char EqualitySymbol { get; set; } = '=';
        public char OpenSectionSymbol { get; set; } = '[';
        public char CloseSectionSymbol { get; set; } = ']';
    }
}