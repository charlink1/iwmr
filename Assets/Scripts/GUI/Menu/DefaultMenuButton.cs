using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMenuButton : MonoBehaviour {

    [SerializeField]
    GUIController guiController;

    protected Menu parent;

    protected bool defaultLoading = true;

    protected virtual void Start()
    {
        parent = transform.parent.GetComponent<Menu>();
        if(parent == null)
            parent = transform.root.GetComponentInChildren<Menu>();
    }
    public virtual void CheckToPlaySound()
    {
        if (defaultLoading)
            defaultLoading = false;
        else
        {
            if(guiController!= null)
                guiController.PlayItemSound(SoundType.ITEM_SELECTED);
        }
    }

    protected void OnDisable()
    {
        defaultLoading = true;
    }

    public virtual void CheckToPlaySoundInList(bool battle = false)
    {
        if (parent.IsLoading)
            parent.IsLoading = false;
        else
        {
            parent.PlayItemSelectedSound(battle);
           
        }
    }
}
