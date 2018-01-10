using UnityEngine;
using System.Collections;

public class Tree1 : ITree {


    public static int PREFABID = 0;
    public static int [] TEXTUREEFFECTS = { 0, 5, -10, 0, 0 };
    public int yearChange = 0; //jakou změnu zdraví mu dává statické prostředí za rok
    public int health = 100; // současné zdraví
    public int nexthealth = 100;
    public int age = 1; // současný věk
    public static float NEWTREELENGHT =25.1f;// 32.7f;  //jak daleko muže být nový strom
    public static int CHILDRENNUMBER = 9; //kolik potomku v jednom roce
    public static float MAXNEGATIVELENGHT =18.1f;// 16.2f; //jak daleko má strom svuj max dosah-oslabování
    public static int MAXAGE = 100; // maximální věk před umíráním
    public static int PARRENTAGE = 17; // minimální věk pro to mít potomky
    public static int MAXALTITUDE = 100;
    public static int MINALTITUDE =60;
    public static int MAXGRADIENT = 20;
  
	
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   
    public override int GetHashCode()
    {
        return GetPosition().GetHashCode();
    }
     
    public override bool Equals(Object obj) {
        if (obj is ITree) {
            ITree temp = (ITree) obj;
            return GetPosition().Equals(temp.GetPosition());
            
        }
        return true;
    }


    public override float GetNegativeLenght()
    {
        if (age == 0) {
            return 0;
        }
        double ageMath = ((float) age / (float) MAXAGE);

        if (ageMath < 0.25) {
            return MAXNEGATIVELENGHT * ((float) health / 100.0f) * 0.2f;
        } else if (ageMath < 0.55) {
            return MAXNEGATIVELENGHT * ((float) health / 100.0f) * 0.5f;
        } else if (ageMath < 0.75) {
            return MAXNEGATIVELENGHT * ((float) health / 100.0f) * 0.7f;
        }
        return MAXNEGATIVELENGHT * ((float) health / 100.0f);


        // stimhle si ještě pohrát
        // negeneruje to uplně pekně
    }


    public override float GetNewTreeLenght()
    {
        return NEWTREELENGHT;
    } // i tohle by mělo být ovlivněné stářím


    public override int GetChildrenNumber()
    {
        if (age > MAXAGE) {
            return CHILDRENNUMBER;
        }
        if (age == 0) {
            return 0;
        }
        return CHILDRENNUMBER / (MAXAGE / age);
    }


    public override Vector2 GetPosition()
    {
        return new Vector2(this.transform.position.x, this.transform.position.z); 
    }


    public override int GetHealth()
    {
        return health;
    }


    public override void AddHealth(short newHealth)
    {
        if (GetAge() > 0.2 * MAXAGE && newHealth < 0) {
            this.nexthealth += (short)(newHealth * 2);
        } else {
            this.nexthealth += newHealth;
        }
        if (nexthealth < 0) {
            nexthealth = (short) Mathf.Max(nexthealth, -10);
        } else if (nexthealth > 100) {
            nexthealth = 100;
        }
    }


    public override void SetAge(short age)
    {
        this.age = age;
        //this.transform.localScale.x = 0.2f + Mathf.Min(1, (float)age / (float)MAXAGE);
        float scale =0.3f + Mathf.Min(0.7f, (float)age / (float)MAXAGE);
        this.transform.localScale = new Vector3(scale, scale, scale);
        ApplyYearChange();
        if (GetAge() > MAXAGE) {
            AddHealth((short) -15);
        }
    }


    public override int GetAge()
    {
        return age;
    }


    public override void ApplyYearChange()
    {
        this.health = nexthealth;
    }


    public override bool CanChildren()
    {
        return (GetHealth() > 75 && GetAge() >= PARRENTAGE);
    }

    
    /// <summary>
    /// dájí se kompletní souřadnice - tj x,y,z
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public override ITree CreateChildren(Vector3 position)
    {
        
        Collider[] colliders = Physics.OverlapSphere(position, Forest.treeSize);
        foreach (Collider collid in colliders)
        {
            if (collid.gameObject.GetComponent<ITree>() == null)
            {
                if (collid.gameObject.GetComponent<Map>() == null)
                {
                    Debug.Log(collid.gameObject.tag + " blbej objekt");
                    return null;
                }
            }
            
        }
        Quaternion rot = new Quaternion();
        rot.y = Random.Range(0, 360);
        ITree newTree = (ITree)Object.Instantiate(this, position, rot);
        newTree.AddHealth(100);
        newTree.SetAge(1);
        newTree.SetYearChange(0);
        return newTree;// vector 3-doladit, quaternion random roatce
            //new Tree1(position);
    }


    public override int Weaktree(ITree tree)
    {
        if (Mathf.Abs(GetPosition().x - tree.GetPosition().x) < tree.GetNegativeLenght() && Mathf.Abs(GetPosition().y - tree.GetPosition().y) < tree.GetNegativeLenght()) {
            double distance = Vector2.Distance(GetPosition(), tree.GetPosition());
            if (distance <= tree.GetNegativeLenght()) {
                return (int) ((1.0 - (distance / tree.GetNegativeLenght())) * (double) tree.GetHealth()); // zkusit jiný?
            }
            //popřemýšlet nad tím jestli vzdálenost není ovlivněna zdravím
        }
        return 0;


    }


    public override int GetMaxAltitude()
    {
        return MAXALTITUDE;
    }


    public override int GetMinAltitude()
    {
        return MINALTITUDE;
    }


    public override void SetYearChange(short yearChange)
    {
        this.yearChange = (short) yearChange;
    }


    public override int GetYearChange()
    {
        return yearChange;
    }


    public override int GetGradient()
    {
        return MAXGRADIENT;
    }


    public override int GetMaxAge()
    {
        return MAXAGE;
    }


    public override int GetParrentAge()
    {
        return PARRENTAGE;
    }


    public override float GetMaxNegativeLenght()
    {
        return MAXNEGATIVELENGHT;
    }


    public override int GetMaxChildrenNumber()
    {
        return CHILDRENNUMBER;
    }



    public override int GetPrefabID()
    {
        return PREFABID;
    }

    public override void SetPrefabID(int ID)
    {
        PREFABID = ID;
    }

    public override int [] GetTextureEffect()
    {
        return TEXTUREEFFECTS;
    }
}
