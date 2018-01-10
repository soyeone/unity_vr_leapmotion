using UnityEngine;
using System.Collections;

public abstract class ITree : MonoBehaviour{

	 public abstract ITree CreateChildren(Vector3 position); // vytvoří



     public abstract int GetPrefabID();
     public abstract void SetPrefabID(int ID);
     public abstract int [] GetTextureEffect();
     public abstract int GetMaxAge();
     public abstract float GetMaxNegativeLenght();
     public abstract int GetParrentAge();
     public abstract float GetNegativeLenght();
     public abstract float GetNewTreeLenght();
     public abstract int GetChildrenNumber();
     public abstract int GetMaxChildrenNumber();
     public abstract Vector2 GetPosition();
     public abstract int GetHealth();
     public abstract void AddHealth(short healthy);
     public abstract void SetAge(short age);
     public abstract int GetAge();

     public abstract int GetMaxAltitude();
     public abstract int GetMinAltitude();
     public abstract int GetGradient();
   
    /**
     * apply year change -> refresh health value, it is called every year of after health change 
     */
     public abstract void ApplyYearChange();
     public abstract bool CanChildren();

    /**
     * yearChange is value which increments health every year, this value is based mostly on MapData 
     * if any map attribute is changed, new year change must be recalculated and set
     *
     * @param yearChange nová hodnota která přpeíše starou, o tolik se mění
     * hodnota zdraví každý rok, když se zavolá addYear
     */
     public abstract void SetYearChange(short yearChange);

     public abstract int GetYearChange();

     

     public abstract  bool Equals(Object obj);

    /**
     * Zepta se zda strom který dostane jako paramter ovlivňuje zdraví jeho
     * samotného a vrací kolik ze zdraví/health mu ubere
     * 
     * ask other tree how strong this tree changing health (this tree lost xx health)
     *
     * @param tree strom který oslabuje
     * @return číslo o kolik je strom oslaben (-heath) pokud 0 tak vubec
     */
     public abstract int Weaktree(ITree tree);
}
