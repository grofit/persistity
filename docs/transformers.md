# Transformers

So transformers are basically wrappers around serializers and offer a way to convert a statically typed model into a homogonised stream for use on other parts of the pipeline. They also wrap up the concept of type mapping, which is the way it knows how to extract and process your models.

## Serialization

Currently there are 3 transformers which will let you convert any objets with correct attribute `[PersistData]` to and from:

- Json
- Binary
- Xml

You can easily add your own serializers, which are derived from `ISerializer` and `IDeserializer` then wrapped into a single useable form via `ITransformer`.


## Type Mappings

Type mappings are visitor tree style objects which express the underlying type as a tree, so for example when you pass in a class like:

```csharp
[Persist]
public class SomeClass
{
    [PersistData]
    public float SomeValue { get; set; }
}
```

It will behind the scenes analyse your type and work out 

## Creating A Transformer

This will hopefully become simpler in the future but the main idea is that you will be passed the type mapping which represents the structure of the data, and you iterate over that drilling down into the tree and converting each step into a piece of data.

So for example if you were to want to create a CSV transformer you would probably create a string builder and look at something like the `BinarySerializer` and `BinaryDeserializer` to see how it writes out each value in a flat manner and just put a comma after it.