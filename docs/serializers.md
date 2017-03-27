# Serializers

Serializers offer a way to convert a statically typed model into a homogonised data object for use on other parts of the pipeline. They also wrap up the concept of type mapping, which is the way it knows how to extract and process your models.

## Serialization

Currently there are 3 serializers which will let you convert any objets with correct attribute `[PersistData]` to and from:

- Json
- Binary
- Xml

You can easily add your own serializers, which implement `ISerializer` and `IDeserializer`.

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

It will behind the scenes analyse your type and work out what properties need to be bound, by default the `DefaultTypeMapper` should be used, but there is an interface for you to make your own, or an alternative `EverythingTypeMapper` which will just auto map every public property which has a get/set.

## Known Types

The serializers know of most basic unity and .net types and how to handle them, however there will be scenarios where things like `ReactiveProperty<T>` from unirx or other frameworks appear, that need to be mapped in a certain way, (i.e extract the value and bin the rest of it). This needs 2 things to be setup for these types to be processed correctly.

 1. You need to make sure your `TypeMapper` (the interface of `ITypeMapper` does not know about this notion) is provided a list of known primitives upon creation

 2. You need to provide your serializer with an implementation of `ITypeHandler` to tell it how to serialize the value.

So for example if I wanted to support the unirx `ReactiveProperty<T>` in binary I would do something like:

```csharp
// TypeMapper setup
var typeMapper = new DefaultTypeMapper(new []{ typeof(ReactiveProperty<int>)});

// Class to handle known type
public class ReactiveIntHandler : ITypeHandler<BinaryWriter, BinaryReader>
{
   public bool MatchesType(Type type)
   { return type == typeof(ReactiveProperty<int>); }
   
    public void HandleTypeIn(BinaryWriter writer, object data)
    {
        var typedObject = (ReactiveProperty<int>)object;
        writer.Write((int)typedObject.Value);
    }
    
    public object HandleTypeOut(BinaryReader reader)
    {
        var value = reader.ReadInt32();
        return new ReactiveProperty<int>(value);
    }
}

// Pass known type handler to the Serializer and Deserializer
var binaryConfig = new BinaryConfiguration {
    TypeHandlers = new []{ new ReactiveIntHandler() }
}
var serializer = new BinarySerializer(binaryConfig);
var deserializer = new BinaryDeserializer(binaryConfig);

// I can now use models with this property
[Persist]
public class SomeClass
{
    [PersistData]
    public ReactiveProperty<int> MyProperty {get;set;}
}
```

It is a bit long winded and each serializer would need its own type handler but once it is setup you will be able to serialize any type you want in a given way, even the dreaded built in unity types if you wanted to strip certain data from a `GameObject` or whatever.

## Creating A Serializer

This will hopefully become simpler in the future but the main idea is that you will be passed the type and need to get a mapping for the type which represents the structure of the data, and you iterate over that drilling down into the tree and converting each step into a piece of data.

So for example if you were to want to create a CSV serializer you would probably create a string builder and look at something like the `BinarySerializer` and `BinaryDeserializer` to see how it writes out each value in a flat manner and just put a comma after it (well its obviously a bit more complex but thats the basics).

Ultimately almost all De/serializers will take some configuration and an `IMappingRegistry` which will handle the type mapping for the types.