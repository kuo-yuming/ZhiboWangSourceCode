using UnityEngine;
using System.Collections;

public class DoubleUp_AnimLocation : MonoBehaviour
{
    void SetAnimLocation(int No)
    {
        DoubleUp_Control.Inst.ChangeAnimLocation(No);
    }

    void Success()
    {
        Invoke("ShowWordSuccess", 0.5f);
    }

    void Fail()
    {
        Invoke("ShowWordFail", 0.5f);
    }

    void ShowWordSuccess()
    {
        DoubleUp_Control.Inst.ShowWordSuccess();
    }

    void ShowWordFail()
    {
        DoubleUp_Control.Inst.ShowWordFail();
    }
}