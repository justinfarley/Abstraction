using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpikeTower", menuName = "TowerUpgradeSprites")]
public class TowerUpgradeSprites : ScriptableObject
{
    [Header("Top Path Exclusive sprites")]
    public List<Sprite> topPathExc = new List<Sprite>();

    [Space(10f)]
    [Header("Middle Path Exclusive sprites")]
    public List<Sprite> middlePathExc = new List<Sprite>();

    [Header("Bottom Path Exclusive sprites")]
    public List<Sprite> bottomPathExc = new List<Sprite>();

    [Header("Top Path Cross sprites")]
    public List<Sprite> topPathCross = new List<Sprite>();

    [Header("Middle Path Cross sprites")]
    public List<Sprite> middlePathCross = new List<Sprite>();

    [Header("Bottom Path Cross sprites")]
    public List<Sprite> bottomPathCross = new List<Sprite>();

    public static List<Sprite> GetAllSprites(TowerUpgradeSprites u)
    {
        List<Sprite> list = new List<Sprite>();
        foreach(var v in u.topPathExc){
            list.Add(v);
        }
        foreach (var v in u.middlePathExc)
        {
            list.Add(v);
        }
        foreach (var v in u.bottomPathExc)
        {
            list.Add(v);
        }
        foreach (var v in u.topPathCross)
        {
            list.Add(v);

        }
        foreach (var v in u.middlePathCross)
        {
            list.Add(v);
        }
        foreach (var v in u.bottomPathCross)
        {
            list.Add(v);
        }
        return list;
    }

}
