using System.Collections.Generic;
using System.IO;

namespace SupercellProxy
{
    partial class PacketDefinition
    {
        public void Read(BinaryReader br)
        {
            var valueList = new List<object>();
            var value = (object)null;
            for (int i = 0; i < Fields.Count; i++)
            {
                var field = Fields[i];

                if (field.IsOptional)
                {
                    var shouldRead = br.ReadBoolean();
                    if (!shouldRead)
                        continue;
                }

                var count = 1;
                if (field.IsArray)
                {
                    // Use array's fixed size from definition file.
                    if (field.ArrayLength != 0)
                        count = field.ArrayLength;
                    // Otherwise read from the prefix.
                    else
                        count = br.ReadIntWithEndian();
                }

                var array = new object[field.ArrayLength];
                for (int k = 0; k < count; k++)
                {
                    switch (field.Type)
                    {
                        case FieldType.Boolean:
                            value = br.ReadBoolean();
                            break;

                        case FieldType.Byte:
                            value = br.ReadByte();
                            break;

                        case FieldType.Int32:
                            value = br.ReadIntWithEndian();
                            break;

                        case FieldType.Int64:
                            value = br.ReadLongWithEndian();
                            break;

                        case FieldType.String:
                            value = br.ReadScString();
                            break;

                        case FieldType.ZlibString:
                            value = br.ReadZlibString();
                            break;

                        case FieldType.Component:
                            //var component = PacketDefinition.GetDefinition(field.TypeName);
                            //component.Read(br);
                            break;
                    }
                    if (field.IsArray)
                        array[k] = value;
                }

                if (field.IsArray)
                    value = array;
                valueList.Add(value);
            }
        }
    }
}
