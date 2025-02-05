using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* HexCell contains all data that needs to be known by the Cells themselves
	 If you need to add data to the Cells, add a private variable and get and set methods as done for neighbors[]
*/
public class HexCell : MonoBehaviour
{
	public HexCoordinates coordinates;
  public int terrainType = 0;
  public bool tilled = false;
  public bool fungicide = false;
  //Maija - etc... (implement the rest of the attributes later when we know what we're doing)
	public GameObject plant;


  //Maija - below can probably be removed with this comment once we move to 2D and change how the grid is created (in editor)

  [SerializeField]
  HexCell[] neighbors;

  public HexCell GetNeighbor(HexDirection direction)
  {
    return neighbors[(int)direction];
  }

  public void SetNeighbor (HexDirection direction, HexCell cell)
  {
		neighbors[(int)direction] = cell;
    cell.neighbors[(int)direction.Opposite()] = this;
	}

	/* Instantiates an object at the center of the cell + offset. */
	public void InstantiateObject(GameObject obj, Vector3 offset)
	{
		Instantiate(obj, transform.position + offset, Quaternion.identity, gameObject.transform);
	}

  //Maija note: Keep the code below if you end up making changes to the tile system, 
  //it is necessary to keep the tool system functional
}
