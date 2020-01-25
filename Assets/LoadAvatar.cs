using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAvatar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (AvatarSys.Instance().nowCount == 0)
        {
            AvatarSys.Instance().GirlAvatar();
        }
        else
        {
            AvatarSys.Instance().BoyAvatar();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
