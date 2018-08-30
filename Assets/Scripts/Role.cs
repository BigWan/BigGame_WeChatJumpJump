using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 跳一跳的小人
/// </summary>
public class Role : MonoBehaviour {

    [Header("Config Info")]

    /// <summary>
    /// 跳跃速度(按压一秒跳跃的距离)
    /// </summary>
    public float jumpSpeed = 10f;

    public float jumpHeight = 5f;

    public float enterHeight = 20f;

    public float perfercDistance = 0.25f;

    /// <summary>
    /// 跳跃的曲线控制(抛物线)
    /// </summary>
    public AnimationCurve jumpCurve;

    public AnimationCurve enterCurve;


    public GameObject jumpPuff;

    /// <summary>
    /// 按压最大压缩比(仅高度)
    /// </summary>
    public float maxPressRatio = 0.6f;

    public float jumpTime = 1f;

    public Transform body;



    [Header("Runtime Info")]
   
    public int perfectCount = 0;
    private float pressRatio = 1f;

    private bool isJumping;
    private bool isStandOnBlock;

    private bool isPerfect;

    private bool isRreadJump;

    public Block standBlock;
    public Block oldStandBlock;

    public UnityAction<int> ChangeBlockAction;

    private bool pressing;
    private float pressTime;

    private Vector3[] m_orientations = new Vector3[] {
        Vector3.forward,
        Vector3.left,
    };

    public void RoleReset() {
        jumpPuff.SetActive(false);
        
        perfectCount = 0;
        pressRatio = 1f;
        isJumping = false;
        isRreadJump = false;
        isStandOnBlock = false;
        standBlock = null;
        oldStandBlock = null;
        pressing = false;
        pressTime = 1f;
        isPerfect = false;
        transform.localPosition = Vector3.zero;
    }

    private void Awake() {
        RoleReset();
    }

    public void StartJump(Vector3 distance) {
        StartCoroutine(JumpCoroutine(distance));
        StartCoroutine(RatioRecover());
        if (standBlock != null)
            standBlock.StartRecover();
    }

    public IEnumerator RatioRecover() {
        for (int i = 0; i < 30; i++) {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.25f);
            
            yield return null;
        }
    }

    public IEnumerator JumpCoroutine(Vector3 offset) {
        isJumping = true;
        jumpPuff.SetActive(true);
        isStandOnBlock = false;
        Vector3 step = offset / 30f;
        for (int i = 0; i < 30; i++) {
            transform.localPosition += step;
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                jumpCurve.Evaluate(((float)i + 1f) / 30f) * jumpHeight,
                transform.localPosition.z
                );

            body.eulerAngles = new Vector3(-i * 12 - 12, 0, 0);
            yield return new WaitForFixedUpdate();
        }
        jumpPuff.SetActive(false);
        isJumping = false;
    }


    private float calcPressRatio(float pressTime) {
        return Mathf.Exp(-pressTime) * (1 - maxPressRatio) + maxPressRatio;
    }


    private void Update() {
        if (GameManager.Instance.gameStat != GameManager.GameStat.Playing)
            return;
        if (Input.GetKeyDown(KeyCode.Space)) {      
            if(isRreadJump&&isStandOnBlock && !isJumping) {
                pressing = true;
                pressTime = 0f;
                SoundManager.Instance.PlaySnapSound();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            if (!pressing) return;
            pressing = false;
            if (isRreadJump&&!isJumping && isStandOnBlock) {
                SoundManager.Instance.Stop();
                JumpByPressTime();
            }
            
        }

        if (pressing && isStandOnBlock && !isJumping) {
            pressTime += Time.deltaTime;
            pressRatio = calcPressRatio(pressTime);
            transform.localScale = new Vector3(1, pressRatio, 1);
            if (standBlock != null) {
                standBlock.transform.localScale = new Vector3(1, pressRatio, 1);
            }
            if (pressTime >= 2.22f) SoundManager.Instance.PlaySnapLoop();

        } 

    }


    private void JumpByPressTime() {
        float jumpLength = pressTime * jumpSpeed;
        Vector3 offset = Vector3.zero;

        if (standBlock != null) {

            MyUtil.Orientation o = standBlock.orientation;

            Vector3 direction = standBlock.direction;

            offset = direction * pressTime * jumpSpeed;

            // 当玩家位置不在方块的轴线上时,会根据方向进行对应坐标的纠偏
            if (jumpLength >= standBlock.distanceNext()) {
                if (o == MyUtil.Orientation.Forward) {
                    offset = new Vector3(
                        standBlock.transform.localPosition.x - transform.localPosition.x,
                        offset.y,
                        offset.z);
                } else if (o == MyUtil.Orientation.Left) {
                    offset = new Vector3(
                        offset.x,
                        offset.y,
                        standBlock.transform.localPosition.z - transform.localPosition.z);
                }
            }
        } else {
            offset = Vector3.forward;
        }
        StartJump(offset);
    }


    private IEnumerator EnterCoroutine() {
        float time = 0;
        while (time <= 1f) {
            transform.localPosition = enterHeight * Vector3.up * enterCurve.Evaluate(time);
            time += Time.deltaTime;
            yield return null;
        }

        isRreadJump = true;
    }


    public void StartEnter() {
        StartCoroutine(EnterCoroutine());
    }


    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Block")) {
            isStandOnBlock = true;
            oldStandBlock = standBlock;
            standBlock = collision.transform.GetComponent<Block>();

            if (standBlock == null)
                standBlock = collision.transform.GetComponentInParent<Block>();

            if (oldStandBlock != standBlock && oldStandBlock != null && standBlock != null) {
                isPerfect = (transform.position - standBlock.perfect.transform.position).magnitude < perfercDistance;
                perfectCount = isPerfect? perfectCount+1:0;
                ChangeBlockAction?.Invoke(perfectCount);
            }
        }

        if (collision.transform.CompareTag("Plane")) {
            GameManager.Instance.CheckGameOver();
        }
    }

    private void OnTriggerEnter(Collider other) {
        //if (other.CompareTag("Block")) {
        //    isStandOnBlock = true;
        //    oldStandBlock = standBlock;
        //    standBlock = other.GetComponent<Block>();
        //    if(standBlock==null)
        //        standBlock = other.GetComponentInParent<Block>();
        //    if (oldStandBlock != standBlock && oldStandBlock != null && standBlock != null) {
        //        ChangeBlockAction?.Invoke(isPerfect);
        //    }
        //}


        if (other.CompareTag("BlockPerfect")) {
            isPerfect = true;
        }
    }







    //private void OnTriggerExit(Collider other) {
    //    if (other.CompareTag("BlockPerfect"))
    //        isPerfect = false;
    //}
}
