using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeManager  {
    System.Random r = new System.Random();
   

    public List<ITree> PlantTrees(ITree parent) {
        List<ITree> trees = new List<ITree>();
        for (int i = 0; i < parent.GetChildrenNumber(); i++) {
            ITree tree = AtGoodPlace(parent);
            if (!(tree == null) && !trees.Contains(tree)) {
                trees.Add(tree);
            }
        }
        return trees;
    }

    /**
     * Samotné generování nového stormu od rodiče, používá polární souřadnice,
     * tj náhodně generuje úhel a vzdálenost
     *
     * @param parent strom který generuje potomka
     * @return nový strom
     */
    private ITree PolarGenerated(ITree parent)
    {
       
        float distance = (float)r.NextDouble() * parent.GetNewTreeLenght();
        float angle = (float)r.NextDouble() * 360.0f;
        Vector2 point = (new Vector2(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle)));
        //return parent.CreateChildren(new Vector2.(point.getX() + parent.getPosition().getX(), point.getY() + parent.getPosition().getY()));
        return parent.CreateChildren(new Vector3(point.x + parent.GetPosition().x, 0, point.y + parent.GetPosition().y)); // dodělat

       
    }

    /**
     * Metoda kontroluje zda se nový strom nachází dostatečně daleko od svého
     * rodiče, pokud ano, vzniká strom zdravý, pokud je na hranici vzniká nový
     * strom,ale oslabený, nebo nezvniká žádný
     *
     * @param parent rodič.který generuje potomky
     * @return vrací strom pokud byl vygenerován na správném místě, pokud ne
     * vrací null
     */
    private ITree AtGoodPlace(ITree parent)
    {
        ITree newTree = PolarGenerated(parent);
        if (newTree == null)
        {
            return null;
        }
        int parrentNegativePower = NegativePower(parent, newTree) - 15;
        if (parrentNegativePower >= 50)
        {
            Object.DestroyImmediate(newTree.gameObject);
            return null;
        }
        parrentNegativePower = Mathf.Max(0, parrentNegativePower);
        newTree.AddHealth((short)-parrentNegativePower);
        return newTree;

    }

    /**
     * Add only amount of years to current tree
     * no yearchange value added
     *
     * @param tree
     * @param age
     */
    public void AddYears(ITree tree, int age)
    {
        tree.SetAge((short)(tree.GetAge() + age));
    }

    /**
     * Return value how much health take fromTree from goalTree
     *
     * @param goalTree tree which is weakened
     * @param fromTree tree which is weakening
     * @return power of weakening
     */
    public int NegativePower(ITree goalTree, ITree fromTree)
    {
        return goalTree.Weaktree(fromTree);
    }

    /**
     * Add yearchBonus/YearChange value (most times grater than zero) to tree
     * health This method is called every year
     *
     * @param tree choosed tree
     */
    public void YearBonus(ITree tree)
    {
        tree.AddHealth((short)tree.GetYearChange());
        AddYears(tree, 1);
    }

   /// <summary>
   /// tady se musí upravit a výška stromu (y) souřadnice
   /// </summary>
   /// <param name="tree"></param>
   /// <param name="positionInfo"></param>
   /// <param name="yearBonus"></param>
    public void SetPositionEffect(ITree tree, MapData positionInfo, int yearBonus)
    {

        int yearChange = yearBonus;
        if (positionInfo != null)
        {
            if (positionInfo.height > tree.GetMaxAltitude() + 150 || positionInfo.height < tree.GetMinAltitude() - 150)
            {
                yearChange = yearChange - 25;
            }
            else if (positionInfo.height > tree.GetMaxAltitude() + 40 || positionInfo.height < tree.GetMinAltitude() - 40)
            {
                yearChange = yearChange - 15;
            }
            else if (positionInfo.height < tree.GetMaxAltitude() - 40 && positionInfo.height > tree.GetMinAltitude() + 40)
            {
                yearChange = yearChange + 5;
            }//doplnit prozatim  0;

            if (positionInfo.angle > tree.GetGradient() + 20)
            {

                yearChange = yearChange - 50;
            }
            else if (positionInfo.angle > tree.GetGradient() + 5)
            {
                yearChange = yearChange - 12;
            }
            else if (positionInfo.angle < Mathf.Max(0, tree.GetGradient() - 5))
            {
                yearChange = yearChange + 2;
            }
            tree.transform.position = new Vector3(tree.transform.position.x, positionInfo.height - 0.07f, tree.transform.position.z);
            //Debug.Log(tree.GetTextureEffect().ToString() + "::" + positionInfo.textureData.ToString());
            int text = Mathf.Min(positionInfo.textureData.GetLength(0) - 1, tree.GetTextureEffect().GetLength(0) - 1);
           // Debug.Log(text.ToString());
            for (int i = text; i >= 0; i--)
            {
                yearChange = yearChange + (int)((float)tree.GetTextureEffect()[i] * positionInfo.textureData[i]);
            }

            // tree.terrainData = positionInfo.textureData;
        }
        else
        {
            Debug.Log("problem");
        }
        tree.SetYearChange((short)yearChange);

    }
}
