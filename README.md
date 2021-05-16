# Caribou

[![Build Action](https://github.com/philipbelesky/Caribou/workflows/Build%20Grasshopper%20Plugin/badge.svg)](https://github.com/philipbelesky/Caribou/actions/workflows/dotnet-grasshopper.yml)
[![Test Action](https://github.com/philipbelesky/Caribou/workflows/Test%20Grasshopper%20Plugin/badge.svg)](https://github.com/philipbelesky/Caribou/actions/workflows/dotnet-tests.yml)
[![Maintainability](https://api.codeclimate.com/v1/badges/20e0e2fd92a1951ccb20/maintainability)](https://codeclimate.com/github/philipbelesky/Caribou/maintainability)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/6a5919298be744a2bc1018bd9e0ec1c2)](https://www.codacy.com/manual/philipbelesky/Caribou?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=philipbelesky/GrasshopperBootstrap&amp;utm_campaign=Badge_Grade)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Caribou is a [Grasshopper plugin](https://www.grasshopper3d.com/) for parsing downloaded Open Street Map data into Rhino geometry.

Caribou is currently in an alpha state. Feedback, issues, and pull-requests are encouraged.

## Features

- ✅ Very fast parsing of even very large files
- ✅ Parsing is performed asynchronously so Grasshopper does not freeze
- ✅ Components embrace a modular approach to filtering and extracting data
- ✅ Parse multiple OSM files simultaneously with de-duplication
- ✅ Allows for querying for arbitrary data, not just defined features/sub-features
- ✅ Outputs are tree-formatted and organised per data-type to allow for downstream filtering and baking

## Roadmap

- 🕘 Faster!
- 🕘 Intuitive GUI for defining feature/subfeature filters
- 🕘 Dedicated component for defining 3D buildings
- 🕘 Parsing of `<relation>` type data
- 🕘 Integration with Rhino's `EarthAnchorPoint`

## Setup and Use

- Plugin installation
  1. For now, releases are available in the [Rhino package manager](https://www.rhino3d.com/features/package-manager/) only.
- Data gathering
  1. Go to [https://www.openstreetmap.org](openstreetmap.org)
  2. Locate the general area you wish to model and hit `export`, then `manually select an area`
  3. Click the `OVERPASS API` link to download the `xml` file
- Grasshopper Setup
  1. Place an `Extract Nodes` or `Extract Ways` component (or both)
  2. Use a standard `Read File` component for your `xml` file and connect it as `OSM Content`
  3. Using a panel, list the data you wish to extract in a comma-separated `key` or `key=value` format. These can include official [features and sub-features](https://wiki.openstreetmap.org/wiki/Map_Features) types or any form of meta-data.  E.g.:
  ```
    building,
    highway=traffic_signals,
    cuisine=mexican,
    addr:street=Swanston Street
  ```

![Image of the definition setup](/assets/setup-v0.5.png)

## Recognition

Thanks to:

- Dimitrie Stefanescu and the authors of the [GrasshopeprAsyncComponent](https://github.com/specklesystems/GrasshopperAsyncComponent) repo.
- Timothy Logan, the author of [Elk](https://github.com/logant/Elk).

## License

See `LICENSE.md` in this folder/
