using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiChain.Models.MultichainManager
{
    public static class CryptoHelper
    {

        #region RSA
        public static AsymmetricCipherKeyPair GenerateRSAKeyPair()
        {
            var gen = new RsaKeyPairGenerator();
            gen.Init(new KeyGenerationParameters(new SecureRandom(), 1024));
            return gen.GenerateKeyPair();
        }
        public static string GetRSAPublicKey(AsymmetricCipherKeyPair keyPair)
        {
            using (TextWriter t = new StringWriter())
            {
                PemWriter writer = new PemWriter(t);
                writer.WriteObject(keyPair.Public);
                return t.ToString();
            }
        }
        public static string GetRSAPrivateKey(AsymmetricCipherKeyPair keyPair)
        {
            using (TextWriter t = new StringWriter())
            {
                PemWriter writer = new PemWriter(t);
                writer.WriteObject(keyPair.Private);
                return t.ToString();
            }
        }
        public static string EncryptRSA(string unencrypted, string pubKey)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(unencrypted);
            var encryptEngine = new Org.BouncyCastle.Crypto.Encodings.Pkcs1Encoding(new Org.BouncyCastle.Crypto.Engines.RsaEngine());
            using (var txtreader = new StringReader(pubKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();
                encryptEngine.Init(true, keyParameter);
            }
            var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
            return encrypted;
        }
        public static string DecryptRSA(string encrypted, string privKey)
        {
            var bytesToDecrypt = Convert.FromBase64String(encrypted);
            AsymmetricCipherKeyPair keyPair;
            var decryptEngine = new Org.BouncyCastle.Crypto.Encodings.Pkcs1Encoding(new RsaEngine());
            byte[] result;
            using (var txtreader = new StringReader(privKey))
            {
                keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();
                decryptEngine.Init(false, keyPair.Private);
                result = decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length);
            }
            var decrypted = Encoding.UTF8.GetString(result,0,result.Length);
            return decrypted;
        }
        #endregion

        #region AES
        public static string GenerateAESKey()
        {
            SecureRandom random = new SecureRandom();
            byte[] keyBytes = new byte[16];
            random.NextBytes(keyBytes);
            return Convert.ToBase64String(keyBytes);
        }

        public static string EncryptAES(byte[] unencrypted, string secret)
        {
            string unencryptedBase64 = Convert.ToBase64String(unencrypted);
            return EncryptAES(unencryptedBase64, secret);
        }

        public static string EncryptAES(string unencrypted, string secret)
        {
            PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine())); //Default scheme is PKCS5/PKCS7
            ParametersWithIV keyParam = new ParametersWithIV(new KeyParameter(Convert.FromBase64String(secret)), new byte[16], 0, 16);
            cipher.Init(true, keyParam);
            var inbytes = Encoding.UTF8.GetBytes(unencrypted);
            var outbytes = new byte[cipher.GetOutputSize(inbytes.Length)];
            var length = cipher.ProcessBytes(inbytes, outbytes, 0);
            cipher.DoFinal(outbytes, length);
            return Convert.ToBase64String(outbytes);
        }
        public static string DecryptAES(string encrypted, string secret)
        {
            PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine())); //Default scheme is PKCS5/PKCS7
            ParametersWithIV keyParam = new ParametersWithIV(new KeyParameter(Convert.FromBase64String(secret)), new byte[16], 0, 16);

            cipher.Init(false, keyParam);
            var inbytes = Convert.FromBase64String(encrypted);
            var outbytes = new byte[cipher.GetOutputSize(inbytes.Length)];
            var length = cipher.ProcessBytes(inbytes, outbytes, 0);
            cipher.DoFinal(outbytes, length);
            return Encoding.UTF8.GetString(outbytes,0,outbytes.Length).Replace("\0","");
        }
        #endregion

        #region Hash
        public static byte[] RIPEMD160(byte[] data)
        {
            RipeMD160Digest ripemd160 = new RipeMD160Digest();
            ripemd160.BlockUpdate(data, 0, data.Length);
            byte[] buffer = new byte[ripemd160.GetDigestSize()];
            ripemd160.DoFinal(buffer, 0);
            return buffer;
        }
        public static string RIPEMD160(string data)
        {
            var bytes = Hex.Hex2Bytes(data);
            var hashed = RIPEMD160(bytes);
            return Hex.Bytes2Hex(hashed);
        }
        public static byte[] SHA256(byte[] data, int offset = 0, int count = 0)
        {
            Sha256Digest sha256 = new Sha256Digest();
            if (count == 0)
                count = data.Length;
            sha256.BlockUpdate(data, offset, count);
            byte[] buffer = new byte[sha256.GetDigestSize()];
            sha256.DoFinal(buffer, 0);
            return buffer;
        }
        public static string SHA256(string data)
        {
            var bytes = Hex.Hex2Bytes(data);
            var hashed = SHA256(bytes);
            return Hex.Bytes2Hex(hashed);
        }
        #endregion

        #region ECDSA
        public static string GenerateSecp256k1PublicKey(string privateKey, bool useCompression)
        {
            var Ecc = SecNamedCurves.GetByName("secp256k1");
            var DomainParams = new ECDomainParameters(Ecc.Curve, Ecc.G, Ecc.N, Ecc.H);
            var bytes = Hex.Hex2Bytes(privateKey);
            BigInteger d = new BigInteger(bytes);
            ECPoint q = DomainParams.G.Multiply(d);
            q = q.Normalize();
            var publicParams = new ECPublicKeyParameters(q, DomainParams);
            FpPoint fp = new FpPoint(Ecc.Curve, q.AffineXCoord, q.AffineYCoord);
            return Hex.Bytes2Hex(fp.GetEncoded(useCompression));
        }
        #endregion

        #region Base58
        static bool IsSpace(char c)
        {
            switch (c)
            {
                case ' ':
                case '\t':
                case '\n':
                case '\v':
                case '\f':
                case '\r':
                    return true;
            }
            return false;
        }
        static readonly BigInteger bn58 = BigInteger.ValueOf(58);
        const string pszBase58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        internal static byte[] SafeSubarray(this byte[] array, int offset, int count)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (offset < 0 || offset > array.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (count < 0 || offset + count > array.Length)
                throw new ArgumentOutOfRangeException("count");
            if (offset == 0 && array.Length == count)
                return array;
            var data = new byte[count];
            Buffer.BlockCopy(array, offset, data, 0, count);
            return data;
        }
        internal static byte[] SafeSubarray(this byte[] array, int offset)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (offset < 0 || offset > array.Length)
                throw new ArgumentOutOfRangeException("offset");

            var count = array.Length - offset;
            var data = new byte[count];
            Buffer.BlockCopy(array, offset, data, 0, count);
            return data;
        }
        public static string EncodeBase58(byte[] data, int offset, int count)
        {

            BigInteger bn0 = BigInteger.Zero;

            // Convert big endian data to little endian
            // Extra zero at the end make sure bignum will interpret as a positive number
            var vchTmp = data.SafeSubarray(offset, count);

            // Convert little endian data to bignum
            var bn = new BigInteger(1, vchTmp);

            // Convert bignum to std::string
            StringBuilder builder = new StringBuilder();
            // Expected size increase from base58 conversion is approximately 137%
            // use 138% to be safe

            while (bn.CompareTo(bn0) > 0)
            {
                var r = bn.DivideAndRemainder(bn58);
                var dv = r[0];
                BigInteger rem = r[1];
                bn = dv;
                var c = rem.IntValue;
                builder.Append(pszBase58[c]);
            }

            // Leading zeroes encoded as base58 zeros
            for (int i = offset; i < offset + count && data[i] == 0; i++)
                builder.Append(pszBase58[0]);

            // Convert little endian std::string to big endian
            var chars = builder.ToString().ToCharArray();
            Array.Reverse(chars);
            var str = new String(chars); //keep that way to be portable
            return str;
        }
        public static byte[] DecodeBase58(string encoded)
        {
            if (encoded == null)
                throw new ArgumentNullException("encoded");

            var result = new byte[0];
            if (encoded.Length == 0)
                return result;
            BigInteger bn = BigInteger.Zero;
            int i = 0;
            while (IsSpace(encoded[i]))
            {
                i++;
                if (i >= encoded.Length)
                    return result;
            }

            for (int y = i; y < encoded.Length; y++)
            {
                var p1 = pszBase58.IndexOf(encoded[y]);
                if (p1 == -1)
                {
                    while (IsSpace(encoded[y]))
                    {
                        y++;
                        if (y >= encoded.Length)
                            break;
                    }
                    if (y != encoded.Length)
                        throw new FormatException("Invalid base 58 string");
                    break;
                }
                var bnChar = BigInteger.ValueOf(p1);
                bn = bn.Multiply(bn58);
                bn = bn.Add(bnChar);
            }

            // Get bignum as little endian data
            var vchTmp = bn.ToByteArrayUnsigned();
            Array.Reverse(vchTmp);
            if (vchTmp.All(b => b == 0))
                vchTmp = new byte[0];

            // Trim off sign byte if present
            if (vchTmp.Length >= 2 && vchTmp[vchTmp.Length - 1] == 0 && vchTmp[vchTmp.Length - 2] >= 0x80)
                vchTmp = vchTmp.SafeSubarray(0, vchTmp.Length - 1);

            // Restore leading zeros
            int nLeadingZeros = 0;
            for (int y = i; y < encoded.Length && encoded[y] == pszBase58[0]; y++)
                nLeadingZeros++;


            result = new byte[nLeadingZeros + vchTmp.Length];
            Array.Copy(vchTmp.Reverse().ToArray(), 0, result, nLeadingZeros, vchTmp.Length);
            return result;
        }
        #endregion

    }
}
