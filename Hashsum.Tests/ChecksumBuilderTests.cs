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
        public void Same_Mutators_In_Same_Order_Produce_Same_Checksums_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                var checksum1 = builder
                    .Mutate("test string")
                    .Mutate(1337)
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Calculate();

                var checksum2 = builder
                    .Mutate("test string")
                    .Mutate(1337)
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Calculate();

                Assert.That(checksum1, Is.Not.Null.Or.Empty);
                Assert.That(checksum2, Is.Not.Null.Or.Empty);
                Assert.That(checksum1, Is.Not.SameAs(checksum2));
                Assert.That(checksum1, Is.EquivalentTo(checksum2));
            }
        }

        [Test]
        public void Same_Mutators_In_Different_Order_Produce_Different_Checksums_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                var checksum1 = builder
                    .Mutate(1337)
                    .Mutate("test string")
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Calculate();

                var checksum2 = builder
                    .Mutate("test string")
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Mutate(1337)
                    .Calculate();

                Assert.That(checksum1, Is.Not.Null.Or.Empty);
                Assert.That(checksum2, Is.Not.Null.Or.Empty);
                Assert.That(checksum1, Is.Not.SameAs(checksum2));
                Assert.That(checksum1, Is.Not.EquivalentTo(checksum2));
            }
        }

        [Test]
        public void Different_Mutators_Produce_Different_Checksums_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                var checksum1 = builder
                    .Mutate("hello world")
                    .Mutate(99)
                    .Calculate();

                var checksum2 = builder
                    .Mutate("test string")
                    .Mutate(1337)
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Calculate();

                Assert.That(checksum1, Is.Not.Null.Or.Empty);
                Assert.That(checksum2, Is.Not.Null.Or.Empty);
                Assert.That(checksum1, Is.Not.SameAs(checksum2));
                Assert.That(checksum1, Is.Not.EquivalentTo(checksum2));
            }
        }

        [Test]
        public void Checksum_Is_Culture_Agnostic_Test()
        {
            using (var builder = new ChecksumBuilder())
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                var checksumEnUs = builder
                    .Mutate(12345678912123)
                    .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                    .Mutate(-232323)
                    .Calculate();

                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("tr-TR");
                var checksumTrTr = builder
                    .Mutate(12345678912123)
                    .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                    .Mutate(-232323)
                    .Calculate();

                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("zh-CN");
                var checksumZhCn = builder
                    .Mutate(12345678912123)
                    .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                    .Mutate(-232323)
                    .Calculate();

                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
                var checksumJaJp = builder
                    .Mutate(12345678912123)
                    .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                    .Mutate(-232323)
                    .Calculate();

                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("da-DK");
                var checksumDaDk = builder
                    .Mutate(12345678912123)
                    .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                    .Mutate(-232323)
                    .Calculate();

                Assert.That(checksumEnUs, Is.Not.Null.Or.Empty);
                Assert.That(checksumTrTr, Is.Not.Null.Or.Empty);
                Assert.That(checksumZhCn, Is.Not.Null.Or.Empty);
                Assert.That(checksumJaJp, Is.Not.Null.Or.Empty);
                Assert.That(checksumDaDk, Is.Not.Null.Or.Empty);

                Assert.That(checksumEnUs, Is.EquivalentTo(checksumTrTr));
                Assert.That(checksumEnUs, Is.EquivalentTo(checksumZhCn));
                Assert.That(checksumEnUs, Is.EquivalentTo(checksumJaJp));
                Assert.That(checksumEnUs, Is.EquivalentTo(checksumDaDk));
            }
        }
    }
}