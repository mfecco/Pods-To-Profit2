using System.Collections.Generic;
using UnityEngine;


public class Report
{

    public Yield cropYield;
    public float totalValue;
    public RandomEventHandler Handler;
    //figure out how to aggregate all of the details of each active modifier.
    //pull Event.eventDescription
    public List<Modifier> modDetails = new List<Modifier>();

    public void populateReport()
    {
        totalValue = cropYield.calcYield();

        foreach (var modifier in cropYield.activeModifiers)
        {
            modifier.getDetails();
        }

    }

}
