using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceRegistry: MonoBehaviour
{
    public static ResourceRegistry instance;

    private List<Node> nodeRegister;
    private List<TrackSegment> trackRegister;

    public class MapDatabase
    {
        public MapData[] maps;
    }

    public class MapData
    {
        public string name;
        public MapNode[] nodes;
    }

    public class MapNode
    {
        public string nodeType;
        public string name;
        public int milage;
        public int chain;
        public NodeConnection[] connections;
    }

    public class NodeConnection
    {
        public string name;
        public int speed;
    }

    public void Start() //Singleton
    {
        if(instance != null)
        {
            Destroy(instance);
        }

        instance = this;

        nodeRegister = new List<Node>();
        trackRegister = new List<TrackSegment>();
    }

    private void LoadNodes()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "MapsRegistry.json");

        string json = File.ReadAllText(path);

        MapDatabase database = JsonUtility.FromJson<MapDatabase>(json);

        MapData testMap = null;

        foreach(MapData map in database.maps)
        {
            if(map.name == "TestMap")
            {
                testMap = map;
                break;
            }
        }

        if(testMap == null)
        {
            Debug.LogError("TestMap was not found");
            return;
        }

        foreach (MapNode nodeData in testMap.nodes)
        {
            //Create node instances here.


        }
    }

    public ResourceRegistry GetInstance()
    {
        return instance;
    }
}