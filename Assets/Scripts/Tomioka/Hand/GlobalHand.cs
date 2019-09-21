using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GlobalHand
{
    private static GlobalHand globalHand = new GlobalHand();

    private static Dictionary<PlayerID, Dictionary<SituationID, FrameID>> globalHandIDs = new Dictionary<PlayerID, Dictionary<SituationID, FrameID>>();

    private GlobalHand()
    {
        Reset();
    }

    public static void Reset()
    {
        globalHandIDs = new Dictionary<PlayerID, Dictionary<SituationID, FrameID>>();
        foreach(PlayerID playerID in System.Enum.GetValues(typeof(PlayerID)))
        {
            globalHandIDs.Add(playerID, new Dictionary<SituationID, FrameID>());
            foreach(SituationID situationID in System.Enum.GetValues(typeof(SituationID)))
            {
                globalHandIDs[playerID].Add(situationID, FrameID.NullFrame);
            }
        }
    }

    public static void SetHand(PlayerID playerID, SituationID situationID, FrameID frameID)
    {
        globalHandIDs[playerID][situationID] = frameID;
    }

    public static FrameID GetHand(PlayerID playerID, SituationID situationID)
    {
        return globalHandIDs[playerID][situationID];
    }

    public static IEnumerable<FrameID> GetHand(PlayerID playerID)
    {
        return globalHandIDs[playerID].Select(pair => pair.Value);
    }
}
