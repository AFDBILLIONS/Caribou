﻿namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using Caribou.Data;

    /// <summary>
    /// Methods for parsing an XML file and extracting data that use XMLReader-based methods.
    /// </summary>
    public static class ParseViaXMLReader
    {
        private delegate void DispatchDelegate(XmlReader reader, ref RequestHandler request, bool onlyBuildings = false);

        public static void FindItemsByTag(ref RequestHandler request, OSMGeometryType typeToFind, bool readPathAsContents = false)
        {
            DispatchDelegate dispatchForType;
            if (typeToFind == OSMGeometryType.Node)
                dispatchForType = FindNodesInXML;
            else if (typeToFind == OSMGeometryType.Way || typeToFind == OSMGeometryType.Building)
                dispatchForType = FindWaysInXML;
            else
                dispatchForType = null; // Necessary to prevent below paths thinking variable not set

            bool onlyBuildings = false;
            if (typeToFind == OSMGeometryType.Building)
                onlyBuildings = true;

            GetBounds(ref request, readPathAsContents);
            foreach (string xmlPath in request.XmlPaths)
            {
                if (readPathAsContents)
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlPath)))
                    {
                        dispatchForType(reader, ref request, onlyBuildings); // Only used in testing
                    }
                }
                else
                {
                    using (XmlReader reader = XmlReader.Create(xmlPath))
                    {
                        dispatchForType(reader, ref request);
                    }
                }
            }
        }

        public static void FindNodesInXML(XmlReader reader, ref RequestHandler request, bool onlyBuildings)
        {
            string currentNodeId = "";
            double currentLat = 0;
            double currentLon = 0;
            var currentNodeMetaData = new Dictionary<string, string>();
            var ci = CultureInfo.InvariantCulture;

            // Loop (linearly) through all tags. Keep track of each node's metadata and coords while looping through its tags.
            // When encountering the next node add the tracked data.
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "node")
                    {
                        currentNodeId = reader.GetAttribute("id");
                        currentLat = Convert.ToDouble(reader.GetAttribute("lat"));
                        currentLon = Convert.ToDouble(reader.GetAttribute("lon"));
                    }
                    else if (reader.Name == "tag")
                    {
                        currentNodeMetaData[reader.GetAttribute("k").ToLower(ci)] = reader.GetAttribute("v");
                    }
                    else if (reader.Name == "way")
                    {
                         break;
                    }
                }
                else if (reader.Name == "node") // Closing out a node tag
                {
                    request.AddNodeIfMatchesRequest(currentNodeId, currentNodeMetaData, currentLat, currentLon);
                    currentNodeMetaData.Clear();
                }
            }
        }

        public static void FindWaysInXML(XmlReader reader, ref RequestHandler request, bool onlyBuildings)
        {
            string currentWayId = "";
            var currentWayMetaData = new Dictionary<string, string>();
            var currentWayNodes = new List<Coord>();
            var allNodes = new Dictionary<string, Coord>();
            var inANode = false; // Only needed for ways
            var ci = CultureInfo.InvariantCulture;

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "way")
                    {
                        currentWayId = reader.GetAttribute("id");
                        inANode = true; // Add tag data
                    }
                    else if (reader.Name == "node")
                    {
                        var nodeId = reader.GetAttribute("id");
                        allNodes[nodeId] = new Coord(
                            Convert.ToDouble(reader.GetAttribute("lat")),
                            Convert.ToDouble(reader.GetAttribute("lon")));
                    }
                    else if (inANode && reader.Name == "nd")
                    {
                        var ndId = reader.GetAttribute("ref");
                        currentWayNodes.Add(allNodes[ndId]);
                    }
                    else if (inANode && reader.Name == "tag")
                    {
                        currentWayMetaData[reader.GetAttribute("k").ToLower(ci)] = reader.GetAttribute("v");
                    }
                    else if (reader.Name == "relation")
                    {
                        break;
                    }
                }
                else
                {
                    inANode = false;
                    if (reader.Name == "way") // Closing out a way tag
                    {
                        // If finished looping over a prior node
                        if (onlyBuildings) 
                            request.AddBuildingIfMatchesRequest(currentWayId, currentWayMetaData, currentWayNodes); 
                        else
                            request.AddWayIfMatchesRequest(currentWayId, currentWayMetaData, currentWayNodes);
                    }

                    currentWayMetaData.Clear();
                    currentWayNodes.Clear();
                }
            }
        }

        // Identify a minimum and maximum boundary that encompasses all of the provided files' boundaries
        public static void GetBounds(ref RequestHandler result, bool readPathAsContents = false)
        {
            double? currentMinLat = null;
            double? currentMinLon = null;
            double? currentMaxLat = null;
            double? currentMaxLon = null;

            foreach (string xmlPath in result.XmlPaths)
            {
                XmlReader reader;
                if (readPathAsContents) 
                    reader = XmlReader.Create(new StringReader(xmlPath)); // Only used in testing
                else
                    reader = XmlReader.Create(xmlPath);
   
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "bounds")
                        {
                            CheckBounds(reader, ref currentMinLat, ref currentMinLon, ref currentMaxLat, ref currentMaxLon);
                            break;
                        }
                    }
                }
            }

            result.MinBounds = new Coord(currentMinLat.Value, currentMinLon.Value);
            result.MaxBounds = new Coord(currentMaxLat.Value, currentMaxLon.Value);
        }

        private static void CheckBounds(XmlReader reader, ref double? currentMinLat, ref double? currentMinLon,
                                                          ref double? currentMaxLat, ref double? currentMaxLon)
        {
            var boundsMinLat = Convert.ToDouble(reader.GetAttribute("minlat"));
            if (!currentMinLat.HasValue || boundsMinLat < currentMinLat)
            {
                currentMinLat = boundsMinLat;
            }

            var boundsMinLon = Convert.ToDouble(reader.GetAttribute("minlon"));
            if (!currentMinLon.HasValue || boundsMinLon < currentMinLon)
            {
                currentMinLon = boundsMinLon;
            }

            var boundsMaxLat = Convert.ToDouble(reader.GetAttribute("maxlat"));
            if (!currentMaxLat.HasValue || boundsMaxLat > currentMaxLat)
            {
                currentMaxLat = boundsMaxLat;
            }

            var boundsMaxLon = Convert.ToDouble(reader.GetAttribute("maxlon"));
            if (!currentMaxLon.HasValue || boundsMaxLon > currentMaxLon)
            {
                currentMaxLon = boundsMaxLon;
            }
        }
    }
}
