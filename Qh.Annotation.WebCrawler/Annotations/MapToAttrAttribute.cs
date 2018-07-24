using System;
using System.Collections.Generic;
using System.Text;

namespace Qh.Annotation.WebCrawler.Annotations
{
    public class MapToAttrAttribute: MapToAttribute
    {
        public MapToAttrAttribute(string selector, string attribute, string @default = "")
        {
            Selector = selector;
            AttributeName = attribute;
            Default = @default;
            IsMapText = false;
        }
    }
}
