using DG.Tweening;
using DragonBones;
using System.Collections;
using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;
using System.Text;

public class TrainingArena_PlayerController : MonoBehaviour
{
    public TrainingArena_GM gameManager;
    public string SpawnedSetName;
    public int indexSpawnedTerrain;
    public string SpawnedTerrainName;

    public UnityEngine.Transform Player;
    public UnityEngine.Transform transformForLeftRaycast;
    public int Score, Energy;

    float TimerEnergyDecrease = 1;

    static TrainingArena_PlayerController PlayerController;

    public PlayerSoundController SoundController;
    BoxCollider PlayerBoxCollider;
    Rigidbody PlayerRigidbody;

    public bool isPlayerWoman;
    public int idCharSkin;
    public string CurrentCharSkin;

    bool CharSkinIdleForFirstTime;

    [System.Serializable]
    public class Animasi
    {
        public SkeletonAnimation CharSkin;
        public SpriteRenderer[] CharSkin_Back;
        public ReferensiAnimasi Girl;
        public ReferensiAnimasi Boy;
        public StringBuilder state;

        Vector3 flipX = new Vector3(0, 135, 0);
        Vector3 normal = new Vector3(0, -45, 0);

        public void SetAnimation(AnimationReferenceAsset e, string state, float timeScale = 1f, bool loop = true)
        {
            CharSkin.state.SetAnimation(0, e, loop).TimeScale = timeScale;
            this.state.Append(state);
        }

        public void NONE()
        {
            state = new StringBuilder();
            CharSkin.gameObject.SetActive(false);
            CharSkin_Back[0].gameObject.SetActive(false);
            CharSkin_Back[1].gameObject.SetActive(false);
        }
        public void Reset()
        {
            state = new StringBuilder();
            CharSkin.gameObject.SetActive(true);
            CharSkin_Back[0].gameObject.SetActive(false);
            CharSkin_Back[1].gameObject.SetActive(false);
        }
        public void Run(bool isPlayerWoman)
        {
            Reset();
            if (isPlayerWoman) SetAnimation(Girl.Run, "run", 1.5f);
            else SetAnimation(Boy.Run, "run", 1.5f);
        }
        public void GoRight(bool isPlayerWoman)
        {
            Reset();
            if (isPlayerWoman) SetAnimation(Girl.SlideRight, "right");
            else SetAnimation(Boy.SlideRight, "right");
        }
        public void GoLeft(bool isPlayerWoman)
        {
            Reset();
            if (isPlayerWoman) SetAnimation(Girl.SlideLeft, "left");
            else SetAnimation(Boy.SlideLeft, "left");
        }
        public void GoBack(bool isPlayerWoman)
        {
            CharSkin.gameObject.SetActive(false);
            state = new StringBuilder();
            state.Append("back");
            if (isPlayerWoman) CharSkin_Back[0].gameObject.SetActive(true);
            else CharSkin_Back[1].gameObject.SetActive(true);
        }
        public void Idle(bool isPlayerWoman)
        {
            Reset();
            if (isPlayerWoman) SetAnimation(Girl.idleBack, "idle");
            else SetAnimation(Boy.idleBack, "idle");
        }
        public void FacingCamera(bool isPlayerWoman)
        {
            Reset();
            CharSkin.transform.eulerAngles = flipX;
            if (isPlayerWoman) SetAnimation(Girl.idleFront, "idle2");
            else SetAnimation(Boy.idleFront, "idle2");
        }
        public void Normal() { CharSkin.transform.eulerAngles = normal; }
    }

    [System.Serializable]
    public class ReferensiAnimasi
    {
        public AnimationReferenceAsset Run, SlideRight, SlideLeft, idleBack, idleFront;
    }

    public UnityEngine.Transform PlayerCharSkinParent;
    public Animasi[] Pemain; //0 Blue, 1 Red, 2 Green, 3 Yellow
    public UnityArmatureComponent PlayerElementPower;
    public GameObject AnimasiLedakan;
    public GameObject AnimasiMasukSungai;
    SkeletonAnimation _animasiLedakan;

