using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivatable
{
    // interface for communicating between puzzle objects
    void Activate();
    void Deactivate();
}
