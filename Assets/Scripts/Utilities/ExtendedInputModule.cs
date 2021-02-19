using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ExtendedInputModule : BaseInputModule
{
    private float m_NextAction;
    private bool m_EnableNextAction =true;
  
    private string currentAxis = "";

    protected Vector2 m_PreviousMovement = Vector2.zero;

    protected ExtendedInputModule()
    { }

    [SerializeField]
    private string m_DPadHorizontal ="DPadHorizontal";

    [SerializeField]
    private string m_DPadVertical = "DPadVertical";

    [SerializeField]
    private string m_HorizontalAxis = "Horizontal";

    [SerializeField]
    private string m_VerticalAxis = "Vertical";

    [SerializeField]
    private float m_AxisDeadZone = 0.3f;

    [SerializeField]
    private string m_SubmitButton = "Jump";

    [SerializeField]
    private string m_CancelButton = "Cancel";

    [SerializeField]
    private float m_InputActionsPerSecond = 10;

    [SerializeField]
    private float m_RepeatDelay = 0.5f;

    public string horizontalAxis
    {
        get { return m_HorizontalAxis; }
        set { m_HorizontalAxis = value; }
    }

    public string verticalAxis
    {
        get { return m_VerticalAxis; }
        set { m_VerticalAxis = value; }
    }

    public string submitButton
    {
        get { return m_SubmitButton; }
        set { m_SubmitButton = value; }
    }

    public string cancelButton
    {
        get { return m_CancelButton; }
        set { m_CancelButton = value; }
    }

    public override bool ShouldActivateModule()
    {
        if (!base.ShouldActivateModule())
            return false;

        bool shouldActivate = false;
        shouldActivate |= Input.GetButtonDown(OSInputManager.GetPadMapping(m_SubmitButton));
        shouldActivate |= Input.GetButtonDown(OSInputManager.GetPadMapping(m_CancelButton));
        shouldActivate |= !Mathf.Approximately(Input.GetAxisRaw(m_HorizontalAxis), 0.0f);
        shouldActivate |= !Mathf.Approximately(Input.GetAxisRaw(m_VerticalAxis), 0.0f);
        shouldActivate |= !Mathf.Approximately(Input.GetAxisRaw(OSInputManager.GetPadMapping(m_DPadHorizontal)), 0.0f);
        shouldActivate |= !Mathf.Approximately(Input.GetAxisRaw(OSInputManager.GetPadMapping(m_DPadVertical)), 0.0f);
        return shouldActivate;
    }

    public override void ActivateModule()
    {
        base.ActivateModule();

        var baseEventData = GetBaseEventData();
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject, baseEventData);
    }

    public override void DeactivateModule()
    {
        base.DeactivateModule();

        var baseEventData = GetBaseEventData();
        eventSystem.SetSelectedGameObject(null, baseEventData);
    }

    public override void Process()
    {
        bool usedEvent = SendMoveEventToSelectedObject();

        if (!usedEvent)
            SendSubmitEventToSelectedObject();
    }

    /// <summary>
    /// Process submit keys.
    /// </summary>
    private bool SendSubmitEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        if (Input.GetButtonDown(OSInputManager.GetPadMapping(m_SubmitButton)))
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

        if (Input.GetButtonDown(OSInputManager.GetPadMapping(m_CancelButton)))
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
        return data.used;
    }

    private bool AllowMoveEventProcessing(float time)
    {
      
        bool allow = Input.GetButtonDown(m_HorizontalAxis);
        allow |= Input.GetButtonDown(m_VerticalAxis);
       
        allow |= (time > m_NextAction);
      
        return allow;
    }

    private Vector2 GetRawMoveVector()
    {
        Vector2 move = Vector2.zero;
        move.x = SelectHorizontalAxis();//Input.GetAxis(m_HorizontalAxis);

        move.y = SelectVerticalAxis(); //Input.GetAxis(m_VerticalAxis);

        if (Input.GetButtonDown(m_HorizontalAxis))
        {
            if (move.x < 0)
                move.x = -1f;
            if (move.x > 0)
                move.x = 1f;
        }
        if (Input.GetButtonDown(m_VerticalAxis))
        {
            if (move.y < 0)
                move.y = -1f;
            if (move.y > 0)
                move.y = 1f;
        }

        //   Debug.Log(move);
        if (m_PreviousMovement != move)
            m_PreviousMovement = move;
        return move;
    }

    //Check if the axis has been relased (Delay off for next action)
   bool CheckReleasedAxis()
    {
      if(Mathf.Abs(m_PreviousMovement.y) <= m_AxisDeadZone && Mathf.Abs(m_PreviousMovement.x) <= m_AxisDeadZone)
            return true;
        return false;
    }

    //Select if V-DPad or V-Axis is being used (only one of them allowed)
    private float SelectVerticalAxis()
    {
        float verticalAxis = Input.GetAxisRaw(m_VerticalAxis);
        float verticalDPad = Input.GetAxisRaw(OSInputManager.GetPadMapping(m_DPadVertical));
        if (verticalAxis != 0 && currentAxis != "DPad") {
            currentAxis = "Axis";
            return verticalAxis;
        }
        if (verticalDPad != 0 && currentAxis != "Axis")
        {
            currentAxis = "DPad";
            return verticalDPad;
        }

        if(Mathf.Abs(verticalAxis) < m_AxisDeadZone && verticalDPad == 0)
        {
            currentAxis = "";
        }
        return 0;
    }

    //Select if H-DPad or H-Axis is being used (only one of them allowed)
    private float SelectHorizontalAxis()
    {
        float horizontalAxis = Input.GetAxisRaw(m_HorizontalAxis);
        float horizontalDPad = Input.GetAxisRaw(OSInputManager.GetPadMapping(m_DPadHorizontal));
        if (horizontalAxis != 0 && currentAxis != "DPad")
        {
            currentAxis = "Axis";
            return horizontalAxis;
        }
        if (horizontalDPad != 0 && currentAxis != "Axis")
        {
            currentAxis = "DPad";
            return horizontalDPad;
        }

        if (Mathf.Abs(horizontalAxis) < m_AxisDeadZone && horizontalDPad == 0)
        {
            currentAxis = "";
        }
        return 0;
    }



    /// <summary>
    /// Process keyboard events.
    /// </summary>
    private bool SendMoveEventToSelectedObject()
    {

        if ( CheckReleasedAxis() && !m_EnableNextAction)
        {
            m_EnableNextAction = true;
            StopCoroutine("EnableNextEvent");
        }

        float time = Time.unscaledTime;
        if (!AllowMoveEventProcessing(time))
            return false;

        Vector2 movement = GetRawMoveVector();
        var axisEventData = GetAxisEventData(movement.x, movement.y, m_AxisDeadZone);
        if ((!Mathf.Approximately(axisEventData.moveVector.x, 0f)
            || !Mathf.Approximately(axisEventData.moveVector.y, 0f)) && m_EnableNextAction)
        {
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
            StartCoroutine("EnableNextEvent");
        }
        m_NextAction = time + 1f   / m_InputActionsPerSecond;
       
        return axisEventData.used;
    }

    //Timer to set navigation speed
    IEnumerator EnableNextEvent()
    {
        m_EnableNextAction = false;
        yield return new WaitForSeconds(m_RepeatDelay);
        m_EnableNextAction = true;

    }
}
