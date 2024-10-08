using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace LibBase
{
    // public class Arc4Random
    // {
    //     private struct Arc4Stream
    //     {
    //         public byte i;
    //         public byte j;
    //         public byte[] s;
    //
    //         public Arc4Stream(int size)
    //         {
    //             i = 0;
    //             j = 0;
    //             s = new byte[size];
    //             for (int k = 0; k < size; k++)
    //             {
    //                 s[k] = (byte)k;
    //             }
    //         }
    //     }
    //
    //     private static int lockVar = 0;
    //     private static Arc4Stream rs = new Arc4Stream(256);
    //     private static bool rsInitialized = false;
    //     private static bool rsStired = false;
    //     private static int arc4Count = 0;
    //
    //     private static readonly object LockObject = new object();
    //     private static readonly string RANDOMDEV = "/dev/random";
    //
    //     private static void SpinLock(ref int lockVar)
    //     {
    //         while (Interlocked.CompareExchange(ref lockVar, 1, 0) != 0) ;
    //     }
    //
    //     private static void SpinUnlock(ref int lockVar)
    //     {
    //         Interlocked.Exchange(ref lockVar, 0);
    //     }
    //
    //     private static void Arc4AddRandom(byte[] dat, int datlen)
    //     {
    //         int n;
    //         byte si;
    //
    //         rs.i--;
    //         for (n = 0; n < 256; n++)
    //         {
    //             rs.i = (byte)((rs.i + 1) % 256);
    //             si = rs.s[rs.i];
    //             rs.j = (byte)((rs.j + si + dat[n % datlen]) % 256);
    //             rs.s[rs.i] = rs.s[rs.j];
    //             rs.s[rs.j] = si;
    //         }
    //
    //         rs.j = rs.i;
    //     }
    //
    //     private static void Arc4Fetch()
    //     {
    //         byte[] rdat = new byte[KEYSIZE];
    //         int done = 0;
    //
    //         using (FileStream fs = new FileStream(RANDOMDEV, FileMode.Open, FileAccess.Read))
    //         {
    //             if (fs.Read(rdat, 0, KEYSIZE) == KEYSIZE)
    //             {
    //                 done = 1;
    //             }
    //         }
    //
    //         if (!done)
    //         {
    //             byte[] tv = BitConverter.GetBytes(DateTime.UtcNow.ToUniversalTime().Ticks);
    //             byte[] pid = BitConverter.GetBytes(System.Diagnostics.Process.GetCurrentProcess().Id);
    //             Array.Copy(tv, rdat, tv.Length);
    //             Array.Copy(pid, 0, rdat, tv.Length, pid.Length);
    //         }
    //     }
    //
    //     private static void Arc4Stir()
    //     {
    //         if (!rsDataAvailable)
    //         {
    //             Arc4Fetch();
    //         }
    //
    //         rsDataAvailable = false;
    //         Arc4AddRandom((byte[])rdat.Clone(), KEYSIZE);
    //
    //         for (int n = 0; n < 1024; n++)
    //         {
    //             Arc4GetByte();
    //         }
    //
    //         arc4Count = 1600000;
    //         rsStired = true;
    //     }
    //
    //     private static byte Arc4GetByte()
    //     {
    //         rs.i = (byte)((rs.i + 1) % 256);
    //         byte si = rs.s[rs.i];
    //         rs.j = (byte)((rs.j + si) % 256);
    //         byte sj = rs.s[rs.j];
    //
    //         rs.s[rs.i] = sj;
    //         rs.s[rs.j] = si;
    //
    //         return (rs.s[(si + sj) % 256]);
    //     }
    //
    //     private static uint Arc4GetWord()
    //     {
    //         return (uint)(Arc4GetByte() << 24 | Arc4GetByte() << 16 | Arc4GetByte() << 8 | Arc4GetByte());
    //     }
    //
    //     private static void Arc4RandomStir()
    //     {
    //         SpinLock(ref lockVar);
    //         Arc4Stir();
    //         SpinUnlock(ref lockVar);
    //     }
    //
    //     public static uint Arc4Random()
    //     {
    //         SpinLock(ref lockVar);
    //
    //         bool didStir = arc4CheckStir();
    //         uint rnd = Arc4GetWord();
    //         arc4Count -= 4;
    //
    //         SpinUnlock(ref lockVar);
    //         if (didStir)
    //         {
    //             Arc4Fetch();
    //             rsDataAvailable = true;
    //         }
    //
    //         return rnd;
    //     }
    //
    //     private static bool arc4CheckStir()
    //     {
    //         if (!rsStired || arc4Count <= 0)
    //         {
    //             Arc4Stir();
    //             return true;
    //         }
    //
    //         return false;
    //     }
    //
    //     private static int rsDataAvailable = 0;
    //     private const int KEYSIZE = 128;
    //
    //     public static void Arc4RandomAddRandom(byte[] dat, int datlen)
    //     {
    //         SpinLock(ref lockVar);
    //         Arc4CheckStir();
    //         Arc4AddRandom(dat, datlen);
    //         SpinUnlock(ref lockVar);
    //     }
    //
    //     public static void Arc4RandomBuf(byte[] buf, int n)
    //     {
    //         Arc4RandomBuf(buf, 0, n);
    //     }
    //
    //     public static void Arc4RandomBuf(byte[] buf, int offset, int length)
    //     {
    //         int didStir = 0;
    //
    //         SpinLock(ref lockVar);
    //
    //         while (length-- > 0)
    //         {
    //             if (arc4CheckStir())
    //             {
    //                 didStir = 1;
    //             }
    //
    //             buf[offset++] = Arc4GetByte();
    //             arc4Count--;
    //         }
    //
    //         SpinUnlock(ref lockVar);
    //         if (didStir > 0)
    //         {
    //             Arc4Fetch();
    //             rsDataAvailable = 1;
    //         }
    //     }
    //
    //     public static uint Arc4RandomUniform(uint upperBound)
    //     {
    //         if (upperBound < 2)
    //             return 0;
    //
    //         uint min = (uint)(0x100000000 % upperBound);
    //         uint r;
    //
    //         while (true)
    //         {
    //             r = Arc4Random();
    //             if (r >= min)
    //                 break;
    //         }
    //
    //         return r % upperBound;
    //     }
    // }
}