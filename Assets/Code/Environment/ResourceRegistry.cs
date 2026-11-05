using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// The class where all nodes are instantiated from. Add this mono script onto a unity game object.
/// Note: This is a singleton and therefore can have only one instance.
/// </summary>
public class ResourceRegistry: MonoBehaviour
{
    private static ResourceRegistry instance; //Encapsulate
    private Dictionary<int, NodeBase> nodeRegister;
    private Dictionary<string, TrackSegment> trackRegister;
    private MapDatabase mapDatabase;
    private MapData mapData;

    //A set of maps as mapData
    [System.Serializable]
    public class MapDatabase
    {
        public MapData[] maps;
    }

    //A map as a set of nodes and a map name.
    [System.Serializable]
    public class MapData
    {
        public string mapName;
        public MapNode[] nodes;
    }

    //A node as a set of default attributes.
    [System.Serializable]
    public class MapNode
    {
        public string nodeType;
        public int nodeID;
        public string description;
        public int milage;
        public int chain;
        public NodeConnection[] connections;
        public int layoutRowReference;
        public int layoutColumnReference;
        public string signalType;
        public string signalDirection;
        public string junctionDirection;
    }

    //An attribute of a mapNode which describes a connection to another mapNode.
    [System.Serializable]
    public class NodeConnection
    {
        public int nodeID;
        public int speed;
    }

    //Singleton
    public void Start()
    {
        if(instance != null)
        {
            Destroy(instance);
        }

        instance = this;

        nodeRegister = new Dictionary<int, NodeBase>();
        trackRegister = new Dictionary<string, TrackSegment>();

        LoadJSONIntoDatabase();
        LoadNodes();
        LoadTrackSegments();
        IntializeVisuals();
    }

