using UnityEngine;



class Data
{
    public string name;
    public int level;
    public int money;
    public bool skill;

}



public class JsonTest : MonoBehaviour
{
    Data data = new Data() { name = "text" , level = 1, money = 1,skill = false};



    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
