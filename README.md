# Persistity

A pipeline based data persistence framework (a bit like ETL) for game developers for use with Unity, Monogame or other .net based engines/frameworks.

[![Build Status][build-status-image]][build-status-url]
[![Nuget Version][nuget-image]][nuget-url]
[![Join Discord Chat][discord-image]][discord-url]

## What is it?

Its an async pipeline for handling complex extract/transform/load interactions for your models in any format (xml/json/binary out the box).

At its heart it has:

- `Transformers` (Transforms one statically typed model to another)
- `Serializers` (Extracts and serializes models)
- `Processors` (Does stuff on the data before it reaches its destination)
- `Endpoints` (Sends or Recieves data from an end point)

These are basically steps in a workflow or pipeline, so you can chain together steps to do stuff like converting your model to json, url encoding it and then sending it to a web service, or converting it to binary and then pumping it into a file.

## Example 

Here are a few examples covering incoming and outgoing data pipelines.

### Setting Up Models For persistence
```csharp
[Persist]
public class SomeClass
{
    [PersistData]
    public float SomeValue { get; set; }
}
```

The `[Persist]` attribute indicates this is a root class for persisting, and `[PersistData]` indicates that the property should be mapped. In nested classes or collections you can omit the `[Persist]` attribute, its just the root type that needs this.

### Outgoing Pipeline

So for example lets cover a quick use case for saving your game state data to a binary file:

```csharp
// This would normally be setup once in your app or via DI etc
var mappingRegistry = new MappingRegistry(new DefaultTypeMapper());
var binarySerializer = new BinarySerializer(mappingRegistry);
var writeFileEndpoint = new WriteFileEndpoint("savegame.sav");

// Create the pipeline which wraps the underlying steps
var saveToBinaryFilePipeline = new PipelineBuilder()
    .SerializeWith(binarySerializer)
    .SendTo(writeFileEndpoint)
    .Build();

// Execute the pipeline with your game data
await saveToBinaryFilePipeline.Execute(myGameData);
```

Now lets imagine we decide we wanted to encrypt our game data before we spat it out, we would make changes like so:


```csharp
// Same setup as before but we add an encryption processor
var encryptor = new AesEncryptor("some-pass-phrase");
var encryptionProcessor = new EncryptDataProcessor(encryptor);

// Same as before but we now add the processor into the mix
var saveToBinaryFilePipeline = new PipelineBuilder()
    .SerializeWith(binarySerializer)
    .ProcessWith(encryptionProcessor)
    .SendTo(writeFileEndpoint)
    .Build();

// Execute the pipeline with your game data
await saveToBinaryFilePipeline.Execute(myGameData);
```

This then will encrypt your data after it has been transformed and will pass it over to be persisted in a file. You could easily decide to change from using a file to using `PlayerPrefs` by just changing the `FileWriter` to `PlayerPrefWriter` and using that `.SendTo(playerPrefsEndpoint)` which would send your data to player prefs rather than a flat file.

### Incoming Pipeline

So in the previous example we covered sending data to the file system, but lets imagine we wanted that encrypted saved data back in at some point, we could do:

```csharp
// This would normally be setup once in your app or via DI etc
var mappingRegistry = new MappingRegistry(new DefaultTypeMapper());
var binaryDeserializer = new BinaryDeserializer(mappingRegistry);
var encryptor = new AesEncryptor("some-pass-phrase");
var decryptionProcessor = new DecryptDataProcessor(encryptor);
var readFileEndpoint = new ReadFileEndpoint("savegame.sav");

// Create the pipeline which wraps the underlying steps
var loadBinaryFilePipeline = new PipelineBuilder()
    .RecieveFrom(readFileEndpoint)
    .ProcessWith(decryptionProcessor)
    .DeserializeWith(binaryDeserializer)
    .Build();

// Execute the pipeline to get your game data
var myGameData = await loadBinaryFilePipeline.Execute<MyGameData>();
```

This will decrypt and convert your lovely binary data back into a statically typed object for you to do as you wish with.

### Creating Pipelines Without Builder

So the builder offers a simple and flexible way to create pipelines wherever you want, however in most complex situations you may just want to create your own pipeline implementation and pass that around, which can be done for the above like so:

```csharp
public class SaveEncryptedBinaryFilePipeline : SendDataPipeline
{
   public SaveEncryptedBinaryFilePipeline(IBinarySerializer serializer, EncryptDataProcessor processor, WriteFileEndpoint endpoint) : base(serializer, sendToEndpoint, processor, null)
   {}
}
```

There are basic `SendDataPipeline` and `ReceiveDataPipeline` classes which will basically wrap up the basic handling for you, however if you want to have more control you can implement your own class from the ground up with `ISendDataPipeline` and `IReceiveDataPipeline` interfaces.

## Using The Framework

All you need to do is download the stable release and install the unity package.

### Docs

There are a load of documents covering the various classes and how to use them within the docs folder.

### Advised Setup

It is HIGHLY recommended you use some sort of DI system to setup your high level pipelines and just inject them in via IoC to your objects. This will make your setup and configuration far more sane.

For those using unity something like [Zenject](https://github.com/modesttree/Zenject) works wonders here.

### Dependencies

- `LazyData` (Which in turn depends on JSON.NET)
- `System.Net.Http`

Historically **LazyData** used to be part of this project, but realistically it could be consumed outside of here without any problem, so it made sense to make it into its own library.

#### HELP I USE UNITY!

If you are using Unity you can use either of these libraries instead which should work as drop in replacements:

- [JSON.NET (Free from Asset Store)](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347)
- [JSON.NET (Another one on github)](https://github.com/SaladLab/Json.Net.Unity3D)

## Docs

Check out the docs directory for docs which go into more detail.

## More Waffle

The general idea here is that this framework provides a basis for you to add your own transformers, processors and endpoints. So you may be fine with the default transformers and processors out of the box, but you will probably want to add some of your own for your own workflows.

You can just use pieces of this as well if you want, like if you just want to use the serialization, just take that and use it without the transformers and the other endpoints etc.

It was originally started as a branch in the [EcsRx](https://github.com/grofit/ecsrx) repository but as it was not tied to that was brought out into its own repo. 

The overall goal is to attempt to build simple building blocks for consumers to create larger workflows of data, such as scenarios like extracting data as JSON and posting to a web service, or extracting data as binary and putting into a flat file.

[build-status-image]: https://ci.appveyor.com/api/projects/status/wuthq2w1oavx24tf/branch/master?svg=true
[build-status-url]: https://ci.appveyor.com/project/grofit/persistity/branch/master
[nuget-image]: https://img.shields.io/nuget/v/persistity.svg
[nuget-url]: https://www.nuget.org/packages/persistity/
[discord-image]: https://img.shields.io/discord/488609938399297536.svg
[discord-url]: https://discord.gg/bS2rnGz