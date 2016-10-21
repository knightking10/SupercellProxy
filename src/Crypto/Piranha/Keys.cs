using System;
using System.Net;

namespace SupercellProxy
{
    class Keys
    {
        private static WebClient KeyDownloader = new WebClient();

        /// <summary>
        /// The generated private key, according to the modded public key.
        /// </summary>
        public static byte[] GeneratedPrivateKey
            => "1891d401fadb51d25d3a9174d472a9f691a45b974285d47729c45c6538070d85".ToByteArray();

        /// <summary>
        /// The modded public key.
        /// </summary>
        public static byte[] ModdedPublicKey
            => "72f1a4a4c48e44da0c42310f800e96624e6dc6a641a9d41c3b5039d8dfadc27e".ToByteArray();

        /// <summary>
        /// The original, unmodified public key.
        /// </summary>
        public static byte[] OriginalPublicKey { get; set; }


        /// <summary>
        /// Downloads the latest public keys from InfinityModding
        /// </summary>
        public static void GetPublicKey()
        {
            /* CoC 8.332.16  = "BB9CA4C6B52ECDB40267C3BCCA03679201A403EF6230B9E488DB949B58BC7479".ToByteArray();
               CR 1.5.0  = "BBDBA8653396D1DF84EFAEA923ECD150D15EB526A46A6C39B53DAC974FFF3829".ToByteArray();
               BB 27.136  = "3BF256F1C9457F4465625DBA145F2BA2F65B64338351590E796E8119E648755D".ToByteArray();
             */
            if (Config.Game == Game.BOOM_BEACH)
                OriginalPublicKey = "3BF256F1C9457F4465625DBA145F2BA2F65B64338351590E796E8119E648755D".ToByteArray();
            else if (Config.Game == Game.CLASH_OF_CLANS)
                OriginalPublicKey = "BB9CA4C6B52ECDB40267C3BCCA03679201A403EF6230B9E488DB949B58BC7479".ToByteArray();
            else if (Config.Game == Game.CLASH_ROYALE)
                OriginalPublicKey = "BBDBA8653396D1DF84EFAEA923ECD150D15EB526A46A6C39B53DAC974FFF3829".ToByteArray();

            Logger.Log("Latest public key for " + Config.Game.ReadableName() + " initialized:");
            Logger.Log(OriginalPublicKey.ToHexString());
        }
    }

}
