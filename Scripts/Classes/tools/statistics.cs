using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Bonds
{
   
    public Player bondManagerPlayer;
    public string playerBondedID;
   
    // for testing purpose
    public int _nbBonds;
    public bool _isBonded;
    
    // encapsulated values
    private int nbBonds;
    private bool isBonded;

    public Bonds(string id, Player player)
    {
        bondManagerPlayer = player;
        playerBondedID = id;
        nbBonds = 0; 
    }
    public Bonds(string id, Player player,int bonds)
    {
        bondManagerPlayer = player;
        playerBondedID = id;
        nbBonds = bonds;
    }
    public int getNbBonds() { return nbBonds; }
    public void setBond(int value) { nbBonds += value; isBonded = true; _nbBonds = nbBonds; _isBonded = isBonded; }
    public bool didPlayerBonded() { return isBonded; }
    public void onCancelBond() { isBonded = false; _isBonded = isBonded; }
}

[System.Serializable]
public class BondsManager
{
    public Player player;
    public List<Bonds> bondList;
    public BondsManager(Player _player)
    {
        player = _player;
        bondList = new List<Bonds>();
    }
    

    public void onPlayerBond(string id, int value)
    {
        returnBondsByID(id).setBond(value);
    }

    public Bonds returnBondsByID(string id)
    {
        Bonds bond = null;
        foreach (Bonds bonds in bondList)
            if (bonds.playerBondedID == id)
                return bonds;


        Debug.Assert(bond == null, "BOND HAS NOT BEEN FOUND ON" + id);
        return bond;

    }

}



