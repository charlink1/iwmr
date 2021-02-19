using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoController : MonoBehaviour {

    protected Rigidbody2D rb;
   
    [SerializeField]
    protected float speed = 50f;
    [SerializeField]
    protected bool m_CanMove = false;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected Vector2 destination;
    protected Vector2 direction;

    protected bool moveByAnimation;

    protected Sequencer sequencer;
   

    public bool isWalking { get; set; }
    public bool canMove{ get; set; }


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sequencer = GameObject.FindWithTag("Sequencer").GetComponent<Sequencer>();
        spriteRenderer = GetComponent<SpriteRenderer>();


    }

    void FixedUpdate()
    {
        MoveByAnimation();
    }

    protected void MoveByAnimation()
    {
        if (moveByAnimation)
        {
            float input_x = direction.x;
            float input_y = direction.y;
            ChechIfPlayerIsWalking(input_x, input_y);
            rb.velocity = new Vector2(input_x, input_y).normalized * speed * Time.fixedDeltaTime;
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
            if (Vector2.Distance(currentPosition, destination) < 0.1)
            {
                moveByAnimation = false;
                ChechIfPlayerIsWalking(0, 0);
                rb.velocity = new Vector2(0, 0);
                sequencer.PerformNextSequenceStep();

            }

        }
    }

    public IEnumerator ChangeRenderOrder()
    {
        yield return null;
        spriteRenderer.sortingOrder = 2;
        sequencer.PerformNextSequenceStep();
    }

    public IEnumerator MoveToPosition(GameObject destinyPosition)
    {
        destination = new Vector2(destinyPosition.transform.position.x, destinyPosition.transform.position.y);
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        direction = (destination - currentPosition).normalized;
        moveByAnimation = true;

        yield return null;
       
    }

    protected virtual void ChechIfPlayerIsWalking(float input_x, float input_y)
    {
        bool hasInput = (Mathf.Abs(input_x) + Mathf.Abs(input_y)) > 0;
         if (hasInput)
            isWalking = hasInput;
        else
            isWalking = false;

        animator.SetBool("isWalking", isWalking);
        if (isWalking)
        {
            animator.SetFloat("x", input_x);
            animator.SetFloat("y", input_y);
        }

    }

    public void ApplyRootMotionFromAnimator(int apply)
    {
        bool var = false;
        if (apply == 1)
            var = true;
        animator.applyRootMotion = var;
    }

    public IEnumerator WaitASecond()
    {
        yield return new WaitForSeconds(1);
        sequencer.PerformNextSequenceStep();
    }

    public IEnumerator LookTarget(GameObject target)
    {
        yield return null;
        animator.SetBool("isWalking", false);

        Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        float input_x = 0, input_y = 0;
        float distanceX = Mathf.Abs(Mathf.Abs(targetPosition.x) - Mathf.Abs(position.x));
        float distancey = Mathf.Abs(Mathf.Abs(targetPosition.y) - Mathf.Abs(position.y));
        if (distanceX > distancey)
        {
            if (targetPosition.x > position.x)
                input_x = 1;
            else if (targetPosition.x < position.x)
                input_x = -1;
        }else
        {
            input_x = 0;
                 if (targetPosition.y > position.y)
                     input_y = 1;
                 else if (targetPosition.y < position.y)
                     input_y = -1;
        }
      
        animator.SetFloat("y", input_y);
        animator.SetFloat("x", input_x);

        if(sequencer.isOnSequence)
            sequencer.PerformNextSequenceStep();
    }

    public void SetPosition(string targetName)
    {
        GameObject target = GameObject.Find(targetName);
        transform.position = target.transform.position;
    }

    public IEnumerator ResetLookAt()
    {
        animator.SetFloat("x", -1);
        yield return null;
        sequencer.PerformNextSequenceStep();
    }

    public void NextSequencerStep()
    {
        if(sequencer.isOnSequence)
            sequencer.PerformNextSequenceStep();
    }
}
