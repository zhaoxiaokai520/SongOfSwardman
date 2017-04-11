using UnityEngine;
using System;
using System.Collections;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class CondHideAttr : PropertyAttribute
{
    public string CondSrcField = "";
    public string CondSrcField2 = "";
    public bool HideInInspector = false;
    public bool Inverse = false;
    public bool privateHideCond = false;//hide field by external private condition

    // Use this for initialization

    public CondHideAttr(bool hideCondition)
    {
        this.CondSrcField = null;
        this.HideInInspector = hideCondition;
        this.Inverse = false;
        //this.privateHideCond = hideCondition;
    }

    public CondHideAttr(string conditionalSourceField)
    {
        this.CondSrcField = conditionalSourceField;
        this.HideInInspector = false;
        this.Inverse = false;
        //privateHideCond = false;
    }

    public CondHideAttr(string conditionalSourceField, bool hideInInspector)
    {
        this.CondSrcField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.Inverse = false;
        //this.privateHideCond = false;
    }

    public CondHideAttr(string conditionalSourceField, bool hideInInspector, bool inverse)
    {
        this.CondSrcField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.Inverse = inverse;
        //privateHideCond = false;
    }
}