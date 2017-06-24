using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Stat 
{
    [SerializeField]
    private Bar bar;
    [SerializeField]
    private float max;
    [SerializeField]
    private float current;

    public float CurrentVal
    {
        get
        {
            return current;
        }

        set
        {
            this.current = value;
            bar.Value = current;
        }

    }

    public float MaxVal
    {
        get
        {

            return max;
        }
        set
        {
            this.max = value;
            bar.MaxValue = max;
        }
    }

    public void Initialize()
    {
        this.MaxVal = max;
        this.CurrentVal = current;
    }

 }
