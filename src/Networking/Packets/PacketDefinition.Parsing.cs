using Newtonsoft.Json.Linq;

namespace SupercellProxy
{
    partial class PacketDefinition
    {
        private static Field ParseField(JToken token)
        {
            // Look for the name token, type token and comment token.
            var name = token.TryToObject<string>("name");
            var type = token.TryToObject<string>("type");
            var comment = token.TryToObject<string>("comment");

            // If we don't know the type of field.
            if (type == null)
            {
                // TODO: Handle.
            }

            var typeName = string.Empty;
            var arrayLength = 0;
            var isArray = false;
            var isOptional = false;

            var index = 0;

            if (type[index] == '?')
            {
                isOptional = true;
                index++;
            }

            while (index < type.Length)
            {
                if (type[index] == '[')
                {
                    var str = string.Empty;
                    while (type[++index] != ']')
                        str += type[index];

                    isArray = true;
                    if (!string.IsNullOrEmpty(str))
                        arrayLength = int.Parse(str);
                    continue;
                }

                typeName += type[index];
                index++;
            }

            var fieldType = FieldType.None;
            switch (typeName)
            {
                // Primitives.
                case "BOOLEAN":
                    fieldType = FieldType.Boolean;
                    break;
                case "BYTE":
                    fieldType = FieldType.Byte;
                    break;
                case "INT":
                    fieldType = FieldType.Int32;
                    break;
                case "LONG":
                    fieldType = FieldType.Int64;
                    break;
                case "STRING":
                    fieldType = FieldType.String;
                    break;
                case "ZIP_STRING":
                    fieldType = FieldType.ZlibString;
                    break;

                // Components or unknown types.
                default:
                    fieldType = FieldType.Component;
                    break;
            }

            return new Field(name, comment, isOptional, isArray, arrayLength, typeName, fieldType);
        }
    }
}
