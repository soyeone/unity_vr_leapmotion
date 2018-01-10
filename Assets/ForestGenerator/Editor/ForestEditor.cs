
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using System.IO;
public class ForestEditor : EditorWindow
{

    public Map mapPrefab = null;
    public Texture2D texture = null;
    public ITree [] prefabs = new ITree [10] { null, null, null, null, null, null, null, null, null, null};
    public int treePrefabsCount = 1;
    public Collider m_colider;
    private Forest f;
    private int years;
    
    string myString = "Forest generator";
    

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/ForestEditor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ForestEditor window = (ForestEditor)EditorWindow.GetWindow(typeof(ForestEditor));
        Random.seed = (int)Time.time;

    }
   

    void OnGUI()
    {
        GUILayout.Label(myString, EditorStyles.boldLabel);
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        mapPrefab = (Map)EditorGUILayout.ObjectField("Map", mapPrefab, typeof(Map),true);
        treePrefabsCount = EditorGUILayout.IntSlider("Tree prefab count", treePrefabsCount, 1, 10);
        for (int i = 0; i < treePrefabsCount; i++)
        {
          prefabs [i] = (ITree)EditorGUILayout.ObjectField("Tree:" + (1+i).ToString(), prefabs [i], typeof(ITree),true);
        }
        years = EditorGUILayout.IntSlider("Years", years, 1, 90);
        texture = (Texture2D)EditorGUILayout.ObjectField("Position texture", texture, typeof(Texture2D), false);
        if (GUILayout.Button("Next: " + years.ToString() + " years"))
        {
            NextYear();
        }
      /*  if (GUILayout.Button("Trees from Terrain: "))
        {
            Map map = FindObjectOfType(typeof(Map)) as Map;
            if (map == null)
            {
                Debug.Log("mapa null");
            }
            TerrainToInstances();
        }*/
        if (GUILayout.Button("Tree to Terrain: "))
        {
            Map map = FindObjectOfType(typeof(Map)) as Map;
            if (map == null)
            {
                Debug.Log("mapa null 2");
            }
            InstancesToTerrain();
        }
         
        if (GUILayout.Button("Delete all trees "))
        {
            DeleteAllTrees();
        }

        if (GUILayout.Button("Save Forest "))
        {
            SaveForestXML(LoadForest());
        }
        
       
    }

   /// <summary>
   /// trees from terrain treeinstnce to gameobjects
   /// </summary>
    public void TerrainToInstances()
    {
        Map map = FindObjectOfType(typeof(Map)) as Map;
        if (map != null)
        {
            //přepsat na tempy
            List<ITree> trees = new List<ITree>();
            List<TreeInstance> otherInstances = new List<TreeInstance>();
            TreeInstance [] instances = map.terrain.terrainData.treeInstances;
            //List<TreeInstance> instances = new List<TreeInstance>(map.terrain.terrainData.treeInstances);
            if (instances.LongLength == 0 )
            {
                Debug.Log("no trees in terrain");
                return;
            }
            System.IO.File.Delete("Debug.txt");
            System.IO.File.AppendAllText("Debug.txt", "Debug log treeinstances -> gameobject:" + System.DateTime.Now.ToShortTimeString());
            long percent = (long)Mathf.Max(1,instances.GetLongLength(0) / 100);
            long counter = percent;
            int currentPercent =0;
                
            for (long i = instances.GetLongLength(0) - 1; i >= 0; i--)
            {
                bool finded = false;
                for (int prefabFinder = 0; i < treePrefabsCount; i++)
                {
                    
                    if (instances[i].prototypeIndex == prefabs[prefabFinder].GetInstanceID())
                    {
                        ITree temp = ((GameObject)PrefabUtility.InstantiatePrefab(prefabs[0].gameObject)).GetComponent<ITree>();
                        SetIntanceToTerrain(temp, map, instances, trees, i);
                        finded = true;
                        break;
                    }
                }
                if (!finded)
                {
                    otherInstances.Add(instances[i]);
                }

               
                counter--;
                if (counter == 0)
                {
                    System.IO.File.AppendAllText("Debug.txt", currentPercent.ToString() + "% " + System.DateTime.Now.ToShortTimeString());
                    currentPercent++;
                }
            }
            if (texture == null)
             {
                f = new Forest(trees, trees, map);
             }
             else
             {
                 f = new Forest(trees, trees, map,texture);
             }
            f.TestTreesOnPositionTexture();
            map.terrain.terrainData.RefreshPrototypes();
            map.terrain.terrainData.treeInstances = new TreeInstance[10];
        }
        else
        {
            Debug.Log("set map/terrain");
        }
    }

    /// <summary>
    /// game objects to terrain treeinstance
    /// currently unused
    /// </summary>
    public void InstancesToTerrain()
    {
        Map map = FindObjectOfType(typeof(Map)) as Map;
        if (map != null)
        {   
            Object[] objTrees =  FindObjectsOfType(typeof (ITree));
            if (objTrees.GetLongLength(0) == 0)
            {
                Debug.Log("no trees instances, all in terrain?");
            }
            System.IO.File.Delete("Debug.txt");
            System.IO.File.AppendAllText("Debug.txt", "Debug log gameobject -> treeinstances:" + System.DateTime.Now.ToShortTimeString());
            long percent = (long)Mathf.Max(1,objTrees.GetLongLength(0) / 100);
            long counter = percent;
            int currentPercent = 0;
            for (long i = objTrees.GetLongLength(0)-1; i >=0; i--)
            {
                 ITree tree =(ITree)objTrees[i];
                TreeInstance tI = new TreeInstance();
                float healtScale = ((float)tree.GetHealth()) / 1000.0f;
                tI.color = new Color(1, 1, 1);
                tI.lightmapColor = new Color(255, 255, 255);
                tI.position = new Vector3(tree.transform.position.x / map.terrain.terrainData.size.x, tree.transform.position.y / map.terrain.terrainData.size.y, tree.transform.position.z / map.terrain.terrainData.size.z);
                tI.prototypeIndex = tree.GetPrefabID();
                float scale = 0.3f + Mathf.Min(0.7f, (float)tree.GetAge() / (float)Tree1.MAXAGE);
                tI.heightScale = scale - healtScale;
                tI.widthScale = scale;
                map.terrain.AddTreeInstance(tI);
                GameObject.DestroyImmediate(((ITree)objTrees[i]).gameObject);
                counter--;
                if (counter == 0)
                {
                    System.IO.File.AppendAllText("Debug.txt", currentPercent.ToString() + "% " + System.DateTime.Now.ToShortTimeString());
                    currentPercent++;
                }
            }
            System.GC.Collect();
        }
        else
        {
            Debug.Log("set map/terrain");
        }
    }

    /// <summary>
    /// set one treeinstance to gameobject - all data necessary for ITree
    /// currently unused
    /// </summary>
    /// <param name="tree"></param>
    /// <param name="map"></param>
    /// <param name="instances"></param>
    /// <param name="trees"></param>
    /// <param name="instancePosiiton"></param>
    public void SetIntanceToTerrain(ITree tree, Map map, TreeInstance[] instances,List<ITree>trees, long instancePosiiton)
    {
        Vector3 v3 = new Vector3(instances [instancePosiiton].position.x * (float)map.GetHeight(), 10, instances [instancePosiiton].position.z * (float)map.GetWidth());
        tree.transform.position = v3;
        float health = (instances [instancePosiiton].widthScale - instances [instancePosiiton].heightScale) * 1000.0f;
        if (health < 1)
        {
            health = Random.Range(30, 80);
        }
        tree.AddHealth((short)health);
        tree.transform.parent = map.transform;
        if (instances [instancePosiiton].heightScale == 1)
        {
            tree.SetAge((short)Random.Range(1, 100));
        }
        else
        {
            tree.SetAge((short)((instances [instancePosiiton].widthScale - 0.3f) * (float)Tree1.MAXAGE));
        }
        tree.SetYearChange(0);
        if (tree == null)
        {
            Debug.Log("error");
        }
        if (f == null)
        {
            Debug.Log("bug problem");
        }
        trees.Add(tree);
    }

    /// <summary>
    /// loading already contained forest (gameobjects-trees)
    /// </summary>
    /// <returns></returns>
    public Forest LoadForest() {
        Object [] objects = Resources.FindObjectsOfTypeAll(typeof(ITree));
        
        List<ITree> trees = new List<ITree>();
        Debug.Log(objects.GetLength(0) + "finded trees");
        for (int i = 0; i < objects.Length; i++)
        {
            if (PrefabUtility.GetPrefabParent((ITree)objects[i]) == null && PrefabUtility.GetPrefabObject((ITree)objects[i]) != null)
            {
            }// Is a prefab
            else
            {
                trees.Add((ITree)objects[i]);
            }
        }
        Map map = FindObjectOfType(typeof(Map)) as Map;
       // pokud se najde malo stromu je třeba generovat znova
        if (texture != null)
        {
            f = new Forest(trees, null, map, texture);
        }
        else
        {
            f = new Forest(trees, null, map);
        }
        return f;
    }

    /// <summary>
    /// button for counting next years check all conditions
    /// </summary>
    public void NextYear()
    {
        
        if (prefabs[0] == null)
        {
            Debug.Log("load tree data");
            f = null;
        }
        else
        {
            Map map = FindObjectOfType(typeof(Map)) as Map;
            if (map == null)
            {
                # region NeniMapaAniStromyVytvoritVse
                if (mapPrefab == null)
                {
                    Debug.Log("cannot find map");
                    return;
                }
                f = null;
                Debug.Log("Loading map or trees");
                List<ITree> species = new List<ITree>();
                for (int i = 0; i < treePrefabsCount; i++)
                {
                    if (prefabs[i] != null)
                    {
                        ITree tree = ((GameObject)Instantiate(prefabs[i].gameObject)).GetComponent<ITree>();
                        tree.transform.position = new Vector3(-20, -200, -20);
                        species.Add(tree);

                    }
                    else
                    {
                        break;
                    }
                }

                Map tempMap = ((GameObject)Instantiate(mapPrefab.gameObject)).GetComponent<Map>();
                tempMap.transform.position = new Vector3(0, 0, 0);
                // max size which tree can growth

               
                Forest.treeSize = (prefabs[0].gameObject).GetComponent<Renderer>().bounds.size.y;
                if (texture == null)
                {
                    f = new ForestManager().createForest(tempMap, species);
                }else
                {
                    f = new ForestManager().createForest(tempMap, species, texture);
                }
                f.DeleteDead(species);
                # endregion
            }
            else
            {
                # region MapaJeStromyNagenerovat
                //check if there is enought trees on map (at least prefabcount+1 trees in scene or loading new
                if (f == null)// || Resources.FindObjectsOfTypeAll(typeof(ITree)).GetLength(0)+1 <=treePrefabsCount)
                {
                    f = LoadForest();
                }
                if (f.allTrees.Count <= treePrefabsCount + 1)
                {
                    List<ITree> species = new List<ITree>();
                    for (int i = 0; i < treePrefabsCount; i++)
                    {
                        if (prefabs[i] != null)
                        {
                            ITree tree = ((GameObject)Instantiate(prefabs[i].gameObject)).GetComponent<ITree>();
                            tree.transform.position = new Vector3(-20, -200, -20);
                            species.Add(tree);

                        }
                        else
                        {
                            break;
                        }
                    }
                    

                    Forest.treeSize = (prefabs[0].gameObject).GetComponent<Renderer>().bounds.size.y;

                    if (texture == null)
                    {
                        f = new ForestManager().createForest(map, species);
                    }else
                    {
                        f = new ForestManager().createForest(map, species, texture);
                    }
                    f.DeleteDead(species);

                }
                else
                {
                    Debug.Log("enought trees");
                }
                #endregion

            }
            Debug.Log("start generating");
            f.positionTexture = texture;
            System.IO.File.Delete("Debug.txt");
            System.IO.File.AppendAllText("Debug.txt", "Debug log forest generating:" + System.DateTime.Now.ToShortTimeString());
            for (int i = 0; i < years; i++)
            {
                f.NextYear(10);
                System.GC.Collect();
                System.IO.File.AppendAllText("Debug.txt", "\n" + System.DateTime.Now.ToShortTimeString() + " next year" + i.ToString());
            }
            Debug.Log(f.allTrees.Count.ToString() + " trees in generated forest");
        }
    }

    public void DeleteAllTrees()
    {
        if (f != null)
        {
            List<ITree> treesToDelete = new List<ITree>(f.allTrees);
            foreach (ITree tree in treesToDelete)
            {
                f.Deletetree(tree);
            }
        }
        Object[] objects = Resources.FindObjectsOfTypeAll(typeof(ITree));
        /*foreach (Object o in objects)
        {
            DestroyImmediate(((ITree)o).gameObject);
        }*/
        Map foundedMap = FindObjectOfType(typeof(Map)) as Map;
        if (foundedMap != null)
        {
            foundedMap.terrain.terrainData.treeInstances = new TreeInstance[0];
            ITree[] allChildren = foundedMap.GetComponentsInChildren<ITree>();
            foreach (ITree child in allChildren)
             {
                 DestroyImmediate(((ITree)child).gameObject);
             }
        }

    }

    public void SaveForestXML(Forest forest)
    {
        string date = System.DateTime.Today.Day.ToString();
        string filePath = "forest_"+date;

        if (File.Exists(filePath + ".xml"))
        {
            filePath = filePath + "_" + System.DateTime.Now.Hour.ToString();
        }
        using (XmlWriter writer = XmlWriter.Create(filePath+".xml"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Forest");
            writer.WriteElementString("Size", forest.allTrees.Count.ToString());
            writer.WriteStartElement("Map");
           
            writer.WriteElementString("Position", forest.forestMap.terrain.GetPosition().ToString());
            writer.WriteElementString("MapWidth", forest.forestMap.GetWidth().ToString());   // <-- These are new
            writer.WriteElementString("Mapheight", forest.forestMap.GetHeight().ToString());
          
            writer.WriteEndElement();
            writer.WriteStartElement("Trees");
            foreach (ITree tree in forest.allTrees)
            {
                writer.WriteStartElement("Tree");

                writer.WriteElementString("ID", tree.GetPrefabID().ToString());
                writer.WriteElementString("Position", tree.transform.position.ToString());
                writer.WriteElementString("YearChange", tree.GetYearChange().ToString()); 
                writer.WriteElementString("Health", tree.GetHealth().ToString());
                writer.WriteElementString("Age", tree.GetAge().ToString());

                writer.WriteElementString("TEXTUREEFFECTS", string.Join(",", System.Array.ConvertAll(tree.GetTextureEffect(), x => x.ToString())));
                writer.WriteElementString("NEWTREELENGHT", tree.GetNewTreeLenght().ToString());
                writer.WriteElementString("CHILDRENNUMBER", tree.GetChildrenNumber().ToString());
                writer.WriteElementString("MAXNEGATIVELENGHT", tree.GetMaxNegativeLenght().ToString());
                writer.WriteElementString("MAXAGE", tree.GetMaxAge().ToString());
                writer.WriteElementString("PARRENTAGE", tree.GetParrentAge().ToString());
                writer.WriteElementString("MAXALTITUDE", tree.GetMaxAltitude().ToString());
                writer.WriteElementString("MINALTITUDE", tree.GetMinAltitude().ToString());
                writer.WriteElementString("MAXGRADIENT", tree.GetGradient().ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }

   }
