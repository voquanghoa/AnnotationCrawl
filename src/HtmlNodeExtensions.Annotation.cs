#region Copyright and License
//
// Fizzler - CSS Selector Engine for Microsoft .NET Framework
// Copyright (c) 2009 Atif Aziz, Colin Ramsay. All rights reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
// details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation, Inc.,
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
#endregion

namespace Fizzler.Systems.HtmlAgilityPack
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using global::HtmlAgilityPack;
    using Fizzler.Systems.HtmlAgilityPack.Annontations;

    #endregion

    /// <summary>
    /// HtmlNode extension methods.
    /// </summary>
    public static partial class HtmlNodeExtensions
    {
        private static bool IsSupportedPrimitiveType(Type type)
        {
            return type == typeof(string);
        }

        private static bool IsSupportedListPrimitiveType(Type type)
        {
            return type == typeof(List<string>);
        }

        public static void Bind<T>(this HtmlNode node, T target) where T: class
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var mapableProperties = FindAllPropertiesWithMapTo(target.GetType()).ToList();

            foreach (var property in mapableProperties)
            {
                var type = property.Key.PropertyType;
                if (IsSupportedPrimitiveType(type))
                {
                    property.Value.BindPrimitiveProperty(node, target, property.Key);
                }
                else if (type.IsArray)
                {
                    if (IsSupportedPrimitiveType(type.GetElementType()))
                    {
                        property.Value.BindPrimitiveProperties(node, target, property.Key);
                    }
                    else
                    {
                        property.Value.BindObjectProperties(node, target, property.Key);
                    }
                }
                else
                {
                    property.Value.BindObjectProperty(node, target, property.Key);
                }
            }
        }

        private static IEnumerable<KeyValuePair<PropertyInfo, MapToAttribute>> FindAllPropertiesWithMapTo(Type type)
        {
            foreach (var propertyInfor in type.GetRuntimeProperties())
            {
                var mapAttributes = propertyInfor
                        .GetCustomAttributes()
                        .OfType<MapToAttribute>()
                        .ToList();

                if (mapAttributes.Count > 1)
                {
                    throw new Exception("Multiple annotations is not allowed for property !" + propertyInfor.Name);
                }
                
                if (mapAttributes.Count == 1)
                {
                    if (!propertyInfor.CanWrite)
                    {
                        throw new Exception("Can not bind value to the property: " + propertyInfor.Name + " because it's readonly");
                    }

                    yield return new KeyValuePair<PropertyInfo, MapToAttribute>(propertyInfor, mapAttributes.First());
                }
            }
        }
    }
}
