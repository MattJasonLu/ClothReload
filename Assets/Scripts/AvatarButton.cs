using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarButton : MonoBehaviour
{
    public void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            if (gameObject.name == "girl")
            {
                AvatarSys.Instance().SexChange(0);
                return;
            }
            else if (gameObject.name == "boy")
            {
                AvatarSys.Instance().SexChange(1);
                return;
            }
            string[] names = gameObject.name.Split('-');
            AvatarSys.Instance().OnChangePeople(names[0], names[1]);
            switch(names[0])
            {
                case "pants":
                    PlayAnimation("item_pants");
                    break;
                case "shoes":
                    PlayAnimation("item_boots");
                    break;
                case "top":
                    PlayAnimation("item_shirt");
                    break;
                default:
                    break;
            }
        }
    }

    public void PlayAnimation(string animName)
    {
        Animation anim = GameObject.FindWithTag("Player").GetComponent<Animation>();
        if (!anim.IsPlaying(animName))
        {
            anim.Play(animName);
            anim.PlayQueued("idle1");
        }
    }

    public void LoadScenes()
    {
        SceneManager.LoadScene(1);
    }
}
