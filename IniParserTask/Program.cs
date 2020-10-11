using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using FluentResults;

namespace IniParserTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new IniReader();
            Result readResult = reader.TryRead("file.ini", out var data);

            if (readResult.IsFailed)
            {
                Console.WriteLine(string.Join(' ', readResult.Errors));
                return;
            }

            var parser = new IniParser(new IniConfig());
            Result<Ini> parserResult = parser.Parse(data);

            if (parserResult.IsFailed)
            {
                Console.WriteLine(string.Join(' ', parserResult.Errors));
            }

            Ini ini = parserResult.Value;

            Result<Variable> variableResult = ini.TryGetVariableValue("Section1", "var1");

            if (variableResult.IsFailed)
            {
                Console.WriteLine(string.Join(' ', variableResult.Errors));
            }

            Result<int> intParseResult = variableResult.Value.TryGetInt32Value();

            if (intParseResult.IsFailed)
            {
                Console.WriteLine(string.Join(' ', intParseResult.Errors));
            }

            Console.WriteLine(intParseResult.Value);
        }
    }
}
