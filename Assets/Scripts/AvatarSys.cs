using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSys : MonoBehaviour
{
    private static AvatarSys _instance;

    // 女孩对象
    private GameObject girlTarget;
    private Transform girlSourceTrans;
    // 小女孩所有的资源
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> girlData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();
    // 小女孩的骨骼信息
    Transform[] girlHips;
    // 身上的衣服
    private Dictionary<string, SkinnedMeshRenderer> girlSmr = new Dictionary<string, SkinnedMeshRenderer>();
    // 换装数组
    private string[,] girlStr = new string[,] { { "eyes", "1" }, { "hair", "1" }, { "top", "1" }, { "pants", "1" }, { "shoes", "1" }, { "face", "1" }, };

    // 男孩对象
    private GameObject boyTarget;
    private Transform boySourceTrans;
    // 男孩所有的资源
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> boyData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();
    // 男孩的骨骼信息
    Transform[] boyHips;
    // 身上的衣服
    private Dictionary<string, SkinnedMeshRenderer> boySmr = new Dictionary<string, SkinnedMeshRenderer>();
    // 换装数组
    private string[,] boyStr = new string[,] { { "eyes", "1" }, { "hair", "1" }, { "top", "1" }, { "pants", "1" }, { "shoes", "1" }, { "face", "1" }, };

    // 0女孩，1男孩
    public int nowCount = 0;
    public GameObject girlPanel;
    public GameObject boyPanel;

    public static AvatarSys Instance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
        // 不删除游戏物体
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        GirlAvatar();
        BoyAvatar();
        boyTarget.AddComponent<SpinWithMouse>();
        girlTarget.AddComponent<SpinWithMouse>();
        boyTarget.SetActive(false);
    }

    public void GirlAvatar()
    {
        InstantiateGirl();
        SaveData(girlSourceTrans, girlData, girlSmr, girlTarget);
        InitAvatarGirl();
    }

    public void BoyAvatar()
    {
        InstantiateBoy();
        SaveData(boySourceTrans, boyData, boySmr, boyTarget);
        InitAvatarBoy();
    }

    void InstantiateGirl()
    {
        GameObject girlSource = Instantiate(Resources.Load("FemaleModel")) as GameObject;
        girlSourceTrans = girlSource.transform;
        girlSource.SetActive(false);
        girlTarget = Instantiate(Resources.Load("FemaleTarget")) as GameObject;
        girlHips = girlTarget.GetComponentsInChildren<Transform>();
    }

    void InstantiateBoy()
    {
        GameObject boySource = Instantiate(Resources.Load("MaleModel")) as GameObject;
        boySourceTrans = boySource.transform;
        boySource.SetActive(false);
        boyTarget = Instantiate(Resources.Load("MaleTarget")) as GameObject;
        boyHips = boyTarget.GetComponentsInChildren<Transform>();
    }

    void SaveData(Transform source, Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> data, 
        Dictionary<string, SkinnedMeshRenderer> smr, GameObject target)
    {
        data.Clear();
        smr.Clear();
        if (source == null)
        {
            return;
        }

        // 遍历所有子物体
        SkinnedMeshRenderer[] parts = source.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var part in parts)
        {
            string[] names = part.name.Split('-');
            // 初始化
            if (!data.ContainsKey(names[0]))
            {
                // 生成对应的部位
                GameObject partGo = new GameObject();
                partGo.name = names[0];
                partGo.transform.parent = target.transform;
                // 目前穿的衣服
                smr.Add(names[0], partGo.AddComponent<SkinnedMeshRenderer>());
                // 所有可换衣服字典
                data.Add(names[0], new Dictionary<string, SkinnedMeshRenderer>());
            }
            // 便利存储
            data[names[0]].Add(names[1], part);
        }
    }

    // 传入部位与编号，从data里面拿取对应的skin
    void ChangeMesh(string part, string num, Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> avatarData,
        Dictionary<string, SkinnedMeshRenderer> avatarSmr, Transform[] avatarHips, string[,] str)
    {
        // 通过号码获取衣服
        SkinnedMeshRenderer skm = avatarData[part][num];
        // 匹配骨骼
        List<Transform> bones = new List<Transform>();
        foreach (var trans in skm.bones)
        {
            foreach (var bone in avatarHips)
            {
                if (bone.name == trans.name)
                {
                    bones.Add(bone);
                    break;
                }
            }
        }
        avatarSmr[part].bones = bones.ToArray();
        avatarSmr[part].materials = skm.materials;
        avatarSmr[part].sharedMesh = skm.sharedMesh;

        SaveData(part, num, str);
    }

    // 初始化让小人有材质和骨骼
    void InitAvatarGirl()
    {
        // 长度
        int length = girlStr.GetLength(0);
        // 穿衣
        for (int i = 0; i < length; i++)
        {
            ChangeMesh(girlStr[i, 0], girlStr[i, 1], girlData, girlSmr, girlHips, girlStr);
        }
    }

    // 初始化让小人有材质和骨骼
    void InitAvatarBoy()
    {
        // 长度
        int length = boyStr.GetLength(0);
        // 穿衣
        for (int i = 0; i < length; i++)
        {
            ChangeMesh(boyStr[i, 0], boyStr[i, 1], boyData, boySmr, boyHips, boyStr);
        }
    }

    public void OnChangePeople(string part, string num)
    {
        if (nowCount == 0)
        {
            // 女孩
            ChangeMesh(part, num, girlData, girlSmr, girlHips, girlStr);
        }
        else
        {
            // 女孩
            ChangeMesh(part, num, boyData, boySmr, boyHips, boyStr);
        }
    }

    // 性别转换，人物隐藏，面板隐藏
    public void SexChange(int sex)
    {
        // 男孩
        if (sex == 1)
        {
            nowCount = 1;
            boyTarget.SetActive(true);
            girlTarget.SetActive(false);
            boyPanel.SetActive(true);
            girlPanel.SetActive(false);
        }
        // 女孩
        else
        {
            nowCount = 0;
            boyTarget.SetActive(false);
            girlTarget.SetActive(true);
            boyPanel.SetActive(false);
            girlPanel.SetActive(true);
        }
    }

    void SaveData(string part, string num, string[,] str)
    {
        int length = str.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            if (str[i, 0] == part)
            {
                str[i, 1] = num;
            }
        }
    }
}
