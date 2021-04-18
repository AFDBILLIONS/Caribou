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

    public class ChicagoBenchmarks
    {
        // These are benchmarks for a large XML case (100mbs)

        private string chicagoFile = Properties.Resources.ChicagoOSM;
        private RequestedFeature[] features = new RequestedFeature[]
        {
            new RequestedFeature("amenity", ""), new RequestedFeature("highway",  ""),
            new RequestedFeature("amenity", "restaurant"), new RequestedFeature("highway",  "residential")
        };

        public ChicagoBenchmarks()
        {
        }

        [Benchmark]
        public void TestParseViaXMLReader()
        {
            var result = Caribou.Processing.FindNodesViaXMLReader.FindByFeatures(features, chicagoFile);
        }
        
        [Benchmark]
        public void TestParseViaXMLDocument()
        {
            var result = Caribou.Processing.FindNodesViaXMLDocument.FindByFeatures(features, chicagoFile);
        }

        [Benchmark]
        public void TestParseViaLinq()
        {
            var result = Caribou.Processing.FindNodesViaLinq.FindByFeatures(features, chicagoFile);
        }
    }
}
