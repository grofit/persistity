# Pipelines

Pipleines can be thought of as steps in a bigger workflow that is expressed as a single object. So for example if you wanted to take a model and convert it to xml, encrypt it, then store it in a file normally this would be quite a few calls to extract, serializer, encrypt then finally store. So the pipelines give you a simple way to express these steps as a single useable object like so:

```csharp
// Create the pipeline which wraps the underlying steps
var saveToBinaryFilePipeline = new PipelineBuilder()
    .SerializeWith(binarySerializer)
    .ProcessWith(encryptionProcessor)
    .SendTo(writeFileEndpoint)
    .Build();
    
// Execute the pipeline with your game data
saveToBinaryFilePipeline.Execute(myGameData, SuccessCallback, ErrorCallback);
```

In an ideal world you would have DI and be adhering to IoC throughout your code base, so you should be able to setup all your pipelines when setting up your app, then just injecting them in to your objects like so:

```csharp
public class SomeClass
{
    private ISendDataPipeline _saveGamePipeline;
    
    public SomeClass(ISendDataPipeline saveGamePipeline)
    { _saveGamePipeline = saveGamePipeline; }
    
    // Use your pipeline whenever you want in your game
}
```

## Transforming Objects

You can also transform objects within the pipeline, this allows you to pass in an object, but have it converted to another object before serialization. This is handy for when you have a quite complex game object but want it transformed into a simpler intermediary object to be serialized. Then the deserialized data can transformed back into the original object, but you have to tell it how to do so by implementing your own custom `ITransformer` objects, then passing them into the pipeline, like so:

```
// Create the pipeline which transforms an object and saves
var saveToBinaryFilePipeline = new PipelineBuilder()
    .SerializeWith(binarySerializer)
    .TransformWith(someTransformer)
    .SendTo(writeFileEndpoint)
    .Build();
```

## Creating Pipelines

You can create pipelines 3 ways:

 - Using the `PipelineBuilder`
 - Implementing your own `ISendDataPipeline` or extending `SendDataPipeline` (same for receive)
 - Manually instantiating `SendDataPipeline` and passing steps into constructor (same for receive)

You would probably want to look at making your own pipelines and exposing them as interfaces to make your IoC contracts more explicit, like:

```csharp
public interface ISaveEncryptedDataPipeline : ISendDataPipeline
{}

public class SaveEncryptedDataPipeline : SendDataPipeline, ISaveEncryptedDataPipeline
{
    // ... implementation
}

public class SomeConsumerOfPipeline
{
    private ISaveEncryptedDataPipeline _savePipeline;
    
    public SomeConsumerOfPipeline(ISaveEncrypedDataPipeline savePipeline)
    { _savePipeline = savePipeline; }
}
```

This makes your code a bit more explanitory as you can see what type of pipeline you are have rather than it just being a high level send/recieve one its got more context, which helps with DI concerns and maintenance.