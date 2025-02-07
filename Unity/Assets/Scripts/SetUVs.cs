//Maija - This script allows us to change an object's UV, which controls the texture being
//rendered from our TextureArray shader.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SetUVs : MonoBehaviour
{
    //Maija - increase range when new textures are added, there may be a way to 
    //automate this that I am not aware of
    [Range(0,6)] 
    public int index = 0;

    void OnValidate()
    {
        List<Vector3> uvs = new List<Vector3>();

        //Maija - I dont like using GetComponent here 
        //but SerializeField will not work on a ExecuteInEditMode script
        Mesh mesh = this.GetComponent<MeshFilter>().sharedMesh;

        mesh.GetUVs(0, uvs);
        for(int i = 0; i < uvs.Count; i++){
            uvs[i] = new Vector3(uvs[i].x, uvs[i].y, index);
        }

        mesh.SetUVs(0, uvs);
    }

}
