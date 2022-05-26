using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] private GameObject[] towerTemplates;

    private GameObject playerCamera;

    private bool isClickedOn = false;

    private void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void ShowBlueprint(int towerID)
    {
        if (isClickedOn)
        {
            Debug.Log(towerID);
            Instantiate(towerTemplates[towerID], transform.position + new Vector3(0, 1, 0), towerTemplates[towerID].transform.rotation);
        }
    }

    public void HideBlueprint()
    {
        Destroy(GameObject.FindGameObjectWithTag("Blueprint"));
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isClickedOn = true;
            playerCamera.GetComponent<PlayerController>().setSelectedTile(this.gameObject); ;
            ShowBlueprint(0);
        }


        //fix this pls
        if (playerCamera.GetComponent<PlayerController>().getSelectedTile() != null)
        {
            if (this.gameObject != playerCamera.GetComponent<PlayerController>().getSelectedTile())
            {
                isClickedOn = false;
                HideBlueprint();
            }
        }
    }
}
