using System.Xml.Serialization;
using UnityEngine;

public class MaskedCultistLogic : MeleeClass
{

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        MeleeCheck();
    }

}
