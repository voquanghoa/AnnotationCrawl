using System;
using System.Collections.Generic;
using System.Text;

namespace Fizzler.Systems.HtmlAgilityPack.Annontations
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
