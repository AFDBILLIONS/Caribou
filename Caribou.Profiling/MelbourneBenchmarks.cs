﻿namespace Caribou.Profiling
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using Caribou.Processing;

    public class MelbourneBenchmarks
    {
        // These are benchmarks for a medium sized XML case (10mbs)

        private string melbourneFile = Properties.Resources.MelbourneOSM;
        private RequestedFeature[] features = new RequestedFeature[]
        {
            new RequestedFeature("amenity", ""), new RequestedFeature("highway",  ""),
            new RequestedFeature("amenity", "restaurant"), new RequestedFeature("highway",  "residential")
        };

        public MelbourneBenchmarks()
        {
        }

        [Benchmark]
        public void TestParseViaXMLReader()
        {
            var result = Caribou.Processing.FindNodesViaXMLReader.FindByFeatures(features, melbourneFile);
        }
        
        [Benchmark]
        public void TestParseViaXMLDocument()
        {
            var result = Caribou.Processing.FindNodesViaXMLDocument.FindByFeatures(features, melbourneFile);
        }

        [Benchmark]
        public void TestParseViaLinq()
        {
            var result = Caribou.Processing.FindNodesViaLinq.FindByFeatures(features, melbourneFile);
        }
    }
}
