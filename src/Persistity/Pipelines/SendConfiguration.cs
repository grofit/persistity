namespace Persistity.Pipelines
{
    public class SendConfiguration
    {
        /// <summary>
        /// Sometimes you will not want to send type information if the pipeline is one way to a HTTP endpoint etc
        /// So this allows you to omit type information for deserializing on the pass back through
        /// </summary>
        public bool IncludeType { get; set; } = true;
    }
}