using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceRegistry: MonoBehaviour
{
    public static ResourceRegistry instance;

    private Dictionary<int, NodeBase> nodeRegister;
    private Dictionary<string, TrackSegment> trackRegister;

    private MapDatabase mapDatabase;
    private MapData mapData;

    [System.Serializable]
    public class MapDatabase
    {
        public MapData[] maps;
    }

    [System.Serializable]
    public class MapData
    {
        public string mapName;
        public MapNode[] nodes;
    }

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
    }

    [System.Serializable]
    public class NodeConnection
    {
        public int nodeID;
        public int speed;
    }

    public void Start() //Singleton
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

    private void LoadJSONIntoDatabase()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "MapRegistry.json");

        string json = File.ReadAllText(path);

        Debug.Log($"JSON contains signalType: {json.Contains("\"signalType\"")}");
        Debug.Log($"Raw JSON:\n{json}");

        mapDatabase = JsonUtility.FromJson<MapDatabase>(json);

        if (mapDatabase == null)
        {
            Debug.LogError("Failed to load map database");
            return;
        }

        if (mapDatabase.maps == null || mapDatabase.maps.Length == 0)
        {
            Debug.LogError("No maps found in the database");
            return;
        }

        foreach (MapData map in mapDatabase.maps)
        {
            if (map.mapName == "TestMap")
            {
                mapData = map;
                break;
            }
        }

        if (mapData == null)
        {
            Debug.LogError("TestMap was not found");
            return;
        }
    }

    private void LoadNodes()
    {
        if(mapData == null)
        {
            Debug.LogError("Map data is null. Cannot load nodes.");
            return;
        }

        foreach (MapNode nodeData in mapData.nodes)
        {
            //Create node instances here.
            NodeData data = CreateNodeData(nodeData);
            NodeBase nodeBase;

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

                //case "Junction":
                //    break;

                default:
                    nodeBase = CreateNode<NodeBase>(nodeData, data);
                    break;
            }
        }
    }

    private void LoadTrackSegments()
    {
        if (mapData == null)
        {
            Debug.LogError("Map data is null. Cannot load nodes.");
            return;
        }

        foreach (MapNode nodeData in mapData.nodes)
        {
            NodeBase startNode = nodeRegister[nodeData.nodeID];

            foreach(NodeConnection connection in nodeData.connections)
            {
                if(nodeRegister.TryGetValue(connection.nodeID, out NodeBase endNode))
                {
                    CreateTrackSegment(startNode, endNode, connection);
                }
                else
                {
                    Debug.LogError($"End node with ID {connection.nodeID} not found in register.");
                }
            }
        }
    }

    private NodeData CreateNodeData(MapNode data)
    {
        NodeData nodeData = new NodeData
        {
            id = data.nodeID,
            description = data.description,
            milage = data.milage,
            chain = data.chain,
            type = NodeType.Default
        };

        nodeData.connections = new int[data.connections.Length];
        nodeData.speeds = new int[data.connections.Length];

        for (int i = 0; i < data.connections.Length; i++)
        {
            nodeData.connections[i] = data.connections[i].nodeID;
            nodeData.speeds[i] = data.connections[i].speed;
        }

        return nodeData;
    }

    private NodeBase CreateNode<T>(MapNode data, NodeData nodeData) where T : NodeBase
    {
        GameObject nodeObject = new GameObject(System.Convert.ToString(data.nodeID));
        NodeBase nodeInstance = nodeObject.AddComponent<T>();

        nodeInstance.InitializeNode(nodeData, data.layoutRowReference, data.layoutColumnReference);

        AddToNodeRegister(nodeInstance, nodeData);

        return nodeInstance;
    }

    private void AddSignalData(MapNode data, Signal signalInstance)
    {
        SignalData signalData = new SignalData
        {
            aspect = SignalAspect.aspectRed,
        };

        Debug.Log($"{data.signalType}");

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
        }

        Debug.Log(
            $"Signal {data.nodeID}: JSON type = {data.signalType}, " +
            $"assigned type = {signalData.signalType}"
        );

        //Signals are NEVER bidirectional
        if (data.signalDirection == "UP")
        {
            signalData.signalDirection = NodeOperatingDirection.up;
        }
        else
        {
            signalData.signalDirection = NodeOperatingDirection.down;
        }

        signalInstance.SetSignalData(signalData);
    }

    private void AddToNodeRegister(NodeBase nodeToAdd, NodeData data)
    {
        try
        {
            nodeRegister.Add(data.id, nodeToAdd);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to add node {data.id} to register: {e.Message}");
        }
    }

    private void CreateTrackSegment(NodeBase startNode, NodeBase endNode, NodeConnection connection)
    {
        string trackSegementID;
        int startNodeID = startNode.GetNodeID();
        int endNodeID = endNode.GetNodeID();

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

    private void AddTrackToRegister(TrackSegment segment, TrackData data)
    {
        try
        {
            trackRegister.Add(data.trackID, segment);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to add track segment {data.trackID} to register: {e.Message}");
        }
    }

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

    public static ResourceRegistry GetInstance()
    {
        return instance;
    }
}