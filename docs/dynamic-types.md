# Dynamic Types

One of the recent changes is to support the notion of dynamic types automatically. This will occur if the type mapper runs into any types which are interfaces, `object` or some type which is abstract or ambiguous.

When it finds a dynamic type it flags it and the serialization layer will output type information so the deserializer can create the right type without needing to be told what it is.

## How to apply it

As mentioned above it will automatically apply to any type which can not be automatically resolved at deserialization time. However in some cases you may be using inheritance and have a base class which all other classes inherit from, this will not be picked up automatically so in this case you would need to apply the `DynamicTypeAttribute` to the property in question.

## Overhead

When this dynamic typing occurs it will output the type information with the serialized data, which will be output as a string, so here is an example:

```csharp
// The model being used
[Persist]
public class DynamicTypesModel
{
    [PersistData]
    public object DynamicNestedProperty { get; set; }

    [PersistData]
    public object DynamicPrimitiveProperty { get; set; }

    [PersistData]
    public IList<object> DynamicList { get; set; }

    [PersistData]
    public IDictionary<object, object> DynamicDictionary { get; set; }
}
```
So the class above uses `object` type which is too ambiguous for deserialization, so the json serializer would output the below content:

```json
{
   "DynamicNestedProperty":{
      "Type":"Tests.Editor.Models.E",
      "Data":{
         "IntValue":10
      }
   },
   "DynamicPrimitiveProperty":{
      "Type":"System.Int32",
      "Data":12
   },
   "DynamicList":[
      {
         "Type":"Tests.Editor.Models.E",
         "Data":{
            "IntValue":22
         }
      },
      {
         "Type":"Tests.Editor.Models.C",
         "Data":{
            "FloatValue":25.0
         }
      },
      {
         "Type":"System.Int32",
         "Data":20
      }
   ],
   "DynamicDictionary":[
      {
         "Key":{
            "Type":"System.String",
            "Data":"key1"
         },
         "Value":{
            "Type":"System.Int32",
            "Data":62
         }
      },
      {
         "Key":{
            "Type":"Tests.Editor.Models.E",
            "Data":{
               "IntValue":99
            }
         },
         "Value":{
            "Type":"System.Int32",
            "Data":54
         }
      },
      {
         "Key":{
            "Type":"System.Int32",
            "Data":1
         },
         "Value":{
            "Type":"Tests.Editor.Models.C",
            "Data":{
               "FloatValue":51.0
            }
         }
      }
   ],
   "Type":"Tests.Editor.Models.DynamicTypesModel"
}
```

So as you can see because it cannot infer the type correctly it needs to analyze it and output the type with the data, which will increase the file size, however it will at least ensure your types are correctly mapped.