    public bool TurnOFFAutoReposition = false;
    public float PlayerStepDistance;
    public float PlayerMinOffsetPositionX;
    public float PlayerMaxOffsetPositionX;
    public float PlayerMinOffsetPositionZ;
    float _playerMinOffsetPositionZ;
    Vector3 originPosition = Vector3.zero; // 1st tab artinya digunakan saat pemain menekan tombol gerak sekali 
    Vector3 targetPosition_1stTab = Vector3.zero;  // 2nd tab artinya digunakan saat pemain menekan tombol gerak ketika 1st dipakai
    Vector3 targetPosition_2ndTab = Vector3.zero;
    Vector3 SavedPositionToLoad = Vector3.zero;
    Vector3 CorrectDistanceFromNearWall = Vector3.zero;

    public bool isMoving;
    public bool isIdle = false;
    float isMovingHorizontal = 0; // -1 left, 1 right
    float isMovingVertical = 0; // 1 up, -1 down

    //  private float elapsedTime;

    public bool CanItemBePickedUp = false;
    // public bool DontDestroyTerrainBeforeRocketTerrain = false;

    public bool isQuizOn = false;
    public bool isEnergyStartDecreasing;
    public bool hasPassedCheckpoint = false;
    public bool hasCollidedWithEnemy = false;

    public bool hasCollidedWithRocket = false;
    float TimerDelayAfterCollidedWithRocket = 0;

    public bool isJumping = false;
    public bool hasLanded = false;

    //jump
    public UnityEngine.Transform[] PointJump = null;
    int indexPointJump = -1; // -1 = not yet to jump
    float TimerPlayerToJump = 0;

    //for invisible power
    public bool invisible = false;
    public bool invisible_PowerActivated = false;
    public float DurationPowerActivated = 15f;
    private float TimerInvisiblePower;

    //Floating Board
    public List<MoveEnemy> Papans;
    int indexPapans = -1;
    public UnityEngine.Transform HookForHookingFloatingBoard;
    bool FloatingBoardDirectionIsLeft;
    float FloatingBoardSpeed = 0;

    //public float duration = 0.5f;

    // TB_Rocket tbr;

    public static TrainingArena_PlayerController GetThisPlayerController()
    {
        return PlayerController;
    }
 
    //PlayerCharacterSkin
    void InitPlayerCharSkin()
    {
        string _currHeroSelect = TrainingArenaSettingManager.Instance.CurrHeroSelect;
        Debug.Log(_currHeroSelect);
        string gender = "";

        if (_currHeroSelect.EndsWith("_F"))
        {
            gender = "girl";
            isPlayerWoman = true;
        }
        else
        {
            gender = "boy";
            isPlayerWoman = false;
        }
        int index = _currHeroSelect.IndexOf("_") + 1;
        int panjang = _currHeroSelect.LastIndexOf("_") - index;
        _currHeroSelect = _currHeroSelect.Substring(index, panjang);

        switch (_currHeroSelect.ToCharArray()[0])
        {
            case 'B': idCharSkin = 0; break;
            case 'R': idCharSkin = 1; break;
            case 'G': idCharSkin = 2; break;
            case 'Y': idCharSkin = 3; break;
        }

        CharSkinIdleForFirstTime = true;
        CurrentCharSkin = gender + _currHeroSelect;
        _animasiLedakan = AnimasiLedakan.GetComponent<SkeletonAnimation>();
        AnimasiLedakan.SetActive(false);

        for (int i = 0; i < Pemain.Length; i++)
        {
            if (i != idCharSkin) Pemain[i].NONE();
        }

        Pemain[idCharSkin].FacingCamera(isPlayerWoman);
        PlayerElementPower.gameObject.SetActive(false);
    }

