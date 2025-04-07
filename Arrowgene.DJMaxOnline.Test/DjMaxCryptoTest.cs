using System.Text;
using Arrowgene.DJMaxOnline.Server;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Arrowgene.DJMaxOnline.Test;

public class DjMaxCryptoTest
{
    [Test]
    public void Test1()
    {
        DjMaxCrypto crypto = DjMaxCrypto.Init();

        Span<byte> test = Encoding.UTF8.GetBytes("This is a test");
        Span<byte> test2 = Encoding.UTF8.GetBytes("With a 2nd part");

        crypto.Encrypt(ref test);
        crypto.Encrypt(ref test2);
        crypto.Decrypt(ref test);
        crypto.Decrypt(ref test2);

        Assert.AreEqual("This is a test", Encoding.UTF8.GetString(test));
        Assert.AreEqual("With a 2nd part", Encoding.UTF8.GetString(test));
    }
}