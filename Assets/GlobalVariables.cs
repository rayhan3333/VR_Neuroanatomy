using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {

    public bool is3d;
    public bool is2d;
    public bool Annot3d;
    public bool Annot2d;
    public bool isLabel;
    public bool sliceView;
    public void Start() { }
    public void Update() { }

    public void ActivateIs3d()
    {
        is3d = true;
        is2d = false;
        Annot3d = false;
        Annot2d = false;
        isLabel = false;
    }
    public void DeactivateIs3d()
    {
        is3d = false;
        is2d = true;
    }
    public void ActivateIs2d()
    {
        is3d = false;
        is2d = true;
    }
    public void DeactivateIs2d()
    {
        is2d = false;
        is3d=true;
    }

    public void Activate2dAnnot()
    {
        is3d = false;
        is2d = true;
        Annot2d = true;
        Annot3d = false;
    }
    public void Deactivate2dAnnot()
    {
        Annot2d = false;
    }


    public void Activate3dAnnot()
    {
        Annot3d = true;
        Annot2d = false;
        is3d = false;
        is2d = true;
    }
    public void Deactivate3dAnnot()
    {
        Annot3d = false;
    }

    public void ActivateLabel()
    {
        isLabel = true;
        is3d = false;
        is2d = true;
    }
    public void DeactivateLabel()
    {
        isLabel = false;
    }

    public void ToggleSliceView()
    {
        sliceView = !sliceView;
    }
}
