﻿#region license

// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using SolrNet.Utils;

namespace SolrNet.Impl.ResponseParsers
{
    /// <summary>
    /// Parses spell-checking results from a query response
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    public class SpellCheckResponseParser<T> : ISolrResponseParser<T>
    {
        public void Parse(XDocument xml, AbstractSolrQueryResults<T> results)
        {
            results.Switch(query: r => Parse(xml, r),
                moreLikeThis: F.DoNothing);
        }

        public void Parse(XDocument xml, SolrQueryResults<T> results)
        {
            var spellCheckingNode = xml.XPathSelectElement("response/lst[@name='spellcheck']");
            if (spellCheckingNode != null)
                results.SpellChecking = ParseSpellChecking(spellCheckingNode);
        }

        /// <summary>
        /// Parses spell-checking results
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public SpellCheckResults ParseSpellChecking(XElement node)
        {
            var r = new SpellCheckResults();
            var suggestionsNode = node.XPathSelectElement("lst[@name='suggestions']");
            r.Collations = CollectionNodeChecker(node);

            var spellChecks = suggestionsNode.Elements("lst");
            foreach (var c in spellChecks)
            {
                var result = new SpellCheckResult();
                result.Query = c.Attribute("name")?.Value;
                result.NumFound = Convert.ToInt32(c.XPathSelectElement("int[@name='numFound']").Value);
                result.EndOffset = Convert.ToInt32(c.XPathSelectElement("int[@name='endOffset']").Value);
                result.StartOffset = Convert.ToInt32(c.XPathSelectElement("int[@name='startOffset']").Value);
                var suggestionNodes = c.XPathSelectElements("arr[@name='suggestion']/str");
                result.Suggestions = suggestionNodes.Select(suggestionNode => suggestionNode.Value).ToList();
                r.Add(result);
            }
            return r;
        }

        private IEnumerable<string> CollectionNodeChecker(XElement node)
        {
            //var xName = XName.Get("str");
            //var collationNode = node.Descendants(xName).Attributes("name").FirstOrDefault(y => y.Value == "collation");
            List<string> res = new List<string>();
            var collationNode = node.Descendants("str");//.Attributes("name");
            if (collationNode == null) return null;
            foreach (var xElement in collationNode)
            {
                if (!xElement.HasAttributes) continue;
                if (xElement.Attribute("name").Value.Equals("collation")) res.Add(xElement.Value);
            }
            if (res.Count < 1) return null;
            //var result=collationNode.Select(item => item.Parent).Select(i => i.Value);
            return res;
            //XElement collationNode;
            //List<String> sugg = new List<string>();
            //if ((collationNode = node.XPathSelectElement("lst[@name='collations']")) != null)
            //{
            //    sugg.AddRange(collationNode.Elements("str").Select(str => str.Value));
            //    return sugg;
            //}
            //var xElement = (collationNode = node.XPathSelectElement("lst[@name='suggestions']").XPathSelectElement("str[@name='collation']"));
            //if (xElement == null) return null;
            //sugg.Add(collationNode.Value);
            //return sugg;
        }
    }
}