    //player position
    public void SetTargetPositionVertical(float direction)
    {
        hasLanded = false;
        
        if(!HookForHookingFloatingBoard.parent.Equals(Player) && indexPapans > -1) { Papans[indexPapans].isSTOP = true; }

        if (isJumping.Equals(false))
        {
            if (isMoving.Equals(false))
            {
                isMoving = true;
                isMovingVertical = direction;

                targetPosition_1stTab = new Vector3(Player.localPosition.x, Player.localPosition.y, Mathf.Max(PlayerMinOffsetPositionZ, Player.localPosition.z + (PlayerStepDistance * direction)));
                CorrectDistanceFromNearWall = Vector3.right * isMovingVertical * PlayerStepDistance;
            }

            else if (isMoving.Equals(true) && targetPosition_2ndTab.Equals(Vector3.zero))
            {
                targetPosition_2ndTab = new Vector3(targetPosition_1stTab.x, targetPosition_1stTab.y, Mathf.Max(PlayerMinOffsetPositionZ, targetPosition_1stTab.z + (PlayerStepDistance * direction)));
            }
        }
    }
    public void SetTargetPositionHorizontal(float direction)
    {
        if (!isMoving)
        {
            isMoving = true;
            isMovingHorizontal = direction;

            targetPosition_1stTab = new Vector3(Mathf.Clamp(Player.localPosition.x + (PlayerStepDistance * direction), PlayerMinOffsetPositionX, PlayerMaxOffsetPositionX), Player.localPosition.y, Player.localPosition.z);
            if (!HookForHookingFloatingBoard.parent.Equals(Player))
            {
                FloatingBoardDirectionIsLeft = HookForHookingFloatingBoard.parent.GetComponent<MoveEnemy>().left;
                FloatingBoardSpeed = HookForHookingFloatingBoard.parent.GetComponent<MoveEnemy>().speed;
            }
            CorrectDistanceFromNearWall = Vector3.forward * isMovingHorizontal * PlayerStepDistance;
        }
    }
    Vector3 PlayerPositionSnapToGrid(Vector3 vec)
    {
        Vector3 poss = new Vector3(
            (Mathf.Round(vec.x / PlayerStepDistance) * PlayerStepDistance) + 1,
            vec.y,
            Mathf.Round(vec.z / PlayerStepDistance) * PlayerStepDistance);
        return poss;
    }
    public void Position_2ndTabReset()
    {
        targetPosition_2ndTab = Vector3.zero;
    }
    public void MainPlayerReset()
    {
        Position_2ndTabReset();
        PlayerReset();
    }
    public void PlayerReset()
    {
        originPosition = targetPosition_1stTab = Vector3.zero;
        if (!invisible_PowerActivated) invisible = false;
        isMovingHorizontal = isMovingVertical = 0;
        isMoving = false;

        //  elapsedTime = 0;
        AnimasiPlayerCharSkin();
    }
    public void PlayerMovementController()
    {
        float direction_H = isMovingHorizontal;
        float direction_V = isMovingVertical;
        Vector3 _player = Player.position;
        Vector3 _playerLocal = Player.localPosition;

        UnityEngine.Transform hook = HookForHookingFloatingBoard;

        if (isMoving)
        {
            if (direction_H != 0)
            {
                originPosition = new Vector3(_playerLocal.x + (0.125f * direction_H), _playerLocal.y, _playerLocal.z);
                //di pakai saat player berdiri di atas floating board
                if (!hook.parent.Equals(Player))
                {
                    hook.position = new Vector3(Mathf.Clamp(hook.position.x + (0.125f * direction_H), PlayerMinOffsetPositionX, PlayerMaxOffsetPositionX), _player.y, _player.z);
                    originPosition = hook.position;

                    float _floatingBoardSpeed = 0;
                    if (FloatingBoardDirectionIsLeft == false && direction_H < 0)
                    {
                        _floatingBoardSpeed = FloatingBoardSpeed * 0.05f;
                        targetPosition_1stTab.x = Mathf.Clamp(targetPosition_1stTab.x + _floatingBoardSpeed, PlayerMinOffsetPositionX, PlayerMaxOffsetPositionX);
                    }
                    else if (FloatingBoardDirectionIsLeft == true && direction_H > 0)
                    {
                        _floatingBoardSpeed = FloatingBoardSpeed * -0.05f;
                        targetPosition_1stTab.x = Mathf.Clamp(targetPosition_1stTab.x + _floatingBoardSpeed, PlayerMinOffsetPositionX, PlayerMaxOffsetPositionX);
                    }
                }

                PlayerRigidbody.MovePosition(originPosition);

                if (direction_H > 0 && originPosition.x > targetPosition_1stTab.x) PlayerReset();
                else if (direction_H < 0 && originPosition.x < targetPosition_1stTab.x) PlayerReset();
            }
            else if (direction_V != 0)
            {
                originPosition = new Vector3(_playerLocal.x, _playerLocal.y, _playerLocal.z + (direction_V * 0.125f));

                //di pakai saat player berdiri di atas floating board
                if (!hook.parent.Equals(Player))
                {
                    hook.position = new Vector3(Mathf.Clamp(hook.position.x, PlayerMinOffsetPositionX, PlayerMaxOffsetPositionX), _player.y, _playerLocal.z + (direction_V * 0.125f));
                    originPosition = hook.position;
                    targetPosition_1stTab.x = Mathf.Clamp(hook.position.x, PlayerMinOffsetPositionX, PlayerMaxOffsetPositionX);
                }

                PlayerRigidbody.MovePosition(originPosition);

                if (direction_V < 0 && originPosition.z < targetPosition_1stTab.z)
                {
                    TurnOFFAutoReposition = false;
                    MainPlayerReset();
                }
                else if (direction_V > 0 && originPosition.z > targetPosition_1stTab.z)
                {
                    if (targetPosition_2ndTab != Vector3.zero)
                    {
                        targetPosition_1stTab = targetPosition_2ndTab;
                        targetPosition_2ndTab = Vector3.zero;
                    }
                    else PlayerReset();
                }
            }

        }
        else
        {
            //di pakai saat player berdiri di atas floating board
            if (!hook.parent.Equals(Player))
            {
                Player.position = new Vector3(hook.position.x, _player.y, _player.z);
            }
        }

        AnimasiPlayerCharSkin();
    }

