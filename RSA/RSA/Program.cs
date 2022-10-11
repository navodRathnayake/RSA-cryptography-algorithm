using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{

    public class RSA
    {
        protected RsaKeyParameters publickey;
        protected RSACryptoServiceProvider readPublic;

        protected RsaKeyParameters privatekey;
        protected RSACryptoServiceProvider readPrivate;

        public RSA(string path)
        {
            RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            AsymmetricCipherKeyPair keyPair = rsaKeyPairGenerator.GenerateKeyPair();

            this.publickey = (RsaKeyParameters)keyPair.Public;
            this.privatekey = (RsaKeyParameters)keyPair.Private;
            FIleHandling.writeFile(path, this.getPublicKeyInPem());
            FIleHandling.writeFile("private.pem", this.getPrivateKeyInPem());
        }

        public RSA()
        {
        }

        public string getPublicKeyInPem()
        {
            //To print the public key in pem format
            TextWriter textWriter1 = new StringWriter();
            PemWriter pemWriter1 = new PemWriter(textWriter1);
            pemWriter1.WriteObject(this.publickey);
            pemWriter1.Writer.Flush();
            string pemPublicKey = textWriter1.ToString();
            return pemPublicKey;
        }
        public string getPrivateKeyInPem()
        {
            //To print the private key in pem format
            TextWriter textWriter1 = new StringWriter();
            PemWriter pemWriter1 = new PemWriter(textWriter1);
            pemWriter1.WriteObject(this.privatekey);
            pemWriter1.Writer.Flush();
            string pemPrivateKey = textWriter1.ToString();
            return pemPrivateKey;
        }

    }
    //  RSA

    public class Encrypt : RSA
    {
        public void publicKeyGenerate(string publicKeyPath)
        {
            try
            {
                readPublic = GetPublicKeyFromPemFile(publicKeyPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Constructor : "+e.Message.ToString());
            }
        }

        

        public string encryption(string text)
        {
            var encryptedBytes = this.readPublic.Encrypt(Encoding.UTF8.GetBytes(text), false);
            return Convert.ToBase64String(encryptedBytes);
        }

      
        private RSACryptoServiceProvider GetPublicKeyFromPemFile(String filePath)
        {
            using (TextReader publicKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                RsaKeyParameters publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();

                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKeyParam);

                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }

    }
    //  Encrypt

    public class Decrypt : RSA
    {
        public void privateKeyGenerate()
        {
            try
            {
                
                Console.WriteLine($"[PROGRAM] : THE PRIVATE KEY HAS GENERATED IN Privet.pem PEM FILE");
                readPrivate = this.GetPrivateKeyFromPemFile("private.pem");
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] : DECRYPTION ERROR\n"+e.Message.ToString());
            }

        }
        
        public string decryption(string encrypted)
        {
            var decryptedBytes = readPrivate.Decrypt(Convert.FromBase64String(encrypted), false);
            return Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
        }

        public RsaKeyParameters getPrivateKey()
        {
            return this.privatekey;
        }
        private RSACryptoServiceProvider GetPrivateKeyFromPemFile(string filePath)
        {
            using (TextReader privateKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                AsymmetricCipherKeyPair readKeyPair = (AsymmetricCipherKeyPair)new PemReader(privateKeyTextReader).ReadObject();

                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)readKeyPair.Private);
                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }


    }
    //  Decrypt

    class FIleHandling
    {
        public static async Task writeFile(string fileName, string Content)
        {
            await File.WriteAllTextAsync(fileName, Content);
        }

        public static string read(string file)
        {
            string content = System.IO.File.ReadAllText(file);
            return content;
        }
    }
    //  File Handling

    class Program
    {
        static void Main(string[] args)
        {
            string extensionOfFile = args[0].Substring(args[0].Length - 3);
            int contentLength = FIleHandling.read(args[1]).Length;
            if(extensionOfFile == "pem")
            {
                if(!(File.Exists(args[1])))
                {
                    Console.WriteLine($"[WARNING] : MISSING FILE {args[1]}");
                    Console.WriteLine("[PROGRAM]  : RUN AGAIN WITH VALID INPUTS");
                    Console.Write("[PROGRAM]  : PRESS ANY KEY TO EXIT : ");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                else
                {
                    if(contentLength == 0)
                    {
                        Console.WriteLine($"[WARNING] : MISSING CONTENT OF {args[1]}");
                        Console.WriteLine("[PROGRAM]  : RUN AGAIN WITH VALID SOURCE FILE");
                        Console.Write("[PROGRAM]  : PRESS ANY KEY TO EXIT : ");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                    else
                    {
                        try
                        {
                            RSA algorithm = new RSA(args[0]);
                            Encrypt encrypt = new Encrypt();
                            Decrypt decrypt = new Decrypt();

                            encrypt.publicKeyGenerate(args[0]);
                            decrypt.privateKeyGenerate();

                            string cipher = encrypt.encryption(FIleHandling.read(args[1]));
                            FIleHandling.writeFile(args[2], cipher);
                            Console.WriteLine($"\n[PROGRAM] : THE DATA HAS ENCRYPTED AND STORED IN {args[0]} SUCCESSFULLY");
                            Console.WriteLine("[PROGRAM] : DECRYPTION PROCESS HAS STARTED");
                            string cipheredText = FIleHandling.read(args[2]);
                            string decrypted = decrypt.decryption(cipheredText);
                            FIleHandling.writeFile("decrypted.txt", decrypted);
                            Console.WriteLine($"\n[PROGRAM] : THE DATA HAS DECRYPTED IN [decrypted.txt] SUCCESSFULLY!");
                            Console.WriteLine("\n[PROGRAM] : PROGRAM HAS FINISHED SUCCESSFULLY!\n");
                            Console.ReadLine();
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("[ERROR] : EXCEPTION HAS FOUNDED IN MAIN PROCESS\n"+e.Message.ToString());
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"[WARNING] : PUBLIC KEY SHOULD BE IN PEM FORMAT INSTEAD OF .{extensionOfFile} EXTENSION");
                Console.WriteLine("[PROGRAM]  : RUN AGAIN WITH VALID FORMATS");
                Console.Write("[PROGRAM]  : PRESS ANY KEY TO EXIT : ");
                Console.ReadLine();
                Environment.Exit(0);
            }   
        }
    }
}
