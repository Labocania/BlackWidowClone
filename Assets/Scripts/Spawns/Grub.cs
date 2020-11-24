using System.Collections.Generic;
using UnityEngine;

public class Grub : MonoBehaviour
{
    public float timer = 5f;
    public int Score { get; private set; }

    float _timer;
    bool isChanging = true;
    ColorSwapper swapper;
    List<ColorNames> colors = new List<ColorNames> { ColorNames.LightBlue, ColorNames.Green, ColorNames.Blue };
    List<int> points = new List<int> { 250, 100, 50 };
    IEnumerator<ColorNames> colorEnum;
    IEnumerator<int> pointEnum;

    void Awake()
    {
        EventList.playerDeath += Grub_OnPlayerDeath;
        swapper = GetComponent<ColorSwapper>();
        swapper.ToggleObjectsColor(ColorNames.White);
        Score = 500;
        _timer = timer;
        colorEnum = colors.GetEnumerator();
        pointEnum = points.GetEnumerator();
    }

    private void Grub_OnPlayerDeath()
    {
        isChanging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChanging)
        {
            return;
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            if (colorEnum.MoveNext() && pointEnum.MoveNext())
            {
                swapper.ToggleObjectsColor(colorEnum.Current);
                Score = pointEnum.Current;
                _timer = timer;
            }
            else
            {
                _timer = 0;
            }            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventList.grubCollect.Invoke(Score);
            //TO DO: Object Pool for grubs
            Destroy(gameObject);
        }
    }
}
