using System;
using System.Security.Cryptography;

namespace BudgetApp
{
  public class GenID
  {
    // GenID should return a ulong that has a
    // 35-bit timestamp since a specific date (in deciseconds)
    // 29-bit random number

    private static readonly DateTime Epoch = new DateTime(2022, 02, 16);
    private readonly byte[] rand = new byte[4];
    private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

    public ulong Next()
    {
      TimeSpan timestamp = DateTime.UtcNow - Epoch;
      ulong ts = (ulong)Math.Floor(timestamp.TotalMilliseconds * .01);
      rng.GetBytes(rand);
      ulong randoGen = (ulong)BitConverter.ToUInt64(rand);
      return (ts << 29) | (randoGen & 536870911);
    }
  }
}