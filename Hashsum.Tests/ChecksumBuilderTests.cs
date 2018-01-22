using System;
using System.Globalization;
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
                var checksum1 = builder
                    .Mutate("test string")
                    .Mutate(1337)
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Calculate()
                    .ToString();

                var checksum2 = builder
                    .Mutate("test string")
                    .Mutate(1337)
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Calculate()
                    .ToString();

                Assert.That(checksum1, Is.Not.Null.Or.Empty);
                Assert.That(checksum2, Is.Not.Null.Or.Empty);
                Assert.That(checksum1, Is.EqualTo(checksum2));
            }
        }

        [Test]
        public void Calculate_SameMutatorsDifferentOrder_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                var checksum1 = builder
                    .Mutate(1337)
                    .Mutate("test string")
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Calculate()
                    .ToString();

                var checksum2 = builder
                    .Mutate("test string")
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Mutate(1337)
                    .Calculate()
                    .ToString();

                Assert.That(checksum1, Is.Not.Null.Or.Empty);
                Assert.That(checksum2, Is.Not.Null.Or.Empty);
                Assert.That(checksum1, Is.Not.EqualTo(checksum2));
            }
        }

        [Test]
        public void Calculate_DifferentMutators_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                var checksum1 = builder
                    .Mutate(99)
                    .Mutate(Guid.NewGuid())
                    .Calculate()
                    .ToString();

                var checksum2 = builder
                    .Mutate(1337)
                    .Mutate(Guid.NewGuid())
                    .Calculate()
                    .ToString();

                Assert.That(checksum1, Is.Not.Null.Or.Empty);
                Assert.That(checksum2, Is.Not.Null.Or.Empty);
                Assert.That(checksum1, Is.Not.EqualTo(checksum2));
            }
        }

        [Test]
        public void Calculate_CultureInvariant_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                var checksumEnUs = builder
                    .Mutate(12345678912123)
                    .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                    .Mutate(-232323)
                    .Mutate(-5252323.123)
                    .Calculate()
                    .ToString();

                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("tr-TR");
                var checksumTrTr = builder
                    .Mutate(12345678912123)
                    .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                    .Mutate(-232323)
                    .Mutate(-5252323.123)
                    .Calculate()
                    .ToString();

                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("zh-CN");
                var checksumZhCn = builder
                    .Mutate(12345678912123)
                    .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                    .Mutate(-232323)
                    .Mutate(-5252323.123)
                    .Calculate()
                    .ToString();

                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
                var checksumJaJp = builder
                    .Mutate(12345678912123)
                    .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                    .Mutate(-232323)
                    .Mutate(-5252323.123)
                    .Calculate()
                    .ToString();

                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("da-DK");
                var checksumDaDk = builder
                    .Mutate(12345678912123)
                    .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                    .Mutate(-232323)
                    .Mutate(-5252323.123)
                    .Calculate()
                    .ToString();

                Assert.That(checksumEnUs, Is.Not.Null.Or.Empty);
                Assert.That(checksumTrTr, Is.Not.Null.Or.Empty);
                Assert.That(checksumZhCn, Is.Not.Null.Or.Empty);
                Assert.That(checksumJaJp, Is.Not.Null.Or.Empty);
                Assert.That(checksumDaDk, Is.Not.Null.Or.Empty);

                Assert.That(checksumEnUs, Is.EqualTo(checksumTrTr));
                Assert.That(checksumEnUs, Is.EqualTo(checksumZhCn));
                Assert.That(checksumEnUs, Is.EqualTo(checksumJaJp));
                Assert.That(checksumEnUs, Is.EqualTo(checksumDaDk));
            }
        }
    }
}