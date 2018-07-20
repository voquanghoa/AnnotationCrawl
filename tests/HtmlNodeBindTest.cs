using Fizzler.Systems.HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack.Annontations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fizzler.Tests
{
    class FirstParagraph
    {
        [MapToAttr("a", "href")]
        public string Url { get; set; }

        [MapToText("a")]
        public string Greeting { get; set; }

        [MapToAttr("span", "class")]
        public string Class { get; set; }

        [MapToText("span")]
        public string[] Texts { set; get; }
    }

    class SelectorTest
    {
        [MapToText("#someOtherDiv")]
        public string SomeOtherDiv { get; set; }

        [MapTo("p")]
        public FirstParagraph FirstParagraph { get; set; }

        [MapToText(".checkit")]
        public List<string> CheckIt { get; set; }
    }

    [TestFixture]
    public class HtmlNodeBindTest: SelectorBaseTest
    {
        [Test]
        public void TestMapToText()
        {
            var selectorTest = new SelectorTest();

            Document.DocumentNode.Bind(selectorTest);

            Assert.AreEqual(selectorTest.SomeOtherDiv, "subdiv!");
        }

        [Test]
        public void TestMapToObject()
        {
            var selectorTest = new SelectorTest();

            Document.DocumentNode.Bind(selectorTest);

            Assert.IsNotNull(selectorTest.FirstParagraph);

            Assert.AreEqual(selectorTest.FirstParagraph.Class, "hyphen-separated");
        }

        [Test]
        public void TestMapToAttr()
        {
            var selectorTest = new SelectorTest();

            Document.DocumentNode.Bind(selectorTest);

            Assert.AreEqual(selectorTest.FirstParagraph.Class, "hyphen-separated");
        }

        [Test]
        public void TestMapToAttrArray()
        {
            var selectorTest = new SelectorTest();

            Document.DocumentNode.Bind(selectorTest);

            Assert.AreEqual(selectorTest.FirstParagraph.Texts[0], "test");
            Assert.AreEqual(selectorTest.FirstParagraph.Texts[1], "oh");
            Assert.AreEqual(selectorTest.FirstParagraph.Texts.Length, 2);
        }
    }
}
