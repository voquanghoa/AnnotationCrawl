using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Fizzler.Systems.HtmlAgilityPack.Annontations
{
    /// <summary>
    /// Map property by element
    /// </summary>
    public class MapToAttribute: Attribute
    {
        public string Selector { get; set; }

        public bool IsMapText { get; set; } = false;

        public string AttributeName { set; get; }

        public string Default { set; get; }

        public MapToAttribute()
        {

        }

        public MapToAttribute(string selector)
        {
            Selector = selector;
        }

        private string StringOr(string value, string alternative)
        {
            return string.IsNullOrEmpty(value) ? alternative : value;
        }

        private HtmlNode SelectOne(HtmlNode htmlNode)
        {
            if (string.IsNullOrEmpty(Selector))
            {
                return htmlNode;
            }
            return htmlNode.QuerySelector(Selector);
        }

        private List<HtmlNode> SelectMany(HtmlNode htmlNode)
        {
            return htmlNode.QuerySelectorAll(Selector).ToList();
        }

        internal void BindPrimitiveProperty(HtmlNode sourceNode, object target, PropertyInfo propertyInfor)
        {
            object value = null;

            if (IsMapText)
            {
                value = StringOr(SelectOne(sourceNode)?.InnerText, Default);
            }
            else
            {
                value = StringOr(SelectOne(sourceNode)?.Attributes[AttributeName]?.Value, Default);
            }

            propertyInfor.SetValue(target, value);
        }

        internal void BindObjectProperty(HtmlNode sourceNode, object target, PropertyInfo propertyInfor)
        {
            var value = Activator.CreateInstance(propertyInfor.PropertyType);

            SelectOne(sourceNode)?.Bind(value);

            propertyInfor.SetValue(target, value);
        }


        internal void BindPrimitiveProperties(HtmlNode sourceNode, object target, PropertyInfo propertyInfor)
        {
            object value = null;
            var sourceNodes = SelectMany(sourceNode);

            if (IsMapText)
            {
                value = sourceNodes.Select(x => StringOr(x.InnerText, Default)).ToArray();
            }
            else
            {
                value = sourceNodes.Select(x => StringOr(x.Attributes[AttributeName]?.Value, Default)).ToArray();
            }

            propertyInfor.SetValue(target, value);
        }

        internal void BindObjectProperties(HtmlNode sourceNode, object target, PropertyInfo propertyInfor)
        {
            var sourceNodes = SelectMany(sourceNode);
            var propertyType = propertyInfor.PropertyType;
            
            var elementType = propertyType.GetGenericTypeDefinition().GenericTypeArguments[0];

            var array = sourceNodes.Select(x =>
            {
                var obj = Activator.CreateInstance(elementType);

                x.Bind(obj);

                return obj;

            }).ToArray();

            propertyInfor.SetValue(target, array);
        }
    }
}
