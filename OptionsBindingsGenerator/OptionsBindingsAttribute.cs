using System;

namespace CodingFlow.OptionsBindingsGenerator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OptionsBindingsAttribute : Attribute
    {
        public OptionsBindingsAttribute(bool validateOnStart, string sectionKey = "")
        {
        }
    }
}
