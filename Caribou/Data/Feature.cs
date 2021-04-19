﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caribou.Data
{
    public struct Feature
    {
        public Feature(string id, string name, string explanation)
        {
            this.Id = id;
            this.Name = name;
            this.Explanation = explanation;
            this.SubFeatures = new List<Feature>();
        }

        public string Id { get; }
        public string Name { get; }
        public string Explanation { get; }
        public List<Feature> SubFeatures { get; }

        public override string ToString() => $"({this.Name}, {this.Explanation})";
    }
}
