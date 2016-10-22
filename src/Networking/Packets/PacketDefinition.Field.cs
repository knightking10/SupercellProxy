using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercellProxy
{
    partial class PacketDefinition
    {
        public enum FieldType
        {
            None,
            Boolean,
            Byte,
            Int32,
            Int64,
            String,

            ZlibString, // -> Required dependency.

            Component
        };

        public struct Field
        {
            public Field(string name, string comment, bool optional, bool array, int arrayLength, string typename, FieldType type)
            {
                _value = null;
                _arrayLength = arrayLength;
                _optional = optional;
                _array = array;
                _name = name;
                _comment = comment;
                _typeName = typename;
                _type = type;
            }

            private readonly int _arrayLength;
            private readonly object _value;
            private readonly bool _optional;
            private readonly bool _array;
            private readonly string _name;
            private readonly string _comment;
            private readonly string _typeName;
            private readonly FieldType _type;

            public string Name => _name;
            public string Comment => _comment;
            public bool IsOptional => _optional;
            public bool IsArray => _array;
            public int ArrayLength => _arrayLength;
            public FieldType Type => _type;
            public string TypeName => _typeName;

            public override string ToString()
            {
                var name = _name == null ? "null" : "\"" + _name + "\"";
                return $"{name}:{_type}";
            }
        }
    }
}
