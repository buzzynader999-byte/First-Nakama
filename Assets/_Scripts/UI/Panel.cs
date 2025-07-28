using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : Overlay
{
    // NOTE : recursive reference between ClientCoordinator() and Panel()
    //public virtual void Close()
    //{
    //    ClientCoordinator.Instance.Back(this);
    //}
}