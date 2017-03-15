using System.Collections.Generic;
using System.Text;
using Persistity.Mappings;

namespace Persistity.Serialization.Debug
{
    public class DebugSerializer : ISerializer<string>
    {
        public string SerializeData<T>(TypeMapping typeMapping, T data) where T : new()
        {
            var output = new StringBuilder();

            var result = Serialize(typeMapping.InternalMappings, data);
            output.AppendLine(result);
            return output.ToString();
        }

        private string SerializeProperty<T>(PropertyMapping propertyMapping, T data)
        {
            var output = propertyMapping.GetValue(data);
            return string.Format("{0} : {1}", propertyMapping.ScopedName, output);
        }

        private string SerializeNestedObject<T>(NestedMapping nestedMapping, T data)
        {
            var output = new StringBuilder();
            var currentData = nestedMapping.GetValue(data);
            var result = Serialize(nestedMapping.InternalMappings, currentData);
            output.Append(result);
            return output.ToString();
        }

        private string Serialize<T>(IEnumerable<Mapping> mappings, T data)
        {
            var output = new StringBuilder();

            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                {
                    var result = SerializeProperty((mapping as PropertyMapping), data);
                    output.AppendLine(result);
                }
                else if (mapping is NestedMapping)
                {
                    var result = SerializeNestedObject((mapping as NestedMapping), data);
                    output.Append(result);
                }
                else if (mapping is DictionaryMapping)
                {
                    var result = SerializeDictionary((mapping as DictionaryMapping), data);
                    output.Append(result);
                }
                else
                {
                    var result = SerializeCollection((mapping as CollectionMapping), data);
                    output.Append(result);
                }
            }
            return output.ToString();
        }

        private string SerializeCollection<T>(CollectionMapping collectionMapping, T data)
        {
            var output = new StringBuilder();
            var collectionValue = collectionMapping.GetValue(data);
            output.AppendFormat("{0} : {1} \n", collectionMapping.ScopedName, collectionValue.Count);

            for (var i = 0; i < collectionValue.Count; i++)
            {
                var currentData = collectionValue[i];
                if (collectionMapping.InternalMappings.Count > 0)
                {
                    var result = Serialize(collectionMapping.InternalMappings, currentData);
                    output.Append(result);
                }
                else
                {
                    output.AppendFormat("{0} : {1} \n", collectionMapping.ScopedName + ".value", currentData);
                }
            }

            return output.ToString();
        }

        private string SerializeDictionary<T>(DictionaryMapping dictionaryMapping, T data)
        {
            var output = new StringBuilder();
            var dictionaryValue = dictionaryMapping.GetValue(data);
            output.AppendFormat("{0} : {1} \n", dictionaryMapping.ScopedName, dictionaryValue.Count);

            foreach (var currentKey in dictionaryValue.Keys)
            {
                if (dictionaryMapping.KeyMappings.Count > 0)
                {
                    var result = Serialize(dictionaryMapping.KeyMappings, currentKey);
                    output.Append(result);
                }
                else
                {
                    output.AppendFormat("{0} : {1} \n", dictionaryMapping.ScopedName + ".key", currentKey);
                }

                var currentValue = dictionaryValue[currentKey];
                if (dictionaryMapping.ValueMappings.Count > 0)
                {
                    var result = Serialize(dictionaryMapping.ValueMappings, currentValue);
                    output.Append(result);
                }
                else
                {
                    output.AppendFormat("{0} : {1} \n", dictionaryMapping.ScopedName + ".value", currentValue);
                }
            }

            return output.ToString();
        }
    }
}