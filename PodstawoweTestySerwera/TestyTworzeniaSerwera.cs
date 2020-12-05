using System;
using SerwerTCPAsynch;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PodstawoweTestySerwera
{
    [TestClass]
    public class TestyTworzeniaSerwera
    {

        [TestMethod]
        public void TestPoprawnosciPortu()
        {
            try
            {
                SerwerTCPAsynch.SerwerAsynchroniczny<ProtokolSzyfrowania> serwer = new SerwerTCPAsynch.SerwerAsynchroniczny<ProtokolSzyfrowania>(IPAddress.Parse("127.0.0.1"), 2048);
                Assert.Fail();
            }
            catch (AssertFailedException)
            {
                Assert.Fail("błąd");
            }
            catch (Exception e)
            {

            }
        }

        [TestMethod]
        public void TestPoprawnosciIP()
        {
            try
            {
                SerwerTCPAsynch.SerwerAsynchroniczny<ProtokolSzyfrowania> serwer = new SerwerTCPAsynch.SerwerAsynchroniczny<ProtokolSzyfrowania>(IPAddress.Parse("127.0.0.10"), 2048);
                Assert.Fail();
            }
            catch (AssertFailedException)
            {
                Assert.Fail("błąd");
            }
            catch (Exception e)
            {

            }
        }

    }
}
