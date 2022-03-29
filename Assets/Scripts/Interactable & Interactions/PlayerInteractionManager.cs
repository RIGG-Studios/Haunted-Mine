using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteractionManager : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayer;

    [SerializeField] private GameObject interactionAssist;
    [SerializeField] private TextMeshPro interactionName;
    [SerializeField] private TextMeshPro interactionType;

    [SerializeField] private float assistRadius = 0.0f;
    [SerializeField] private float minInteractionDistance = 0.0f;


    [HideInInspector]
    public InteractableObject hoveredObject;
    [HideInInspector]
    public bool allowInteractions;

    private Player player = null;
    private Camera camera = null;

    private void Awake()
    {
        player = GetComponent<Player>();
        camera = Camera.main;

        if(interactionAssist != null)
        {
            interactionAssist.transform.SetParent(null);
            interactionAssist.transform.position = new Vector3(150, 150, 0);
        }
    }

    private void Start()
    {
        player.playerInput.Player.Interact.performed += ctx => InteractWithObject();
        allowInteractions = true;
    }


    private void Update()
    {
        Vector3 mouseWorldSpace = camera.ScreenToWorldPoint(Utilites.GetMousePosition());

        RaycastHit2D spriteHit = Physics2D.Raycast(mouseWorldSpace, transform.position - mouseWorldSpace, minInteractionDistance, interactionLayer);
        /*/
        RaycastResult uiHit = Utilites.IsPointerOverUIElement();

        if (uiHit.gameObject != null)
        {
            OnHoverOverInteractable(uiHit.gameObject, Vector2.zero);
        }
        else if (uiHit.gameObject == null && hoveredObject != null && spriteHit.collider == null)
        {
            OnStopHoverInteractable();
        }
        /*/

        if (allowInteractions)
        {
            if (spriteHit.collider != null)
            {
                float dist = (spriteHit.collider.transform.position - transform.position).magnitude;

                if (dist < 2)
                    OnHoverOverInteractable(spriteHit.collider.gameObject, spriteHit.point);
            }
            else if (spriteHit.collider == null && hoveredObject != null)
            {
                OnStopHoverInteractable();
            }
        }
    }

    private void OnHoverOverInteractable(GameObject collision, Vector2 point)
    {
        if (hoveredObject != null)
            OnStopHoverInteractable();

        hoveredObject = collision.GetComponent<InteractableObject>();

        if (hoveredObject.useAssist)
        {
            interactionName.text = hoveredObject.interactionName;
            interactionType.text = hoveredObject.interactionType.ToString();
            interactionAssist.transform.position = point;
            interactionAssist.SetActive(true);
        }

        hoveredObject.HoverInteract();
    }

    private void OnStopHoverInteractable()
    {
        if (hoveredObject == null)
            return;

        hoveredObject.StopHoverInteract();
        interactionAssist.SetActive(false);
        hoveredObject = null;
    }

    public void InteractWithObject()
    {
        if (hoveredObject == null)
            return;

        if (!hoveredObject.open)
            hoveredObject.ButtonInteract();

        interactionAssist.SetActive(false);
    }
}