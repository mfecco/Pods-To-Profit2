//Maija - This script allows us to change an object's UV in runtime
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetUVsRuntime : MonoBehaviour
{
    [SerializeField] private HexCell hexCell;

    private void Start() {
        Player.Instance.ChangeTileUV += Player_ChangeTileUV;
    }

    private void Player_ChangeTileUV(object sender, Player.ChangeTileUVEventArgs e) {
        //this entire function could go in here but I may refactor
        if(e.selectedTile == hexCell){
            Debug.Log("Selected Tile: "+ e.selectedTile+ "; HexCell: "+ hexCell);
            ChangeUV(e.newUV);
        }
    }

    private void ChangeUV(int newUV)
    {
        List<Vector3> uvs = new List<Vector3>();
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;

        mesh.GetUVs(0, uvs);
        for(int i = 0; i < uvs.Count; i++){
            uvs[i] = new Vector3(uvs[i].x, uvs[i].y, newUV);
        }

        mesh.SetUVs(0, uvs);
    }

}
