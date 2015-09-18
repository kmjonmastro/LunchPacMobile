using System;

namespace LunchPac.Attributes
{
    public class Table : Attribute
    {
        public string TableName { get; private set; }

        public Table(string name)
        {
            TableName = name;
        }
    }
}
