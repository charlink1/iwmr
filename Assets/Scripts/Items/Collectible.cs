using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    [SerializeField]
    protected bool destroyable;
    [SerializeField]
    protected Item item;

    public bool obtained { get; set; }

    void Start()
    {
        obtained = false;     
    }

    public virtual void ObtainItem()
    {
       obtained = true;
       item.AddItem(1);
       if (destroyable)
        Destroy(gameObject);
    }

}
