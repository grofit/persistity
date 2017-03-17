using System;

namespace Persistity.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PersistDataAttribute : Attribute
    {}
}