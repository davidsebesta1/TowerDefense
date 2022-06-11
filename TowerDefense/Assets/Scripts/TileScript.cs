using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [Header("Blueprint Prefabs")]
    [SerializeField] private GameObject[] towerTemplates;

    [Header("Tower Prefabs")]
    [SerializeField] private GameObject[] cannonBasicPrefabs;
    [SerializeField] private GameObject[] minigunPrefabs;
    [SerializeField] private GameObject[] railgunPrefabs;
    [SerializeField] private GameObject[] missileLauncherPrefabs;
    [SerializeField] private GameObject[] artyPrefabs;

    private GameObject playerCamera;
    private PlayerController playerController;

    private GameObject tower;

    private bool isClickedOn = false;
    private bool buildMode = false;
    private bool isOccupied = false;

    private int currectTowerTier = 0;

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

    public void UpgradeTower()
    {
        Debug.Log(gameObject.name);
        switch (tower.tag)
        {
            case "Cannon":
                if (currectTowerTier < 3)
                {
                    Destroy(tower);
                    currectTowerTier++;
                    tower = Instantiate(cannonBasicPrefabs[currectTowerTier], transform.position + new Vector3(0, 0.1f, 0), cannonBasicPrefabs[currectTowerTier].transform.rotation);
                }
                break;
            case "Minigun":
                if (currectTowerTier < 3)
                {
                    Destroy(tower);
                    currectTowerTier++;
                    tower = Instantiate(minigunPrefabs[currectTowerTier], transform.position + new Vector3(0, 0.1f, 0), minigunPrefabs[currectTowerTier].transform.rotation);
                }
                break;
            case "Railgun":
                if (currectTowerTier < 3)
                {
                    Destroy(tower);
                    currectTowerTier++;
                    tower = Instantiate(railgunPrefabs[currectTowerTier], transform.position + new Vector3(0, 0.1f, 0), railgunPrefabs[currectTowerTier].transform.rotation);
                }
                break;

            case "MissileLauncher":
                if (currectTowerTier < 3)
                {
                    Destroy(tower);
                    currectTowerTier++;
                    tower = Instantiate(missileLauncherPrefabs[currectTowerTier], transform.position + new Vector3(0, 0.1f, 0), missileLauncherPrefabs[currectTowerTier].transform.rotation);
                }
                break;

            case "Artillery":
                if (currectTowerTier < 3)
                {
                    Destroy(tower);
                    currectTowerTier++;
                    tower = Instantiate(artyPrefabs[currectTowerTier], transform.position + new Vector3(0, 0.1f, 0), artyPrefabs[currectTowerTier].transform.rotation);
                }
                break;
        }
    }

    public void BuildTower(int id)
    {
        if (buildMode && !isOccupied)
        {
            Debug.Log("id: " + id);
            var PlrControl = playerCamera.GetComponent<PlayerController>();
            switch (id)
            {
                case 0:
                    if (PlrControl.GetMoneyAmount() >= 20) // basic tower
                    {
                        PlrControl.AddMoneyAmount(-20);
                        HideBlueprint();
                        isOccupied = true;
                        tower = Instantiate(cannonBasicPrefabs[0], transform.position + new Vector3(0, 0.1f, 0), cannonBasicPrefabs[0].transform.rotation);
                    }
                    break;
                case 1:
                    if (PlrControl.GetMoneyAmount() >= 75) // machine gun
                    {
                        PlrControl.AddMoneyAmount(-75);
                        HideBlueprint();
                        isOccupied = true;
                        tower = Instantiate(minigunPrefabs[0], transform.position + new Vector3(0, 0.1f, 0), minigunPrefabs[0].transform.rotation);
                    }
                    break;
                case 2:
                    if (PlrControl.GetMoneyAmount() >= 200) // rail gun
                    {
                        PlrControl.AddMoneyAmount(-200);
                        HideBlueprint();
                        isOccupied = true;
                        tower = Instantiate(railgunPrefabs[0], transform.position + new Vector3(0, 0.1f, 0), railgunPrefabs[0].transform.rotation);
                    }
                    break;
                case 3:
                    if (PlrControl.GetMoneyAmount() >= 350) // rocket launcher
                    {
                        PlrControl.AddMoneyAmount(-350);
                        HideBlueprint();
                        isOccupied = true;
                        tower = Instantiate(missileLauncherPrefabs[0], transform.position + new Vector3(0, 0.1f, 0), missileLauncherPrefabs[0].transform.rotation);
                    }
                    break;
                case 4:
                    if (PlrControl.GetMoneyAmount() >= 550) // artillery
                    {
                        PlrControl.AddMoneyAmount(-550);
                        HideBlueprint();
                        isOccupied = true;
                        tower = Instantiate(artyPrefabs[0], transform.position + new Vector3(0, 0.1f, 0), artyPrefabs[0].transform.rotation);
                    }
                    break;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!playerController.GetUpgradeMenuOpen())
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
    }

    private void OnMouseExit()
    {
        isClickedOn = false;
    }

    public GameObject GetTower()
    {
        return tower;
    }
}
