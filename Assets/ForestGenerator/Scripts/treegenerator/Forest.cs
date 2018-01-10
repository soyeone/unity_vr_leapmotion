using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;


public class Forest {

	public HashSet<ITree> allTrees = new HashSet<ITree>();
    public static float treeSize = 1;
    private HashSet<ITree>[,] trees;
    public Texture2D positionTexture;
    private TreeManager treeManager = new TreeManager();
    private int yearChange = 10;
    public Map forestMap;
    private List<ITree> treesSpecies;
    //private Collection<Thread> threads = new ArrayList<>();
    public  int boxSize =5;
    public int maxDistance = 20;
    public static int counter = 1;
  
   


    public Forest(List<ITree> startTrees, List<ITree> treesSpecies,Map map ) {
        this.treesSpecies = treesSpecies;
        this.forestMap =map;
        allTrees.Clear();
        this.allTrees = new HashSet<ITree>();
        trees = new HashSet<ITree>[forestMap.GetWidth()/boxSize+2,forestMap.GetHeight()/boxSize+2];
        for (int i = 0; i < trees.GetLength(0); i++) {
            for (int j = 0; j < trees.GetLength(1); j++) {
                trees[i,j] = new HashSet<ITree>();
            }
        }
        foreach (ITree tree in  startTrees) {
            AddTree(tree);
        }

    }

    public Forest(List<ITree> startTrees, List<ITree> treesSpecies, Map map, Texture2D positionTexture) :this (startTrees, treesSpecies,map )
     {
         this.positionTexture =positionTexture;
     }

  
    public void DeleteDead(ICollection<ITree> deadTrees) {

        foreach (ITree tree in deadTrees) {
            Deletetree(tree);
        }
        deadTrees.Clear();
    }

    private void EndYear(int dead) {
        ICollection<ITree> deadTrees = new HashSet<ITree>();
        foreach (ITree tree in allTrees) {
            treeManager.YearBonus(tree);
            if (tree.GetHealth() < dead) {
                deadTrees.Add(tree);
            }
        }
        DeleteDead(deadTrees);
    }

    public bool AddTree(ITree tree)
     {
         if (tree == null)
         {
             return false;
         }
 
        if (positionTexture != null)
         {
             if (positionTexture.GetPixel((int)tree.GetPosition().x, (int)tree.GetPosition().y) != Color.black)
             {
               Object.DestroyImmediate(tree.gameObject);
                 return false;
             }
         }
         if (tree.GetPosition().x >= forestMap.GetWidth() || tree.GetPosition().x < 0 || tree.GetPosition().y >= forestMap.GetHeight() || tree.GetPosition().y < 0)
         {
             Object.DestroyImmediate(tree.gameObject);
             return false;
         }
         int x = (int)tree.GetPosition().x / boxSize;
         int y = (int)tree.GetPosition().y / boxSize;

        
             MapData mapData = forestMap.GetInfo(tree.GetPosition());
             treeManager.SetPositionEffect(tree, mapData, yearChange);
             if (tree.GetYearChange() < -30)
             {
                 Object.DestroyImmediate(tree.gameObject);
                 return false;
             }
         if (!trees [x,y].Contains(tree))
         {
             trees [x,y].Add(tree);
             allTrees.Add(tree);
         }
         if (tree.transform.parent == null)
         {
             tree.transform.parent = forestMap.transform;
         }
         return true;
     }

    public void Deletetree(ITree tree)
     {

         if (tree == null)
         {
             return;
         }
        int x = (int)tree.GetPosition().x / boxSize;
         int y = (int)tree.GetPosition().y / boxSize;
         try
         {
             allTrees.Remove(tree);
             
             if (tree.GetPosition().x >= forestMap.GetWidth() || tree.GetPosition().x < 0 || tree.GetPosition().y >= forestMap.GetWidth() || tree.GetPosition().y < 0)
             {
                Object.DestroyImmediate(tree.gameObject);
                 Debug.Log("problem potřeba řešit, pokud se to vyskytne jenom X, kde X je počet druhu stromu tak je to vpořádku");
                 return;
             }
             trees [x, y].Remove(tree);
            
                 Object.DestroyImmediate(tree.gameObject,false);
             

         }
         catch (System.Exception)
         {
             Debug.Log("there was an error in deleting tree");
             //já vím že prasárna ale prozatím
             //trees [x, y].Remove(tree);
         }
         

     }

    public void NextYear(int deletedHeal)
     {
         SetTrees();
         Interact();
         EndYear(deletedHeal);
     }
    
    private void SetTrees()
    {
         List<ITree> newTrees = new List<ITree>();
        foreach (ITree tree in allTrees) {
            if (tree.CanChildren()) { //určuje si strom
                newTrees.AddRange(treeManager.PlantTrees(tree));
            }
        }
        foreach (ITree tree in newTrees) {
            AddTree(tree);
        }
       // newTrees(newTrees);
        
    }

    public void Interact()  {
        for (int i = 0; i < trees.GetLength(0); i++) {
            for (int j = 0; j < trees.GetLength(1); j++) {
                foreach (ITree checkedTree in trees[i,j]) {
                    int xPos = i;
                    int yPos = j;
                    int xMax = Mathf.Min(xPos + (int) Mathf.Ceil((float)maxDistance / (float)boxSize),forestMap.GetWidth()/boxSize+1);
                    int yMax = Mathf.Min(yPos + (int) Mathf.Ceil((float)maxDistance / (float)boxSize), forestMap.GetHeight()/boxSize+1);
                    int weaknes = checkedTree.GetHealth();
                    for (int startX = Mathf.Max(xPos - (int)Mathf.Ceil((float)maxDistance / (float)boxSize), 0); startX < xMax; startX++)
                    {
                        for (int startY = Mathf.Max(yPos - (int)Mathf.Ceil((float)maxDistance / (float)boxSize), 0); startY < yMax; startY++)
                        {
                            foreach (ITree otherTree in trees[startX,startY]) {
                                if (startX != xPos || startY != yPos || !checkedTree.Equals(otherTree)) {
                                    weaknes = weaknes - treeManager.NegativePower(checkedTree, otherTree);//checkedTree.weaktree(otherTree);
                                }
                            }
                        }
                    }
                    checkedTree.AddHealth((short) -(checkedTree.GetHealth() - weaknes));
                }
            }
        }

    }

    public void TestTreesOnPositionTexture()
    {
        List<ITree> treesToDelete = new List<ITree>();
        foreach( ITree tree in allTrees) 
            
        {
            if (positionTexture != null)
            {
                if (positionTexture.GetPixel((int)tree.GetPosition().x, (int)tree.GetPosition().y) != Color.black)
                {
                    treesToDelete.Add(tree);
                }
            }
        }
        DeleteDead(treesToDelete);

    }

    /// <summary>
    /// vrátí stromy ze čtverce x,y - x+1,y+1
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public List<ITree> TreesInSquare(int x, int y)
    {
        int boxX =  x / boxSize;
        int boxY =  y / boxSize;
        List<ITree> findedTrees = new List<ITree>();
        foreach (ITree tree in trees [boxX, boxY])
        {
            Vector2 position = tree.GetPosition();
            if (position.x <= x && (position.x + 1) >= x && position.y <= y && (position.y + 1) >= y)
            {
                findedTrees.Add(tree);
            }
        }
        return findedTrees;
    }

   

   
}
