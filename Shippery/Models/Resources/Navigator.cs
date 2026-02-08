using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shippery.Models.Resources
{
    public class IntNavigator<Type>
    {
        int i;
        int defaultValue;
        int maximumValue;
        List<Type> sections;

        public IntNavigator() { }
        public IntNavigator(int defaultValue, int maximumValue, List<Type> attachedList)
        {
            sections = attachedList;
            i = defaultValue;
            DefaultValue = defaultValue;
            MaximumValue = maximumValue;
        }

        public int Iteration { get => i; }
        public string IterationString { get => i.ToString(); set => i = int.Parse(value); }
        public List<Type> Sections { get => sections; set => sections = value; }
        public int DefaultValue { get => defaultValue; set => defaultValue = value; }
        public int MaximumValue { get => maximumValue; set => maximumValue = value; }

        public Type this[int i] { get => sections[i]; }
    }

    public class IntOneSectionNavigator<Type>
    {
        int i;
        int defaultValue;
        int maximumValue;
        Type attachedObject;

        public IntOneSectionNavigator() { }
        public IntOneSectionNavigator(int defaultValue, int maximumValue, Type attachedList)
        {
            attachedObject = attachedList;
            i = defaultValue;
            DefaultValue = defaultValue;
            MaximumValue = maximumValue;
        }

        public int Iteration { get => i; }
        public string IterationString { get => i.ToString(); set => i = int.Parse(value); }
        public Type AttachedObject { get => attachedObject; set => attachedObject = value; }
        public int DefaultValue { get => defaultValue; set => defaultValue = value; }
        public int MaximumValue { get => maximumValue; set => maximumValue = value; }
    }

    public class DateTimeNavigator<Type>
    {
        DateTime i;
        DateTime defaultValue;
        List<DateTime> possibleValues;
        List<Type> sections;

        public DateTimeNavigator() { }
        public DateTimeNavigator(DateTime defaultValue, List<DateTime> possibleValues, List<Type> attachedList)
        {
            sections = attachedList;
            i = defaultValue;
            DefaultValue = defaultValue;
            this.possibleValues = possibleValues;
        }

        public DateTime Iteration { get => i; }
        public string IterationString { get => i.ToString(); set => i = DateTime.Parse(value); }
        public List<Type> Sections { get => sections; set => sections = value; }
        public DateTime DefaultValue { get => defaultValue; set => defaultValue = value; }
        public List<DateTime> PossibleValues { get => possibleValues; set => possibleValues = value; }

        public Type this[int i] { get => sections[i]; }
    }

    public class DateTimeOneSectionNavigator<Type>
    {
        DateTime i;
        DateTime defaultValue;
        List<DateTime> possibleValues;
        Type attachedObject;

        public DateTimeOneSectionNavigator() { }
        public DateTimeOneSectionNavigator(DateTime defaultValue, List<DateTime> possibleValues, Type attachedList)
        {
            attachedObject = attachedList;
            i = defaultValue;
            DefaultValue = defaultValue;
            PossibleValues = possibleValues;
        }

        public DateTime Iteration { get => i; }
        public string IterationString { get => i.ToString(); set => i = DateTime.Parse(value); }
        public Type AttachedObject { get => attachedObject; set => attachedObject = value; }
        public DateTime DefaultValue { get => defaultValue; set => defaultValue = value; }
        public List<DateTime> PossibleValues { get => possibleValues; set => possibleValues = value; }
    }
}
