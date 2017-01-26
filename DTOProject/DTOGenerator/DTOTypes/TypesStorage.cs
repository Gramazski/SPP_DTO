using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOProject.DTOGenerator.DTOTypes
{
    class TypesStorage
    {
        private Dictionary<DTOKey, string> typeTable;

        public TypesStorage()
        {
            typeTable = new Dictionary<DTOKey, string>();

            AddType("integer", "int32", "System.Int32");
            AddType("integer", "int64", "System.Int64");
            AddType("number", "float", "System.Single");
            AddType("number", "double", "System.Double");
            AddType("string", "byte", "System.Byte");
            AddType("string", "date", "System.DateTime");
            AddType("string", "string", "System.String");
            AddType("boolean", "", "System.Boolean");
        }

        public void AddType(string type, string format, string csType)
        {
            if (type == null)
                throw new ArgumentException("Invalid argument - type!");
            if (format == null)
                throw new ArgumentException("Invalid argument - format!");
            if (csType == null)
                throw new ArgumentException("Invalid argument - C# type!");

            DTOKey key = new DTOKey(type, format);

            if (!typeTable.ContainsKey(key))
            {
                typeTable.Add(key, csType);
            }
            else
            {
                throw new ArgumentException("Such type has already exist in a table!");
            }

        }

        public string GetCSType(string type, string format)
        {
            if (type == null)
                throw new ArgumentException("DTO Type storage: Invalid argument - type!");
            if (format == null)
                throw new ArgumentException("DTO Type storage: Invalid argument - format!");

            DTOKey key = new DTOKey(type, format);

            if (typeTable.ContainsKey(key))
            {
                return typeTable[key];
            }
            else
            {
                throw new ArgumentException("DTO Type storage: Invalid argument - type!");
            }
        }


        private class DTOKey : IEquatable<DTOKey>
        {
            private string type;
            private string format;
            private int hashCode;

            public DTOKey(string type, string format)
            {
                this.type = type;
                this.format = format;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int result = hashCode;

                    if (result == 0)
                    {
                        result = 31;
                        result = 19 * result + type.GetHashCode();
                        result = 19 * result + format.GetHashCode();
                        hashCode = result;
                    }

                    return result;
                }
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null))
                    return false;
                if (ReferenceEquals(obj, this))
                    return true;
                if (obj.GetType() != GetType())
                    return false;

                return Equals(obj as DTOKey);
            }

            public bool Equals(DTOKey other)
            {
                if (other == null)
                    return false;
                return string.Equals(type, other.type) && string.Equals(format, other.format);
            }
        }
    }
}
