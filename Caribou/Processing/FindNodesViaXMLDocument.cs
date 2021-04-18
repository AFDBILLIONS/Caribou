﻿namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    public class FindNodesViaXMLDocument
    {
        public static ResultsForFeatures FindByFeatures(RequestedFeature[] featuresSpecified, string xmlContents)
        {
            var matches = new ResultsForFeatures(featuresSpecified); // Output
            var matchAllKey = RequestedFeature.SearchAllKey;
            double latitude = 0.0;
            double longitude = 0.0;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContents);
            XmlNode root = doc.DocumentElement;

            foreach (var tagKey in matches.Results.Keys)
            {
                XmlNodeList nodeList;

                if (matches.Results[tagKey].ContainsKey(matchAllKey))
                {
                    // If searching for all instances of a key regardless of the value
                    nodeList = root.SelectNodes("/osm/node/tag[@k='" + tagKey + "']");
                    foreach (XmlNode featureTag in nodeList)
                    {
                        var tagValue = featureTag.Attributes.GetNamedItem("v").Value;
                        var lat = Convert.ToDouble(featureTag.ParentNode.Attributes.GetNamedItem("lat").Value);
                        var lon = Convert.ToDouble(featureTag.ParentNode.Attributes.GetNamedItem("lon").Value);
                        matches.AddCoordForFeature(tagKey, tagValue, lat, lon);
                    }
                }
                else
                {
                    // If searching for a specific key and value attribute strings
                    foreach (string tagValue in matches.Results[tagKey].Keys)
                    {
                        nodeList = root.SelectNodes("/osm/node/tag[@k='" + tagKey + "' and @v='" + tagValue + "']");
                        foreach (XmlNode featureTag in nodeList)
                        {
                            var lat = Convert.ToDouble(featureTag.ParentNode.Attributes.GetNamedItem("lat").Value);
                            var lon = Convert.ToDouble(featureTag.ParentNode.Attributes.GetNamedItem("lon").Value);
                            matches.AddCoordForFeatureAndSubFeature(tagKey, tagValue, lat, lon);
                        }
                    }
                }
            }

            return matches;
        }
    }
}
