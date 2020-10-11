using System;
using System.Collections.Generic;
using System.Text;
using FluentResults;

namespace IniParserTask
{
    public class Variable
    {
        public Variable(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }

        public int GetInt32Value()
        {
            return int.Parse(Value);
        }

        public double GetDoubleValue()
        {
            return double.Parse(Value);
        }

        public Result<int> TryGetInt32Value()
        {
            try
            {
                return Result.Ok(GetInt32Value());
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        public Result<double> TryGetDoubleValue()
        {
            try
            {
                return Result.Ok(GetDoubleValue());
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }
    }
}
