using System;
using System.Linq;
using NUnit.Framework;
using Persistity;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Persistity.Registries;
using Persistity.Serialization;
using Persistity.Serialization.Binary;
using Persistity.Serialization.Json;
using Persistity.Serialization.Xml;
using Tests.Editor.Helpers;

namespace Tests.Editor.PerformanceTest
{
    [TestFixture]
    public class PerformanceScenarios
    {
        const int Iterations = 10000;

        private void RunSerializeAndDeserializeStep(object model, object modelList, ISerializer serializer, IDeserializer deserializer)
        {
            var startTime = DateTime.Now;
            for (var i = 0; i < Iterations; i++)
            { serializer.Serialize(model); }
            var endTime = DateTime.Now;
            var totalTime = endTime - startTime;
            var average = totalTime.TotalMilliseconds / Iterations;
            Console.WriteLine("Serialized {0} Entities in {1} with {2}ms average", Iterations, totalTime, average);

            startTime = DateTime.Now;
            var output = serializer.Serialize(modelList);
            endTime = DateTime.Now;
            totalTime = endTime - startTime;
            Console.WriteLine("Serialized Large Entity with {0} elements in {1}", Iterations, totalTime);
            Console.WriteLine("Large Entity Size {0}bytes", output.AsBytes.Length);

            startTime = DateTime.Now;
            deserializer.Deserialize(output);
            endTime = DateTime.Now;
            totalTime = endTime - startTime;
            Console.WriteLine("Deserialized Large Entity with {0} elements in {1}", Iterations, totalTime);
        }

        private void RunStepsForFormats(MappingRegistry mappingRegistry, object model, object modelList)
        {
            var typeCreator = new TypeCreator();

            ISerializer serializer;
            IDeserializer deserializer;
            DataObject warmupOutput;

            // Binary Warmup
            serializer = new BinarySerializer(mappingRegistry);
            serializer.Serialize(model);
            warmupOutput = serializer.Serialize(modelList);
            deserializer = new BinaryDeserializer(mappingRegistry, typeCreator);
            deserializer.Deserialize(warmupOutput);

            Console.WriteLine("");
            Console.WriteLine("Binary Serializing");
            RunSerializeAndDeserializeStep(model, modelList, serializer, deserializer);
            Console.WriteLine("");

            // JSON Warmup
            serializer = new JsonSerializer(mappingRegistry);
            serializer.Serialize(model);
            warmupOutput = serializer.Serialize(modelList);
            deserializer = new JsonDeserializer(mappingRegistry, typeCreator);
            deserializer.Deserialize(warmupOutput);

            Console.WriteLine("");
            Console.WriteLine("Json Serializing");
            RunSerializeAndDeserializeStep(model, modelList, serializer, deserializer);
            Console.WriteLine("");

            // XML Warmup
            serializer = new XmlSerializer(mappingRegistry);
            serializer.Serialize(model);
            warmupOutput = serializer.Serialize(modelList);
            deserializer = new XmlDeserializer(mappingRegistry, typeCreator);
            deserializer.Deserialize(warmupOutput);

            Console.WriteLine("");
            Console.WriteLine("Xml Serializing");
            RunSerializeAndDeserializeStep(model, modelList, serializer, deserializer);
            Console.WriteLine("");
        }

        [Test]
        public void test_performance_with_simple_models()
        {
            var model = new Person
            {
                Age = 99999,
                FirstName = "Windows",
                LastName = "Server",
                Sex = Sex.Male,
            };

            var modelList = new PersonList();
            modelList.Models = Enumerable.Range(Iterations, Iterations).Select(x => new Person { Age = x, FirstName = "Windows", LastName = "Server", Sex = Sex.Female }).ToArray();

            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new EverythingTypeMapper(typeAnalyzer);
            var mappingRegistry = new MappingRegistry(typeMapper);

            RunStepsForFormats(mappingRegistry, model, modelList);
        }

        [Test]
        public void test_performance_with_complex_models()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();

            var modelList = new ComplexModelList();
            modelList.Models = Enumerable.Range(Iterations, Iterations).Select(x => SerializationTestHelper.GeneratePopulatedModel()).ToArray();

            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new EverythingTypeMapper(typeAnalyzer);
            var mappingRegistry = new MappingRegistry(typeMapper);

            RunStepsForFormats(mappingRegistry, model, modelList);
        }

        [Test]
        public void test_performance_with_dynamic_models()
        {
            var model = SerializationTestHelper.GeneratePopulatedDynamicTypesModel();

            var modelList = new DynamicModelList();
            modelList.Models = Enumerable.Range(Iterations, Iterations).Select(x => SerializationTestHelper.GeneratePopulatedDynamicTypesModel()).ToArray();

            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new EverythingTypeMapper(typeAnalyzer);
            var mappingRegistry = new MappingRegistry(typeMapper);

            RunStepsForFormats(mappingRegistry, model, modelList);
        }
    }
}