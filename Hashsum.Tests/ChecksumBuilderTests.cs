using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace Hashsum.Tests
{
    [TestFixture]
    public class ChecksumBuilderTests
    {
        [Test]
        public void Calculate_SameMutatorsSameOrder_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                var checksums = new[]
                {
                    builder
                        .Mutate("test string")
                        .Mutate(1337)
                        .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                        .Calculate()
                        .ToString(),

                    builder
                        .Mutate("test string")
                        .Mutate(1337)
                        .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                        .Calculate()
                        .ToString()
                };

                Assert.That(checksums, Is.All.Not.Null.Or.Empty);
                Assert.That(checksums, Is.All.EqualTo(checksums.First()));
            }
        }

        [Test]
        public void Calculate_SameMutatorsDifferentOrder_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                var checksums = new[]
                {
                    builder
                        .Mutate("test string")
                        .Mutate(1337)
                        .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                        .Calculate()
                        .ToString(),

                    builder
                        .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                        .Mutate(1337)
                        .Mutate("test string")
                        .Calculate()
                        .ToString()
                };

                Assert.That(checksums, Is.All.Not.Null.Or.Empty);
                Assert.That(checksums, Is.Unique);
            }
        }

        [Test]
        public void Calculate_DifferentMutators_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                var date = DateTimeOffset.Now;

                var checksums = new[]
                {
                    builder
                        .Mutate(1)
                        .Calculate()
                        .ToString(),

                    builder
                        .Mutate(2)
                        .Calculate()
                        .ToString(),

                    builder
                        .Mutate(date)
                        .Calculate()
                        .ToString(),

                    builder
                        .Mutate(date.AddMilliseconds(1))
                        .Calculate()
                        .ToString(),

                    builder
                        .Mutate(Guid.NewGuid())
                        .Calculate()
                        .ToString(),

                    builder
                        .Mutate(Guid.NewGuid())
                        .Calculate()
                        .ToString()
                };


                Assert.That(checksums, Is.All.Not.Null.Or.Empty);
                Assert.That(checksums, Is.Unique);
            }
        }

        [Test]
        public void Calculate_CultureInvariant_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures | CultureTypes.SpecificCultures);
                var checksums = new List<string>();

                foreach (var culture in cultures)
                {
                    Thread.CurrentThread.CurrentCulture = culture;
                    checksums.Add(builder
                        .Mutate(12345678912123)
                        .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                        .Mutate(-232323)
                        .Mutate(-5252323.123)
                        .Calculate()
                        .ToString());
                }

                Assert.That(checksums, Is.All.Not.Null.Or.Empty);
                Assert.That(checksums, Is.All.EqualTo(checksums.First()));
            }
        }
    }
}