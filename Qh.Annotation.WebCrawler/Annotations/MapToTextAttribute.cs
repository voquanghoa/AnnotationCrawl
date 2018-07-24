using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace Qh.Annotation.WebCrawler.Annotations
{
    public class MapToTextAttribute : MapToAttribute
    {
        public MapToTextAttribute(string selector, string @default = "")
        {
            Selector = selector;
            Default = @default;
            IsMapText = true;
        }
    }
}