    //Check Wall
    public string CheckWall()
    {
        float step = PlayerStepDistance;
        RaycastHit upHit, leftHit, rightHit;
        if (Physics.Raycast(Player.position, Vector3.forward, out upHit, step))
        {
            if (upHit.collider.gameObject.CompareTag("wall")) return "up";
        }
        else if (Physics.Raycast(Player.position, Vector3.right, out rightHit, step * 2))
        {
            if (rightHit.collider.gameObject.CompareTag("wall")) return "right";
        }
        else if (Physics.Raycast(transformForLeftRaycast.position, Vector3.left, out leftHit, step * 2))
        {
            if (leftHit.collider.gameObject.CompareTag("wall")) return "left";
        }
        return "";
    }
    public string CheckWallDown()
    {
        float step = PlayerStepDistance;
        RaycastHit downHit;
        if (Physics.Raycast(Player.position, Vector3.back, out downHit, step * 2))
        {
            string[] tempTag = { "wall", "wall_sungai" };
            foreach (string e in tempTag)
            {
                if (downHit.collider.CompareTag(e)) return "down";
            }
        }
        return "";
    }

    //energy
    public void EnergyControllerTimer()
    {
        if(isEnergyStartDecreasing == true)
        {
            if (isQuizOn != true)
            {
                TimerEnergyDecrease -= Time.deltaTime;
                if (TimerEnergyDecrease <= 0)
                {
                    Energy--;
                    TimerEnergyDecrease = 1;
                    if (Energy < 0) Energy = 0;
                }
            }
        }
    }

