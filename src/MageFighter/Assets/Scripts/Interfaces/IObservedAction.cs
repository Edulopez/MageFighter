using UnityEngine;
using System.Collections;

public interface IObservedAction
{
    void Idle();

    void BeingObserved();

    void StopedBeingObserved();

    void DoAction();

    bool IsObservable { get;  }

    bool HasAction { get; }
    bool HasAnimation{ get; }
}
