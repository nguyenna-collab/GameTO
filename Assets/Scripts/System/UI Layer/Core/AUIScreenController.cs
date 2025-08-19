using System;
using Sirenix.OdinInspector;
using UnityEngine;

 public abstract class AUIScreenController : MonoBehaviour
{
    public abstract ScreenProperties BaseProperties { get; }

    [Header("Screen Settings")]
    public string ScreenID;

    [Header("Animation")]
    [SerializeField] protected ATransitionComponent transitionIn;
    [SerializeField] protected ATransitionComponent transitionOut;

    [Header("Interaction Control")]
    [SerializeField] protected bool blockInteractionDuringTransition = true;

    protected CanvasGroup canvasGroup;
    public bool IsVisible { get; private set; }
    protected bool IsTransitioning { get; private set; }

    public virtual void Show(Action onShowComplete = null)
    {
        gameObject.SetActive(true);            
        IsVisible = true;
        
        if (transitionIn != null)
        {
            IsTransitioning = true;
            if (blockInteractionDuringTransition)
            {
                BlockInteraction();
            }

            transitionIn.Animate(transform, () =>
            {
                IsTransitioning = false;
                if (blockInteractionDuringTransition)
                {
                    UnblockInteraction();
                }
                onShowComplete?.Invoke();
            });
        }
        else
        {
            onShowComplete?.Invoke();
        }
    }

    public virtual void Hide(Action onHideComplete = null)
    {
        if (transitionOut != null)
        {
            IsTransitioning = true;
            if (blockInteractionDuringTransition)
            {
                BlockInteraction();
            }
            
            transitionOut.Animate(transform, () =>
            {
                gameObject.SetActive(false);
                IsVisible = false;
                IsTransitioning = false;
                if (blockInteractionDuringTransition)
                {
                    UnblockInteraction();
                }
                onHideComplete?.Invoke();
            });
        }
        else
        {
            gameObject.SetActive(false);
            IsVisible = false;
            onHideComplete?.Invoke();
        }
    }

    protected virtual void BlockInteraction()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
        }
    }

    protected virtual void UnblockInteraction()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
        }
    }

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        if (string.IsNullOrEmpty(ScreenID))
        {
            Debug.LogWarning($"Screen {gameObject.name} has no ScreenID assigned!", gameObject);
        }
    }

    // The UIManager is now responsible for registration.
    // This Start method can be removed or used for other initialization.
    protected virtual void Start()
    {
    }

    // The UIManager should also handle unregistering if it destroys the object.
    // If screens are destroyed by other means, this might be needed, but for now, we'll let the UIManager handle it.
    protected virtual void OnDestroy()
    {
    }
}

public abstract class AUIScreenController<T> : AUIScreenController where T : class
{
    public override ScreenProperties BaseProperties => Properties as ScreenProperties;
    protected T Properties;
    protected bool PropertiesSet { get; private set; }

    public void SetProperties(T properties)
    {
        this.Properties = properties;
        PropertiesSet = true;
        OnPropertiesSet();
    }

    public T GetProperties()
    {
        return Properties;
    }

    public bool HasProperties()
    {
        return PropertiesSet && Properties != null;
    }

    protected virtual void OnPropertiesSet() { }

    // Override Show to handle properties
    public override void Show(Action onShowComplete = null)
    {
        base.Show(() =>
        {
            // If properties are set after showing, update the UI
            if (PropertiesSet)
            {
                OnPropertiesSet();
            }
            onShowComplete?.Invoke();
        });
    }
}