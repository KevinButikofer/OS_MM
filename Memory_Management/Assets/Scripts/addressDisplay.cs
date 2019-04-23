using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addressDisplay : MonoBehaviour
{
    public Dictionary<int, int> address;
    public Dictionary<int, GameObject> addressUi;
    public List<Text> texts;
    public Canvas canvas;
    public GameObject textPrefab;
    // Start is called before the first frame update
    void Start()
    {
        address = new Dictionary<int, int>();
        addressUi = new Dictionary<int, GameObject>();
        /*for(int i =0; i< 120; i++)
        {
            AddAdress(i, 120 - i);
        }*/
    }
    public void AddAdress(int key, int value)
    {
        if (address.ContainsKey(key))
            address[key] = value;
        else
        {
            address.Add(key, value);
            GameObject g = Instantiate(textPrefab, canvas.transform);
            g.GetComponent<Text>().text = key.ToString() + " -> " + value.ToString();
            addressUi.Add(key, g);
        }
        UpdateDisplay();
    }
    public void RemoveAdress(int key)
    {
        address.Remove(key);
        addressUi.Remove(key);
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        int i = 0;
        int j = 0;
        foreach (KeyValuePair<int, GameObject> item in addressUi)
        {
            if (i == 17 && j == 0)
            {
                j = 1;
                i = 0;
            }
            else if (i == 17)
            {
                j++;
                i = 0;
            }
            item.Value.transform.localPosition = new Vector3(-780.0f + j * 250, 320.0f - 40 * i, 0.0f);
            i++;
        }
    }

}
