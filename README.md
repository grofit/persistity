# Persistity

A pipeline based data persistence framework for unity (a bit like ETL).

[![Gitter](https://badges.gitter.im/grofit/persistity.svg)](https://gitter.im/grofit/persistity?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

## What is it?

It provides a framework for extracting data from your models into any format you want and doing stuff with it.

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
saveToBinaryFilePipeline.Execute(myGameData, SuccessCallback, ErrorCallback);
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
saveToBinaryFilePipeline.Execute(myGameData, SuccessCallback, ErrorCallback);
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
loadBinaryFilePipeline.Execute<MyGameData>(myGameData => { ... }, ErrorCallback);
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

### Advised Setup

It is HIGHLY recommended you use some sort of DI system to setup your high level pipelines and just inject them in via IoC to your objects. This will make your setup and configuration far more sane, something like [Zenject](https://github.com/modesttree/Zenject) works wonders here.

### Dependencies

- [JSON.NET (for unity)](https://github.com/SaladLab/Json.Net.Unity3D)

If you do not need to support JSON then you can remove the json serializer files and you will not need the above dependency.

It has been created to have as little dependencies as possible, if unity was a little better at dependency management I would love to use promises and include some http, gzip and other libs to provide more stuff built in, but it would cause headaches for consumers so this may be looked at in some way as it would be great to provide RESTful interactions, GZipping processors and some other common use cases within the lib, but in an opt-in fashion.

## Docs

Check out the docs directory for docs which go into more detail.

## More Waffle

The general idea here is that this framework provides a basis for you to add your own transformers, processors and endpoints. So you may be fine with the default transformers and processors out of the box, but you will probably want to add some of your own for your own workflows.

You can just use pieces of this as well if you want, like if you just want to use the serialization, just take that and use it without the transformers and the other endpoints etc.

It was originally started as a branch in the [EcsRx](https://github.com/grofit/ecsrx) repository but as it was not tied to that was brought out into its own repo. 

The overall goal is to attempt to build simple building blocks for consumers to create larger workflows of data, such as scenarios like extracting data as JSON and posting to a web service, or extracting data as binary and putting into a flat file.
