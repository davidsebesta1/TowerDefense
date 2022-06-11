using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //Serialized Variables
    [Header("Property")]
    [SerializeField] private float speed;

    [Header("UI Elements")]
    [SerializeField] private CanvasRenderer BuildPanel;
    [SerializeField] private CanvasRenderer UpgradeMenu;
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button winButton;
    [SerializeField] private Button artyTargetButton;

    [SerializeField] private TextMeshProUGUI towerNameLabel;
    [SerializeField] private TextMeshProUGUI towerLevelLabel;
    [SerializeField] private TextMeshProUGUI towerCostUpgradeLabel;
    [SerializeField] private TextMeshProUGUI towerDamageLabel;
    [SerializeField] private TextMeshProUGUI towerFireRateLabel;
    [SerializeField] private TextMeshProUGUI towerRangeLabel;

    [Header("Else")]
    [SerializeField] private GameObject artyTargetPrefab;

    //Rigidbody
    private Rigidbody rb;

    //Selections
    private GameObject selectedTile;
    private GameObject selectedArty;

    //Inputs
    private float horizontalInput;
    private float verticalInput;

    private int selectedTowerID = 0;

    //Money
    private int money = 150;

    private int UILayer;

    //Bool stuff
    private bool buildMode = false;
    private bool isGameActive = false;
    private bool isOverUIElement = false;
    private bool artySelectionMode = false;
    private bool upgradeMenuOpen = false;

    //Creative mode bool
    [Header("Creative Mode")]
    [SerializeField] private bool creativeMode = false;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        UILayer = LayerMask.NameToLayer("UI");

        if (creativeMode)
        {
            this.money = 999999;
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        startButton.gameObject.SetActive(false);
    }

    public void EndGame()
    {
        isGameActive = false;
        verticalInput = 0f;
        horizontalInput = 0f;
        restartButton.gameObject.SetActive(true);
    }

    public void WinGame()
    {
        isGameActive = false;
        verticalInput = 0f;
        horizontalInput = 0f;
        winButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isGameActive)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.B) && !upgradeMenuOpen) // build mode
            {
                if (buildMode)
                {
                    buildMode = false;
                    BuildPanel.gameObject.SetActive(false);
                }
                else
                {
                    buildMode = true;
                    BuildPanel.gameObject.SetActive(true);
                }
            }

            isOverUIElement = IsPointerOverUIElement();
            if (Input.GetMouseButtonDown(0) && !isOverUIElement)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
                {

                    var GameObj = hit.collider.gameObject;

                    if (GameObj.CompareTag("Tile") && !buildMode) // tile stuff
                    {
                        Debug.Log(GameObj.name);
                        if (GameObj.GetComponent<TileScript>().GetTower() != null) // upgrade menu
                        {
                            if (upgradeMenuOpen)
                            {
                                selectedTile = null;
                                upgradeMenuOpen = false;
                                UpgradeMenu.gameObject.SetActive(false);

                                if (GameObj.GetComponent<TileScript>().GetTower().CompareTag("Artillery"))
                                {
                                    selectedArty = null;
                                    SetArtySelectionMode(false);
                                    artyTargetButton.gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                selectedTile = GameObj;
                                upgradeMenuOpen = true;
                                UpgradeMenu.gameObject.SetActive(true);

                                if (GameObj.GetComponent<TileScript>().GetTower().CompareTag("Artillery"))
                                {
                                    selectedArty = GameObj.GetComponent<TileScript>().GetTower().GetComponentInChildren<ArtilleryScript>().gameObject;
                                    SetArtySelectionMode(true);
                                    artyTargetButton.gameObject.SetActive(true);
                                }

                                var tower = GameObj.GetComponent<TileScript>().GetTower();

                                UpdateUpgradeMenuText(tower);

                            }
                        }
                    }

                    if (GameObj.CompareTag("Tile") && buildMode) // building tower
                    {
                        GameObj.GetComponent<TileScript>().BuildTower(selectedTowerID);
                    }

                    if (GameObj.CompareTag("Path") && artySelectionMode) // arty target
                    {
                        Vector3 localHit = hit.point;
                        selectedArty.GetComponent<ArtilleryScript>().SetTarget(Instantiate(artyTargetPrefab, localHit + new Vector3(0, 0.01f, 0), Quaternion.Euler(90, 0, 0)));

                        SetArtySelectionMode(false);

                        selectedTile = null;
                        upgradeMenuOpen = false;
                        UpgradeMenu.gameObject.SetActive(false);
                        artyTargetButton.gameObject.SetActive(false);
                    }
                }
            }
        }

        if (gameObject.transform.position.x > 20f)
        {
            horizontalInput += -1.5f;
        }

        if (gameObject.transform.position.x < 0f)
        {
            horizontalInput += 1.5f;
        }

        if (gameObject.transform.position.z > 25f)
        {
            verticalInput += -1.5f;
        }

        if (gameObject.transform.position.z < 0f)
        {
            verticalInput += 1.5f;
        }
    }

    public void UpgradeSelectedTower()
    {
        var tower = selectedTile.GetComponent<TileScript>().GetTower();

        switch (tower.tag)
        {
            case "Cannon":
                if (money >= tower.GetComponentInChildren<BasicTowerScript>().GetCost())
                {
                    selectedTile.GetComponent<TileScript>().UpgradeTower();
                    AddMoneyAmount(-tower.GetComponentInChildren<BasicTowerScript>().GetCost());
                }
                break;

            case "Minigun":
                if (money >= tower.GetComponentInChildren<MachineGunTowerScript>().GetCost())
                {
                    selectedTile.GetComponent<TileScript>().UpgradeTower();
                    AddMoneyAmount(-tower.GetComponentInChildren<MachineGunTowerScript>().GetCost());
                }
                break;
            case "Railgun":
                if (money >= tower.GetComponentInChildren<RailgunScript>().GetCost())
                {
                    selectedTile.GetComponent<TileScript>().UpgradeTower();
                    AddMoneyAmount(-tower.GetComponentInChildren<RailgunScript>().GetCost());
                }
                break;
            case "MissileLauncher":
                if (money >= tower.GetComponentInChildren<RocketLauncherScript>().GetCost())
                {
                    selectedTile.GetComponent<TileScript>().UpgradeTower();
                    AddMoneyAmount(-tower.GetComponentInChildren<RocketLauncherScript>().GetCost());
                }
                break;
            case "Artillery":
                if (money >= tower.GetComponentInChildren<ArtilleryScript>().GetCost())
                {
                    selectedTile.GetComponent<TileScript>().GetTower().GetComponentInChildren<ArtilleryScript>().DestroyTarget();
                    selectedTile.GetComponent<TileScript>().UpgradeTower();
                    selectedArty = selectedTile.GetComponent<TileScript>().GetTower().GetComponentInChildren<ArtilleryScript>().gameObject;
                    AddMoneyAmount(-tower.GetComponentInChildren<ArtilleryScript>().GetCost());
                }
                break;
        }

        UpdateUpgradeMenuText(selectedTile.GetComponent<TileScript>().GetTower());
    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> raysastResults = new();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }


    private void LateUpdate()
    {
        Vector3 dir = speed * Time.deltaTime * new Vector3(horizontalInput, 0, verticalInput);
        rb.AddForce(dir, ForceMode.Impulse);
    }

    private void UpdateUpgradeMenuText(GameObject tower)
    {
        switch (tower.tag)
        {
            case "Cannon":
                var towerScript = tower.GetComponentInChildren<BasicTowerScript>();

                towerNameLabel.text = "Cannon";

                towerCostUpgradeLabel.text = "Cost Upgrade: " + towerScript.GetCost();
                towerDamageLabel.text = "Damage:  " + towerScript.GetDamageOverride();
                towerFireRateLabel.text = "Firerate:" + towerScript.GetFireRate();
                towerLevelLabel.text = "Tower Level: " + towerScript.GetTowerLevel();
                towerRangeLabel.text = "Range: " + towerScript.GetRange();
                break;

            case "Minigun":
                var towerScript2 = tower.GetComponentInChildren<MachineGunTowerScript>();

                towerNameLabel.text = "Machine Gun";

                towerCostUpgradeLabel.text = "Cost Upgrade: " + towerScript2.GetCost();
                towerDamageLabel.text = "Damage:  " + towerScript2.GetDamageOverride();
                towerFireRateLabel.text = "Firerate:" + towerScript2.GetFireRate();
                towerLevelLabel.text = "Tower Level: " + towerScript2.GetTowerLevel();
                towerRangeLabel.text = "Range: " + towerScript2.GetRange();
                break;

            case "Railgun":
                var towerScript3 = tower.GetComponentInChildren<RailgunScript>();

                towerNameLabel.text = "Railgun";

                towerCostUpgradeLabel.text = "Cost Upgrade: " + towerScript3.GetCost();
                towerDamageLabel.text = "Damage:  " + towerScript3.GetDamageOverride();
                towerFireRateLabel.text = "Firerate:" + towerScript3.GetFireRate();
                towerLevelLabel.text = "Tower Level: " + towerScript3.GetTowerLevel();
                towerRangeLabel.text = "Range: " + towerScript3.GetRange();
                break;

            case "MissileLauncher":
                var towerScript4 = tower.GetComponentInChildren<RocketLauncherScript>();

                towerNameLabel.text = "Missile Launcher";

                towerCostUpgradeLabel.text = "Cost Upgrade: " + towerScript4.GetCost();
                towerDamageLabel.text = "Damage:  " + towerScript4.GetDamageOverride();
                towerFireRateLabel.text = "Firerate:" + towerScript4.GetFireRate();
                towerLevelLabel.text = "Tower Level: " + towerScript4.GetTowerLevel();
                towerRangeLabel.text = "Range: " + towerScript4.GetRange();
                break;

            case "Artillery":
                var towerScript5 = tower.GetComponentInChildren<ArtilleryScript>();

                towerNameLabel.text = "Artillery";

                towerCostUpgradeLabel.text = "Cost Upgrade: " + towerScript5.GetCost();
                towerDamageLabel.text = "Damage:  " + towerScript5.GetDamageOverride();
                towerFireRateLabel.text = "Firerate:" + towerScript5.GetFireRate();
                towerLevelLabel.text = "Tower Level: " + towerScript5.GetTowerLevel();
                towerRangeLabel.text = "Range: Whole Map";
                break;
        }
    }

    public void SetSelectedTile(GameObject tile)
    {
        this.selectedTile = tile;
    }

    public GameObject GetSelectedTile()
    {
        return this.selectedTile;
    }

    public bool GetBuildMode()
    {
        return buildMode;
    }

    public void SetSelectedTowerID(int id)
    {
        this.selectedTowerID = id;
    }

    public int GetSelectedTowerID()
    {
        return selectedTowerID;
    }

    public int GetMoneyAmount()
    {
        return money;
    }

    public void AddMoneyAmount(int add)
    {
        money += add;
    }

    public void SetArtySelectionMode(bool newMode)
    {
        this.artySelectionMode = newMode;
        if (selectedArty != null)
        {
            selectedArty.GetComponent<ArtilleryScript>().SetEditMode(newMode);
        }
    }

    public bool GetUpgradeMenuOpen()
    {
        return upgradeMenuOpen;
    }
}