using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    public RectTransform enemyUI1;
    public RectTransform enemyUI2;
    public RectTransform playerUI1;
    public RectTransform playerUI2;
    public TurnState currentTurn = TurnState.PlayerTurn;
    public List<RectTransform> buttonPanels;
    public EnemySelect currentEnemy = EnemySelect.enemy1;
    private bool actionInProgress = false;
    public List<Enemy> enemies; // Gán enemy1, enemy2 trong Inspector

    private EnemyHealth enemyHealth1;
    private EnemyHealth enemyHealth2;
    public List<Player> players;
    public PlayerSelect currentPlayer = PlayerSelect.player1;
    public Animator aniPlayer;

    [Header("Winner")]
    public bool playerWin = false;
    public bool enemyWin = false;
    [Header("Game mode")]
    public bool onevsOne = false;
    public bool onevsMany = false;
    public bool manyvsMany = false;
    [Header("Index Player & Enemy")]
    public int currentEnemyIndex = 0;
    public int currentPlayerIndex = 0;
    [Header("Enemy Dead")]
    public bool enemy1Dead = false;
    public bool enemy2Dead = false;
    [Header("Player Dead")]
    public bool player1Dead = false;
    public bool player2Dead = false;
    [Header("UI")]
    public GameObject UIWin;
    public GameObject UILose;
    public TMP_Text txtError;
    public Button btnNextLV;
    public Button btnRestart;
    //public Animator aniEnermy;
    void Start()
    {
        btnNextLV.onClick.AddListener(LevelGenerator.Instance.NextLevel);
        btnRestart.onClick.AddListener(LevelGenerator.Instance.RestartLevel);
        //UIWinne.SetActive(false);
        if (GameObject.Find("Enemy") != null)
        {
            onevsOne = true;
        }
        if (GameObject.Find("Enemy2") != null && GameObject.Find("Player2") == null)
        {
            onevsMany = true;
        }
        if (GameObject.Find("Player2") != null)
        {
            manyvsMany = true;
        }

        if (onevsOne)
        {
            enemyHealth1 = enemies[0].GetComponent<EnemyHealth>();
        }
        else if (onevsMany)
        {
            enemyHealth1 = enemies[0].GetComponent<EnemyHealth>();
            enemyHealth2 = enemies[1].GetComponent<EnemyHealth>();
        }
        else if (manyvsMany)
        {
            enemyHealth1 = enemies[0].GetComponent<EnemyHealth>();
            enemyHealth2 = enemies[1].GetComponent<EnemyHealth>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (onevsOne)
        {
            if (enemy1Dead)
            {
                players[0].GetComponent<PlayerHealth>().IsWin();
                Invoke("PlayerWin", 3f);
                actionInProgress = true;
            }
            if (players[0].GetComponent<PlayerHealth>().currentHealth <= 0)
            {
                enemies[0].GetComponent<EnemyHealth>().IsWin();
                Invoke("EnemyWin", 3f);
                actionInProgress = true;
            }
            if (currentTurn == TurnState.EnemyTurn && !actionInProgress)
            {
                actionInProgress = true;
                if (currentEnemy == EnemySelect.enemy2)
                {

                    SwapEnemyUI();
                    currentEnemyIndex = 1;
                }
                Enemy currentEnemyL = enemies[currentEnemyIndex];

                StartCoroutine(currentEnemyL.Attack(() =>
                {
                    currentTurn = TurnState.PlayerTurn;
                    actionInProgress = false;
                    UpdateButtonInteractability();

                }));
            }
        }
        if (onevsMany)
        {
            if (enemy1Dead && enemy2Dead)
            {
                //if (currentPlayer == PlayerSelect.player1)
                //{
                players[0].GetComponent<PlayerHealth>().IsWin();
                //}
                //else
                //{
                //    players[1].GetComponent<PlayerHealth>().isWin();
                //}

                Invoke("PlayerWin", 3f);
                actionInProgress = true;
            }

            if (players[0].GetComponent<PlayerHealth>().currentHealth <= 0)
            {
                if (currentEnemy == EnemySelect.enemy1)
                {
                    enemies[0].GetComponent<EnemyHealth>().IsWin();
                }
                else
                {
                    enemies[1].GetComponent<EnemyHealth>().IsWin();
                }
                Invoke("EnemyWin", 3f);
                actionInProgress = true;
            }
            if (currentTurn == TurnState.EnemyTurn && !actionInProgress)
            {
                actionInProgress = true;
                if (currentEnemy == EnemySelect.enemy2)
                {
                    SwapEnemyUI();
                }
                Enemy currentEnemyL = enemies[currentEnemyIndex];

                StartCoroutine(currentEnemyL.Attack(() =>
                {
                    if (!enemy1Dead && !enemy2Dead)
                    {
                        currentEnemyIndex++;
                        SwapEnemyUI();
                        // Nếu còn enemy thì tiếp tục lượt enemy
                        if (currentEnemyIndex < enemies.Count)
                        {
                            actionInProgress = false;
                        }
                        else
                        {
                            // Hết lượt enemy, chuyển sang player
                            //if (!enemy1Dead && enemy2Dead)
                            //{
                            //    currentEnemyIndex = 0;
                            //}
                            //else if (!enemy2Dead && enemy1Dead)
                            //{
                            //    currentEnemyIndex = 1;
                            //}

                            currentTurn = TurnState.PlayerTurn;
                            actionInProgress = false;
                            UpdateButtonInteractability();
                        }
                    }
                    else
                    {
                        currentTurn = TurnState.PlayerTurn;
                        actionInProgress = false;
                        UpdateButtonInteractability();
                    }


                }));
            }
        }
        if (manyvsMany)
        {
            if (enemy1Dead && enemy2Dead)
            {
                if (currentPlayer == PlayerSelect.player1)
                {

                    players[0].GetComponent<PlayerHealth>().IsWin();
                }
                else
                {

                    players[1].GetComponent<PlayerHealth>().IsWin();
                }
                actionInProgress = true;
                Invoke("PlayerWin", 3f);
            }

            if (players[0].GetComponent<PlayerHealth>().currentHealth <= 0 && players[1].GetComponent<PlayerHealth>().currentHealth <= 0)
            {
                if (currentEnemy == EnemySelect.enemy1)
                {
                    enemies[0].GetComponent<EnemyHealth>().IsWin();
                }
                else
                {
                    enemies[1].GetComponent<EnemyHealth>().IsWin();
                }
                Invoke("EnemyWin", 3f);
            }
            else if (players[0].GetComponent<PlayerHealth>().currentHealth <= 0 && players[1].GetComponent<PlayerHealth>().currentHealth > 0)
            {
                Invoke("DelayPlayer1Dead", 3f);
            }
            if (currentTurn == TurnState.EnemyTurn && !actionInProgress)
            {
                actionInProgress = true;
                if (currentEnemy == EnemySelect.enemy2)
                {

                    SwapEnemyUI();


                }
                Enemy currentEnemyL = enemies[currentEnemyIndex];

                StartCoroutine(currentEnemyL.Attack(() =>
                {
                    if (!enemy1Dead && !enemy2Dead)
                    {
                        currentEnemyIndex++;
                        SwapEnemyUI();
                        // Nếu còn enemy thì tiếp tục lượt enemy
                        if (currentEnemyIndex < enemies.Count)
                        {
                            actionInProgress = false;
                        }
                        else
                        {
                            // Hết lượt enemy, chuyển sang player
                            //if (!enemy1Dead && enemy2Dead)
                            //{
                            //    currentEnemyIndex = 0;
                            //}
                            //else if (!enemy2Dead && enemy1Dead)
                            //{
                            //    currentEnemyIndex = 1;
                            //}

                            currentTurn = TurnState.PlayerTurn;
                            actionInProgress = false;
                            UpdateButtonInteractability();
                        }
                    }
                    else
                    {
                        currentTurn = TurnState.PlayerTurn;
                        actionInProgress = false;
                        UpdateButtonInteractability();
                    }


                }));
            }
        }

    }
    public void PlayerWin()
    {
        playerWin = true;
        UIWin.SetActive(true);
    }
    public void EnemyWin()
    {
        enemyWin = true;
        UILose.SetActive(true);
    }
    public void PlayerAttack(int attackType)
    {
        if (currentTurn != TurnState.PlayerTurn && actionInProgress) return;

        if (attackType == 1)
        {
            players[currentPlayerIndex].HeadPunch(() =>
            {
                if (onevsOne)
                {
                    currentPlayerIndex = 0;
                    currentTurn = TurnState.EnemyTurn;
                    actionInProgress = false;
                    UpdateButtonInteractability();
                }
                if (onevsMany)
                {
                    currentPlayerIndex = 0;
                    currentTurn = TurnState.EnemyTurn;
                    actionInProgress = false;
                    UpdateButtonInteractability();
                }
                if (manyvsMany)
                {
                    currentPlayerIndex++;
                    SwapPlayerUI();
                    if (currentPlayerIndex < players.Count)
                    {
                        actionInProgress = false;
                    }
                    else
                    {
                        currentPlayerIndex = 0;
                        currentTurn = TurnState.EnemyTurn;
                        actionInProgress = false;
                        UpdateButtonInteractability();
                    }
                }

            });
        }
        else if (attackType == 2)
        {
            players[currentPlayerIndex].StomachPunch(() =>
            {
                if (onevsOne)
                {
                    currentPlayerIndex = 0;
                    currentTurn = TurnState.EnemyTurn;
                    actionInProgress = false;
                    UpdateButtonInteractability();
                }
                if (onevsMany)
                {
                    currentPlayerIndex = 0;
                    currentTurn = TurnState.EnemyTurn;
                    actionInProgress = false;
                    UpdateButtonInteractability();
                }
                if (manyvsMany)
                {
                    currentPlayerIndex++;
                    SwapPlayerUI();
                    if (currentPlayerIndex < players.Count)
                    {
                        actionInProgress = false;
                    }
                    else
                    {
                        currentPlayerIndex = 0;
                        currentTurn = TurnState.EnemyTurn;
                        actionInProgress = false;
                        UpdateButtonInteractability();
                    }
                }
            });
        }
        else if (attackType == 3)
        {
            players[currentPlayerIndex].KidneyPunch(() =>
            {
                if (onevsOne)
                {
                    currentPlayerIndex = 0;
                    currentTurn = TurnState.EnemyTurn;
                    actionInProgress = false;
                    UpdateButtonInteractability();
                }
                if (onevsMany)
                {
                    currentPlayerIndex = 0;
                    currentTurn = TurnState.EnemyTurn;
                    actionInProgress = false;
                    UpdateButtonInteractability();
                }
                if (manyvsMany)
                {
                    currentPlayerIndex++;
                    SwapPlayerUI();
                    if (currentPlayerIndex < players.Count)
                    {
                        actionInProgress = false;
                    }
                    else
                    {
                        currentPlayerIndex = 0;
                        currentTurn = TurnState.EnemyTurn;
                        actionInProgress = false;
                        UpdateButtonInteractability();
                    }
                }
            });
        }
        if (onevsOne)
        {
            if (enemyHealth1.currentHealth <= 0 && !enemy1Dead)
            {
                Invoke("DelayEnemy1Dead", 3f);
            }
        }
        if (onevsMany)
        {
            if (enemyHealth1.currentHealth <= 0 && !enemy1Dead)
            {
                Invoke("DelayEnemy1Dead", 3f);
            }
            else if (enemyHealth2.currentHealth <= 0 && !enemy2Dead)
            {
                Invoke("DelayEnemy2Dead", 3f);
            }
            else if (!enemy1Dead && !enemy2Dead)
            {
                currentEnemyIndex = 0;
            }
        }
        if (manyvsMany)
        {
            if (enemyHealth1.currentHealth <= 0 && !enemy1Dead)
            {
                Invoke("DelayEnemy1Dead", 3f);
            }
            else if (enemyHealth2.currentHealth <= 0 && !enemy2Dead)
            {
                Invoke("DelayEnemy2Dead", 3f);
            }
            else if (!enemy1Dead && !enemy2Dead)
            {
                currentEnemyIndex = 0;
            }
        }

    }
    public void DelayPlayer1Dead()
    {
        SwapPlayerUI();
        player1Dead = true;
        currentPlayerIndex = 1;
    }
    public void DelayEnemy1Dead()
    {
        SwapEnemyUI();
        enemy1Dead = true;
        currentEnemyIndex = 1;
    }
    public void DelayEnemy2Dead()
    {
        SwapEnemyUI();
        enemy2Dead = true;
        currentEnemyIndex = 0;
    }
    public void SwapEnemyUI()
    {
        if (manyvsMany || onevsMany)
        {
            if (!enemy1Dead && !enemy2Dead)
            {
                Image panel1 = enemyUI1.transform.Find("Panel").gameObject.GetComponent<Image>();
                Image panel2 = enemyUI2.transform.Find("Panel").gameObject.GetComponent<Image>();
                Vector2 pos1 = enemyUI1.anchoredPosition;
                Vector2 pos2 = enemyUI2.anchoredPosition;

                Vector3 scale1 = enemyUI1.localScale;
                Vector3 scale2 = enemyUI2.localScale;

                float duration = 0.5f;

                // Hoán đổi vị trí
                enemyUI1.DOAnchorPos(pos2, duration).SetEase(Ease.InOutSine);
                enemyUI2.DOAnchorPos(pos1, duration).SetEase(Ease.InOutSine);

                // Hoán đổi scale
                enemyUI1.DOScale(scale2, duration).SetEase(Ease.InOutSine);
                enemyUI2.DOScale(scale1, duration).SetEase(Ease.InOutSine);

                if (scale1.x > scale2.x)
                {
                    panel1.enabled = true;
                    panel2.enabled = false;
                    currentEnemy = EnemySelect.enemy2;
                    enemies[1].transform.GetChild(0).gameObject.SetActive(true);
                    enemies[1].transform.GetChild(1).gameObject.SetActive(true);
                    enemies[0].transform.GetChild(0).gameObject.SetActive(false);
                    enemies[0].transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    panel2.enabled = true;
                    panel1.enabled = false;
                    currentEnemy = EnemySelect.enemy1;
                    enemies[1].transform.GetChild(0).gameObject.SetActive(false);
                    enemies[1].transform.GetChild(1).gameObject.SetActive(false);
                    enemies[0].transform.GetChild(0).gameObject.SetActive(true);
                    enemies[0].transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            else if (enemy1Dead && !enemy2Dead)
            {
                Vector3 scale1 = enemyUI1.localScale;
                Vector3 scale2 = enemyUI2.localScale;
                if (scale1.x > scale2.x)
                {
                    Vector2 pos1 = enemyUI1.anchoredPosition;
                    Vector2 pos2 = enemyUI2.anchoredPosition;

                    //Vector3 scale1 = enemyUI1.localScale;
                    //Vector3 scale2 = enemyUI2.localScale;

                    float duration = 0.5f;

                    // Hoán đổi vị trí
                    enemyUI1.DOAnchorPos(pos2, duration).SetEase(Ease.InOutSine);
                    enemyUI2.DOAnchorPos(pos1, duration).SetEase(Ease.InOutSine);

                    // Hoán đổi scale
                    enemyUI1.DOScale(scale2, duration).SetEase(Ease.InOutSine);
                    enemyUI2.DOScale(scale1, duration).SetEase(Ease.InOutSine);
                }
            }
        }

    }
    public void SwapPlayerUI()
    {
        if (!player1Dead && !player2Dead)
        {
            Image panel1 = playerUI1.transform.Find("Panel").gameObject.GetComponent<Image>();
            Image panel2 = playerUI2.transform.Find("Panel").gameObject.GetComponent<Image>();
            Vector2 pos1 = playerUI1.anchoredPosition;
            Vector2 pos2 = playerUI2.anchoredPosition;

            Vector3 scale1 = playerUI1.localScale;
            Vector3 scale2 = playerUI2.localScale;

            float duration = 0.5f;

            // Hoán đổi vị trí
            playerUI1.DOAnchorPos(pos2, duration).SetEase(Ease.InOutSine);
            playerUI2.DOAnchorPos(pos1, duration).SetEase(Ease.InOutSine);

            // Hoán đổi scale
            playerUI1.DOScale(scale2, duration).SetEase(Ease.InOutSine);
            playerUI2.DOScale(scale1, duration).SetEase(Ease.InOutSine);

            if (scale1.x > scale2.x)
            {
                panel1.enabled = true;
                panel2.enabled = false;
                currentPlayer = PlayerSelect.player2;
                players[1].transform.GetChild(0).gameObject.SetActive(true);
                players[1].transform.GetChild(1).gameObject.SetActive(true);
                players[0].transform.GetChild(0).gameObject.SetActive(false);
                players[0].transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                panel2.enabled = true;
                panel1.enabled = false;
                currentPlayer = PlayerSelect.player1;
                players[1].transform.GetChild(0).gameObject.SetActive(false);
                players[1].transform.GetChild(1).gameObject.SetActive(false);
                players[0].transform.GetChild(0).gameObject.SetActive(true);
                players[0].transform.GetChild(1).gameObject.SetActive(true);
            }
            GameObject parent1 = playerUI1.transform.parent.gameObject;
            GameObject parent2 = playerUI2.transform.parent.gameObject;
            int index1 = parent1.transform.GetSiblingIndex();
            int index2 = parent2.transform.GetSiblingIndex();

            parent1.transform.SetSiblingIndex(index2);
            parent2.transform.SetSiblingIndex(index1);
        }

    }


    void ShowButtons()
    {
        for (int i = 0; i < buttonPanels.Count; i++)
        {
            buttonPanels[i].gameObject.SetActive(true);
            buttonPanels[i].DOAnchorPosY(165f, 0.5f).SetEase(Ease.OutBack); // Từ trên trượt xuống
        }

    }

    void HideButtons()
    {
        for (int i = 0; i < buttonPanels.Count; i++)
        {
            buttonPanels[i].DOAnchorPosY(-130f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                //buttonPanel.gameObject.SetActive(false);
            });
        }

    }
    void UpdateButtonInteractability()
    {
        bool isPlayerTurn = currentTurn == TurnState.PlayerTurn && !actionInProgress;
        if (isPlayerTurn)
            ShowButtons();
        else
            HideButtons();
    }

    private IEnumerator WaitForAnimation(Action onComplete)
    {
        yield return new WaitForSeconds(3.0f); // thời gian animation
        onComplete?.Invoke();
    }
    public void btnSwap()
    {
        ShowAndFadeOut();
    }
    public void ShowAndFadeOut()
    {
        // Reset alpha về 0 (ẩn trước)
        //txtError.alpha = 0;

        // Hiện lên trong 0.5s
        txtError.DOFade(1, 0.5f).OnComplete(() =>
        {
            // Sau khi hiện, đợi 2s rồi mờ dần trong 1s
            DOVirtual.DelayedCall(2f, () =>
            {
                txtError.DOFade(0, 1f);
            });
        });
    }
    public void Skip()
    {
        if (onevsOne || onevsMany)
        {
            if (currentTurn != TurnState.PlayerTurn || actionInProgress) return;

            currentTurn = TurnState.EnemyTurn;
            actionInProgress = false;
            UpdateButtonInteractability();
        }
        if (manyvsMany)
        {
            if (currentTurn != TurnState.PlayerTurn || actionInProgress) return;
            if (currentPlayer == PlayerSelect.player1)
            {
                currentPlayerIndex = 1;
                SwapPlayerUI();
            }
            else if (currentPlayer == PlayerSelect.player2)
            {
                currentTurn = TurnState.EnemyTurn;
                SwapPlayerUI();
                actionInProgress = false;
                UpdateButtonInteractability();
            }

        }
    }
}
