using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField]
    private RectTransform healthBar;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text timerText, timerWin;
    [SerializeField]
    private GameObject deathPanel, winPanel;

    [SerializeField]
    private float seconds = 0;

    private void Awake()
    {
        instance = this;
    }

    public void StartRun()
    {
        if (seconds != 0)
            return;

        ThirdPersonCamera.instance.GetComponent<AudioSource>().enabled = true;
        StartCoroutine(Timer());
    }

    public void EndRun()
    {
        ThirdPersonCamera.instance.GetComponent<AudioSource>().enabled = false;
        StopAllCoroutines();

        winPanel.SetActive(true);
        PlayerController.instance.enabled = false;
        PlayerController.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ThirdPersonCamera.instance.canRotate = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        timerWin.text = ((int)(seconds * 100) / 100f).ToString() + " seconds";
    }


    public void PlayerLost()
    {
        ThirdPersonCamera.instance.GetComponent<AudioSource>().enabled = false;
        StopAllCoroutines();
        deathPanel.SetActive(true);
        PlayerController.instance.enabled = false;
        PlayerController.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ThirdPersonCamera.instance.canRotate = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart()
    {
        SceneManager.LoadScene("test level");
    }

    public void ChangeHealthBar(int currentHealth, int maxHealth)
    {
        healthBar.localScale = new Vector3((float)currentHealth / maxHealth, 1, 1);
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public IEnumerator Timer()
    {
        while (true)
        {
            yield return null;
            seconds += Time.deltaTime;

            string sec = ((int)seconds).ToString();
            sec = sec.Length > 1 ? sec : "0" + sec;

            int msecInt = (int)((seconds - (int)seconds) * 100);
            string msec = msecInt < 10 ? "0" + msecInt.ToString() : msecInt.ToString();

            timerText.text = sec + ":" + msec;
        }
    }
}
