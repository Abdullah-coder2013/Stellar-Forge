using UnityEngine;

public class Roulette : MonoBehaviour
{
    public float RotatePower;
    public float StopPower;

    private Rigidbody2D rbody;
    private int inRotate;

    [SerializeField] private BuildUI uiController;
    [SerializeField] private Experience experience;
    

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    private float t;
    private void Update()
    {           
        
        if (rbody.angularVelocity > 0)
        {
            rbody.angularVelocity -= StopPower*Time.deltaTime;

            rbody.angularVelocity =  Mathf.Clamp(rbody.angularVelocity, 0 , 1440);
        }

        if(rbody.angularVelocity == 0 && inRotate == 1) 
        {
            t +=1*Time.deltaTime;
            if(t >= 0.5f)
            {
                GetReward();

                inRotate = 0;
                t = 0;
            }
        }
    }

    public enum winType
    {
        material,
        energy,
        oil,
        exp,
        money
    }


    public void Rotate() 
    {
        if(inRotate == 0)
        {
            rbody.AddTorque(RotatePower);
            inRotate = 1;
        }
    }



    public void GetReward()
    {
        var rot = transform.eulerAngles.z;
        var rect = GetComponent<RectTransform>();
        if (rot > 0 && rot <= 30)
        {
            rect.eulerAngles = new Vector3(0, 0, 0);
            Win(599, winType.energy);
        }
        else if (rot > 30 && rot <= 60)
        {
            Win(850, winType.exp);
            rect.eulerAngles = new Vector3(0, 0, 30);
        }
        else if (rot > 60 && rot <= 90)
        {
            rect.eulerAngles = new Vector3(0, 0, 60);
            Win(755, winType.material);
        }
        else if (rot > 90 && rot <= 125)
        {
            rect.eulerAngles = new Vector3(0, 0, 90);
            Win(1567, winType.money);
        }
        else if (rot > 125 && rot <= 155)
        {
            rect.eulerAngles = new Vector3(0, 0, 125);
            Win(945, winType.oil);
        }
        else if (rot > 155 && rot <= 185)
        {
            rect.eulerAngles = new Vector3(0, 0, 155);
            Win(1346, winType.material);
        }
        else if (rot > 185 && rot <= 215)
        {
            rect.eulerAngles = new Vector3(0, 0, 185);
            Win(5000, winType.energy);
        }
        else if (rot > 215 && rot <= 245)
        {
            rect.eulerAngles = new Vector3(0, 0, 215);
            Win(6357, winType.material);
        }
        else if (rot > 245 && rot <= 275)
        {
            rect.eulerAngles = new Vector3(0, 0, 245);
            Win(2000, winType.money);
        }
        else if (rot > 275 && rot <= 305)
        {
            rect.eulerAngles = new Vector3(0, 0, 275);
            Win(1600, winType.oil);
        }
        else if (rot > 305 && rot <= 335)
        {
            rect.eulerAngles = new Vector3(0, 0, 305);
            Win(2500, winType.exp);
        }
        else if (rot > 335 && rot <= 365)
        {
            rect.eulerAngles = new Vector3(0, 0, 335);
            Win(347, winType.money);
        }

    }


    public void Win(int amount, winType winType)
    {
        if (winType == winType.material)
        {
            uiController.UpdateMaterial(amount);
        }
        else if (winType == winType.energy)
        {
            uiController.UpdateEnergy(amount);
        }
        else if (winType == winType.oil)
        {
            uiController.UpdateOil(amount);
        }
        else if (winType == winType.exp)
        {
            experience.AddExperience(amount);
        }
        else if (winType == winType.money)
        {
            uiController.UpdateMoney(amount);
        }
    }


}