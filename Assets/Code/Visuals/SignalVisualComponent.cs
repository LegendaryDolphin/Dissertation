using UnityEngine;
using UnityEngine.Rendering;

public class SignalVisualComponent : NodeVisualBase
{
    [SerializeField] protected GameObject bulbRenderers;

    protected GameObject operatingArrowObject;
    protected SpriteRenderer operatingArrowRenderer;

    protected Color red;
    protected Color green;
    protected Color yellow;
    protected Color off;
    protected Color grey;
    protected Color error;

    private void Awake()
    {
        red = new Color(1, 0, 0, 1);
        green = new Color(0, 1, 0, 1);
        yellow = new Color(1, 0.8101983f, 0, 1);
        off = new Color(0, 0, 0, 1);
        grey = new Color(0.75f, 0.75f, 0.75f, 1);
        error = new Color(1, 0, 1, 1);
    }

    public override void SetNodeVisualComponent()
    {
        base.SetNodeVisualComponent();

        operatingArrowObject = new GameObject("OperatingArrow");
        operatingArrowObject.transform.SetParent(transform, false);

        operatingArrowRenderer = operatingArrowObject.AddComponent<SpriteRenderer>();
        operatingArrowRenderer.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        operatingArrowRenderer.sprite = Resources.Load<Sprite>("Sprites/OperatingArrowSprite");
        operatingArrowRenderer.color = grey;
    }

    public void SetArrow(SignalData data)
    {
        if(data.signalDirection == NodeOperatingDirection.up)
        {
            operatingArrowObject.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            operatingArrowObject.transform.position += new Vector3(0.13f, 0.24f, 0f);
        }
        else
        {
            operatingArrowObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            operatingArrowObject.transform.position += new Vector3(-0.13f, 0.24f, 0f);
        }
    }

    public virtual void SetAspect(SignalAspect aspect)
    {
        Debug.LogError("Implementation does not exist!");
    }
}