using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace Fizzler.Systems.HtmlAgilityPack.Annontations
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
