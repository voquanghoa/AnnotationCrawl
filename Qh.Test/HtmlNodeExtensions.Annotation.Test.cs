using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fizzler.Systems.HtmlAgilityPack;
using Qh.Annotation.WebCrawler.Annotations;

namespace Qh.Annotation.WebCrawler.Test
{
    class Manufactory
    {
        [MapToHref("")]
        public string Url { get; set; }

        [MapToText("")]
        public string Name { get; set; }
    }

    class Specification
    {
        [MapToText(".network")]
        public string Network { get; set; }

        [MapToText(".resolution")]
        public string Resolution { get; set; }

        [MapToText(".resolution")]
        public string Os { get; set; }
    }

    class Production
    {
        [MapToText("strong")]
        public string Title { get; set; }

        [MapToAttr(".thumb", "src")]
        public string ThumbUrl { get; set; }

        [MapToText(".tag span")]
        public string[] Tags { get; set; }

        [MapTo(".specs")]
        public Specification Specification { get; set; }

        [MapTo("a")]
        public Manufactory Manufactory { get; set; }
    }

    class Shop
    {
        [MapToText("strong")]
        public string Title { get; set; }

        [MapToHref("a")]
        public string Url { get; set; }

        [MapTo(".all-productions .production")]
        public Production[] Productions { get; set; }
    }

    [TestClass]
    public class HtmlNodeExtensionsTest
    {
        public HtmlDocument Document { get; set; }

        public HtmlNodeExtensionsTest()
        {
            string html;
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "Qh.Test.Production.html";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new Exception($"Resource, named {resourceName}, not found.");
                using (var reader = new StreamReader(stream))
                    html = reader.ReadToEnd();
            }
            var Document = new HtmlDocument();
            Document.LoadHtml2(html);
        }

        [TestMethod]
        public void TestAllContentIsCorrect()
        {
            var shop = new Shop();
            Document.DocumentNode.Bind(shop);
            Assert.AreEqual(shop.Title, "Online shopping");
            Assert.AreEqual(shop.Url, "www.shop.com");

            Assert.AreEqual(shop.Productions.Length, 2);

            Assert.AreEqual(shop.Productions[0].Manufactory.Url, "samsung.com");
            Assert.AreEqual(shop.Productions[1].Manufactory.Name, "Apple");


            Assert.IsTrue(shop.Productions[0].Specification.Network.Contains("GSM / HSPA / LTE"));
            Assert.IsTrue(shop.Productions[1].Specification.Network.Contains("GSM / CDMA / HSPA / EVDO / LTE"));
        }
    }
}
