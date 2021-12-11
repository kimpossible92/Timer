using System.Collections;
using UnityEngine;
public enum UnitTime
{
    Hour,Minute
}
public abstract class Unit : MonoBehaviour
{
    [SerializeField]private UnitTime unit;

    public UnitTime _Unit => unit;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}