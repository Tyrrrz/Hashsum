using System;
using System.Collections.Generic;
using System.IO;
using Hashsum.Internal;

namespace Hashsum
{
    /// <summary>
    /// Extensions for <see cref="Hashsum"/>.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Mutates checksum by given values.
        /// </summary>
        public static ChecksumBuilder Mutate<T>(this ChecksumBuilder builder, IEnumerable<T> values)
            where T : IFormattable
        {
            builder.GuardNotNull(nameof(builder));
            values.GuardNotNull(nameof(values));

            foreach (var value in values)
                builder.Mutate(value);

            return builder;
        }

        /// <summary>
        /// Mutates checksum by given dates.
        /// </summary>
        public static ChecksumBuilder Mutate(this ChecksumBuilder builder, IEnumerable<DateTimeOffset> dates)
        {
            builder.GuardNotNull(nameof(builder));
            dates.GuardNotNull(nameof(dates));

            foreach (var date in dates)
                builder.Mutate(date);

            return builder;
        }

        /// <summary>
        /// Mutates checksum by given stream.
        /// </summary>
        public static ChecksumBuilder Mutate(this ChecksumBuilder builder, Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return builder.Mutate(memoryStream.ToArray());
            }
        }
    }
}