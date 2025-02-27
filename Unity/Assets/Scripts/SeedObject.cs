using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedObject : MonoBehaviour
{
    [SerializeField] private SeedObjectSO seedObjectSO;
    private HexCell seedObjectParent;

    public SeedObjectSO GetSeedObjectSO(){
        return seedObjectSO;
    }

    //SetSeedObjectParent is called in SeedInteractable
    public void SetSeedObjectParent(HexCell seedObjectParent){
        this.seedObjectParent = seedObjectParent;

        if (seedObjectParent.HasSeedObject()){
            Debug.LogError("HexCell already has SeedObject");
        }

        seedObjectParent.SetSeedObject(this);

        transform.parent = seedObjectParent.GetSeedObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public void DestroySelf() {
        seedObjectParent.ClearSeedObject();

        Destroy(gameObject);
    }
}
