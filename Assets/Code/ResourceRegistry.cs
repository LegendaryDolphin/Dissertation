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

            switch (nodeData.nodeType)
            {
                case "Default":
                    CreateDefaultNode(nodeData);
                    break;

                //case "Signal":
                //    break;

                //case "Station":
                //    break;

                //case "Junction":
                //    break;

                default:
                    CreateDefaultNode(nodeData);
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

    private void CreateDefaultNode(MapNode data)
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

        GameObject nodeObject = new GameObject(System.Convert.ToString(data.nodeID));
        Node nodeInstance = nodeObject.AddComponent<Node>();

        nodeInstance.InitializeNode(nodeData, data.layoutRowReference, data.layoutColumnReference);

        AddToNodeRegister(nodeInstance, nodeData);
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