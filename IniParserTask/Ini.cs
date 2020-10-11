using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FluentResults;

namespace IniParserTask
{
    public class Ini
    {
        private readonly Dictionary<Section, List<Variable>> sections = new Dictionary<Section, List<Variable>>();
        private Section lastSection;

        public Variable this[string sectionName, string variableName]
        {
            get
            {
                var section = sections.Keys.FirstOrDefault(s => s.Name == sectionName);

                if (section == null)
                {
                    throw new NullReferenceException("Section not founded");
                }

                if (!sections[section].Exists(variable => variable.Name == variableName))
                {
                    throw new NullReferenceException("Variable not founded");
                }

                return sections[section].FirstOrDefault(variable => variable.Name == variableName);
            }
        }

        public Result<Variable> TryGetVariableValue(string sectionName, string variableName)
        {
            try
            {
                Variable variable = this[sectionName, variableName];
                return Result.Ok(variable);
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        public void AddSection(Section section)
        {
            sections.Add(section, new List<Variable>());
            lastSection = section;
        }

        public Result TryAddSection(Section section)
        {
            try
            {
                AddSection(section);
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        public void AddVariable(Variable variable)
        {
            sections[lastSection].Add(variable);
        }

        public Result TryAddVariable(Variable variable)
        {
            try
            {
                AddVariable(variable);
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }
    }
}