    //invisible
    private void InvisibleControllerTimer()
    {
        if (invisible_PowerActivated)
        {
            invisible = true;
            TimerInvisiblePower += Time.deltaTime;
            PlayerElementPower.gameObject.SetActive(true);
            if (TimerInvisiblePower > DurationPowerActivated)
            {
                TimerInvisiblePower = 0;
                invisible = false;
                invisible_PowerActivated = false;
                PlayerElementPower.gameObject.SetActive(false);
            }
        }
    }
    private void InvisibleControllerEffect()
    {
        if (invisible)
        {
            AnimasiElementPower();

            if (!isJumping) { PlayerRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation; }
            else { PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation; PlayerRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY; }

            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), true);
        }
        else
        {
            AnimasiElementPower();
            PlayerRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
            PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), false);
        }
    }
    public int CheckPowerInvisible()
    {
        // 0 false // 1 true
        int condition = 0;
        if (invisible)
        {
            if (invisible_PowerActivated) condition = 0;
            else condition = 1;
        }
        return condition;
    }

    //player_rocket
    private void InsideRocketTerrain(string nama = null, TB_Rocket tb_Rocket = null)
    {
        switch (nama)
        {
            case "PointDestroy1": // Collide with Trigger
                string temp = tb_Rocket.GetRocketSequence();
                gameManager.TurnOnRambuTandaSeru(temp);
                SavedPositionToLoad = new Vector3(Player.localPosition.x, Player.localPosition.y, Player.localPosition.z - 2.5f);
                SavedPositionToLoad = PlayerPositionSnapToGrid(SavedPositionToLoad);
                tb_Rocket.StartCount();
                break;

            case "PointDestroy2": // Collide with Trigger
                _playerMinOffsetPositionZ = PlayerMinOffsetPositionZ;
                PlayerMinOffsetPositionZ = Player.position.z + 2;
                break;

            case "PointDestroy3": // Collide with Trigger
                gameManager.TurnOffTandaSeru();
                _playerMinOffsetPositionZ = PlayerMinOffsetPositionZ;
                PlayerMinOffsetPositionZ = Player.position.z + 2;
                break;

            case "dead":
                AnimasiLedakanController();
                hasCollidedWithRocket = true;
                invisible = true;
                Pemain[idCharSkin].NONE();
                break;

            case "revived":
                Player.localPosition = SavedPositionToLoad;
                PlayerMinOffsetPositionZ = _playerMinOffsetPositionZ;
                Position_2ndTabReset();
                invisible = false;
                Pemain[idCharSkin].Idle(isPlayerWoman);
                AnimasiPlayerCharSkin();
                break;
        }
    }
    void DelayAfterCollidedWithRocket()
    {
        if (hasCollidedWithRocket)
        {
            TimerDelayAfterCollidedWithRocket += Time.deltaTime;
            if (TimerDelayAfterCollidedWithRocket >= 0.5f)
            {
                TimerDelayAfterCollidedWithRocket = 0;
                hasCollidedWithRocket = false;
                InsideRocketTerrain("revived");
            }
        }
    }

    //player_enemy
    void TeriakAfterCollidedWithEnemy()
    {
        if (hasCollidedWithEnemy)
        {
            hasCollidedWithEnemy = false;
            if (!isPlayerWoman) SoundController.Mulut_Cowok_GetDamage();
            else SoundController.Mulut_Cewek_GetDamage();
        }
    }
    void TriggerStepBack(int layar = 0)
    {
        if (!invisible)
        {
            if (layar == 13)
            {
                SoundController.PlayerGettingHitByRocket();
                InsideRocketTerrain("dead");
            }
            else
            {
                SoundController.PlayerGettingHitByGada();
                invisible = true;
                Pemain[idCharSkin].GoBack(isPlayerWoman);
                SetTargetPositionVertical(-1);
            }
        }
    }

    //player_sungai
    private void InsideSungai(string nama = null, int layar = 0, string tag = "")
    {
        if (tag.Equals("wall_sungai_out_of_range"))
        {
            isMoving = false;
            isJumping = false;
            PlayerStayInFloatingBoardForTooLong();
            return;
        }

        //PointJump untuk melacak player sesudah lompat dan reset timer lompat
        else if (tag.Equals("WS_0"))
        {
            if (invisible_PowerActivated) indexPointJump = PointJump.Length - 1;
            else indexPointJump = 0;
            TimerPlayerToJump = 0.4f;
        }
        else if (tag.Equals("WS_1"))
        {
            if (invisible_PowerActivated) indexPointJump = PointJump.Length - 1;
            else
            {
                indexPointJump++;
                if (indexPointJump >= 2) { indexPointJump = 1; }
            }
            TimerPlayerToJump = 0.4f;
        }
        else if (tag.Equals("WS_2"))
        {
            indexPointJump = 2;
            TimerPlayerToJump = 0.4f;
        }

        if (nama == "Trigger3")
        {
            SavedPositionToLoad = Vector3.zero;
        }
        else if (nama == "Trigger2")
        {
            Vector3 lompat = Vector3.zero;
            MainPlayerReset();
            if (layar == 12)
            {
                TurnOFFAutoReposition = true;
                SavedPositionToLoad = new Vector3(0, Player.localPosition.y, Player.localPosition.z - 2.5f);
                SavedPositionToLoad = PlayerPositionSnapToGrid(SavedPositionToLoad);
            }
            else
            {
                TurnOFFAutoReposition = false;
            }
            isJumping = true;

            indexPapans++;

            if (indexPointJump < PointJump.Length)
            {
                float max = Player.position.x + 0.1f;
                float min = Player.position.x - 0.1f;
                float eks = Mathf.Clamp(Player.position.x, min, max);
                lompat = new Vector3(eks, PointJump[indexPointJump].position.y, PointJump[indexPointJump].position.z);
            }

            StartCoroutine(MakePlayerJump(lompat, 2.5f));
            if (!isPlayerWoman) SoundController.Mulut_Cowok();
            else SoundController.Mulut_Cewek();
        }

        // di sini ceritanya player tercebur ke sungai
        if (layar == 4)
        {
            isMoving = false;
            isJumping = false;
            StartCoroutine(WaitAfterDrown());
            return;
        }
    }
    private void PlayerStayInFloatingBoardForTooLong()
    {
        indexPointJump = -1;
        indexPapans = -1;
        HookForHookingFloatingBoard.parent = Player;
        Player.localPosition = SavedPositionToLoad;
        //Debug.Log(Player.localPosition);
        TurnOFFAutoReposition = false;
        invisible = false;
        Position_2ndTabReset();
        AnimasiPlayerCharSkin();
    }
    public IEnumerator MakePlayerJump(Vector3 endpoint, float height)
    {
        Vector3 basePos = Player.position;
        float distance = endpoint.z - basePos.z;
        float x1 = 0;
        float x2 = distance;
        float x3 = (x2 + x1) / 2.0f;
        float a = height / ((x3 - x1) * (x3 - x2));

        for (float passed = 0.0f; passed < TimerPlayerToJump;)
        {
            passed += Time.deltaTime;
            float f = passed / TimerPlayerToJump;
            if (f > 1) f = 1;
            float z = distance * f;
            float y = a * (z - x1) * (z - x2);

            Player.position = new Vector3(Player.position.x, basePos.y + y, basePos.z + z);
            yield return 0;
        }
        AfterPlayerJump();
    }
    void AfterPlayerJump()
    {
        isJumping = false;
        hasLanded = true;
    }
    private IEnumerator WaitAfterDrown()
    {
        AnimasiTenggelamDalamSungai();
        yield return new WaitForSeconds(0.5f);
        AnimasiTenggelamDalamSungai(true);
        Player.localPosition = SavedPositionToLoad;
        yield return new WaitForSeconds(0.3f);
        TurnOFFAutoReposition = false;

        // elapsedTime = 0;
        indexPointJump = -1;
        indexPapans = -1;
        invisible = false;
        Position_2ndTabReset();
        AnimasiPlayerCharSkin();
    }

    //Animasi
    void AnimasiLedakanController()
    {
        _animasiLedakan.state.SetAnimation(0, "explosive", false);
        AnimasiLedakan.SetActive(true);
    }
    void AnimasiElementPower()
    {
        if (invisible_PowerActivated)
        {
            switch (idCharSkin)
            {
                case 0:
                    PlayerElementPower.animation.Play("water");
                    break;
                case 2:
                    PlayerElementPower.animation.Play("earth");
                    break;
                case 1:
                    PlayerElementPower.animation.Play("fire");
                    break;
                case 3:
                    PlayerElementPower.animation.Play("air");
                    break;
                default:
                    PlayerElementPower.animation.Play("water");
                    break;
            }
        }
        else
        {
            PlayerElementPower.animation.Stop();
        }
    }
    void AnimasiPlayerCharSkin()
    {
        if (isMoving)
        {
            if (isMovingHorizontal.Equals(1))
            {
                SoundController.PlayerSlide();
                Pemain[idCharSkin].GoRight(isPlayerWoman);
            }
            else if (isMovingHorizontal.Equals(-1))
            {

                SoundController.PlayerSlide();
                Pemain[idCharSkin].GoLeft(isPlayerWoman);
            }
            else if (isMovingVertical < 0)
            {
                SoundController.PlayerSlide();
                Pemain[idCharSkin].GoBack(isPlayerWoman);
            }
            else
            {
                if (hasCollidedWithRocket.Equals(false))
                {
                    if (Pemain[idCharSkin].state.ToString() != "run")
                    {
                        Pemain[idCharSkin].Run(isPlayerWoman);
                    }
                }

            }

            isIdle = false;
            CharSkinIdleForFirstTime = false;
            Pemain[idCharSkin].Normal();

        }
        else if (isMoving.Equals(false))
        {
            if (CharSkinIdleForFirstTime.Equals(false))
            {
                if (Pemain[idCharSkin].state.ToString() != "idle")
                {
                    SoundController.PlayerStop();
                    if (hasCollidedWithRocket.Equals(false))
                    {
                        Pemain[idCharSkin].Idle(isPlayerWoman);
                        isIdle = true;
                    }
                }
            }
        }
    }
    void AnimasiTenggelamDalamSungai(bool reset = false)
    {
        if (reset)
        {
            AnimasiMasukSungai.SetActive(false);
            PlayerCharSkinParent.localPosition = new Vector3(0, 0.18f, 0);
            AnimasiMasukSungai.transform.localScale = (Vector3.one * 1.2f);
        }
        else
        {
            AnimasiMasukSungai.SetActive(true);
            PlayerCharSkinParent.DOLocalMoveY(-4.63f, 0.3f);
            AnimasiMasukSungai.transform.DOScale(0f, 0.5f);
        }
    }

    public void FakeDestroy(GameObject temp) { temp.SetActive(false); }

    void RUpdate()
    {
        PlayerMovementController();
        if (isMoving.Equals(false) && TurnOFFAutoReposition.Equals(false))
        {
            Player.position = PlayerPositionSnapToGrid(Player.position);
            HookForHookingFloatingBoard.parent = Player;
            HookForHookingFloatingBoard.localPosition = Vector3.zero;
        }
        TeriakAfterCollidedWithEnemy();
        InvisibleControllerEffect();
    }

    //Unity
    void Awake()
    {
        PlayerController = this;
    }
    void Start()
    {
        PlayerBoxCollider = Player.GetComponent<BoxCollider>();
        PlayerRigidbody = Player.GetComponent<Rigidbody>();
        CanItemBePickedUp = TrainingArenaSettingManager.Instance.POWERUP;

        InitPlayerCharSkin();
        Position_2ndTabReset();

        PointJump = null;
        Papans = null;

    }

    void Update()
    {
       // Debug.Log("indexP" + indexPapans);
        EnergyControllerTimer();
        InvisibleControllerTimer();
        if (GetComponent<AudioListener>().Equals(null)) SoundController.PlayerStop();
        if (isJumping) Position_2ndTabReset();
        //if (tbr != null) {PlayerController}
        DelayAfterCollidedWithRocket();
        if(!HookForHookingFloatingBoard.parent.Equals(Player))RUpdate();
    }

    void FixedUpdate()
    {
        if (HookForHookingFloatingBoard.parent.Equals(Player))RUpdate();
    }

    void OnTriggerEnter(Collider colli)
    {
        switch (colli.gameObject.tag)
        {
            case "score":
                Score++;
                if (Score % 10 == 0)
                {
                    int temp = UnityEngine.Random.Range(0, PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList.Count);
                    gameManager.mendapatkan_kartu_gratis(temp);
                }
                FakeDestroy(colli.gameObject);
                break;

            case "mark":
                if (CanItemBePickedUp)
                {
                    int temp = colli.GetComponent<PowerUpSetting>().markID;
                    SoundController.PlayerStop();
                    isQuizOn = true;
                    Levelling.Get_Type_Of_Prize(temp);
                    FakeDestroy(colli.gameObject);
                }
                break;

            case "Respawn":
                string tempString = colli.transform.parent.name;
                string[] tempArrayString = tempString.Split('+');
                SpawnedSetName = tempArrayString[0];
                indexSpawnedTerrain = int.Parse(tempArrayString[1]);
                SpawnedTerrainName = tempArrayString[2];

                PlayerMinOffsetPositionZ = Player.position.z;
                FakeDestroy(colli.gameObject);
                hasPassedCheckpoint = true;
                break;

            case "dot_jump":
                if (colli.gameObject.name.Equals("Trigger4")) {
                    PointJump = colli.transform.parent.GetComponent<TB_Sungai>().PointJump;
                    Papans = colli.transform.parent.GetComponent<TB_Sungai>().papanKayu;
                }
                else if (colli.gameObject.name.Equals("Trigger5"))
                {
                    PointJump = null;
                    Papans = null;
                    indexPointJump = -1;
                    indexPapans = -1;
                }
                break;

            case "wall_sungai":
                targetPosition_2ndTab = targetPosition_1stTab = Vector3.zero;
                InsideSungai(colli.gameObject.name, colli.gameObject.layer, "");
                break;

            case "WS_0":
            case "WS_1":
            case "WS_2":
                targetPosition_2ndTab = targetPosition_1stTab = Vector3.zero;
                InsideSungai(colli.gameObject.name, colli.gameObject.layer, colli.gameObject.tag);
                break;

            case "wall_sungai_out_of_range":
                Debug.Log("wall_sungai_out_of_range");
                InsideSungai(null, 0, colli.gameObject.tag);
                break;

            case "reverse":
                InsideRocketTerrain(colli.gameObject.name, colli.transform.parent.GetComponent<TB_Rocket>());
                break;
            case "tutor":
                gameManager.StartTutorial();
                colli.gameObject.SetActive(false);
                break;
        }
    }

    void OnCollisionEnter(Collision colli)
    {
        switch (colli.gameObject.tag)
        {
            case "landing":
                if (hasLanded)
                    SoundController.PlayerLanding();
                break;
        }
    }

    Vector3 _playerPosition = Vector3.zero;
    void OnCollisionStay(Collision colli)
    {
        switch (colli.gameObject.layer)
        {
            case 4:
                InsideSungai(null, 4);
                return;
            case 11:
                TurnOFFAutoReposition = false;
                break;
        }

        switch (colli.gameObject.tag)
        {
            case "musuh":
                if (!invisible)
                {
                    TurnOFFAutoReposition = true;
                    isMoving = false;
                    hasCollidedWithEnemy = true;
                    if (colli.transform.parent.gameObject.layer.Equals(13)) { TriggerStepBack(13); }
                    else TriggerStepBack();
                }
                break;
            case "log":
                if (!isJumping)
                {
                    TurnOFFAutoReposition = true;
                    if (HookForHookingFloatingBoard.parent.Equals(Player))
                    {
                        _playerPosition = Player.position;
                        HookForHookingFloatingBoard.parent = colli.gameObject.transform.parent;
                        HookForHookingFloatingBoard.position = _playerPosition;
                    }
                }
                break;
            case "wall":
                isMoving = false;
                // (8.5, 0, 0) + [ (0,0,1) * -1 * 2.5] = 
                //near_wall = Vector3.forward * move_h * step;
                originPosition = targetPosition_1stTab = transform.localPosition + CorrectDistanceFromNearWall;
                break;
        }
    }

    public void Debug_EnterQuiz()
    {
        int temp = Random.Range(1, 11);
        SoundController.PlayerStop();
        isQuizOn = true;
        Levelling.Get_Type_Of_Prize(temp);
    }
}
