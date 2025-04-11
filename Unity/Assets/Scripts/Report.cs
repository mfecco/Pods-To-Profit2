using System.Collections.Generic;
using UnityEngine;
using System;


public static class Report
{
    //Simple report to console that shows current active events and total value of buschels.

    public static void populateReport(TurnPhase current, Yield y)
    {   

        Debug.Log($"**********FARM REPORT**************");
        Debug.Log($"*********{current} PHASE)**********");
        foreach (var e in y.activeEvents)
        {
            e.PrintDetails();
            
        }

        Debug.Log($"************* ${(float)Math.Truncate(y.cropYield*10.25f*100)/100} *****************");
    }


}
