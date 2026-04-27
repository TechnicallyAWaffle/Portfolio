using UnityEngine;
using UnityEngine.Rendering;

public class JarManager : MonoBehaviour
{

    //Singleton
    public static JarManager Instance;

    //Runtime Vars
    [SerializeField] private JarBase currentOpenJar;
    [SerializeField] private JarBase[] jars;

    //Refs
    [SerializeField] private VideoManager videoManager;

    private void Awake()
    {
        Instance = this;
    }

    public void SwitchOpenJar(JarBase jarToOpen)
    {
        if (currentOpenJar)
        {
            if (currentOpenJar == jarToOpen)
            {
                currentOpenJar = null;
                return;
            }
            else
                currentOpenJar.SelectJar();
        }
        SetLockoutForAllJars();
        currentOpenJar = jarToOpen;
    }

    private void SetLockoutForAllJars()
    {
        foreach (JarBase jar in jars)
        {
            jar.Lockout();
            Debug.Log("meow!!!");
        }
    }


}
