### v1.1.3 (17-Feb-2018)

- Downgraded .NET Standard target framework to 1.3.

### v1.1.2 (16-Feb-2018)

- Removed/added some mutators.
- `ChecksumBuilder` will now throw an exception when used after being disposed.

### v1.1.1 (23-Jan-2018)

- `ChecksumBuilder.Mutate()` methods will now throw on `null` values.
- Added equality methods to `Checksum`.

### v1.1 (22-Jan-2018)

- Dates are now formatted as UTC ticks.
- `ChecksumBuilder.Calculate()` now returns `Checksum` instead of a byte array.