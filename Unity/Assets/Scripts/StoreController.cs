using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreController : MonoBehaviour
{
    public List<StoreItem> storeItems;
	private int currentPage = 0;
	public GameObject[] storePages;
    
    // Start is called before the first frame update
    void Start()
    {
        ShowPage(0);
    }

    public void ShowPage(int idx)
    {
		// Ensure the index is within the valid range
		if (idx < 0 || idx >= storePages.Length) return;

		// Deactivate all pages
		for (int i = 0; i < storePages.Length; i++)
		{
			storePages[i].SetActive(i == idx);
		}

		currentPage = idx;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
