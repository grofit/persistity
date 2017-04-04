using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Persistity.Registries;
using Persistity.Serialization.Binary;

namespace Tests.Editor.PerformanceTest
{
    [TestFixture]
    public class PerformanceScenarios
    {
        const int Iterations = 100000;

        [Test]
        public void test_performance_with_simple_models()
        {
            var person = new Person
            {
                Age = 99999,
                FirstName = "Windows",
                LastName = "Server",
                Sex = Sex.Male,
            };

            var personList = new PersonList();
            personList.People = Enumerable.Range(Iterations, Iterations).Select(x => new Person { Age = x, FirstName = "Windows", LastName = "Server", Sex = Sex.Female }).ToArray();

            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new EverythingTypeMapper(typeAnalyzer);
            var mappingRegistry = new MappingRegistry(typeMapper);
            var serializer = new BinarySerializer(mappingRegistry);

            // Warmup
            serializer.Serialize(person);
            serializer.Serialize(personList);

            Console.WriteLine("Serializing");

            var startTime = DateTime.Now;
            for (var i = 0; i < Iterations; i++)
            { serializer.Serialize(person); }
            var endTime = DateTime.Now;
            var totalTime = endTime - startTime;
            var average = totalTime.TotalMilliseconds / Iterations;
            Console.WriteLine("Serialized {0} Entities in {1} with {2}ms average", Iterations, totalTime, average);

            startTime = DateTime.Now;
            var output = serializer.Serialize(personList);
            endTime = DateTime.Now;
            totalTime = endTime - startTime;
            Console.WriteLine("Serialized Large Entity with {0} elements in {1}", Iterations, totalTime);
            Console.WriteLine("Large Entity Size {0}bytes", output.AsBytes.Length);

        }
    }
}