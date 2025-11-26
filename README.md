# SLC-S-SDM-GQIDS-Registration

## Overview

This solution provides custom Ad Hoc Data Sources for Skyline DataMiner, enabling advanced querying and live updates for Solution and Model registrations via the Generic Query Interface (GQI). It is designed for integration with DataMiner SRM environments and supports sorting, paging, and real-time data synchronization.

## Components

- **Solution Data Source (`GetSolutions.cs`)**  
  Exposes solution registration data, including GUID, ID, display name, API endpoints, and version. Supports sorting and live updates.

- **Model Data Source (`GetModels.cs`)**  
  Exposes model registration data, including GUID, name, display name, API script name, API endpoint, visualization endpoints, associated solution, and version. Supports sorting and live updates.

## Usage

1. Add the compiled Ad Hoc Data Source to your DataMiner system.
2. Configure GQI queries in your DataMiner dashboards or automation scripts to retrieve solution and model registration data.
3. Use sorting and paging features for efficient data handling.
4. Live updates are handled automatically when registration data changes.
