using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MapGen : MonoBehaviour
{
    [SerializeField] GameObject[] Helps;
    [SerializeField] public int lvls = 1;
    CameraMove cam;
    [Header("Trash")]
    [SerializeField] GameObject AdIcon;
    [SerializeField] Text TrashText;
    [SerializeField] Image TrashBar;
    [SerializeField] int WorldTrash = 0;
    [SerializeField] int CurrentTrash = 0;

    [Header("Trees")]
    [SerializeField] Text TreeText;
    [SerializeField] Image TreeBar;
    [SerializeField] int WorldTrees = 0;
    [SerializeField] int CurrentTrees = 0;

    [Header("Secondary")]
    [SerializeField] Text SecondText;
    [SerializeField] Image SecondBar;
    [SerializeField] int WorldSecond = 0;
    [SerializeField] int CurrentSecond = 0;

    [Header("Robots")]
    [SerializeField] Text RobotText;
    [SerializeField] Image RobotBar;
    [SerializeField] int WorldRobot = 0;
    [SerializeField] int CurrentRobots = 0;

    [Header("Upgrades")]
    [SerializeField] GameObject MoneyU;
    [SerializeField] GameObject BackU;
    [SerializeField] GameObject PickU;
    [SerializeField] GameObject RunU;
    [SerializeField] GameObject HatsU;

    [Header("Misc")]
    [SerializeField] List<GameObject> Trees = new List<GameObject>();
    [SerializeField] List<GameObject> Trash = new List<GameObject>();
    [SerializeField] public GameObject RottenTree;
    [SerializeField] GameObject Robot;
    [SerializeField] GameObject[] Tutors;
    public float[,] Map = new float[10, 10];

    public bool IsTrashDone()
    {
        if (CurrentTrash <= 0) return true;
        return false;
    }
    public bool IsTreesDone()
    {
        if (CurrentTrees >= WorldTrees) return true;
        return false;
    }
    public bool IsSecondDone()
    {
        if (CurrentSecond <= 0) return true;
        return false;
    }
    public bool IsRobotsDone()
    {
        if (CurrentRobots <= 0) return true;
        return false;
    }
    public void RobotMinus()
    {
        CurrentRobots--;
        CurrentTrash += 4;
    }
    public void RobotPlus()
    {
        CurrentRobots++;
        WorldRobot++;
    }

    public int GetKilledRobots()
    {
        return WorldRobot - CurrentRobots;
    }

    public int GetTrash()
    {
        return CurrentTrash;
    }
    public int GetTrees()
    {
        return CurrentTrees;
    }
    public int GetSecond()
    {
        return CurrentSecond;
    }
    public int GetWorldSecond()
    {
        return WorldSecond;
    }
    public int GetWorldTrees()
    {
        return WorldTrees;
    }
    public void SecondMinus()
    {
        CurrentSecond--;
    }
    public void SecondPlus()
    {
        CurrentSecond++;
        WorldSecond++;
    }
    public void WorldTrashPlus()
    {
        WorldTrash++;
    }
    public void TrashPlus()
    {
        CurrentTrash++;
    }
    public void TrashMinus()
    {
        CurrentTrash--;
    }
    public void WorldTrashMinus()
    {
        WorldTrash--;
    }
    public void TreePlus()
    {
        CurrentTrees++;
    }
    public void TreeMinus()
    {
        CurrentTrees--;
    }
    public GameObject GetRandomTrash()
    {
        return Trash[Random.Range(0, Trash.Count)];
    }
    public GameObject GetRandomTree()
    {
        return Trees[Random.Range(0, Trees.Count)];
    }

    void SaveObject(string tag, GameObject obj, int id, int cid)
    {
        PlayerPrefs.SetInt($"{tag}{cid}id", id);
        PlayerPrefs.SetFloat($"{tag}{cid}x", obj.transform.position.x);
        PlayerPrefs.SetFloat($"{tag}{cid}y", obj.transform.position.y);
        PlayerPrefs.SetFloat($"{tag}{cid}z", obj.transform.position.z);
        PlayerPrefs.SetFloat($"{tag}{cid}rx", obj.transform.rotation.x);
        PlayerPrefs.SetFloat($"{tag}{cid}ry", obj.transform.rotation.y);
        PlayerPrefs.SetFloat($"{tag}{cid}rz", obj.transform.rotation.z);
        PlayerPrefs.SetFloat($"{tag}{cid}rw", obj.transform.rotation.w);
    }
    private void OnApplicationQuit()
    {
        SaveAll();
    }
    private void OnApplicationFocus(bool Focus)
    {
        if(Focus == false)
            SaveAll();
    }

    public void CleanMap()
    {
        GameObject[] trash = GameObject.FindGameObjectsWithTag("Trash");
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        GameObject[] rotten = GameObject.FindGameObjectsWithTag("RottenTree");
        GameObject[] robot = GameObject.FindGameObjectsWithTag("Robot");
        foreach (GameObject obj in trash) Destroy(obj);
        foreach (GameObject obj in trees) Destroy(obj);
        foreach (GameObject obj in rotten) Destroy(obj);
        foreach (GameObject obj in robot) Destroy(obj);
        CurrentTrash = 0;
        WorldTrash = 0;
        CurrentTrees = 0;
        WorldTrees = 0;
        CurrentSecond = 0;
        WorldSecond = 0;
        WorldRobot = 0;
        CurrentRobots = 0;
        lvls++;
        if(lvls % 2 == 0)
            FindObjectOfType<RewardedAdsButton>().LoadBasicAd();
    }

    IEnumerator Regen()
    {
        //cam.Rot();
        //yield return new WaitForSeconds(4);
        CleanMap();
        GenMap();
        yield return new WaitForSeconds(0);
    }
    
    public void ActivateUpgrades()
    {
        if (lvls > 5) MoneyU.SetActive(true);
        else MoneyU.SetActive(false);

        if (lvls > 4) HatsU.SetActive(true);
        else HatsU.SetActive(false);

        if (lvls > 3) RunU.SetActive(true);
        else RunU.SetActive(false);

        if (lvls > 2) PickU.SetActive(true);
        else PickU.SetActive(false);

        if (lvls > 1) BackU.SetActive(true);
        else BackU.SetActive(false);
    }

    public void RegenMap()
    {
        StartCoroutine(Regen());
    }

    void SaveAll()
    {
        bool a = PlayerPrefs.HasKey("IsReTrashed");
        PlayerPrefs.DeleteAll();
        if (a) PlayerPrefs.SetInt("IsReTrashed", 1);
        PlayerPrefs.SetInt("Saved", 0); 
        PlayerPrefs.SetInt("WorldTrash", WorldTrash); 
        PlayerPrefs.SetInt("WorldTrees", WorldTrees);
        PlayerPrefs.SetInt("WorldSecond", WorldSecond);
        PlayerPrefs.SetInt("WorldRobot", WorldRobot);
        PlayerPrefs.SetInt("Levels", lvls);
        int id = 0;
        int gid = 0;
        GameObject[] trash = GameObject.FindGameObjectsWithTag("Trash");
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        GameObject[] rotten = GameObject.FindGameObjectsWithTag("RottenTree");
        GameObject[] robot = GameObject.FindGameObjectsWithTag("Robot");
        foreach (GameObject obj in trash)
        {
            for(int i = 0; i<Trash.Count; i++)
            {
                if (obj.name.Remove(obj.name.IndexOf("(Clone)")) == Trash[i].name)
                {
                    id = i;
                    break;
                }
            }
            SaveObject("Trash", obj, id, gid);
            gid++;
        }
        gid = 0;
        foreach (GameObject obj in trees)
        {
            for (int i = 0; i < Trees.Count; i++)
            {
                if (obj.name.Remove(obj.name.IndexOf("(Clone)")) == Trees[i].name)
                {
                    id = i;
                    break;
                }
            }
            SaveObject("Tree", obj, id, gid);
            gid++;
        }
        gid = 0;
        foreach (GameObject obj in rotten)
        {
            SaveObject("RottenTree", obj, 0, gid);
            gid++;
        }
        gid = 0;
        foreach (GameObject obj in robot)
        {
            SaveObject("Robot", obj, 0, gid);
            gid++;
        }
        PlayerPrefs.SetFloat("Offset", FindObjectOfType<dg_simpleCamFollow>().Offset);
        PlayerPrefs.Save();
        FindObjectOfType<BackScript>().Save();
        FindObjectOfType<HatsManager>().SaveHats();
    }

    public void Help(int i)
    {
        Helps[i].SetActive(true);
    }
    public void Help2(int i)
    {
        Helps[i].SetActive(false);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DisableTut()
    {
        Tutors[0].SetActive(false);
        Tutors[1].SetActive(false);
    }
    public void Restart2()
    {
        PlayerPrefs.DeleteAll();
        FindObjectOfType<BackScript>().Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RegenTrash()
    {
        GameObject[] trash = GameObject.FindGameObjectsWithTag("Trash"); foreach (GameObject obj in trash) Destroy(obj);
        int gid = 0;
        float deltax = Random.Range(0.0f, 0.9f);
        float deltay = Random.Range(0.0f, 0.9f); 
        for (int i = 0; i < Map.GetLength(0); i++)
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                Map[i, j] = Mathf.PerlinNoise(i + deltax, j + deltay);
            }
        for (int i = 1; i < Map.GetLength(0) - 1; i++)
            for (int j = 1; j < Map.GetLength(1) - 1; j++)
                if (Map[i, j] >= 0.3f)
                {
                    int b = Random.Range(1, 7);
                    for (int z = 0; z < b; z++)
                    {
                        int id = Random.Range(0, Trash.Count);
                        GameObject a = Instantiate(Trash[id], new Vector3((i + 1) * Random.Range(3.5f, 5.5f), 1 + 3.5f, (j + 1) * Random.Range(3.5f, 5.5f)), transform.rotation);
                        a.transform.Rotate(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));
                        SaveObject("Trash", a, id, gid);
                        gid++;
                    }
                }
        WorldTrash = gid;
        PlayerPrefs.SetInt("IsReTrashed", 1);
        PlayerPrefs.SetInt("WorldTrash", WorldTrash);
        CurrentTrash = WorldTrash;
        PlayerPrefs.Save();
        AdIcon.SetActive(false);
    }

    public void GenMap()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Saved", 0);
        FindObjectOfType<BackScript>().Save();
        if(lvls==1)
        {
            Tutors[0].SetActive(true);
            Tutors[1].SetActive(false);
        }
        else if (lvls == 2)
        {
            Tutors[0].SetActive(false);
            Tutors[1].SetActive(true);
        }
        else
        {
            Tutors[0].SetActive(false);
            Tutors[1].SetActive(false);
        }
        ActivateUpgrades();
        int gid = 0;
        float deltax = Random.Range(0.0f, 0.9f);
        float deltay = Random.Range(0.0f, 0.9f);
        float TreeDensity = Random.Range(0.4f, 1f);
        for (int i = 0; i < Map.GetLength(0); i++)
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                Map[i, j] = Mathf.PerlinNoise(i + deltax, j + deltay);
            }
        if (lvls % 15 == 0 && lvls > 0) TreeDensity = 2;
        for (int i = 0; i < Map.GetLength(0); i++)
            for (int j = 0; j < Map.GetLength(1) - 1; j++)
                if (Map[i, j] >= TreeDensity)
                {
                    if (i == 0 && j == 0) continue; 
                    if (i == 0 && j == 8) continue;
                    int id = Random.Range(0, Trees.Count);
                    GameObject a = Instantiate(Trees[id], new Vector3((i+1) *Random.Range(4.8f, 5.2f), 2.5f, (j+1) * Random.Range(4.8f, 5.2f)), transform.rotation);
                    a.transform.Rotate(0, Random.Range(0, 90), 0);
                    SaveObject("Tree", a, id, gid);
                    gid++;
                }
        for (int i = 0; i < Map.GetLength(0); i++)
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                Map[i, j] = Mathf.PerlinNoise(i + deltax, j + deltay);
            }
        CurrentTrees = gid;
        WorldTrees = Random.Range(CurrentTrees * 11/10, CurrentTrees * 3);
        if (WorldTrees < 80) WorldTrees = Random.Range(100, 200);
        PlayerPrefs.SetInt("WorldTrees", WorldTrees);
        //print(WorldTrees);
        //print(PlayerPrefs.GetInt("WorldTrees"));
        gid = 0;
        for (int i = 1; i < Map.GetLength(0)-1; i++)
            for (int j = 1; j < Map.GetLength(1)-1; j++)
                if (Map[i, j] >= 0.3f)
                {  
                    int b = Random.Range(1, lvls % 10) * lvls % 10;
                    for (int z = 0; z < b; z++)
                    {
                        int id = Random.Range(0, Trash.Count);
                        GameObject a = Instantiate(Trash[id], new Vector3((i+1) * Random.Range(3.5f, 5.5f), 1+ 3.5f, (j+1) * Random.Range(3.5f, 5.5f)), transform.rotation);
                        a.transform.Rotate(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));
                        SaveObject("Trash", a, id, gid);
                        gid++;
                    }
                }
        WorldTrash = gid;
        PlayerPrefs.SetInt("WorldTrash", WorldTrash); 
        CurrentTrash = WorldTrash;
        //доп задачи
        int rndcase = Random.Range(0, 2);
        if (lvls % 10 <= 1) rndcase = 0;
        if (lvls == 1) rndcase = -1;
        if (lvls == 2) rndcase = 1;
        if (lvls % 15 == 0 && lvls > 0) rndcase = 1;
        //print(rndcase);
        if (rndcase == 0)
        {
            gid = 0;
            float TreeDensity2 = Random.Range(0.5f, 0.9f);
            for (int i = 1; i < Map.GetLength(0) - 1; i++)
                for (int j = 1; j < Map.GetLength(1) - 1; j++) 
                    if (Map[i, j] < TreeDensity && Random.Range(0.0f, 1f) > TreeDensity2) 
                    {
                    if (i == 0 && j == 0) continue;
                    if (i == 0 && j == 8) continue;
                    GameObject a = Instantiate(RottenTree, new Vector3((i + 1) * Random.Range(4.8f, 5.2f), 2.5f, (j + 1) * Random.Range(4.8f, 5.2f)), transform.rotation);
                    a.transform.Rotate(0, Random.Range(0, 90), 0);
                    SaveObject("RottenTree", a, 0, gid);
                    gid++;
                }
            WorldSecond = gid;
            CurrentSecond = WorldSecond;
            PlayerPrefs.SetInt("WorldSecond", WorldSecond);
        }
        if(rndcase == 1)
        {
            int robots = 0, robotslimit = Random.Range(1, lvls % 10);
            if (lvls % 15 == 0) robotslimit = 15;
            gid = 0;
            for (int i = 1; i < Map.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < Map.GetLength(1) - 1; j++)
                    if (Map[i, j] < 0.5f)
                    {
                        if (i == 0 && j == 0) continue;
                        if (i == 0 && j == 8) continue;
                        GameObject a = Instantiate(Robot, new Vector3((i + 1) * Random.Range(4.8f, 5.2f), 2.5f, (j + 1) * Random.Range(4.8f, 5.2f)), transform.rotation);
                        a.transform.Rotate(0, Random.Range(0, 90), 0);
                        SaveObject("Robot", a, 0, gid);
                        robots++;
                        gid++;
                        if (robots > robotslimit) break;
                    }
                if (robots > robotslimit) break;
            }
            CurrentRobots = gid;
            WorldRobot = gid;
            PlayerPrefs.SetInt("WorldRobot", WorldRobot);
        }
        
        //доп задачи
        PlayerPrefs.Save();
    }
    void LoadObj(string tag)
    {
        int obj = 0;
        while (PlayerPrefs.HasKey($"{tag}{obj}id"))
        {
            Quaternion a = new Quaternion(PlayerPrefs.GetFloat($"{tag}{obj}rx"), PlayerPrefs.GetFloat($"{tag}{obj}ry"), PlayerPrefs.GetFloat($"{tag}{obj}rz"), PlayerPrefs.GetFloat($"{tag}{obj}rw"));
            GameObject objecct = null;
            switch(tag)
            {
                case "RottenTree":
                    CurrentSecond++;
                    objecct = RottenTree;
                    break;
                case "Tree":
                    CurrentTrees++;
                    objecct = Trees[PlayerPrefs.GetInt($"{tag}{obj}id")];
                    break;
                case "Trash":
                    CurrentTrash++;
                    objecct = Trash[PlayerPrefs.GetInt($"{tag}{obj}id")];
                    break;
                case "Robot":
                    objecct = Robot;
                    CurrentRobots++;
                    break;
            }
            Instantiate(objecct, new Vector3(PlayerPrefs.GetFloat($"{tag}{obj}x"), PlayerPrefs.GetFloat($"{tag}{obj}y"), PlayerPrefs.GetFloat($"{tag}{obj}z")), a);
            obj++;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        if (PlayerPrefs.HasKey("Saved"))
        {
            LoadObj("Tree");
            LoadObj("Trash");
            LoadObj("RottenTree");
            LoadObj("Robot");
            WorldTrash = PlayerPrefs.GetInt("WorldTrash");
            WorldTrees = PlayerPrefs.GetInt("WorldTrees"); 
            WorldSecond = PlayerPrefs.GetInt("WorldSecond");
            WorldRobot = PlayerPrefs.GetInt("WorldRobot");
            lvls = PlayerPrefs.GetInt("Levels"); if (lvls == 1)
            {
                Tutors[0].SetActive(true);
                Tutors[1].SetActive(false);
            }
            else if (lvls == 2)
            {
                Tutors[0].SetActive(false);
                Tutors[1].SetActive(true);
            }
            else
            {
                Tutors[0].SetActive(false);
                Tutors[1].SetActive(false);
            }
            ActivateUpgrades();
        }
        else
        {
            GenMap();
        }

        cam = FindObjectOfType<CameraMove>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentSecond > 0)
        {
            SecondBar.transform.parent.gameObject.SetActive(true);
            SecondBar.fillAmount = (float)(CurrentSecond) / (float)WorldSecond;
            SecondText.text = $"{CurrentSecond}/{WorldSecond}";
        }
        else SecondBar.transform.parent.gameObject.SetActive(false);
        if (CurrentTrees < WorldTrees)
        {
            TreeBar.transform.parent.gameObject.SetActive(true);
            TreeText.text = $"{CurrentTrees}/{WorldTrees}";
            TreeBar.fillAmount = (float)(CurrentTrees) / (float)WorldTrees;
        }
        else TreeBar.transform.parent.gameObject.SetActive(false);
        if (CurrentTrash > 0)
        {
            TrashBar.transform.parent.gameObject.SetActive(true);
            TrashText.text = $"{CurrentTrash}/{WorldTrash}";
            TrashBar.fillAmount = (float)(CurrentTrash) / (float)WorldTrash;
            AdIcon.SetActive(false);
        }
        else
        {
            TrashBar.transform.parent.gameObject.SetActive(false);
            if(!PlayerPrefs.HasKey("IsReTrashed")) AdIcon.SetActive(true);
        }
        if (CurrentRobots > 0)
        {
            RobotBar.transform.parent.gameObject.SetActive(true);
            RobotText.text = $"{CurrentRobots}/{WorldRobot}";
            RobotBar.fillAmount = (float)(CurrentRobots) / (float)WorldRobot;
        }
        else
        {
            RobotBar.transform.parent.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
            {
            CleanMap();
            }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GenMap();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            lvls += 100;
            print(lvls);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
