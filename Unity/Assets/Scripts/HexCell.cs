using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* HexCell contains all data that needs to be known by the Cells themselves
	 If you need to add data to the Cells, add a private variable and get and set methods as done for neighbors[]
*/
[System.Serializable]
public class HexCell : MonoBehaviour
{
  [SerializeField] private Transform plantPoint;
  private SeedObject seedObject;



  private bool tilled = false;
  private bool fertilizer = false;
  private bool insecticide = false;
  private bool fungicide = false;
  private bool herbicide = false;
  
  public plantYield yield;

//Needed to allow for collection with Yield
void Start(){
  gameObject.tag = "HexCell";
}
// used to attach a plantYield component and scirpt to hexcell at runtime
void Awake()
{
  if (yield == null)
  {
    yield = gameObject.AddComponent<plantYield>();
  }
}

//adapted from CodeMonkey's Kitchen Chaos game tutorial
  public Transform GetSeedObjectFollowTransform() {
    return plantPoint;
  }
  public void SetSeedObject(SeedObject seedObject) {
    this.seedObject = seedObject;
  }

  public SeedObject GetSeedObject() {
    return seedObject;
  }

  public void ClearSeedObject() {
    seedObject = null;
  }

  public bool HasSeedObject() {
    return seedObject != null;
  }  




  //getters/setters
  public bool getTilled(){
    return tilled;
  }

  public void setTilled(bool tillBool){
    tilled = tillBool;
  }

  public bool getFertilizer(){
    return fertilizer;
  }
  public void setFertilizer(bool fertilizerBool){
    Debug.Log("Before setFertilizer : " + this + ": Fertilizer = "+ fertilizer);
    fertilizer = fertilizerBool;
    Debug.Log("After setFertilizer : " + this + ": Fertilizer = "+ fertilizer);
  }

  public bool getInsecticide(){
    return insecticide;
  }
  public void setInsecticide(bool insecticideBool){
    insecticide = insecticideBool;
  }

  public bool getFungicide(){
    return fungicide;
  }
  public void setFungicide(bool fungicideBool){
    fungicide = fungicideBool;
  }

  public bool getHerbicide(){
    return herbicide;
  }
  public void setHerbicide(bool herbicideBool){
    herbicide = herbicideBool;
  }
}
