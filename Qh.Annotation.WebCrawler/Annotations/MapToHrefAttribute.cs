using System;
using System.Collections.Generic;
using System.Text;

namespace Qh.Annotation.WebCrawler.Annotations
{
    public class MapToHrefAttribute: MapToAttrAttribute
    {
        public MapToHrefAttribute(string selector, string @default = ""): base
            (selector, "href", @default)
        {

        }
    }
}
