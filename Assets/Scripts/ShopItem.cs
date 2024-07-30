using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    public int index;
    public int prise;
    public TextMeshProUGUI itemText;
    public Image ItemImage;

    public GameObject DogUnlockPanel;
    public int coin;
    public int coins;

    public static ShopItem instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void OnClickBtn()
    {
        AudioManager.instance.buttonAudio.Play();
        if (itemText.text != "Select" || itemText.text == "Selected")
        {
            coin = PlayerPrefs.GetInt("Coin");
            coin = coin - (int.Parse(itemText.text));
            if (coin < 0)
            {
                DogUnlockPanel.SetActive(true);
                return;
            }
            PlayerPrefs.SetInt("Coin", coin);
            ShopPanel.Instance.coinTxt.text = coin.ToString();
            itemText.text = "Select";
            PlayerPrefs.SetInt(index + "_ShopItem", 1);
        }
        else
        {
            for (int i = 0; i < ShopPanel.Instance.shopItems.Count; i++)
            {
                if (i == (index - 1))
                {
                    itemText.text = "";
                    ShopPanel.Instance.shopItems[i].ItemImage.sprite = ShopPanel.Instance.SelectImage;
                    PlayerPrefs.SetInt("PlayerSkin", index - 1);
                }
                else
                {
                    if (PlayerPrefs.GetInt(ShopPanel.Instance.shopItems[i].index + "_ShopItem") == 1)
                    {
                        ShopPanel.Instance.shopItems[i].itemText.text = "Select";
                        ShopPanel.Instance.shopItems[i].ItemImage.sprite = ShopPanel.Instance.unSelectImage;
                    }
                    else
                    {
                        ShopPanel.Instance.shopItems[i].itemText.text = ShopPanel.Instance.shopItems[i].prise.ToString();
                        ShopPanel.Instance.shopItems[i].ItemImage.sprite = ShopPanel.Instance.unSelectImage;
                    }
                }
            }
        }
    }

    public void UnlockPanelOff()
    {
        DogUnlockPanel.SetActive(false);
    }

    public void UnlockDog()
    {
        CoinAdded(true);
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!" + coin);

    }

    public void CoinAdded(bool complete)
    {
        if (complete)
        {
            DogUnlockPanel.SetActive(false);
            coins = PlayerPrefs.GetInt("Coin");
            coins += 100;
            PlayerPrefs.SetInt("Coin", coins);
            ShopPanel.Instance.coinTxt.text = PlayerPrefs.GetInt("Coin",coins).ToString();
        }
    }
}
