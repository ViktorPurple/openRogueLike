using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    public Animator animator;
    private Text damageText;
    
    void OnEnable()
    {
        AnimatorClipInfo[] clipinfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipinfo[0].clip.length);
        damageText = animator.GetComponent<Text>();

    }

    public void setText(string text)
    {
        animator.GetComponent<Text>().text = text;
    }


    public void setColor(Color color)
    {
        animator.GetComponent<Text>().color = color;
    }


}
