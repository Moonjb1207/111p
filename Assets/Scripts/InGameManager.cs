using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Inst = null;

    int Score = 0;
    int Life = 5;
    int maxLife = 5;

    public TMPro.TMP_Text Score_t;
    public Image fillSkill;
    public Image[] lifeImage = new Image[5];
    public Slider gameTime;

    float regenterm = 3.0f;
    float curterm = 0.0f;

    public GameObject regenPos;
    public GameObject Gameover;
    public GameObject Clear;

    public List<GameObject> curGen = new List<GameObject>();

    float fullTime = 60.0f;
    public float secondWeapon = 40.0f;
    public float curWeapon = 0.0f;
    
    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Score_t.text = Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (curGen.Count == 0)
        {
            curterm += Time.deltaTime;
        }

        if (curterm > regenterm)
        {
            curterm = 0.0f;
            regen();
        }
        
        if (Life <= 0)
        {
            Gameover.SetActive(true);
        }

        curWeapon += Time.deltaTime;
        gameTime.value = curWeapon / fullTime;

        if (curWeapon > fullTime)
        {
            Clear.SetActive(true);
        }
    }

    public void changeScore(int n)
    {
        Score += n;

        Score_t.text = Score.ToString();
    }

    public void changeLife(int n)
    {
        Life += n;
        if (Life > maxLife)
        {
            Life = maxLife;
        }

        for (int i = 0; i < maxLife; i++)
        {
            lifeImage[i].gameObject.SetActive(false);
        }
        
        for (int i = 0; i < Life; i++)
        {
            lifeImage[i].gameObject.SetActive(true);
        }
    }

    public void changeSkillfill(int cur, int max)
    {
        fillSkill.fillAmount = cur / max;
    }

    void regen()
    {
        Vector3 pos = regenPos.transform.position;

        int rnd = Random.Range(1, 6);

        GameObject obj = Instantiate(Resources.Load("Prefabs/Enemy_" + rnd) as GameObject, pos, Quaternion.identity);

        obj.GetComponentInChildren<Enemy>().gameObject.layer = 10;

        curGen.Add(obj);

        for (int i = 0; i < 9; i++)
        {
            pos += new Vector3(0.0f, 1.0f, 0.0f);
            rnd = Random.Range(1, 6);

            obj = Instantiate(Resources.Load("Prefabs/Enemy_" + rnd) as GameObject, pos, Quaternion.identity);
            curGen.Add(obj);
        }
    }
}
