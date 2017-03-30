using System;
using System.IO;
using Persistity.Extensions;
using Persistity.Registries;
using UnityEngine;

namespace Persistity.Serialization.Binary
{
    public class BinarySerializer : GenericSerializer<BinaryWriter, BinaryReader>, IBinarySerializer
    {
        public static readonly char[] NullDataSig = {(char) 141, (char) 141};
        public static readonly char[] NullObjectSig = {(char) 141, (char)229, (char)141};
        
        public BinarySerializer(IMappingRegistry mappingRegistry, BinaryConfiguration configuration = null) : base(mappingRegistry)
        {
            Configuration = configuration ?? BinaryConfiguration.Default;
        }

        public override void HandleNullData(BinaryWriter state)
        { state.Write(NullDataSig); }

        public override void HandleNullObject(BinaryWriter state)
        { state.Write(NullObjectSig); }

        public override void AddCountToState(BinaryWriter state, int count)
        { state.Write(count); }

        public override void SerializeDefaultPrimitive(object value, Type type, BinaryWriter writer)
        {
            if (type == typeof(byte)) { writer.Write((byte)value); }
            else if (type == typeof(short)) { writer.Write((short)value); }
            else if (type == typeof(int)) { writer.Write((int)value); }
            else if (type == typeof(long)) { writer.Write((long)value); }
            else if (type == typeof(bool)) { writer.Write((bool)value); }
            else if (type == typeof(float)) { writer.Write((float)value); }
            else if (type == typeof(double)) { writer.Write((double)value); }
            else if (type == typeof(decimal)) { writer.Write((decimal)value); }
            else if (type.IsEnum) { writer.Write((int)value); }
            else if (type == typeof(Vector2))
            {
                var vector = (Vector2)value;
                writer.Write(vector.x);
                writer.Write(vector.y);
            }
            else if (type == typeof(Vector3))
            {
                var vector = (Vector3)value;
                writer.Write(vector.x);
                writer.Write(vector.y);
                writer.Write(vector.z);
            }
            else if (type == typeof(Vector4))
            {
                var vector = (Vector4)value;
                writer.Write(vector.x);
                writer.Write(vector.y);
                writer.Write(vector.z);
                writer.Write(vector.w);
            }
            else if (type == typeof(Quaternion))
            {
                var quaternion = (Quaternion)value;
                writer.Write(quaternion.x);
                writer.Write(quaternion.y);
                writer.Write(quaternion.z);
                writer.Write(quaternion.w);
            }
            else if (type == typeof(DateTime)) { writer.Write(((DateTime)value).ToBinary()); }
            else if (type == typeof(Guid)) { writer.Write(((Guid)value).ToString()); }
            else if (type == typeof(string)) { writer.Write(value.ToString()); }
        }
        
        public override DataObject Serialize(object data)
        {
            var typeMapping = MappingRegistry.GetMappingFor(data.GetType());
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write(typeMapping.Type.GetPersistableName());
                Serialize(typeMapping.InternalMappings, data, binaryWriter);
                binaryWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                return new DataObject(memoryStream.ToArray());
            }
        }
    }
}