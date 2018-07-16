using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon {

    float damage { get; set; }
    void OnStartAttack();
    void OnEndAttack();

}
