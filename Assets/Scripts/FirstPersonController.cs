using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    
    [Header("Movement")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityScale;


    [Header("Ground Detection")]
    [SerializeField] private Transform feet;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask whatIsGround;

    private CharacterController controller;

    private bool isGrounded;
    

    private Vector2 inputVector; 
    private Vector3 horizontalMovement; 
    private Vector3 verticalMovement;
    private Vector3 totalMovement;

    private Camera cam;
    private PlayerInput input;
    
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        
        input = GetComponent<PlayerInput>(); //obtengo el componente player input
        
        //Bloquea el cursor en el centro de la pantalla y lo esconde.
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Awake -- On Enable -- Start
    private void OnEnable() //se hace cada vez que se activa el script, SUELE SER EL LUGAR CONVENIENTE EN EL CUAL SUSCRIBIRTE A EVENTOS
    {
        //.started (GetKeyDown) -- cuando empieza a hacerse.
        //.perform () -- en teclado no tiene sentido, es mas para joystick. Se realiza cuando hay cambios de valor.
        //.canceled (GetKeyUp) -- cuando se cancela la accion, la gran mayoria de veces se levanta la tecla.
        input.actions["Jump"].started += JumpStarted;
        input.actions["Move"].performed += UpdateMovement; //mantiene el valor del vector
        input.actions["Move"].canceled += UpdateMovement;
    }
    
    private void OnDisable()
    {
        input.actions["Jump"].started -= JumpStarted; //Ojo que hay que ponerlo en ambos.
        input.actions["Move"].performed -= UpdateMovement;
        input.actions["Move"].canceled -= UpdateMovement;
    }
    
    private void UpdateMovement(InputAction.CallbackContext ctx)
    {
        inputVector = ctx.ReadValue<Vector2>();
    }

    private void JumpStarted(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            Jump();
        }
    }

    private void Start()
    {
        
    }

    
    
    private void Jump()
    {
        //MRUA
        verticalMovement.y= Mathf.Sqrt(-2 * gravityScale * jumpHeight); //es la puñetera formula de la gravedad
    }


    void Update()
    {
        GroundCheck(); 
        ApplyGravity();
        MoveAndRotate();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void MoveAndRotate()
    {
        //roto al personaje completo en funcion de la camara
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0); 
        

        if (inputVector.sqrMagnitude > 0) //el jugador quiere moverse
        {
            //conversion de direccion al angulo + el angulo de la camara
            float angleToRotate = Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            
            //multiplicar un cuaternion por un vector es rotar el vector.
            horizontalMovement = (Quaternion.Euler(0, angleToRotate, 0) * Vector3.forward) * movementSpeed; 
            
        }
        else // si no quiere moverse
        {
            horizontalMovement = Vector3.zero;
        }
        
        totalMovement = horizontalMovement + verticalMovement;
        
        //solo hacer una unica llamada al controller.move por fotograma
        controller.Move(totalMovement * Time.deltaTime);
    }

    

    private void ApplyGravity()
    {
        if (isGrounded && verticalMovement.y < 0) //si esta en el suelo y vas en bajada -- asegurarme de que tocamos bien el suelo
        {
            verticalMovement.y = -2f; 
        }
        else
        {
            verticalMovement.y += gravityScale * Time.deltaTime; //cuando estoy en el aire se me aplica la gravedad
        }
    }

    private void GroundCheck()
    {
        if (Physics.CheckSphere(feet.position, detectionRadius, whatIsGround)) //crea un radio para mirar si estas tocando suelo
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(feet.position, detectionRadius);
    }
}
