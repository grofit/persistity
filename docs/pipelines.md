# Pipelines

Pipelines are built up of lots of small steps that all form a continual flow, they are very much like streams and link from one step to another.

In most cases you will split these out into:

- **Incoming Pipelines** (i.e read data from a file and do stuff with it)
- **Outgoing Pipelines** (i.e take data do something with it then put it in a file)

To keep things simple you can implement your own Pipelines however you want but there is a builder available to let you build your own pipeline from smaller steps like the example below.

```csharp
// Create the pipeline which wraps the underlying steps
var saveToBinaryFilePipeline = new PipelineBuilder()
    .StartFromInput()
    .SerializeWith(binarySerializer)
    .ProcessWith(encryptionProcessor)
    .ThenSendTo(writeFileEndpoint)
    .Build();
    
// Execute the pipeline with your game data
await saveToBinaryFilePipeline.Execute(myGameData);
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

```csharp
// Create the pipeline which transforms an object and saves
var saveToBinaryFilePipeline = new PipelineBuilder()
    .StartFromInput()
    .SerializeWith(binarySerializer)
    .TransformWith(someTransformer)
    .ThenSendTo(writeFileEndpoint)
    .Build();
```

## Creating Pipelines

You can create pipelines 2 ways:

 - Using the `PipelineBuilder`
 - Implementing your own `IPipeline`

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

This makes your code a bit more explanatory as you can see what type of pipeline you are have rather than it just being a high level send/recieve one its got more context, which helps with DI concerns and maintenance.

## State

So as pipelines are meant to be created once and re-used there is the notion of state that is an optional argument on executing pipelines, this is not used by the default implementations, but if you make custom pipeline implementations you can make use of the state to allow each call to provide extra contextual information for each call.

For example if you wanted to have a single pipeline which could actually go to multiple files you may want to make a custom implementation where you provide some state that provides information on where the file should go, or even a token to tie it to some other bit of information, like the id of something. 