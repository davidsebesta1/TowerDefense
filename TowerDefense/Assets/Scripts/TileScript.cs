using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] private GameObject[] towerTemplates;
    [SerializeField] private GameObject[] towerObjects;

    private GameObject playerCamera;
    private PlayerController playerController;

    private bool isClickedOn = false;
    private bool buildMode = false;
    private bool isOccupied = false;

    private void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerController = playerCamera.GetComponent<PlayerController>();
    }

    private void ShowBlueprint(int towerID)
    {
        if (isClickedOn && buildMode)
        {
            Debug.Log(towerID);
            Instantiate(towerTemplates[towerID], transform.position + new Vector3(0, 0.1f, 0), towerTemplates[towerID].transform.rotation);
        }
    }

    private void HideBlueprint()
    {
        Destroy(GameObject.FindGameObjectWithTag("Blueprint"));
    }

    public void BuildTower(int id)
    {
        if (buildMode && !isOccupied)
        {
            Debug.Log("id: " + id);
            switch (id)
            {
                case 0:
                    if (playerCamera.GetComponent<PlayerController>().GetMoneyAmount() >= 20) // basic tower
                    {
                        playerCamera.GetComponent<PlayerController>().AddMoneyAmount(-20);
                        HideBlueprint();
                        isOccupied = true;
                        Instantiate(towerObjects[id], transform.position + new Vector3(0, 0.1f, 0), towerObjects[id].transform.rotation);
                    }
                    break;
                case 1:
                    if (playerCamera.GetComponent<PlayerController>().GetMoneyAmount() >= 75) // machine gun
                    {
                        playerCamera.GetComponent<PlayerController>().AddMoneyAmount(-75);
                        HideBlueprint();
                        isOccupied = true;
                        Instantiate(towerObjects[id], transform.position + new Vector3(0, 0.1f, 0), towerObjects[id].transform.rotation);
                    }
                    break;
                case 2:
                    if (playerCamera.GetComponent<PlayerController>().GetMoneyAmount() >= 200) // rail gun
                    {
                        playerCamera.GetComponent<PlayerController>().AddMoneyAmount(-200);
                        HideBlueprint();
                        isOccupied = true;
                        Instantiate(towerObjects[id], transform.position + new Vector3(0, 0.1f, 0), towerObjects[id].transform.rotation);
                    }
                    break;
                case 3:
                    if (playerCamera.GetComponent<PlayerController>().GetMoneyAmount() >= 350) // rail gun
                    {
                        playerCamera.GetComponent<PlayerController>().AddMoneyAmount(-350);
                        HideBlueprint();
                        isOccupied = true;
                        Instantiate(towerObjects[id], transform.position + new Vector3(0, 0.1f, 0), towerObjects[id].transform.rotation);
                    }
                    break;
                case 4:
                    if (playerCamera.GetComponent<PlayerController>().GetMoneyAmount() >= 550) // artillery
                    {
                        playerCamera.GetComponent<PlayerController>().AddMoneyAmount(-550);
                        HideBlueprint();
                        isOccupied = true;
                        Instantiate(towerObjects[id], transform.position + new Vector3(0, 0.1f, 0), towerObjects[id].transform.rotation);
                    }
                    break;
            }
        }
    }

    private void OnMouseEnter()
    {
        isClickedOn = true;
        buildMode = playerController.GetBuildMode();

        if (playerController.GetSelectedTile() != null)
        {
            playerController.GetSelectedTile().GetComponent<TileScript>().HideBlueprint();
        }

        playerController.SetSelectedTile(this.gameObject);

        if (!isOccupied)
        {
            ShowBlueprint(playerController.GetSelectedTowerID());
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isOccupied)
        {
            Debug.Log("Building tower");
            BuildTower(playerController.GetSelectedTowerID());
        }
    }

    private void OnMouseExit()
    {
        isClickedOn = false;
    }
}
