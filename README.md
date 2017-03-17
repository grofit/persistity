# Persistity

An ETL style persistance framework for unity.

## What is it?

It provides a framework for extracting data from your models into any format you want, then sending that data somewhere, or even pulling data from somewhere and deserializing it into objects.

It was originally started as a branch in the EcsRx repository but as it was not tied to that was brought out into its own repo. It still has lots of work to be done on it but the overall goal is to attempt to build simple building blocks for consumers to create larger workflows of data, such as scenarios like extracting data as JSON and posting to a web service, or extracting data as binary and putting into a flat file.

## Serialization

So currently there are 3 transformers which will let you convert any objets with correct attribute `[PersistData]` to and from:

- Json
- Binary
- Xml

You can easily add your own serializers, which are derived from `ISerializer` and `IDeserializer` then wrapped into a single useable form via `ITrasformer`.

## Usage
There is a lot more to come here as ideally it would be consumed via DI so the setup becomes next to nothing, but without DI you would need to new up some stuff.

If you look at the tests for now it will give you a basis, but there needs to be a lot more work done before this is ready for the wider audience.