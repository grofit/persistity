# Gotchas

## 3rd Party Objects

The main gotcha is around the way the types are mapped for transforming, so types need to have the `[PersistData]` attribute, which you cannot really insert into 3rd party classes, so it is recommended for this that you make your own models which contain the bits you care about and just copy the data into these transient models before you transform.

## Ducktyped style data

So if you were to have something like a `List<object>` which may contain many different types of object, this would not work without a custom serializer and type mapper, as it would not be able to statically analyse the type and possible types that would be contained.

As when it came to deserialize it would be unsure as to what type of object each instance would be, so although this library supports lists, dictionaries, arrays etc it is recommended that you make sure your generics are not of a base type wherever possible.

A possible workaround here is to make a custom class which derives from object and making it a known primitive, so you decide how to serialize this object a single unified way.

## Data Types

Also there is only support for basic types, so if you end up having complex structs that need transforming in a specific way there will hopefully be a notion of `KnownTypes` where you can provide the transformers some information on how to serialize/deserialize types, but currently it will throw an exception if it doesnt know how to handle a type and cannot nest the object.

## GameObjects and MonoBehaviours

These and other complex unity scene objects are not supported and cannot be serialized, it is generally bad practice to try and serialize something like this as you ideally want to be persisting raw data which can be used to repopulate the scene or whatever else you are using the data for, much like with 3rd party classes if you need data from a game object or MB it is recommended you extract it into another class and use that class as your persistance target.

## Public Properties

Currently it only supports persisting of public properties, so make sure you factor this into your POCOs.
