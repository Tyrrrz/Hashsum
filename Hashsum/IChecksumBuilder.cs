using System;
using System.IO;
using System.Security.Cryptography;
using Hashsum.Models;

namespace Hashsum
{
    /// <summary>
    /// Interface for <see cref="ChecksumBuilder"/>.
    /// </summary>
    public interface IChecksumBuilder
    {
        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        ChecksumBuilder Mutate(string value);

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        ChecksumBuilder Mutate(char value);

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        ChecksumBuilder Mutate(IFormattable value);

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        ChecksumBuilder Mutate(bool value);

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        ChecksumBuilder Mutate(TimeSpan value);

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        ChecksumBuilder Mutate(DateTime value);

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        ChecksumBuilder Mutate(DateTimeOffset value);

        /// <summary>
        /// Mutates checksum by given data.
        /// </summary>
        ChecksumBuilder Mutate(byte[] data);

        /// <summary>
        /// Mutates checksum by given stream.
        /// </summary>
        ChecksumBuilder Mutate(Stream stream);

        /// <summary>
        /// Calculates the checksum using given algorithm.
        /// </summary>
        Checksum Calculate(HashAlgorithm algorithm, bool disposeAlgorithm = true);

        /// <summary>
        /// Calculates the checksum using <see cref="SHA256"/> algorithm.
        /// </summary>
        Checksum Calculate();
    }
}