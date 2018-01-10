using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestManager {

    System.Random r = new System.Random();


    public Forest createForest(Map map, List<ITree> treesSpecies)
    {
        Forest forest = new Forest(new List<ITree>(), treesSpecies, map);
        generateStartTree(forest, treesSpecies);
        return forest;
    }

    public Forest createForest(Map map, List<ITree> treesSpecies, Texture2D texture)
    {
        Forest forest = new Forest(new List<ITree>(), treesSpecies, map, texture);
        generateStartTree(forest, treesSpecies, texture);
        return forest;
    }

    private void generateStartTree(Forest forest, List<ITree> treesSpecies, Texture2D texture)
    {
       // List<ITree> startTrees = new List<ITree>();

        int width = forest.forestMap.GetWidth();
        int height = forest.forestMap.GetHeight();
       
       for (int i = 0; i < (width / 20)+1; i++) {
            for (int j = 0; j < (height / 20)+1; j++) {
                foreach (ITree tree in treesSpecies) {
                    double treePositionX = r.NextDouble() * 20.0 + (double) (i * 20);
                    double treePositionY = r.NextDouble() * 20.0 + (double) (j * 20);
                    if (texture.GetPixel((int)treePositionX, (int)treePositionY) != Color.black)
                    {
                        continue;
                    }
                    forest.AddTree(tree.CreateChildren(new Vector3((float)treePositionX, 0, (float)treePositionY)));
                }
            }
        }
        //return startTrees;
    }

    private void generateStartTree(Forest forest, List<ITree> treesSpecies)
    {
        // List<ITree> startTrees = new List<ITree>();

        int width = forest.forestMap.GetWidth();
        int height = forest.forestMap.GetHeight();

        for (int i = 0; i < (width / 20) + 1; i++)
        {
            for (int j = 0; j < (height / 20) + 1; j++)
            {
                foreach (ITree tree in treesSpecies)
                {
                    double treePositionX = r.NextDouble() * 20.0 + (double)(i * 20);
                    double treePositionY = r.NextDouble() * 20.0 + (double)(j * 20);
                    forest.AddTree(tree.CreateChildren(new Vector3((float)treePositionX, 0, (float)treePositionY)));
                }
            }
        }
        //return startTrees;
    }
}
