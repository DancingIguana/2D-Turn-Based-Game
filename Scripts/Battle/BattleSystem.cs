using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { Start, PlayerAction, PlayerSkill, PlayerAttack, PlayerWakeUp, PlayerWait,  EnemyMove, Busy, Switching, ChoosePlayer, PlayerSupport}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BatlleHUDEnemy enemyHud;
    [SerializeField] ButtonSpawner skillMenu;
    [SerializeField] ActionBox actionMenu;
    [SerializeField] GameObject iconBox;
    [SerializeField] BattleHud iconTemplate;
    [SerializeField] GameObject selectedMove;
    [SerializeField] Text selectedPlayer;
    [SerializeField] GameObject particleEffect;
    [SerializeField] GameObject cutin;
    [SerializeField] GameObject OneMoreUI;
    [SerializeField] GameObject playerAdvUI;
    [SerializeField] GameObject enemyAdvUI;
    [SerializeField] Image cutinImage;

    [SerializeField] GameObject gameOver;
    bool playerFirstTurn;
    [SerializeField] DamageIcon dmgIcon;
    
    [SerializeField] UISounds uiSounds;
    [SerializeField] GameObject transitionCircle;
    [SerializeField] GameObject blackBG;

    Animator cutinAnimator;
    Animator particleAnimator;
    Animator dmgIconAnimator;
    Animator circleAnimator;
    public float speed = 10f;
    GameObject playerModel;
    Animator playerAnimator;
    GameObject enemyModel;
    Animator enemyAnimator;

    Fighter currentFighter;


    BattleState state;
    int currentAction;
    int currentPlayer;
    int target;
    int chosenPlayer;
    List<bool> knockedPlayers = new List<bool>();
    bool knockedEnemy = false;
    bool OneMore = false;

    Text smText;
    bool initializedSkills = false;
    bool initializedActions = false;
    bool initializedCharSel = false;
    bool partyTurn = false;
    bool foundPlayer = false;
    bool validChange = true;

    FighterParty playerParty;
    Fighter enemyFighter;
    [SerializeField] Fighter playerFighter;
    //[SerializeField] Fighter enemyFighter;

    List<BattleHud> charIcons = new List<BattleHud>();
    List<Animator> charIconAnimators = new List<Animator>();
    List<GameObject> playerModels = new List<GameObject>();
    List<Animator> playerAnimators = new List<Animator>();
    List<float> playerHeights = new List<float>();
    List<float> playerWidths = new List<float>();
    List<Vector3> playerPositions = new List<Vector3>();

    Move move;

    AudioSource audioSrc;

    public static int nTurns;
    private void Start()
    {
        enemyFighter = StoreEnemy.instance.enemy;
        playerParty =  Characters.instance.playerParty;
        playerFirstTurn = StoreEnemy.instance.playerTurn;
        Time.timeScale = 1;
        nTurns = 1;
        Debug.Log("Party size: " + playerParty.Fighters.Count);
        particleAnimator = particleEffect.GetComponentInChildren<Animator>();
        cutinAnimator = cutin.GetComponent<Animator>();
        dmgIconAnimator = dmgIcon.GetComponent<Animator>();
        circleAnimator = transitionCircle.GetComponent<Animator>();
        enemyFighter.Init();

        StartCoroutine(SetupBattle());
    }

    //SETUP
    public IEnumerator SetupBattle() {
        Debug.Log("Setting up battle");
        //Wait for initialization
        yield return new WaitForSeconds(0.01f);
        SoundManagerScript.PlaySound(uiSounds.StartBattle);
        yield return new WaitForSeconds(0.1f);
        circleAnimator.SetTrigger("open");


        //Spawn Character health icons
        spawnCharIcons();

        //Spawn player models
        spawnPlayers();

        //Spawn enemy
        enemyUnit.Setup(enemyFighter);
        enemyHud.SetData(enemyFighter);
        enemyModel = GameObject.FindGameObjectsWithTag("EnemyModel")[0];
        enemyAnimator = enemyModel.GetComponent<Animator>();

        //First party member enters the scene
        currentPlayer = 0;
        selectedPlayer.text = playerParty.Fighters[currentPlayer].Base.Name;
        StartCoroutine(PlayerEntrance(playerModels[0],playerAnimators[0], playerPositions[0]));
        currentFighter = playerParty.Fighters[0];
        playerAnimator = playerAnimators[0];
        playerModel = playerModels[0];
        partyTurn = true;

        yield return new WaitForSeconds(0.5f);
        if(playerFirstTurn) {
            playerAdvUI.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            playerAdvUI.SetActive(false);
        }
        else {
            enemyAdvUI.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            enemyAdvUI.SetActive(false);
        }
        
        transitionCircle.SetActive(false);
        skillMenu.CleanSkillMenu();
        initializedSkills = false;
        skillMenu.Setup(playerParty.Fighters[0]);

        if(playerFirstTurn == true) {
            state = BattleState.PlayerAction;
        }
        else 
            state = BattleState.EnemyMove;
        
    }

    IEnumerator PlayerEntrance(GameObject playerGO, Animator anim, Vector3 destiny) {
        yield return new WaitForSeconds(2f);
        
       // anim = playerGO.GetComponentInChildren<Animator>();
        anim.SetBool("run",true);
        while(Vector3.Distance(playerGO.transform.position, destiny) > 0) {
            playerGO.transform.position = Vector3.MoveTowards(playerGO.transform.position, destiny,speed * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("run",false);
        //anim.Play("Backward");
    }

    IEnumerator PlayerExit(GameObject playerGO, Animator anim) {
        yield return new WaitForSeconds(2f);

        anim.SetBool("exit",true);
        var objective = playerGO.transform.position - new Vector3(10,0,0);
        while(Vector3.Distance(playerGO.transform.position, objective) > 0) {
            playerGO.transform.position = Vector3.MoveTowards(playerGO.transform.position, objective ,speed * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("exit",false);

    }

    IEnumerator Wakeup(GameObject playerGO, Animator anim) {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("knock",false);
        yield return new WaitForSeconds(2f);
    }

    IEnumerator EnableMenuWait()
    {
        yield return new WaitForSeconds(3f);
        state = BattleState.PlayerAction;
        actionMenu.EnableActionMenu(true);
    }


    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        actionMenu.EnableActionMenu(true);
    }

    void PlayerSkill()
    {
        state = BattleState.PlayerSkill;
        actionMenu.EnableActionMenu(false);
        actionMenu.EnableSkillMenu(true);
    }

    IEnumerator PerformPlayerSkill()
    {
        nTurns += 1;
        chosenPlayer = currentPlayer;
        state = BattleState.Busy;
        if(move.Base.Type == FighterType.Heal)
        {
            state = BattleState.ChoosePlayer;
        } else {
        //UI Move text
        Text smText = selectedMove.GetComponentInChildren<Text>();
        smText.text = move.Base.Name;
        selectedMove.SetActive(true);

        //Enemy takes damage and get its details
        var damageDetails = enemyUnit.Fighter.TakeDamage(move, playerParty.Fighters[currentPlayer]);

        //Updating player health/mp bars based on the skillcost
        playerParty.Fighters[currentPlayer].skillCost(move);
        charIcons[currentPlayer].UpdateHP();

        //If damage is critical display animation
        if(damageDetails.Critical > 1f) 
        {
            SoundManagerScript.PlaySound(playerParty.Fighters[currentPlayer].Base.CriticalSound);
            SoundManagerScript.PlaySound(uiSounds.Critical);
            Debug.Log("Critical Hit!");
            cutinImage.sprite = playerParty.Fighters[currentPlayer].Base.Cutin;
            cutinAnimator.SetTrigger("Open");
        }
        else {
            SoundManagerScript.PlaySound(playerParty.Fighters[currentPlayer].Base.AttackSound);
        }

        //Play animation
        
        SoundManagerScript.PlaySound(uiSounds.Summon);
        playerAnimators[currentPlayer].SetTrigger("skill");
        
        yield return new WaitForSeconds(3f);

        //Attack effect
        particleEffect.transform.position = enemyModel.transform.position;


        //Damage display
        dmgIcon.SetHP((float) enemyUnit.Fighter.HP/enemyUnit.Fighter.MaxHp, damageDetails.Damage);
        dmgIcon.transform.position = enemyModel.transform.position +  new Vector3(0,1,0);

        playEffectSound(move.Base.Type);

        particleAnimator.SetTrigger(AttackTypeString(move));

        //Removing UI Text
        selectedMove.SetActive(false);

        Debug.Log("Damage type");
        Debug.Log(damageDetails.Type);

        //Evaluate: normal, weakness or resistance, fainted
        //Evaluate damage effect on enemy
        if(damageDetails.Type > 1f) //If weakness
        {
            dmgIconAnimator.SetTrigger("weak");
            Debug.Log("Hit weakness");
            if(knockedEnemy == false) {
                enemyAnimator.SetBool("knock", true);
                OneMore = true;
                knockedEnemy = true;
                
            } else {
                
                enemyAnimator.SetBool("knock",false);
                enemyAnimator.SetTrigger("hurt");
                knockedEnemy = false;
            }
        }

        else if(damageDetails.Critical > 1f) // If critical
        {
            dmgIconAnimator.SetTrigger("critical");
            if(knockedEnemy == false) {
                knockedEnemy = true;
                enemyAnimator.SetBool("knock",true);
                OneMore = true;
            } else {
                knockedEnemy = false;
                enemyAnimator.SetBool("knock",false);
                enemyAnimator.SetTrigger("hurt");
            }
        }

        else if(damageDetails.Type < 1f) //If resistance
        {
            if(knockedEnemy == true) 
            {
                enemyAnimator.SetBool("knock",false);
                knockedEnemy = false;
            }
            dmgIconAnimator.SetTrigger("resist");
            enemyAnimator.SetTrigger("hurt");
            Debug.Log("Character resisted");
        }

        else { //Normal damage
            if(knockedEnemy == true) 
            {
                enemyAnimator.SetBool("knock",false);
                knockedEnemy = false;
            }
            dmgIconAnimator.SetTrigger("normal");
            enemyAnimator.SetTrigger("hurt");
            SoundManagerScript.PlaySound(enemyFighter.Base.HurtSound);
        }
        
        //Updating damage in HUD
        enemyHud.UpdateHP();

        charIconAnimators[currentPlayer].SetBool("turn",false);


        yield return new WaitForSeconds(1f);

        if(playerParty.GetNextAlive(currentPlayer) == 0) {
            partyTurn = false;
            Debug.Log("Party turn ended!");
        }

        if(damageDetails.Fainted) {
            if(knockedEnemy == false) {
                enemyAnimator.SetBool("knock",true);
            }
            SoundManagerScript.PlaySound(enemyFighter.Base.DeathSound);
            yield return new WaitForSeconds(0.5f);
            playerAnimators[currentPlayer].SetTrigger("win");
            yield return new WaitForSeconds(2f);
            blackBG.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(3);
            Debug.Log("You win!");
        } 
        else if(OneMore)
        {
            yield return new WaitForSeconds(0.5f);
            OneMoreUI.SetActive(true);
            yield return new WaitForSeconds(1f);
            OneMoreUI.SetActive(false);
            Debug.Log("One More!");
            OneMore = false;
            state = BattleState.PlayerAction;
        }
        else if(playerParty.GetAliveCount() == 1) 
        {
            state = BattleState.EnemyMove;
        } 
        else 
        {
            state = BattleState.Switching;
        }
        }
        
    }

    IEnumerator Switching(int index) {

        int fighterIndex = 0;

        //Find next healthy fighter
        for(int i = index+1; i < index + playerParty.Fighters.Count; i++) {
            fighterIndex = i % playerParty.Fighters.Count;
            //If the next healthy fighter is the leader end party turn
            if(fighterIndex == 0) {
                partyTurn = false;
            }
            if(playerParty.Fighters[fighterIndex].HP > 0) {
                break;
            }
        }
        Debug.Log("Switching to fighter number " + fighterIndex);

        StartCoroutine(PlayerExit(playerModel,playerAnimator));
        yield return new WaitForSeconds(1f);

        currentPlayer = fighterIndex;
        selectedPlayer.text = playerParty.Fighters[currentPlayer].Base.Name;
        currentFighter = playerParty.Fighters[fighterIndex];
        playerAnimator = playerAnimators[fighterIndex];
        playerModel = playerModels[fighterIndex];

        Debug.Log("Current Fighter = " + currentFighter.Base.Name);


        skillMenu.CleanSkillMenu();
        Debug.Log("Total buttons " + skillMenu.SkillMenuButtons.Count);
        initializedSkills = false;
        skillMenu.Setup(playerParty.Fighters[fighterIndex]);

        StartCoroutine(PlayerEntrance(playerModel,playerAnimator, playerUnit.transform.position + new Vector3(playerWidths[currentPlayer]/2, playerHeights[currentPlayer]/2)));
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.Busy;
        //Lose turn waking up if enemy is knocked
        if(knockedEnemy) {
            SoundManagerScript.PlaySound(enemyFighter.Base.WakeupSound);
            StartCoroutine(Wakeup(enemyModel,enemyAnimator));
            knockedEnemy = false;
            partyTurn = true;
            state = BattleState.PlayerAction;
        }
        //Normal turn
        else {
        //Choose enemy
        target = playerParty.GetRandomAlive();

        //If the target is not the current player
        if(target != currentPlayer) {
            yield return new WaitForSeconds(2f);

            //Current player exits scene
            var objective = playerModels[currentPlayer].transform.position - new Vector3(10,0,0);
            while(Vector3.Distance(playerModels[currentPlayer].transform.position, objective) > 0) {
                playerModels[currentPlayer].transform.position = Vector3.MoveTowards(playerModels[currentPlayer].transform.position, objective ,(speed +5)* Time.deltaTime);
                yield return null;
            }

            //Selected target enters scene
            while(Vector3.Distance(playerModels[target].transform.position, playerPositions[target]) > 0) {
                playerModels[target].transform.position = Vector3.MoveTowards(playerModels[target].transform.position, playerPositions[target] ,(speed +5)* Time.deltaTime);
                yield return null;
            }
        }

        //Choose move
        move = enemyUnit.Fighter.GetRandomMove();

        //Display selected move text
        Text smText = selectedMove.GetComponentInChildren<Text>();
        smText.text = move.Base.Name;
        selectedMove.SetActive(true);

        //Target takes damage 
        var damageDetails = playerParty.Fighters[target].TakeDamage(move, enemyUnit.Fighter);

        //If critical display cutin
        if(damageDetails.Critical > 1f) {
            Debug.Log("Critical Hit!");
            SoundManagerScript.PlaySound(uiSounds.Critical);
            SoundManagerScript.PlaySound(enemyFighter.Base.CriticalSound);
            //cutinImage.sprite = enemyUnit.Fighter.Base.Cutin;
            //cutinAnimator.SetTrigger("Open");
        }

        //Attack animation
        enemyAnimator.SetTrigger("skill");
        SoundManagerScript.PlaySound(enemyFighter.Base.AttackSound);
        yield return new WaitForSeconds(3f);

        //Attack effect
        particleEffect.transform.position = playerModels[target].transform.position;
        particleAnimator.SetTrigger(AttackTypeString(move));
        playEffectSound(move.Base.Type);

        //Damage displayer
        dmgIcon.SetHP((float) playerParty.Fighters[target].HP/playerParty.Fighters[target].MaxHp, damageDetails.Damage);
        dmgIcon.transform.position = playerModels[target].transform.position + new Vector3(0,1,0);

        //Evaluate damage effect on enemy
        if(damageDetails.Type > 1f) //If weakness
        {
            dmgIconAnimator.SetTrigger("weak");
            Debug.Log("Hit weakness");
            
            if(knockedPlayers[target] == false) {
                playerAnimators[target].SetBool("knock", true);
                OneMore = true;
                knockedPlayers[target] = true;
            } else {
                playerAnimators[target].SetBool("knock",false);
                playerAnimators[target].SetTrigger("hurt");
                knockedPlayers[target] = false;
            }
        }

        else if(damageDetails.Critical > 1f) // If critical
        {
            dmgIconAnimator.SetTrigger("critical");
            if(knockedPlayers[target] == false) {
                knockedPlayers[target] = true;
                playerAnimators[target].SetBool("knock",true);
                OneMore = true;
            } else {
                knockedPlayers[target] = false;
                playerAnimators[target].SetBool("knock",false);
                playerAnimators[target].SetTrigger("hurt");
            }
        }

        else if(damageDetails.Type < 1f) //If resistance
        {
            if(knockedPlayers[target] == true) 
            {
                playerAnimators[target].SetBool("knock",false);
                knockedPlayers[target] = false;
            }
            dmgIconAnimator.SetTrigger("resist");
            playerAnimators[target].SetTrigger("hurt");
        }

        else { //Normal damage
        if(knockedPlayers[target] == true) 
            {
                playerAnimators[target].SetBool("knock",false);
                knockedPlayers[target] = false;
            }
            dmgIconAnimator.SetTrigger("normal");
            playerAnimators[target].SetTrigger("hurt");
        }

        //Check if the target died
        //If it's not knocked down, display knock down animation
        if(damageDetails.Fainted)
        {
            SoundManagerScript.PlaySound(playerParty.Fighters[target].Base.DeathSound);
            charIconAnimators[target].SetBool("death",true);
            Debug.Log("Target died");
            if(knockedPlayers[target] == false) 
            {
                playerAnimators[target].SetBool("knock", true);
                knockedPlayers[target] = true;
            }
        } else
        {
            SoundManagerScript.PlaySound(playerParty.Fighters[target].Base.HurtSound);
        }


        selectedMove.SetActive(false);


        charIcons[target].UpdateHP();

        

        //Return the party leader to scene after the target receives damage
        if(target != currentPlayer) {
            yield return new WaitForSeconds(2f);

            //Current player exits scene
            var objective = playerModels[target].transform.position - new Vector3(10,0,0);
            while(Vector3.Distance(playerModels[target].transform.position, objective) > 0) {
                playerModels[target].transform.position = Vector3.MoveTowards(playerModels[target].transform.position, objective ,(speed +5)* Time.deltaTime);
                yield return null;
            }

            while(Vector3.Distance(playerModels[currentPlayer].transform.position, playerPositions[target]) > 0) {
                playerModels[currentPlayer].transform.position = Vector3.MoveTowards(playerModels[currentPlayer].transform.position, playerPositions[target] ,(speed +5)* Time.deltaTime);
                yield return null;
            }
        }
        //If the party leader is death, finish
        if(playerParty.Fighters[0].HP <= 0)
        {
            gameOver.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            SceneManager.LoadScene(4);
            Debug.Log("Enemy died");
        } else if(OneMore)
        {
            yield return new WaitForSeconds(0.5f);
            OneMoreUI.SetActive(true);
            yield return new WaitForSeconds(1f);
            OneMoreUI.SetActive(false);
        
            OneMore = false;
            Debug.Log("ONE MORE!");
            state = BattleState.EnemyMove;
        }
        else
        {
            state = BattleState.PlayerAction;
        }
        }
    }

    //WAIT
    IEnumerator Wait() 
    {
        state = BattleState.Busy;
        nTurns += 1;
        //Display "Wait text"
        Text smText = selectedMove.GetComponentInChildren<Text>();
        smText.text = "Wait";
        selectedMove.SetActive(true);

        yield return new WaitForSeconds(1f);
        selectedMove.SetActive(false);
        charIconAnimators[currentPlayer].SetBool("turn",false);
        if(playerParty.GetNextAlive(currentPlayer) == 0) {
            partyTurn = false;
        }
        
        if(playerParty.GetAliveCount() == 1) 
        {
            state = BattleState.EnemyMove;
        } 
        else 
        {
            state = BattleState.Switching;
        }
    }
    IEnumerator WakeUp()
    {
        Debug.Log("Waking Up");
        state = BattleState.Busy;

        yield return new WaitForSeconds(1f);
        knockedPlayers[currentPlayer] = false;
        playerAnimators[currentPlayer].SetBool("knock", false);
        SoundManagerScript.PlaySound(playerParty.Fighters[currentPlayer].Base.WakeupSound);
        yield return new WaitForSeconds(2f);

        charIconAnimators[currentPlayer].SetBool("turn",false);

        if(playerParty.GetNextAlive(currentPlayer) == 0) {
            partyTurn = false;
        }

        if(playerParty.GetAliveCount() == 1) 
        {
            state = BattleState.EnemyMove;
        } 
        else 
        {
            state = BattleState.Switching;
        }
    }

    void Update()
    {

        switch(state) 
        {
            case BattleState.PlayerAction:
                Debug.Log("Player Turn");
                HandleActionSelection();
                break;

            case BattleState.PlayerSkill:
                HandleSkillSelection();
                break;

            case BattleState.PlayerAttack:
                break;

            case BattleState.PlayerWakeUp:
                StartCoroutine(WakeUp());
                break;

            case BattleState.PlayerWait:
                StartCoroutine(Wait());
                break;

            case BattleState.Switching:
                StartCoroutine(Switch());
                break;

            case BattleState.EnemyMove:
                StartCoroutine(EnemyMove());
                break;

            case BattleState.ChoosePlayer:
                HandleCharSelection();
                break;
            
            case BattleState.PlayerSupport:
                StartCoroutine(PlayerSupport());
                break;
        }
    }

    //SWITCH
    IEnumerator Switch() 
    {
        state = BattleState.Busy;
        int fighterIndex = playerParty.GetNextAlive(currentPlayer);
        yield return new WaitForSeconds(1f);
        StartCoroutine(PlayerExit(playerModels[currentPlayer],playerAnimators[currentPlayer]));

        //Change current player
        currentPlayer = fighterIndex;
        selectedPlayer.text = playerParty.Fighters[currentPlayer].Base.Name;

        Debug.Log("Current Fighter = " + playerParty.Fighters[currentPlayer].Base.Name + " " + currentPlayer);

        skillMenu.CleanSkillMenu();
        Debug.Log("Total buttons " + skillMenu.SkillMenuButtons.Count);
        initializedSkills = false;
        skillMenu.Setup(playerParty.Fighters[fighterIndex]);

        StartCoroutine(PlayerEntrance(playerModels[currentPlayer],playerAnimators[currentPlayer], playerUnit.transform.position + new Vector3(playerWidths[currentPlayer]/2, playerHeights[currentPlayer]/2)));
        
        yield return new WaitForSeconds(2f);

        if(knockedPlayers[currentPlayer] != true)
            SoundManagerScript.PlaySound(playerParty.Fighters[currentPlayer].Base.EntranceSound);

        if(partyTurn) 
        {
            state = BattleState.PlayerAction;
        }
        else 
        {
            partyTurn = true;
            state = BattleState.EnemyMove;
        }

    }

    void HandleActionSelection() 
    {
        charIconAnimators[currentPlayer].SetBool("turn",true);
        if(knockedPlayers[currentPlayer] == true) 
        {
            state = BattleState.PlayerWakeUp;
        } else {
            
            if(initializedActions == false) {
                List<Button> buttons =  actionMenu.ActionButtons;
                for (int i = 0; i < buttons.Count; i++)
                {
                    int x = i;
                    buttons[x].onClick.AddListener(delegate {Action(x);});
                }
                initializedActions = true;
            }
            actionMenu.EnableActionMenu(true);
        }
    }
    
    void Action(int currentAction) {
        if(currentAction == 0) {
            state = BattleState.PlayerAttack;
            //Attack
            //Debug.Log("Attack");
        }
        if(currentAction == 1) {
            state = BattleState.PlayerSkill;
            //Skill
            //Debug.Log("Skill");
            PlayerSkill();
        }
        if(currentAction == 2) {
            state = BattleState.PlayerWait;
            StartCoroutine(Wait());
            actionMenu.EnableActionMenu(false);
        }
    }


    void HandleSkillSelection() 
    {
        if(initializedSkills == false){
            Debug.Log("initializing buttons from " + skillMenu.SkillMenuButtons.Count);
            List<Button> buttons =  skillMenu.SkillMenuButtons;
            for (int i = 0; i < buttons.Count; i++)
            {
                int x = i;
                buttons[x].onClick.AddListener(delegate {Skill(x);});
            }
            initializedSkills = true;
        }
    }

    void HandleCharSelection()
    {
        if(initializedCharSel == false) {
            List<Button> buttons = actionMenu.SelectButtons;
            for (int i = 0; i < buttons.Count; i++)
            {
                int x = i;
                buttons[x].onClick.AddListener(delegate {chooseCharacter(x);});
            }
            initializedCharSel = true;
        }
        actionMenu.EnableSelectMenu(true);
    }
    
    void Skill(int currentSkill) {

        //Close menu button
        if(currentSkill == 0) {
            actionMenu.EnableSkillMenu(false);
            PlayerAction();
        } 
        //Choose skill buttons
        else {
            currentAction = currentSkill;
            move = playerParty.Fighters[currentPlayer].Moves[currentAction-1];
            if((move.Base.Type == FighterType.Physical && move.Cost >= playerParty.Fighters[currentPlayer].HP) || (move.Base.Type != FighterType.Physical && move.Cost >= playerParty.Fighters[currentPlayer].MP))
            {
                SoundManagerScript.PlaySound(uiSounds.Fail,1f);
            }
            else {
                actionMenu.EnableSkillMenu(false);
                StartCoroutine(PerformPlayerSkill());
            }
        }
    }


    void chooseCharacter(int arrow)
    {
        
        if(arrow == 0 && validChange) //Forward
        {
            Debug.Log("Curently have " + chosenPlayer);
            StartCoroutine(ShowCharacter());
            Debug.Log("Chose next");
        }
        else if(arrow == 1 && validChange) //Backward
        {
            StartCoroutine(ShowCharacter());
            Debug.Log("Chose prev");
        }
        else if(arrow == 2 && validChange)
        {
            actionMenu.EnableSelectMenu(false);
            state = BattleState.PlayerSupport;
        }
        else
        {
            if(validChange) 
            {
                StartCoroutine(ReturnOriginalCharacter());
                actionMenu.EnableSelectMenu(false);
                PlayerAction();
            }
        }
    }


    string AttackTypeString(Move move)
    {
        if(move.Base.Type == FighterType.Physical) 
            return "Physical";
        
        if(move.Base.Type == FighterType.Electric) 
            return "Lightning";
        
        if(move.Base.Type == FighterType.Fire) 
            return "Fire";
        
        if(move.Base.Type == FighterType.Ice) 
            return "Ice";
        
        if(move.Base.Type == FighterType.Wind) 
            return "Wind";
        
        return "";

    }

    void playEffectSound(FighterType t) 
    {
        Debug.Log("Playing effect");
        switch(t) {
            case FighterType.Physical:
                SoundManagerScript.PlaySound(uiSounds.Physical,1);
                break;

            case FighterType.Electric:
                SoundManagerScript.PlaySound(uiSounds.Electric,1);
                break;

            case FighterType.Fire:
                SoundManagerScript.PlaySound(uiSounds.Fire,1);
                break;

            case FighterType.Ice:
                SoundManagerScript.PlaySound(uiSounds.Ice,1);
                break;

            case FighterType.Wind:
                SoundManagerScript.PlaySound(uiSounds.Wind,1);
                break;

            case FighterType.Heal:
                SoundManagerScript.PlaySound(uiSounds.Heal);
                break;
        }
    }

    IEnumerator ShowCharacter() 
    {
        state = BattleState.Busy;
        
        if(playerParty.GetAliveCount() > 1) 
        {
            validChange = false;
            int aux = playerParty.GetNextAlive(chosenPlayer);
            selectedPlayer.text = playerParty.Fighters[aux].Base.Name; 
            Debug.Log("Character " + chosenPlayer + " exits, and character " + aux + " enters");

            //Current player exits scene
            var objective = playerModels[chosenPlayer].transform.position - new Vector3(10,0,0);
            while(Vector3.Distance(playerModels[chosenPlayer].transform.position, objective) > 0) {
                playerModels[chosenPlayer].transform.position = Vector3.MoveTowards(playerModels[chosenPlayer].transform.position, objective ,(speed + 30)* Time.deltaTime);
                yield return null;
            }
            
            //Selected target enters scene
            while(Vector3.Distance(playerModels[aux].transform.position, playerPositions[aux]) > 0) {
                playerModels[aux].transform.position = Vector3.MoveTowards(playerModels[aux].transform.position, playerPositions[aux] ,(speed + 30)* Time.deltaTime);
                yield return null;
            }
            chosenPlayer = aux;
            
            Debug.Log("Chosen character is " + chosenPlayer);
            validChange = true;
            }
        state = BattleState.ChoosePlayer;
    }

    IEnumerator PlayerSupport() 
    {
        state = BattleState.Busy;
        playerParty.Fighters[currentPlayer].skillCost(move);
        charIcons[currentPlayer].UpdateHP();

        if(chosenPlayer != currentPlayer) {
            var objective = playerModels[chosenPlayer].transform.position + new Vector3(5,0,0);
            //target moves forward
            while(Vector3.Distance(playerModels[chosenPlayer].transform.position, objective) > 0) {
                playerModels[chosenPlayer].transform.position = Vector3.MoveTowards(playerModels[chosenPlayer].transform.position, objective ,(speed + 10)* Time.deltaTime);
                yield return null;
            }
            //Selected target enters scene
            while(Vector3.Distance(playerModels[currentPlayer].transform.position, playerPositions[currentPlayer]) > 0) {
                playerModels[currentPlayer].transform.position = Vector3.MoveTowards(playerModels[currentPlayer].transform.position, playerPositions[currentPlayer] ,(speed + 10)* Time.deltaTime);
                yield return null;
            }

            playerAnimators[currentPlayer].SetTrigger("skill");
            SoundManagerScript.PlaySound(uiSounds.Summon);
            SoundManagerScript.PlaySound(playerParty.Fighters[currentPlayer].Base.AttackSound,1f);
            yield return new WaitForSeconds(2.5f);
            particleEffect.transform.position = playerModels[chosenPlayer].transform.position;
            particleAnimator.SetTrigger("Heal");
            playEffectSound(move.Base.Type);
            yield return new WaitForSeconds(0.2f);
            dmgIcon.transform.position = playerModels[chosenPlayer].transform.position + new Vector3(0,1,0);
            dmgIcon.SetHP((float) playerParty.Fighters[chosenPlayer].HP/playerParty.Fighters[chosenPlayer].MaxHp, 50);
            dmgIconAnimator.SetTrigger("normal");

            playerParty.Fighters[chosenPlayer].Heal();
            charIcons[chosenPlayer].UpdateHP();
            yield return new WaitForSeconds(1f);

            //target returns to back of the scene
            objective = playerModels[chosenPlayer].transform.position - new Vector3(15,0,0);
            while(Vector3.Distance(playerModels[chosenPlayer].transform.position, objective) > 0) {
                playerModels[chosenPlayer].transform.position = Vector3.MoveTowards(playerModels[chosenPlayer].transform.position, objective ,(speed + 10)* Time.deltaTime);
                yield return null;
            }
        } else {
            playerAnimators[currentPlayer].SetTrigger("skill");
            SoundManagerScript.PlaySound(uiSounds.Summon);
            SoundManagerScript.PlaySound(playerParty.Fighters[currentPlayer].Base.AttackSound,1f);

            yield return new WaitForSeconds(2.5f);
            
            playerParty.Fighters[currentPlayer].Heal();
            charIcons[currentPlayer].UpdateHP();

            particleEffect.transform.position = playerModels[currentPlayer].transform.position;
            particleAnimator.SetTrigger("Heal");
            playEffectSound(move.Base.Type);
            yield return new WaitForSeconds(0.2f);
            dmgIcon.transform.position = playerModels[currentPlayer].transform.position + new Vector3(0,1,0);
            dmgIcon.SetHP((float) playerParty.Fighters[currentPlayer].HP/playerParty.Fighters[currentPlayer].MaxHp, 50);
            dmgIconAnimator.SetTrigger("normal");

            playerParty.Fighters[chosenPlayer].Heal();
            charIcons[chosenPlayer].UpdateHP();
            yield return new WaitForSeconds(1f);
        }

        charIconAnimators[currentPlayer].SetBool("turn",false);

        if(playerParty.GetNextAlive(currentPlayer) == 0) {
            partyTurn = false;
        }

        if(playerParty.GetAliveCount() == 1) 
        {
            state = BattleState.EnemyMove;
        } 
        else 
        {
            state = BattleState.Switching;
        }

    }

    IEnumerator ReturnOriginalCharacter()
    {
        state = BattleState.Busy;
        if(chosenPlayer != currentPlayer)
        {
            var objective = playerModels[chosenPlayer].transform.position - new Vector3(10,0,0);
            while(Vector3.Distance(playerModels[chosenPlayer].transform.position, objective) > 0) {
                playerModels[chosenPlayer].transform.position = Vector3.MoveTowards(playerModels[chosenPlayer].transform.position, objective ,(speed + 30)* Time.deltaTime);
                yield return null;
            }
            
            //Selected target enters scene
            while(Vector3.Distance(playerModels[currentPlayer].transform.position, playerPositions[currentPlayer]) > 0) {
                playerModels[currentPlayer].transform.position = Vector3.MoveTowards(playerModels[currentPlayer].transform.position, playerPositions[currentPlayer] ,(speed + 30)* Time.deltaTime);
                yield return null;
            }
            selectedPlayer.text = playerParty.Fighters[currentPlayer].Base.Name; 
            chosenPlayer = currentPlayer;
            
        }
        yield return null;
    }
    void spawnCharIcons() 
    {
        int ypos = 0;
        for(int i = 0; i < playerParty.Fighters.Count; i++) {
            BattleHud charIcon = Instantiate(iconTemplate);
            charIcon.transform.SetParent(iconBox.transform, false);
            charIcon.transform.localPosition = new Vector3 (320, 170 - ypos, 0);
            charIcon.SetData(playerParty.Fighters[i]);

            charIconAnimators.Add(charIcon.GetComponent<Animator>());

            charIcons.Add(charIcon);
            ypos += 110;
        }
    }

    //Function to spawn the players
    void spawnPlayers()
    {
        bool isPlayerUnit = true;
        for(int i = 0; i < playerParty.Fighters.Count;i++)
        {
            GameObject model = Instantiate(playerParty.Fighters[i].Base.FModel);

            model.tag = "EnemyModel";

            if(isPlayerUnit)
                model.tag = "PlayerModel";
            
            
            GameObject characterSprite = model.transform.GetChild(0).gameObject;
            float width = characterSprite.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
            float height = characterSprite.GetComponentInChildren<SpriteRenderer>().bounds.size.y;

            playerHeights.Add(height);
            playerWidths.Add(width);

            model.transform.position = playerUnit.transform.position + new Vector3(width/2, height/2, 0);


            playerPositions.Add(playerUnit.transform.position + new Vector3(width/2, height/2, 0));

            if(isPlayerUnit){
                model.transform.position = playerUnit.transform.position + new Vector3(width/2, height/2, 0) + new Vector3(-10,0,0);
                model.transform.rotation = Quaternion.Euler(0,180f,0);
            }
            playerModels.Add(model);
            knockedPlayers.Add(false);
            playerAnimators.Add(model.GetComponent<Animator>());
        }
    }
}