using System;
using System.IO;
using FluentResults;

namespace IniParserTask
{
    public class IniReader
    {
        public string[] Read(string filePath) => File.ReadAllLines(filePath);

        public Result TryRead(string filePath, out string[] data)
        {
            try
            {
                data = Read(filePath);
                return Result.Ok();
            }
            catch(Exception e)
            {
                data = null;
                return Result.Fail(e.Message);
            }
        }
    }
}