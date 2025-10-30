using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PersonagemController : MonoBehaviour
{
    public Vector3 dirMoveDesejada;
    public float velRotDesejada = 5.5f;
    public Animator _animator;
    public float Speed;
    public Camera cam;

    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private float gravidade = 9f;
    [SerializeField]
    private float puloForca;
    [SerializeField]
    private bool armado;
    [SerializeField]
    private float dist;

    public static Transform alvo;

    public Camera camC;
    public CinemachineStateDrivenCamera vcam;
    public CinemachineStateDrivenCamera cmcv;
    [System.Obsolete]
    public CinemachineFreeLook mirandoCam, normalCam;

    public bool mirando;
    public float limite = 0;

    public GameObject riflePrefab, rifle;
    public Transform maoD;
    public Transform cabeca;
    public GameObject canvasMira;
    public Image miraImg;

    public float vida = 1000;

    public GameObject canvasVida;
    public Image vidaImg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Awake()
    {
        cmcv = Instantiate(vcam);
        GameObject canvasObj = Instantiate(canvasMira);
        if (canvasObj != null)
        {
            miraImg = canvasObj.transform.GetChild(0).GetComponent<Image>();
            miraImg.enabled = true;
        }

        cmcv.AnimatedTarget = _animator;
        if (cabeca != null) {
            print(cabeca);
        }
        mirandoCam = cmcv.transform.GetChild(1).GetComponent<CinemachineFreeLook>();
        mirandoCam.LookAt = cabeca;
        mirandoCam.Follow = cabeca;

        normalCam = cmcv.transform.GetChild(0).GetComponent<CinemachineFreeLook>();
        normalCam.Follow = transform;
        normalCam.LookAt = transform;

        riflePrefab.transform.parent = maoD;
        riflePrefab.transform.localPosition = new Vector3(0,0,0);

    }

    public void Dano()
    {
        vida -= 30;
    }

    public void AjustaPosArma(bool armado)
    {
        riflePrefab.SetActive(armado);
        riflePrefab.transform.parent = maoD;
        riflePrefab.transform.localPosition =new Vector3(0,0,0);
    }

    void Start()
    {
        armado = false;
        riflePrefab.SetActive(false);
        alvo = transform;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        MovimentoComOuSemArma();
        Pulo();
        Queda();
        Gravidade();

        if (Input.GetKeyDown(KeyCode.G) && !armado)
        {
            armado = true;
            _animator.SetLayerWeight(1, 1);
            _animator.SetTrigger("Arma");
            riflePrefab.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.F) && armado)
        {
            armado = false;
            _animator.SetTrigger("Desarma");
            
        }
        if (armado) { 
            if(Input.GetMouseButton(1))
            {
                mirando = true;
                _animator.SetLayerWeight(1, 0);
                _animator.SetBool("Mirando", mirando);
            } else
            {
                mirando = false;
                _animator.SetLayerWeight(1, 1);
                _animator.SetBool("Mirando", mirando);
            }
        } else
        {
            if(riflePrefab.activeSelf)
            {
                AjustaPosArma(armado);
            }
        }
    }

    [System.Obsolete]
    void MovimentoComOuSemArma()
    {
        MovimentoSimples();
        _animator.SetFloat(
            "Movimento", dirMoveDesejada.magnitude, 1f, 
            Time.deltaTime * 10);
    }

    [System.Obsolete]
    void MovimentoSimples()
    {
        if (_characterController.isGrounded)
        {
            var hori = Input.GetAxis("Horizontal");
            var vert = Input.GetAxis("Vertical");

            Vector3 frente = camC.transform.forward;
            Vector3 direita = camC.transform.right;

            frente.Normalize();
            direita.Normalize();

            dirMoveDesejada = frente * vert + direita * hori;
            dirMoveDesejada.y = 0;
            dirMoveDesejada *= Speed;

            if(!mirando)
            {
                DirecaoMoveFree(dirMoveDesejada);
            } else
            {
                _animator.SetFloat("INPUTX",hori, 0.01f, Time.deltaTime);
                _animator.SetFloat("INPUTY", vert, 0.01f, Time.deltaTime);
                DirecaoMoveArmado();
            }
        } else
        {
            _animator.SetFloat("Movimento", 0, 1f, Time.deltaTime * 10);
        }
    }

    void Queda()
    {
        if (!_characterController.isGrounded)
        {
            RaycastHit hit;
            if (!Physics.Raycast(
                transform.position,
                transform.TransformDirection(Vector3.down),
                out hit,
                dist))
            {
                if (armado)
                {
                    _animator.SetBool("NoArArmado", true);
                }
                else if (!armado)
                {
                    _animator.SetBool("NoAr", true);
                }
            }
        }
        else if (_characterController.isGrounded)
        {
            if (armado)
            {
                _animator.SetBool("NoArArmado", false);
            }
            else
            {
                _animator.SetBool("NoAr", false);
            }
        }
    }

    void Gravidade()
    {
       dirMoveDesejada.y -= gravidade * Time.deltaTime;
        _characterController.Move(dirMoveDesejada * Time.deltaTime);
    }

    void Pulo()
    {
        if (dirMoveDesejada.x != 0 || dirMoveDesejada.z != 0)
        {
            if (_characterController.isGrounded)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    dirMoveDesejada.y = puloForca;
                    _animator.SetTrigger("Pulo");
                }
            }
        }
    }

    void DirecaoMoveFree(Vector3 mov)
    {
        Quaternion rot;
        if(mov.magnitude > 0)
        {
            Quaternion novaDir = Quaternion.LookRotation(mov);
            rot = Quaternion.Slerp(transform.rotation, novaDir, 
                Time.deltaTime * velRotDesejada);
            transform.rotation = new Quaternion(0, rot.y, 0, rot.w);
        }
    }

    [System.Obsolete]
    void DirecaoMoveArmado()
    {
        if(mirando)
        {
            transform.eulerAngles = new Vector3(0, mirandoCam.m_XAxis.Value, 0);
        }else
        {
            transform.eulerAngles = new Vector3(0, normalCam.m_XAxis.Value, 0);
        }
    }
}
