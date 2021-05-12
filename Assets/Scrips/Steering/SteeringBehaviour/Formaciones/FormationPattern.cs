using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface FormationPattern
{
    int numberOfSlots { get; set; }

    // El offset que puedo cometer ??
    public Vector3 getDriftOffset(List<SlotAssigment> slotAssigments);

    // Localizacion dado el slotNumber
    public Vector3 getSlotLocation(int slotNumber);

    // Devuelve true si el pattern puede soportar el numero de slot

    public bool supportsSlots(int slotCount);
}
