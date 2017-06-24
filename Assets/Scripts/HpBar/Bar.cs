using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bar : MonoBehaviour {

    private float fillAmount;

    [SerializeField]
    private Image Content;

    [SerializeField]
    private float lerpSpeed;
    public float MaxValue { get; set; }

    public float Value
    {
        get
        {
            return Value;
        }
        set
        {
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

	
	// Update is called once per frame
	void Update() 
    {
        HandleBar();	
	}

    private void HandleBar()
    {   
        //fillAmount = GameManager.instance.playerFoodPoints / GameManager.instance.playerMaxFoodPoints;
        if (fillAmount != Content.fillAmount)
            Content.fillAmount = Mathf.Lerp(Content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
