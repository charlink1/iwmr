using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestBehaviour : MonoBehaviour {

    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    AudioSource openSound;

    [SerializeField]
    GameObject toysPrefab;

    SpriteRenderer spriteRenderer;
    Sequencer sequencer;

    public bool isAtBottom { get; set; }
    public bool buttonsDisabled
    {
        get; set;
    }

    public bool isDialogEnabled
    {
        get; set;
    }
   
    void Start()
    {
        isAtBottom = false;
        isDialogEnabled = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        sequencer = GameObject.FindWithTag("Sequencer").GetComponent<Sequencer>();
        buttonsDisabled = false;
        if (Game.chestEmpty)
        {
            Instantiate(toysPrefab);

            if (Game.chestPosition != Vector3.zero)
                transform.position = Game.chestPosition;
        }
    }

    public IEnumerator EmptyChest()
    {
        spriteRenderer.sprite = sprites[0];
        openSound.Play();
        yield return new WaitForSeconds(1);

        spriteRenderer.sprite = sprites[1];
        Instantiate(toysPrefab);
        yield return null;
        sequencer.PerformNextSequenceStep();
    }

    public IEnumerator DisabledDialog()
    {
        yield return null;
        isDialogEnabled = false;
    }

    public IEnumerator CloseChest()
    {
        
        spriteRenderer.sprite = sprites[2];
        Game.chestEmpty = true;
        yield return null;
        buttonsDisabled = false;
        GameManager.instance.canGetEncounter = true;
        sequencer.PerformNextSequenceStep();
    }



}
