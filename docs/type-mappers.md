# Type Mappers

This is generally not that important for most users but there is the notion of a type mapper (`ITypeMapper`) which handles the visiting of the object tree and returning back a list of mappings that it runs through to extract and populate data on the underlying object.

## DefaultTypeMapper

So in most cases you will want to use the `DefaultTypeMapper` which will automatically look at the types attributes and pull out the relevant properties for the objects. In most use cases you will probably want to provide this to the `IMappingRegistry`, however there are other mappers available.

## EverythingTypeMapper & TypeMapper

There is an `EverythingTypeMapper` which doesnt bother checking for attributes and will just pump out EVERY public property that has a get/set available. 

This is primarily useful in scenarios where you are wanting to use 3rd party classes which are POCOs but you cannot decorate or if you are just using the serializers explicitly and want to just dump everything public out.

There is also the `TypeMapper` which is the underlying abstract class which has most of the implementation data for `ITypeMapper`, but this is ultimately seen as a basis class for you to derive your own if needed, if you want to analyze your classes in a completely different way you can completely bypass this `TypeMapper` route and just implement your own `ITypeMapper`.