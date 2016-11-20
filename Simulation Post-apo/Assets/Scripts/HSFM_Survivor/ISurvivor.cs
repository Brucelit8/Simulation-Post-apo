using UnityEngine;
using System.Collections;

public interface ISurvivor {

    void UpdateState();

    void OnTriggerEnter(Collider other);

    void ToBuildState();

    void ToNourrishState();

    void ToCollectState();

    void ToFightState();
}
