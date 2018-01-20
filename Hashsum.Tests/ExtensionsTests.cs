using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace Hashsum.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void Mutate_By_Multiple_Values_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                var checksum = builder
                    .Mutate(new[] {1, 2, 3, 4, 5})
                    .Mutate(new[] {DateTimeOffset.Now, DateTimeOffset.UtcNow})
                    .Mutate(new[] {0.15, 3.14})
                    .Calculate();

                Assert.That(checksum, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void Mutate_By_Stream_Test()
        {
            var thisAssemblyFilePath = Assembly.GetExecutingAssembly().Location;

            using (var fileStream = File.OpenRead(thisAssemblyFilePath))
            using (var builder = new ChecksumBuilder())
            {
                var checksum = builder
                    .Mutate(fileStream)
                    .Calculate();

                Assert.That(checksum, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void CalculateToString_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                var checksum = builder
                    .Mutate(1)
                    .Mutate(2)
                    .Mutate(3)
                    .CalculateToString();

                Assert.That(checksum, Is.Not.Null.Or.Empty);
            }
        }
    }
}