using System;
using System.Collections.Generic;
using System.Text;

namespace IniParserTask
{
    public class Section
    {
        public Section(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
