using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* HexCell contains all data that needs to be known by the Cells themselves
	 If you need to add data to the Cells, add a private variable and get and set methods as done for neighbors[]
*/
public class HexCell : MonoBehaviour
{
  public bool tilled = false;
  public bool fungicide = false;
  //Maija - etc... (implement the rest of the attributes later when we know what we're doing)
	public GameObject plant;
}
