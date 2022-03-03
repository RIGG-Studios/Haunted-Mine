using UnityEngine;

public class MovementState : State
{
    private Vector2 movementInput = Vector2.zero;
    private Vector2 mouseDir = Vector2.zero;

    private Camera camera = null;
    private PlayerCamera cameraController = null;
    private MovementSettings settings = null;
    private Rigidbody2D physics = null;
    private PlayerFootsteps footSteps = null;
    private Animator animator = null;

    private float stepCooldown = 0.0f;

    public MovementState(Player player) : base("PlayerMovement", player) => this.player = player;

    public override void EnterState()
    {
        camera = Camera.main;
        settings = player.movementSettings;
        physics = player.GetComponent<Rigidbody2D>();
        footSteps = player.GetComponent<PlayerFootsteps>();
        animator = player.animator;

        if(camera != null)
        {
            cameraController = camera.GetComponent<PlayerCamera>();
        }

        cameraController.SetTarget(player.transform);
    }

    public override void UpdateInput()
    {
        Vector3 mousePos = camera.ScreenToWorldPoint(Utilites.GetMousePosition());
        mouseDir = (mousePos - player.GetPosition()).normalized;

        movementInput = player.playerInput.Player.Move.ReadValue<Vector2>();
    }

    public override void UpdateLogic()
    {
        bool isMoving = movementInput != Vector2.zero;

        if (isMoving && stepCooldown < 0f)
        {
            footSteps.PlayFootstep();
            stepCooldown = settings.stepRate / GetMovementSpeed() / 2f;
        }

        cameraController.SetOffset(mouseDir * settings.cameraOffset);
        animator.SetInteger("Direction", Utilites.DirectionToIndex(mouseDir, 4));

        stepCooldown -= Time.deltaTime;
    }

    public override void UpdatePhysics()
    {
        physics.MovePosition(physics.position + movementInput * GetMovementSpeed() * Time.fixedDeltaTime);
    }

    private float GetMovementSpeed()
    {
        float speed = settings.movementSpeed + player.paranoidManager.paranoidAmount * settings.movementParanoidMultiplier;

        return speed;
    }
}