using NUnit.Framework;
using Persistity.Extensions;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Tests.Editor.Helpers;
using Tests.Editor.Models;

namespace Tests.Editor.TypeMapper
{
    [TestFixture]
    public class DefaultTypeMapperTests
    {
        [Test]
        public void should_correctly_map_complex_type()
        {
            var type = typeof(ComplexModel);
            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new DefaultTypeMapper(typeAnalyzer);
            var typeMapping = typeMapper.GetTypeMappingsFor(type);

            Assert.That(typeMapping, Is.Not.Null);
            Assert.That(typeMapping.Type, Is.EqualTo(type));
            Assert.That(typeMapping.Name, Is.EqualTo(type.GetPersistableName()));

            TypeAssertionHelper.AssertComplexModel(typeMapping.InternalMappings);
        }

        [Test]
        public void should_correctly_map_dynamic_type()
        {
            var type = typeof(DynamicTypesModel);
            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new DefaultTypeMapper(typeAnalyzer);
            var typeMapping = typeMapper.GetTypeMappingsFor(type);

            Assert.That(typeMapping, Is.Not.Null);
            Assert.That(typeMapping.Type, Is.EqualTo(type));
            Assert.That(typeMapping.Name, Is.EqualTo(type.GetPersistableName()));

            TypeAssertionHelper.AssertDynamicModel(typeMapping.InternalMappings);
        }
    }
}