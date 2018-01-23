using System.Linq;
using Hashsum.Models;
using NUnit.Framework;

namespace Hashsum.Tests
{
    [TestFixture]
    public class ChecksumTests
    {
        [Test]
        public void Equality_Test()
        {
            var data = new byte[] {1, 2, 3, 4, 5};
            var checksums = new[]
            {
                new Checksum(data),
                new Checksum(data)
            };
            var checksumsToByteArrays = checksums.Select(c => c.ToByteArray()).ToArray();
            var checksumsToStrings = checksums.Select(c => c.ToString()).ToArray();

            Assert.That(checksums, Is.All.EqualTo(checksums.First()));
            Assert.That(checksumsToByteArrays, Is.All.EqualTo(checksumsToByteArrays.First()));
            Assert.That(checksumsToStrings, Is.All.EqualTo(checksumsToStrings.First()));
        }

        [Theory]
        public void ToString_Test(ChecksumStringFormat format)
        {
            var data = new byte[] {1, 2, 3, 4, 5};
            var checksum = new Checksum(data);
            var str = checksum.ToString(format);

            Assert.That(str, Is.Not.Null.Or.Empty);
        }
    }
}