    /// <summary>
    /// Loads all json data into a map database object from a static path.
    /// <exception cref="System.Exception">Thrown if reading from the JSON fails or objects fail to generate properly.</exception>
    /// </summary>
    private void LoadJSONIntoDatabase()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "MapRegistry.json");
        string json = File.ReadAllText(path);
        string mapToLoad = "TestMap"; //Change this for different maps at loading.

        mapDatabase = JsonUtility.FromJson<MapDatabase>(json); //Attempt to make a new mapDatabase object from the JSON text.

        if (mapDatabase == null)
        {
            Debug.LogError("Failed to load maps into a map database object.");
            throw new System.Exception("Failed to load maps into a map database object.");
        }

        if (mapDatabase.maps == null || mapDatabase.maps.Length == 0)
        {
            Debug.LogError("No maps found in the database.");
            throw new System.Exception("No maps found in the map database.");
        }

        foreach (MapData map in mapDatabase.maps) //Search for map with required name.
        {
            if (map.mapName == mapToLoad)
            {
                mapData = map;
                break;
            }
        }

        if (mapData == null)
        {
            Debug.LogError("TestMap was not found.");
            throw new System.Exception("TestMap was not found.");
        }
    }

    /// <summary>
    /// Reads through the mapData object and creates instance of all nodes.
    /// Note: Must be called after JSON load.
    /// <exception cref="System.Exception">Thrown when mapData is null or mapData.nodes is empty.</exception>
    /// </summary>
    private void LoadNodes()
    {
        if(mapData == null)
        {
            Debug.LogError("Map data is null. Therefore nodes cannot be loaded. Have you remembered to call LoadJSON?");
            throw new System.Exception("Map data is null. Therefore nodes cannot be loaded. Have you remembered to call LoadJSON?");
        }

        if(mapData.nodes.Length == 0)
        {
            Debug.LogError("The nodes attribute in map data is empty. Has the JSON been read correctly?");
            throw new System.Exception("The nodes attribute in map data is empty. Has the JSON been read correctly?");
        }

        //Loop through all nodeData in mapData to create instance of each node.
        foreach (MapNode nodeData in mapData.nodes)
        {
            //Create node data and empty node instance variable.
            NodeData data = CreateNodeData(nodeData);
            NodeBase nodeBase;

            //Create a node of specific type.
            switch (nodeData.nodeType)
            {
                case "Default":
                    nodeBase = CreateNode<NodeBase>(nodeData, data);
                    break;

                case "Signal":
                    nodeBase = CreateNode<Signal>(nodeData, data);
                    if(nodeBase is Signal signalInstance)
                    {
                        AddSignalData(nodeData, signalInstance);
                    }
                    break;

                //case "Station":
                //    break;

                case "Junction":
                    nodeBase = CreateNode<Junction>(nodeData, data);
                    if(nodeBase is Junction junctionInstance)
                    {
                        AddJunctionData(nodeData, junctionInstance);
                    }
                    break;

                default:
                    nodeBase = CreateNode<NodeBase>(nodeData, data);
                    Debug.LogWarning($"The node data had a type of: {nodeData.nodeType}, which is not recognised by the system. A default node was created instead.");
                    break;
            }
        }
    }

    /// <summary>
    /// Loads track segments based on map data and the node register.
    /// Note: Must be called after LoadJSON and LoadNodes.
    /// <exception cref="System.Exception">Thrown when map data is null.</exception>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">Thrown when a node instance cannot be found.</exception>
    /// </summary>
    private void LoadTrackSegments()
    {
        if (mapData == null)
        {
            Debug.LogError("Map data is null. Therefore track segments cannot be loaded. Have you remembered to call LoadJSON?");
            throw new System.Exception("Map data is null. Therefore track segments cannot be loaded. Have you remembered to call LoadJSON?");
        }

        foreach (MapNode nodeData in mapData.nodes)
        {
            NodeBase startNode = nodeRegister[nodeData.nodeID];

            foreach(NodeConnection connection in nodeData.connections)
            {
                if(nodeRegister.TryGetValue(connection.nodeID, out NodeBase endNode)) //Node instance gathered here.
                {
                    CreateTrackSegment(startNode, endNode, connection);
                }
                else
                {
                    Debug.LogError($"End node with ID {connection.nodeID} not found in register.");
                    throw new System.Collections.Generic.KeyNotFoundException($"End node with ID {connection.nodeID} not found in register.");
                }
            }
        }
    }

    /// <summary>
    /// Create a new struct of NodeData based on a given MapData object.
    /// </summary>
    /// <param name="data">The MapNode data produced from the JSON.</param>
    /// <returns>New instance of NodeData.</returns>
    private NodeData CreateNodeData(MapNode data)
    {
        //Set default data.
        NodeData nodeData = new NodeData
        {
            id = data.nodeID,
            description = data.description,
            milage = data.milage,
            chain = data.chain,
            type = NodeType.Default
        };

        //Initialize new arrays.
        nodeData.connections = new int[data.connections.Length];
        nodeData.speeds = new int[data.connections.Length];

        //Set data of arrays.
        for (int i = 0; i < data.connections.Length; i++)
        {
            nodeData.connections[i] = data.connections[i].nodeID;
            nodeData.speeds[i] = data.connections[i].speed;
        }

        return nodeData;
    }

    /// <summary>
    /// Creates a new node instance of a given type.
    /// </summary>
    /// <typeparam name="T">The type of node that needs to be generated.</typeparam>
    /// <param name="data">The MapNode data generated by the JSON.</param>
    /// <param name="nodeData">The NodeData instance as a struct.</param>
    /// <returns>New node instance as a base type of NodeBase.</returns>
    private NodeBase CreateNode<T>(MapNode data, NodeData nodeData) where T : NodeBase
    {
        GameObject nodeObject = new GameObject(System.Convert.ToString(data.nodeID));
        NodeBase nodeInstance = nodeObject.AddComponent<T>();

        nodeInstance.InitializeNode(nodeData, data.layoutRowReference, data.layoutColumnReference);

        AddToNodeRegister(nodeInstance, nodeData);

        return nodeInstance;
    }

    /// <summary>
    /// Takes mapData and a node instance and creates and adds new signal data.
    /// Note: The node instance must be a signal type and MapNode must contain signal data.
    /// </summary>
    /// <param name="data">The mapData generated from the JSON.</param>
    /// <param name="signalInstance">The signal as a node instance.</param>
    /// <exception cref="System.Exception">Thrown when the signal type is not of an expected type.</exception>
    private void AddSignalData(MapNode data, Signal signalInstance)
    {
        SignalData signalData = new SignalData
        {
            aspect = SignalAspect.aspectRed,
        };

        switch (data.signalType)
        {
            case "2":
                signalData.signalType = SignalType.aspect2;
                break;
            case "3":
                signalData.signalType = SignalType.aspect3;
                break;
            case "4":
                signalData.signalType = SignalType.aspect4;
                break;
            default:
                Debug.LogError($"The signal type was not recognised. Check your JSON file for node: {data.nodeID}.");
                throw new System.Exception($"The signal type was not recognised. Check your JSON file for node: {data.nodeID}.");
        }

        //Signals are NEVER bidirectional
        if (data.signalDirection == "UP")
        {
            signalData.signalDirection = NodeOperatingDirection.up;
        }
        else if(data.signalDirection == "DOWN")
        {
            signalData.signalDirection = NodeOperatingDirection.down;
        }
        else
        {
            Debug.LogError("The attribute signalDirection did not have UP/DOWN as a value.");
            throw new System.Exception("The attribute signalDirection did not have UP/DOWN as a value.");
        }

        signalInstance.SetSignalData(signalData); //Update the signal data with the new data.
    }

    /// <summary>
    /// Takes mapData and a node instance and creates and adds new junction data.
    /// Note: The node instance must be a junction type and MapNode must contain junction data.
    /// </summary>
    /// <param name="data">The mapData generated from the JSON.</param>
    /// <param name="junctionInstance">The junction as a node instance.</param>
    /// <exception cref="System.Exception">Thrown when the attribute junction direction value in JSON is not UP/DOWN.</exception>
    private void AddJunctionData(MapNode data, Junction junctionInstance)
    {
        JunctionData junctionData = new JunctionData
        {
            state = JunctionState.straight,
        };
                                                                                                                                                        
        if(data.junctionDirection == "DOWN")
        {
            junctionData.junctionDirection = NodeOperatingDirection.down;
        }
        else if(data.junctionDirection == "UP")
        {
            junctionData.junctionDirection = NodeOperatingDirection.up;
        }
        else
        {
            Debug.LogError("The attribute junctionDirection did not have UP/DOWN as a value.");
            throw new System.Exception("The attribute junctionDirection did not have UP/DOWN as a value.");
        }

        junctionInstance.SetJunctionData(junctionData);
    }

    /// <summary>
    /// Tries to add a new node instance to the node register.
    /// </summary>
    /// <param name="nodeToAdd">The instance to try and add.</param>
    /// <param name="data">The node data containing node id as a key.</param>
    /// <exception cref="System.Exception">Thrown when the node id already exists in the register.</exception>
    private void AddToNodeRegister(NodeBase nodeToAdd, NodeData data)
    {
        try
        {
            nodeRegister.Add(data.id, nodeToAdd);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to add the node of id {data.id} to the register as the key already exists. Have you made a typo in the JSON?");
            throw new System.Exception($"Failed to add the node of id {data.id} to the register as the key already exists. Have you made a typo in the JSON?");
        }
    }

    /// <summary>
    /// Creates a track segment based off two node instances and a node connection.
    /// </summary>
    /// <param name="startNode">The start of the connection. Cannot be null!</param>
    /// <param name="endNode">The end of the connection. Cannot be null!</param>
    /// <param name="connection">The connection itself and connection data.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when one of the paramters is null.</exception>
    private void CreateTrackSegment(NodeBase startNode, NodeBase endNode, NodeConnection connection)
    {
        if(startNode == null || endNode == null || connection == null)
        {
            Debug.LogError("One of the given parameters was null when creating a track segment.");
            throw new System.ArgumentNullException("One of the given parameters was null when creating a track segment.");
        }

        string trackSegementID;
        int startNodeID = startNode.GetNodeID();
        int endNodeID = endNode.GetNodeID();

        //Generating a track segement ID based on the two connecting nodes.
        if(startNodeID < endNodeID)
        {
            trackSegementID = $"{startNodeID}-{endNodeID}";
        }
        else
        {
            trackSegementID = $"{endNodeID}-{startNodeID}";
        }

        if(!trackRegister.ContainsKey(trackSegementID))
        {
            //Segment is new.

            TrackData data = new TrackData
            {
                aID = startNodeID,
                bID = endNodeID,
                trackID = trackSegementID,
                allocation = TrackAllocationStatus.free
            };

            if(startNodeID < endNodeID)
            {
                //Connetion is up.
                data.upSpeed = connection.speed;
                data.direction = TrackDirection.up;
            }
            else
            {
                //Connection is down.
                data.downSpeed = connection.speed;
                data.direction = TrackDirection.down;
            }

            GameObject trackObject = new GameObject(trackSegementID);
            TrackSegment trackInstance = trackObject.AddComponent<TrackSegment>();

            trackInstance.InitializeTrackSegment(data);
            AddTrackToRegister(trackInstance, data);
        }
        else
        {
            //Edit the track segment if it already exists to make it bi-directional.
            TrackSegment existingSegment = trackRegister[trackSegementID];
            TrackData data = existingSegment.GetTrackData();

            if(data.direction == TrackDirection.up)
            {
                data.downSpeed = connection.speed;
            }
            else if(data.direction == TrackDirection.down)
            {
                data.upSpeed = connection.speed;
            }

            data.direction = TrackDirection.bidirectional;

            existingSegment.UpdateTrackData(data);
        }
    }

    /// <summary>
    /// Loops through both the track register and node register and calls initialization of visuals on all instances.
    /// Note: Should only be called once all instances have loaded correctly.
    /// </summary>
    private void IntializeVisuals()
    {
        foreach(var node in nodeRegister.Values)
        {
            node.InitializeVisuals();
        }

        foreach(var track in trackRegister.Values)
        {
            track.InitializeVisuals();
        }
    }

    /// <summary>
    /// Tries to add a track instance to the track register.
    /// </summary>
    /// <param name="segment">The track instance to add.</param>
    /// <param name="data">The data of that track instance.</param>
    /// <exception cref="System.Exception">Thrown when the track id already exists as a key in the register.</exception>
    private void AddTrackToRegister(TrackSegment segment, TrackData data)
    {
        try
        {
            trackRegister.Add(data.trackID, segment);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"The key for track segment {data.trackID} already exists. Is there an error in your JSON?");
            throw new System.Exception($"The key for track segment {data.trackID} already exists. Is there an error in your JSON?");
        }
    }

    /// <summary>
    /// Gets a node off a given id as an integer.
    /// </summary>
    /// <param name="nodeID">The node id of the node to be retrieved.</param>
    /// <returns>The node as an instance. Can return null!</returns>
    public NodeBase GetNodeByID(int nodeID)
    {
        if (nodeRegister.TryGetValue(nodeID, out NodeBase node))
        {
            return node;
        }
        else
        {
            Debug.LogError($"Node with ID {nodeID} not found in register.");
            return null;
        }
    }

    /// <summary>
    /// A singleton method which retrieves the instance of the ResourceRegistry.
    /// </summary>
    /// <returns>The resource registries instance.</returns>
    public static ResourceRegistry GetInstance()
    {
        return instance;
    }
}