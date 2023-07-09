using System.Collections;
using UnityEngine;
using DG.Tweening;
public class Mouse : MonoBehaviour
{
    private bool _keyFlag = false;                  //false�Ō����������
    private bool _destinationarea = false;          //�ړ���̔���true�ňړ��\
    private bool _damage = false;                   //true�̏ꍇ���S�������s��
    private bool isinput = false;                   //���͂��ꂽ���A�̍s�����I���܂ł͏������Ȃ�(�f�o�b�O�p)
    private int _rotateCount = 0;                   //��]������
    [SerializeField] private float runSpead = 0;    //�ړ����x
    [SerializeField] private float rotateSpead = 0; //��]���x
    private Quaternion startQuaternion;             //���g�̊p�x
    private Vector3 startPos;                       //�������W
    private Vector3 mousePos;                       //���ݍ��W
    private Vector3 destinationPos;                 //�ړ���̍��W
    [HideInInspector]public Vector3 MousePosition;  //�󂯓n���p�̃}�E�X�̍��W

    private StageManager stageManager;
    private Animator anim;

    public bool debugmode = true;
    public Mouse(Vector3 pos, float rotation)
    {
        startPos = pos;//�X�^�[�g���̎��g�̍��W��ێ�����
        mousePos = new Vector3(startPos.x, 0, startPos.z - 1);//���W������Z���W��-1���Ă���
        destinationPos = mousePos;//�ړ���̍��W�����݂̍��W�ɐݒ�

        transform.rotation = Quaternion.EulerRotation(0, rotation, 0);
        startQuaternion = transform.rotation;
    }
    void Start()
    {
        stageManager = StageManager.Instance;
        anim = GetComponent<Animator>();
        //�f�o�b�O�p
        //startPos = transform.position;//�X�^�[�g���̎��g�̍��W��ێ�����
        //startPos = Vector3.zero;
        startPos = transform.position;
        mousePos = new Vector3(startPos.x, 0, startPos.z-1);//���W������Z���W��-1���Ă���
        MousePosition = mousePos;
        destinationPos = mousePos;//�ړ���̍��W�����݂̍��W�ɐݒ�

        transform.rotation = Quaternion.EulerRotation(0, transform.rotation.y, 0);
        startQuaternion = transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isinput&&debugmode)
        {
            //�f�o�b�O�p
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                MouseAct();
            }
        }
    }
    public void MouseAct()//�}�E�X�̈ړ��Ɖ�]
    {
        isinput = true;
        _destinationarea = false;
        anim.SetTrigger("Run");
        //�����̂���}�X�̃C�x���g������
        AreaEvent(stageManager.GetStageObjectType((int)mousePos.x, (int)mousePos.z), true);
        //�ړ���̒T���ƈړ�
        MouseMove();

        //�^�[���I���p�̊֐����Ăяo��
        
    }
    private void RouteSearch()//�ړ���̌���
    {
        switch (transform.localEulerAngles.y)
        {
            case 0:
                destinationPos.z += 1;
                break;
            case 90:
                destinationPos.x += 1;
                break;
            case 180:
                destinationPos.z -= 1;
                break;
            case 270:
                destinationPos.x -= 1;
                break;
            default:
                Debug.LogWarning("�p�x���s���Ȓl�ł��B0�x�ɏC�����܂�");
                transform.Rotate(0, 0, 0);
                break;
        }
    }
    private void MouseMove()
    {
        destinationPos = mousePos;
        RouteSearch();
        if ((destinationPos.x >= 0 && destinationPos.x <= 5) && (destinationPos.z <= 5 && destinationPos.z >= 0))
        {
            AreaEvent(stageManager.GetStageObjectType((int)destinationPos.x, (int)destinationPos.z), false);
            if (_destinationarea && _rotateCount < 4)
            {
                var dif = destinationPos - mousePos; //���ݍ��W�ƈړ���̍�
                for (int i = 0; i < 5; i++)//������̈ړ��ł���}�X����������
                {                  
                    dif = destinationPos - mousePos;
                    if (!_destinationarea || stageManager.GetStageObjectType((int)destinationPos.x, (int)destinationPos.z) == null)
                    {
                        if (dif.x != 0)//x�����Ɉړ�����Ƃ�
                        {
                            dif.x -= Mathf.Sign(dif.x);
                        }
                        else if (dif.z != 0)//z�����Ɉړ�����Ƃ�
                        {
                            dif.z -= Mathf.Sign(dif.z);
                        }
                        break;
                    }
                    else if(_damage&&_destinationarea)
                    {
                        Debug.Log("���S");
                        break;
                    }
                    RouteSearch();
                    if (destinationPos.z < 0 || destinationPos.z > 5 || destinationPos.x < 0 || destinationPos.x > 5)
                    {
                        if (dif.x != 0)//x�����Ɉړ�����Ƃ�
                        {
                            dif.x = Mathf.Sign(dif.x) *(Mathf.Abs(dif.x));
                        }
                        else if (dif.z != 0)//z�����Ɉړ�����Ƃ�
                        {
                            dif.z = Mathf.Sign(dif.z) *(Mathf.Abs(dif.z));
                        }
                        break;
                    }
                    AreaEvent(stageManager.GetStageObjectType((int)destinationPos.x, (int)destinationPos.z), false);//�ړ���̃}�X�̎�ނ��m�F
                }                
                transform.DOMove(dif, runSpead).SetEase(Ease.InOutCubic).SetRelative(true).OnComplete(() =>//���݂̍��W�ƖړI�̍��W���r���A���������Ĉړ�������
                {
                    mousePos += dif;//���W���X�V
                    MousePosition = mousePos;
                    AreaEvent(stageManager.GetStageObjectType((int)mousePos.x, (int)mousePos.z), true);
                    _destinationarea = false;
                    isinput = false;
                });               
                _rotateCount = 0;//��]�̉񐔃��Z�b�g
            }
            else if (_rotateCount >= 4)
            {
                Debug.Log("RotateCount" + _rotateCount);
                anim.SetTrigger("Idle");
            }
            else
            {
                transform.DORotate(Vector3.up * 90f, rotateSpead, mode: RotateMode.WorldAxisAdd).OnComplete(() =>//���ʂ̃}�X��type���ړ��s�Ȃ�90�x��]�����Ă��璼�i
                {
                    destinationPos = mousePos;//�ړ���̍��W�����Z�b�g
                    _rotateCount++;
                    MouseMove();
                });
            }
        }
        else
        {
            transform.DORotate(Vector3.up * 90f, rotateSpead, mode: RotateMode.WorldAxisAdd).OnComplete(() =>//�ړ��悪�z��O�Ȃ�90�x��]�����Ă��璼�i
            {
                destinationPos = mousePos;//�ړ���̍��W�����Z�b�g
                _rotateCount++;
                MouseMove();
            });
        }
        anim.SetTrigger("Idle");
    }
    public void Reset()//���W��p�x�y�ь��̏�����Ԃ����Z�b�g������
    {
        gameObject.SetActive(true);
        transform.position = startPos;
        transform.rotation = startQuaternion;
    }
    private void KeyGet()
    {
        if(_keyFlag)
        {
            Debug.Log("���擾�ς�");
        }
        _keyFlag = true;

    }
    //���S���̏���
    public IEnumerator Death()
    {
        anim.SetTrigger("Death");
        _keyFlag = false;
        _damage = false;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    //���ꂼ��̃}�X���Ƃ̏������s���B�}�E�X�̍��W�Ȃ�true�A�ړ���̍��W�Ƃ��Ďg�p����Ȃ�false
    private void AreaEvent(StageObjectType? stagetype, bool mouse)
    {
        var pos = transform.position;
        var obj = transform.transform;
        if (mouse)
        {
            pos = mousePos;
        }
        else
        {
            pos = destinationPos;
        }
        if (!debugmode)
        {
            obj = GameObject.Find($"[{Mathf.Abs(pos.z - 6)},{pos.x}]").transform.GetChild(0);
        }
        if (stagetype == null) {
            Debug.LogWarning("AreaEvent is null");
            return; }
        if(mouse)
        {
            Debug.Log("mouse:"+stagetype);
        }
        else
        {
            Debug.Log("destination:"+stagetype);
        }
        switch (stagetype)
        {
            case StageObjectType.Magma:
                if (mouse)
                {
                    StartCoroutine(Death());
                }
                else
                {
                    _destinationarea = true;
                    _damage = true;
                }

                break;
            case StageObjectType.Grassland:
                if (!mouse)
                {
                    _destinationarea = true;
                }
                break;
            case StageObjectType.Wood:
                if (mouse)
                {
                    Debug.LogError("���̃G���A�͐i���ł��܂���");
                    Reset();
                }
                else
                {
                    _destinationarea = false;
                }
                break;
            case StageObjectType.Monster://�΂�����Ȃ�ړ��\�ɂ���
                if (mouse)
                {
                    StartCoroutine(Death());

                    //�e�I�u�W�F�N�g�̓��e���X�V���ꂽ��폜����
                    //if (obj.GetComponent<BaseStageObject>().isValidMove())
                    //{
                    //    StartCoroutine(Death());
                    //}
                    //else
                    //{
                    //    Debug.Log("Monster�͓|�ꂽ");
                    //}
                }
                else
                {
                    _destinationarea = true;
                    _damage = true;
                    //�e�I�u�W�F�N�g�̓��e���X�V���ꂽ��폜����
                    //if (!obj.GetComponent<BaseStageObject>().isValidMove())
                    //{
                    //    _destinationarea = true;
                    //    _damage = true;
                    //}
                    //else
                    //{
                    //    _destinationarea = true;
                    //    _damage = false;
                    //}
                }
                break;
            case StageObjectType.Ice://�΂�����Ȃ�ړ��\
                if (mouse)
                {
                    Debug.LogError("���̃G���A�͐i���ł��܂���");
                    Reset();
                }
                else
                {
                    _destinationarea = false;
                }

                break;
            case StageObjectType.Pond://�X������Ȃ�ړ��\
                if (mouse)
                {
                    StartCoroutine(Death());
                    //�e�I�u�W�F�N�g�̓��e���X�V���ꂽ��폜����
                    //if (obj.GetComponent<BaseStageObject>().isValidMove())
                    //{
                    //    StartCoroutine(Death());
                    //}
                    //else
                    //{
                    //    Debug.Log("�r�œM�ꂽ");
                    //}
                }
                else
                {
                    _destinationarea = true;
                    _damage = true;
                    //�e�I�u�W�F�N�g�̓��e���X�V���ꂽ��폜����
                    //if (!obj.GetComponent<BaseStageObject>().isValidMove())
                    //{
                    //    _destinationarea = true;
                    //    _damage = true;
                    //}
                    //else
                    //{
                    //    _destinationarea = true;
                    //    _damage = false;
                    //}
                }
                break;
            case StageObjectType.Abyss://��������Ȃ�ړ��\
                if (mouse)
                {
                    StartCoroutine(Death());
                    //�e�I�u�W�F�N�g�̓��e���X�V���ꂽ��폜����
                    //if (obj.GetComponent<BaseStageObject>().isValidMove())
                    //{
                    //    StartCoroutine(Death());
                    //}
                    //else
                    //{
                    //    Debug.Log("�ޗ���ʂ蔲����");
                    //}
                }
                else
                {
                    _destinationarea = true;
                    _damage = true;
                    //�e�I�u�W�F�N�g�̓��e���X�V���ꂽ��폜����
                    //if (!obj.GetComponent<BaseStageObject>().isValidMove())
                    //{
                    //    _destinationarea = true;
                    //    _damage = true;
                    //}
                    //else
                    //{
                    //    _destinationarea = true;
                    //    _damage = false;
                    //}
                }
                break;
            case StageObjectType.Flame://��������Ȃ�ړ��\
                if (mouse)
                {
                    StartCoroutine(Death());
                }
                else
                {
                    _destinationarea = true;
                    _damage = true;
                }
                break;
            case StageObjectType.Rock:
                if (mouse)
                {
                    Debug.LogError("���̃G���A�͐i���ł��܂���");
                    Reset();
                }
                else
                {
                    _destinationarea = false;
                }

                break;
            case StageObjectType.Key:
                if (mouse)
                {
                    KeyGet();
                    Debug.Log("KeyFlag:"+_keyFlag);
                }
                else
                {
                    KeyGet();
                    Debug.Log("KeyFlag:" + _keyFlag);
                    _destinationarea = true;
                }
                break;
            case StageObjectType.MagicCircle:
                if (_keyFlag && mouse)
                {
                    Debug.Log("�Q�[���N���A");
                }
                else if (!_keyFlag && mouse)
                {
                    Debug.Log("��������܂���");
                }
                else if (!mouse)
                {
                    _destinationarea = true;
                }

                break;
            case StageObjectType.Mouse:
                if (!mouse)
                {
                    _destinationarea = true;
                }
                break;

            case StageObjectType.None:
                if (!mouse)
                {
                    _destinationarea = true;
                }
                break;
            default:
                Debug.Log("����stagetype�͑��݂��܂���");
                if (!mouse)
                {    
                    _destinationarea = false; 
                }
                break;
        }
    }
}
