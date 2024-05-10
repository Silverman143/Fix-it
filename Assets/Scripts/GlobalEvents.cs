using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEvents;

public class GlobalEvents
{
    public delegate void PhantomFilled();
    public static event PhantomFilled OnPhantomFilled;

    public delegate void PhantomEmpty();
    public static event PhantomEmpty OnPhantomEmpty;

    public static void OnPhantomFilledInvoke()
    {
        OnPhantomFilled.Invoke();
    }
    public static void OnPhantomEmptyInvoke()
    {
        OnPhantomEmpty.Invoke();
    }


}
