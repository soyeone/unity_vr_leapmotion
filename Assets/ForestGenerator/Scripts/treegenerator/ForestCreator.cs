using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestCreator : MonoBehaviour {

    Forest f;
    public Map map;
    private float time;

    public ITree tree;    
    // Use this for initialization
	void Start () {
        time = Time.time;
        ForestManager fM = new ForestManager();
       List<ITree> species= new List<ITree>();
        species.Add(tree);
        f = fM.createForest(map, species);
        for (int i = 0; i < 30; i++)
        {
            f.NextYear(10);
            Debug.Log(i.ToString());
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (time + 5 < Time.time)
            {
                for (int i = 0; i < 5; i++)
                {
                    f.NextYear(10);
                    Debug.Log("next year");
                }
               
                time = Time.time;
               
            }
        }
	}
}
