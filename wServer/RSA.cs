using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;

namespace wServer
{
    public class RSA
    {
        public static readonly RSA Instance = new RSA(@"
-----BEGIN RSA PRIVATE KEY-----
MIICXQIBAAKBgQCqccYHTj4QATbK1m4UVgcTeEYtDZwZxwdayVTFs1jAwKWzoDt/
CXhYUX2cEJnA93T4h6a4ysTfUrgBFZ3Nsb4W3/4Crc2TxjOWQOoWQnhzblZEiTcA
mIdwdq8AfuZWjRpGhOzHDPK9hcgySrzqD9HJAUsbG2ZzU5zrxHtEPC0nUQIDAQAB
AoGAGEZ4A9Za9ICXwy/jIbZW76jzpbsIfotgOsft8PJoM5u8febWUnOQ1lNU5oNe
wqL73+HCFxOaEox/dHgVr5QfFHtulePf0mKAq7/+irXFsHzatzjTG/yLUCvGuT3Y
2rAHo2HnLT/V6ymgBiUdDd1x/2xd1C1wh5/eXlhB0bBqwhkCQQDXiAHu71E79Fl9
dQzbKaD/S4PltvptBU/Den1vo5WcjGpH96gEcf5cd8ttJsh4Jjg+ywP98ySyfxf+
8/YpWIA3AkEAynKQ1Rd2+IcknB6RmHIH2IvuA9AygDiMyS0+UBg6TjwJENcVvDmT
IHM9a5GNM2b5oQ92MhxV6fY3pGSHjaKAtwJAIjw+Yl+tPaLAnEb9dcqVAjhSiSiZ
eru/QcOxLQyE9UrwjuHt1FbvGm0E6R+h9EPN51uEFNCMYbYCb8L8tPCT3QJBAIK5
A4h1hF7qdNtSadU0HJjZkjFoKJPe8BmfW1NAsbV+0qPLHr/RtY0InKpu6+w7HAnC
ACJPZbKTSE6gk18DhXUCQQCoPIr7Vf7pGmGtkNYAhQPa3ISkjlm/5DTSpTvABiYq
Ndk0XNr+U+9HLLxxEbQgUcfftRv/7kojO01LtmE743DJ
-----END RSA PRIVATE KEY-----");

        RsaEngine engine;
        AsymmetricKeyParameter key;
        private RSA(string privPem)
        {
            key = (new PemReader(new StringReader(privPem.Trim())).ReadObject() as AsymmetricCipherKeyPair).Private;
            engine = new RsaEngine();
            engine.Init(true, key);
        }

        public string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            byte[] dat = Convert.FromBase64String(str);
            var encoding = new Pkcs1Encoding(engine);
            encoding.Init(false, key);
            return Encoding.UTF8.GetString(encoding.ProcessBlock(dat, 0, dat.Length));
        }
        public string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            byte[] dat = Encoding.UTF8.GetBytes(str);
            var encoding = new Pkcs1Encoding(engine);
            encoding.Init(true, key);
            return Convert.ToBase64String(encoding.ProcessBlock(dat, 0, dat.Length));
        }
    }
}
