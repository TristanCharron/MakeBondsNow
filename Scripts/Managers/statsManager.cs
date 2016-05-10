using UnityEngine;
using System.Collections.Generic;

public class statsManager : MonoBehaviour {
    private static List<Player> playerList;
   
    // testing purposes
    public  List<BondsManager> _bondsManagersList;
    private static List<BondsManager> bondsManagersList;

    // player awards based on results
    private static Player bestPlayer;
    private static Player worstPlayer;
    private static Player[] bestBondedPlayers = new Player[2];
    private static Player[] worstBondedPlayers = new Player[2];

    public static List<BondsManager> returnsBondsList() { return bondsManagersList; }


    public static void Initialize()
    {
        refreshPlayerList();
        onGenerateBondCombinaisonList();
    }

    void Update()
    {
        _bondsManagersList = bondsManagersList;
    }

    public static Player getPlayerBestVoteRatio()
    {
        refreshPlayerList();
        bestPlayer = playerList[0];
        foreach (Player player in playerList)
            bestPlayer = bestPlayer.getNbBonds() > player.getNbBonds() ? bestPlayer : player;
        //Debug.Log(" BEST PLAYER: " + bestPlayer.getName() + " RATIO : " + bestPlayer.getNbBonds());
        return bestPlayer;
    }

    public static Player getPlayerWorstVoteRatio()
    {
        refreshPlayerList();
        Player worstPlayer = playerList[0];
        foreach (Player player in playerList)
            worstPlayer = worstPlayer.getNbBonds() < player.getNbBonds() ? worstPlayer : player;
        //Debug.Log(" WORST PLAYER: " + worstPlayer.getName() + " RATIO : " + worstPlayer.getNbBonds());
        return worstPlayer;
    }

    public static void setBestBondsRatio()
    {
        Bonds bestBondRatio = bondsManagersList[0].bondList[0];

        foreach (BondsManager bondsManager in bondsManagersList)
            foreach (Bonds bond in bondsManager.bondList)
                bestBondRatio = bond.getNbBonds() > bestBondRatio.getNbBonds() ? bond : bestBondRatio;
            
       /* Debug.Log("PLAYERS BEST BONDED: " + bestBondRatio.playerBondedID + bestBondRatio.bondManagerPlayer.getID() + " RATIO : "
            + bestBondRatio.getNbBonds());*/

        bestBondedPlayers[0] = bestBondRatio.bondManagerPlayer;
        bestBondedPlayers[1] = roundManager.returnPlayerByID(bestBondRatio.playerBondedID);

    }
    public static void setWorstBondsRatio()
    {
        Bonds worstBondRatio = bondsManagersList[0].bondList[0];

        foreach (BondsManager bondsManager in bondsManagersList)
            foreach (Bonds bond in bondsManager.bondList)
                worstBondRatio = bond.getNbBonds() < worstBondRatio.getNbBonds() ? bond : worstBondRatio;

        Debug.Log("PLAYERS WORST BONDED: " + worstBondRatio.playerBondedID + worstBondRatio.bondManagerPlayer.getID() + " RATIO : "
            + worstBondRatio.getNbBonds());

        worstBondedPlayers[0] = worstBondRatio.bondManagerPlayer;
        worstBondedPlayers[1] = roundManager.returnPlayerByID(worstBondRatio.playerBondedID);
    }


    public static void refreshPlayerList()
    {
        playerList = new List<Player>();
        GameObject[] playerObjectList = lobbyManager.getPlayerList();
        for (int i = 0; i < playerObjectList.Length; i++)
            playerList.Add(playerObjectList[i].GetComponent<Player>());

    }

    public static void onGenerateBondCombinaisonList()
    {

        bondsManagersList = new List<BondsManager>();
        GameObject[] playerList = lobbyManager.getPlayerList();

        for (int i = 0; i < playerList.Length; i++)
        {
            Player curPlayer = playerList[i].GetComponent<Player>();
            bondsManagersList.Add(new BondsManager(curPlayer));

            for (int j = 0; j < playerList.Length; j++)
            {
                string comparedPlayer = playerList[j].GetComponent<Player>().getID();

                if (comparedPlayer != curPlayer.getID())
                    bondsManagersList[bondsManagersList.Count - 1].bondList.Add(new Bonds(comparedPlayer, curPlayer));

            }
        }


    }

    public static void setBond(string votingPlayerID,string votedPlayerID)
    {
        // find the matching player combinaison
        Bonds currentBond = returnBondsManagerByID(votingPlayerID).returnBondsByID(votedPlayerID);
        Bonds oppositeBond = returnBondsManagerByID(votedPlayerID).returnBondsByID(votingPlayerID);
        currentBond.setBond(1);

        //Testing actual awards in-game
        Debug.Log(currentBond.didPlayerBonded());
        Debug.Log(oppositeBond.didPlayerBonded());
        Debug.LogError(currentBond.bondManagerPlayer + "BONDED WITH : " + currentBond.playerBondedID + "BONDS : " + currentBond.getNbBonds());
        if (currentBond.didPlayerBonded() && oppositeBond.didPlayerBonded())
            Debug.LogError("WOW, THEY BONDED!");
        
        
    }

    public static void resetTurnBonds()
    {
        foreach (BondsManager bondsManager in bondsManagersList)
            foreach (Bonds bond in bondsManager.bondList)
                bond.onCancelBond();
    }

    public static BondsManager returnBondsManagerByID(string id)
    {
        BondsManager bManager = null;
        foreach (BondsManager bondsManager in bondsManagersList)
            if (bondsManager.player.getID() == id)
                return bondsManager;

        Debug.Assert(bManager == null, "BOND MANAGER HAS NOT BEEN FOUND ON" + id);
        return bManager;
    }

    // Return awards for end-game celebration
    public static void returnStatistics()
    {
        worstPlayer = getPlayerWorstVoteRatio();
        bestPlayer = getPlayerBestVoteRatio();
        setWorstBondsRatio();
        setBestBondsRatio();

    }


}
