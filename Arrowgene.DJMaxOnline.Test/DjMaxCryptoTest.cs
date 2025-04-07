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
        crypto.Decrypt(ref test);
        crypto.Encrypt(ref test2);
        crypto.Decrypt(ref test2);
        
        Assert.That(Encoding.UTF8.GetString(test), Is.EqualTo("This is a test"));
        Assert.That(Encoding.UTF8.GetString(test2), Is.EqualTo("With a 2nd part"));
    }
}