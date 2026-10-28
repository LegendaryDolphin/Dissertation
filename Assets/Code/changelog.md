**CHANGE LOG:**

**Version 0.1.0 - 21/10/2026**

* Added JSON map structure. Allows network rail infrastructure to be represented as a JSON object, making it easier to work with and manipulate in code.
* Added structs, interfaces and base classes for basic Nodes.
* Added a simple visual component class to represent Nodes in the Unity Editor.

  * Includes text labels.
  * Includes node markers.
* Added a node registry, which loads JSON map data and creates Node instances.

**version 0.2.0 - 22/10/2026**

* Added connections to nodes.
* Added instatiation of track segments.
* Added foundation visuals for track segments.
* Added node and track ids.
* Made changes to the resource registry. Separation of file reading and loading.
* Made changes to track data structure.

  * Direction and allocation variables.
* Made changes to the 'test map' in the MapRegistry JSON.

  * Added all connections.

**version 0.3.0 - 28/10/2026**

* Removed Node and NodeVisual classes. Replaced by bases.
* Added signal visual component base.

  * Added two, three and four aspect visual types.
  * Added operating arrows.
* Added implementation for signal node type.
* Changed node instantiation in resource registry.
* Changed JSON to support signal types and operating direction.

