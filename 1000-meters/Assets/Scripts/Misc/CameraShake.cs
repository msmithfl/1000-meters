using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Animator camAnim;
    
    public void CamShake()
    {
        int randNum = Random.Range(0, 3);
        if (randNum == 0)
        {
            camAnim.SetTrigger("shake");
        }
        else if (randNum == 1)
        {
            camAnim.SetTrigger("shake2");
        }
        else if (randNum == 2)
        {
            camAnim.SetTrigger("shake3");
        }
    }
}
