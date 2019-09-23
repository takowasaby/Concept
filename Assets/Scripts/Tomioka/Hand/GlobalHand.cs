using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GlobalHand
{
    private static GlobalHand globalHand = new GlobalHand();

    private static Dictionary<PlayerID, Dictionary<int, FrameID>> globalHandIDs = new Dictionary<PlayerID, Dictionary<int, FrameID>>();

    private GlobalHand()
    {
    }

    public static void Reset()
    {
        globalHandIDs = new Dictionary<PlayerID, Dictionary<int, FrameID>>();
        foreach(PlayerID playerID in System.Enum.GetValues(typeof(PlayerID)))
        {
            globalHandIDs.Add(playerID, new Dictionary<int, FrameID>());
            for(int i = 0; i < 3; i++)
            {
                globalHandIDs[playerID].Add(i, FrameID.NullFrame);
            }
        }
    }

    public static void SetHand(PlayerID playerID, int handIndex, FrameID frameID)
    {
        globalHandIDs[playerID][handIndex] = frameID;
    }

    public static FrameID GetHand(PlayerID playerID, int handIndexD)
    {
        return globalHandIDs[playerID][handIndexD];
    }

    public static IEnumerable<FrameID> GetHand(PlayerID playerID)
    {
        return globalHandIDs[playerID].Select(pair => pair.Value);
    }
}
