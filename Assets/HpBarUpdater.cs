using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUpdater : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.isFacingRight)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        image.fillAmount = enemy.HP / enemy.maxHp;

        ChangeBarColor();

    }

    private void ChangeBarColor()
    {
        if (enemy.HP / enemy.maxHp < 0.3f)
        {
            image.color = Color.red;
        }
        else if (enemy.HP / enemy.maxHp < 0.9f)
        {
            image.color = new Color(241,137,0);
        }
    }
}
