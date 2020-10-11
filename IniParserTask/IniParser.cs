using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using FluentResults;

namespace IniParserTask
{
    class IniParser
    {
        private readonly IniConfig config;

        public IniParser(IniConfig config)
        {
            this.config = config;
        }

        public Result<Ini> Parse(string[] lines)
        {
            var ini = new Ini();

            lines = RemoveCommentaries(lines);

            foreach (var line in lines)
            {

                if (IsLineContainsSection(line))
                {
                    var sectionParseResult = TryParseSection(line, out Section section);

                    if (sectionParseResult.IsFailed)
                    {
                        return Result.Fail(
                            $"Section parse result failed {string.Join(' ', sectionParseResult.Errors)}");
                    }

                    var addSectionResult = ini.TryAddSection(section);

                    if (addSectionResult.IsFailed)
                    {
                        return Result.Fail($"Add section result failed {string.Join(' ', addSectionResult.Errors)}");
                    }
                }

                if (IsLineContainsVariable(line))
                {
                    var variableParseResult = TryParseVariable(line, out Variable variable);

                    if (variableParseResult.IsFailed)
                    {
                        return Result.Fail(
                            $"Section parse result failed {string.Join(' ', variableParseResult.Errors)}");
                    }

                    var addVariableResult = ini.TryAddVariable(variable);

                    if (addVariableResult.IsFailed)
                    {
                        return Result.Fail($"Add section result failed {string.Join(' ', addVariableResult.Errors)}");
                    }
                }
            }

            return Result.Ok(ini);
        }

        private Section ParseSection(string line)
        {
            if (!IsLineContainsSection(line))
                throw new ArgumentException("Line doesn't contain section");

            string sectionName = line.Substring(line.IndexOf(config.OpenSectionSymbol) + 1,
                line.IndexOf(config.CloseSectionSymbol) - line.IndexOf(config.OpenSectionSymbol) - 1);

            return new Section(sectionName);
        }

        private Result TryParseSection(string line, out Section section)
        {
            try
            {
                section = ParseSection(line);
                return Result.Ok();
            }
            catch (Exception e)
            {
                section = null;
                return Result.Fail(e.Message);
            }
        }

        private Variable ParseVariable(string line)
        {
            if (!IsLineContainsVariable(line))
                throw new ArgumentException("Line doesn't contain variable");

            var splitLine = line.Split(config.EqualitySymbol);

            var variableName = splitLine.FirstOrDefault();
            var variableValue = splitLine.LastOrDefault();

            return new Variable(variableName, variableValue);
        }

        private Result TryParseVariable(string line, out Variable variable)
        {
            try
            {
                variable = ParseVariable(line);
                return Result.Ok();
            }
            catch (Exception e)
            {
                variable = null;
                return Result.Fail(e.Message);
            }
        }

        private string[] RemoveCommentaries(string[] iniRawData) => iniRawData
            .Select(RemoveCommentaryFromLine)
            .ToArray();

        private string RemoveCommentaryFromLine(string line) => line.Contains(config.CommentarySymbol)
                ? line.Substring(0, line.IndexOf(config.CommentarySymbol))
                : line;

        private bool IsLineContainsCommentary(string line) => line.Contains(config.CommentarySymbol);
        private bool IsLineContainsSection(string line) => line.Contains(config.OpenSectionSymbol) && line.Contains(config.OpenSectionSymbol)
            && line.IndexOf(config.OpenSectionSymbol) < line.IndexOf(config.CloseSectionSymbol);
        private bool IsLineContainsVariable(string line) => line.Contains(config.EqualitySymbol);
    }
}
