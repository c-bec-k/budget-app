using System;
using System.Security.Cryptography;

namespace BudgetApp;


public class GenID
{
  // GenID should return a ulong that has a
  // 35-bit timestamp since a specific date (in deciseconds)
  // 29-bit random number

  private readonly DateTime Epoch;
  private readonly byte[] rand;
  private readonly RandomNumberGenerator rng;

  public GenID()
  {
    Epoch = new DateTime(2022, 02, 16);
    rand = new byte[8];
    rng = RandomNumberGenerator.Create();
  }

  public ulong Next()
  {
    TimeSpan timestamp = DateTime.UtcNow - Epoch;
    ulong ts = (ulong)Math.Floor(timestamp.TotalMilliseconds * .01);
    Console.WriteLine(ts);
    rng.GetBytes(rand);
    ulong randoGen = (ulong)BitConverter.ToUInt64(rand);
    Console.WriteLine(randoGen);
    return (ts << 29) | (randoGen & 536870911);
  }
}