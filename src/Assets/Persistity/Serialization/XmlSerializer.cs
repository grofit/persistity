using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Persistity.Mappings;
using UnityEngine;

namespace Persistity.Serialization
{
    public class XmlSerializer : ISerializer<string>
    {
        private void SerializePrimitive(object value, Type type, XElement element)
        {
            if (type == typeof(Vector2))
            {
                var typedObject = (Vector2)value;
                element.Add(new XElement("x", typedObject.x));    
                element.Add(new XElement("y", typedObject.y));
                return;
            }
            if (type == typeof(Vector3))
            {
                var typedObject = (Vector3)value;
                element.Add(new XElement("x", typedObject.x));    
                element.Add(new XElement("y", typedObject.y));    
                element.Add(new XElement("z", typedObject.z));
                return;
            }
            if (type == typeof(Vector4))
            {
                var typedObject = (Vector4)value;
                element.Add(new XElement("x", typedObject.x));    
                element.Add(new XElement("y", typedObject.y));    
                element.Add(new XElement("z", typedObject.z));    
                element.Add(new XElement("w", typedObject.w));
                return;
            }
            if (type == typeof(Quaternion))
            {
                var typedObject = (Quaternion)value;
                element.Add(new XElement("x", typedObject.x));    
                element.Add(new XElement("y", typedObject.y));    
                element.Add(new XElement("z", typedObject.z));    
                element.Add(new XElement("w", typedObject.w));
                return;
            }
            else if (type == typeof(DateTime))
            {
                var typedValue = (DateTime) value;
                var stringValue = typedValue.ToBinary().ToString();
                element.Value = stringValue;
                return;
            }

            element.Value = value.ToString();
        }

        public string SerializeData<T>(TypeMapping typeMapping, T data) where T : new()
        {
            var element = new XElement("Container");
            Serialize(typeMapping.InternalMappings, data, element);
            return element.ToString();
        }

        private void SerializeProperty<T>(PropertyMapping propertyMapping, T data, XElement element)
        {
            var underlyingValue = propertyMapping.GetValue(data);
            SerializePrimitive(underlyingValue, propertyMapping.Type, element);
        }

        private void SerializeNestedObject<T>(NestedMapping nestedMapping, T data, XElement element)
        {
            var currentData = nestedMapping.GetValue(data);
            Serialize(nestedMapping.InternalMappings, currentData, element);
        }
        
        private void SerializeCollection<T>(CollectionMapping collectionMapping, T data, XElement element)
        {
            var collectionValue = collectionMapping.GetValue(data);
            element.Add(new XAttribute("Count", collectionValue.Count));
            for (var i = 0; i < collectionValue.Count; i++)
            {
                var currentData = collectionValue[i];
                var newElement = new XElement("CollectionElement");
                element.Add(newElement);

                if (collectionMapping.InternalMappings.Count > 0)
                { Serialize(collectionMapping.InternalMappings, currentData, newElement); }
                else
                { SerializePrimitive(currentData, collectionMapping.CollectionType, newElement); }
            }
        }

        private void SerializeDictionary<T>(DictionaryMapping dictionaryMapping, T data, XElement element)
        {
            var dictionaryValue = dictionaryMapping.GetValue(data);
            element.Add(new XAttribute("Count", dictionaryValue.Count));

            foreach (var key in dictionaryValue.Keys)
            {
                var currentValue = dictionaryValue[key];

                var keyElement = new XElement("Key");
                if (dictionaryMapping.KeyMappings.Count > 0)
                { Serialize(dictionaryMapping.KeyMappings, key, keyElement); }
                else
                { SerializePrimitive(key, dictionaryMapping.KeyType, keyElement); }

                var valueElement = new XElement("Value");
                if (dictionaryMapping.ValueMappings.Count > 0)
                { Serialize(dictionaryMapping.ValueMappings, currentValue, valueElement); }
                else
                { SerializePrimitive(currentValue, dictionaryMapping.ValueType, valueElement); }

                var keyValuePairElement = new XElement("KeyValuePair");
                keyValuePairElement.Add(keyElement, valueElement);
                element.Add(keyValuePairElement);
            }
        }

        private void Serialize<T>(IEnumerable<Mapping> mappings, T data, XElement element)
        {
            foreach (var mapping in mappings)
            {
                var newElement = new XElement(mapping.LocalName);
                element.Add(newElement);

                if (mapping is PropertyMapping)
                { SerializeProperty((mapping as PropertyMapping), data, newElement); }
                else if (mapping is NestedMapping)
                { SerializeNestedObject((mapping as NestedMapping), data, newElement); }
                else if (mapping is DictionaryMapping)
                { SerializeDictionary((mapping as DictionaryMapping), data, newElement); }
                else
                { SerializeCollection((mapping as CollectionMapping), data, newElement); }
            }
        }
    }
}