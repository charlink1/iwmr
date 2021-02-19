using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterInWardrobe : MonoBehaviour {

    [SerializeField]
    Transform positionAngie;
    [SerializeField]
    int maxSeconds;
    [SerializeField]
    float offsetY = 0;
    [SerializeField]
    bool testDoor;
    [SerializeField]
    int testLayer;
    //[SerializeField]
    //float angiePositionY;
    [SerializeField]
    int renderOrder;

    int defaultWardrobeRendererOrder;

    Animator animator;
    WardrobeDoorTrigger doorTrigger;
    SpriteRenderer doorRenderer;
    SpriteRenderer wardrobeRenderer;

    float limitPositionY; 
    
	void Awake () {
        animator = GetComponentInChildren<Animator>();
        doorTrigger = GetComponentInChildren<WardrobeDoorTrigger>();
        doorRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        wardrobeRenderer = transform.GetComponent<SpriteRenderer>();
        if (doorRenderer != null)
            renderOrder = doorRenderer.sortingOrder;

        defaultWardrobeRendererOrder = wardrobeRenderer.sortingOrder;
        limitPositionY = transform.position.y + offsetY;
    }
	
	void Update()
    {
        if (testDoor)
        {
            HandleOpenDoorAnimation();
            testDoor = false;
        }

        if (positionAngie.position.y >= limitPositionY && wardrobeRenderer.sortingOrder != 4)
            wardrobeRenderer.sortingOrder = 4;
        else if (positionAngie.position.y < limitPositionY && wardrobeRenderer.sortingOrder != defaultWardrobeRendererOrder)
            wardrobeRenderer.sortingOrder = defaultWardrobeRendererOrder;
        //angiePositionY = positionAngie.position.y;
    }

    public IEnumerator ActivateSocko()
    {
        if (Game.sockoAppears && !Game.sockoDefeated && !doorTrigger.IsInside)
        {
            yield return new WaitForSeconds(Random.Range(1, maxSeconds + 1));
            HandleOpenDoorAnimation();
        }
    }

    private void HandleOpenDoorAnimation()
    {
        if (positionAngie.position.y < limitPositionY)
            doorRenderer.sortingOrder = 2;
        else
            doorRenderer.sortingOrder = renderOrder;

        testLayer = doorRenderer.sortingOrder;
        animator.SetTrigger("peekOutside");
    }
}
