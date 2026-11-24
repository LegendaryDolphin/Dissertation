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
* Removed text from nodes.

**version 0.4.0 - 29/10/2026**

* Added junction implementation.
* Added junction visual.
* Changed JSON to support junction operating directions.
* Minor bug fixes.

**version 0.4.1 - 05/11/2026**

* Added documentation to current environment classes.
* Added throws to error prone methods along with updated Unity debug messages for environment classes.

**version 0.4.2 - 06/11/2026**

* Added documentation to current visual classes.
* Added throws to error prone methids along with updated Unity debug messages for visual classes.

**version 0.5.0 - 07/11/2026**

* Added station node type to environment.
* Added station node visual componenet.
* Added platform placement and station data to JSON.
* Added documentation to station environment and visual classes.
* Added throws to error prone methods along with updated Unity debug messages for station environment, visual classes and resource registry.

**version 0.6.0 - 21/11/2026**

* Added Unity Input System for camera controls.
* Added camera controls for movement and zooming.
* Added documentation to camera class.

**version 0.7.0 - 22/11/2026**

* Added Simulation Clock.
* Added UI for Simulation Clock control.
	* Pause / Resume.
	* Time multipler controls for up to x20.
	* Simulation clock text field.

**version 0.8.0 - 23/11/2026**

* Added Timetable.json data structure.
* Added reading of timetable json to C# structs.
* Added RoutePattern data structures (structs).
* Added Route Resource Manager.
* Added documnetation to Simulation Clock UI classes.
* Added documnetation to RoutePattern structs and manager class.
* Minor bug fix in Resource Registry class.
* Removed unncessary package in simulation clock class.

**version 0.8.1 - 24/11/2026**

* Added scene to Git.