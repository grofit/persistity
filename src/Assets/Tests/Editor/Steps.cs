namespace Tests.Editor
{
    public interface IStep<Tin, Tout>
    {
        Tout Execute(Tin data);
    }

    public interface ITransformStep<Tin, Tout> : IStep<Tin, Tout>
    {

    }

    // new Pipeline().Add(new IJsonTransformer()).Add(new EncryptionProcessor)
}