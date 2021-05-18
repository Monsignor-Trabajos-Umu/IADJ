using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FormationManager : MonoBehaviour
{
    //Lista de ranuras
    [SerializeField]
    List<SlotAssigment> slotAssigments = new List<SlotAssigment>();

    // Posicion y orientacion para evitar los derapes
    Vector3 driftOffset;

    // Patron
    FormationPattern patter;

    /*
     * Punto C.2 del Bloque 1:
     * Si al menos uno de los personajes seleccionados no está en formación, 
     * los que estén  en  formación  (si  los  hay)  romperán  la  formación 
     * y  todos  se  dirigirán  al punto de destino
     * 
     * Se puede asumir que, si con estructura fija tenemos más NPC de los necesarios,
     * que vayan directamente al punto sin necesidad de hacer la formación
     */

    public void UpdateSlotAssignments()
    {
        for (int i = 0; i < slotAssigments.Count; i++)
        {
            SlotAssigment slot = slotAssigments[i];
            slot.slotNumber = i;
            slotAssigments[i] = slot;
        }

        driftOffset = patter.getDriftOffset(slotAssigments);
    }

    public bool AddCharacter(AgentNPC character)
    {
        // Encontramos los slots ocupados
        int occupiedSlot = slotAssigments.Count;
        if (patter.supportsSlots(occupiedSlot + 1))
        {
            var slotAssigment = new SlotAssigment();
            slotAssigment.character = character;
            slotAssigments.Add(slotAssigment);
            UpdateSlotAssignments();
            return true;
        }

        return false;


    }

    public void RemoveCharacter(AgentNPC c)
    {
        var slot = slotAssigments.First(s => s.character == c);
        slotAssigments.Remove(slot);
        UpdateSlotAssignments();
    }


    public void UpdateSlots()
    {
        var anchor = GetAnchorPoint();

    }

    Vector3 GetAnchorPoint()
    {
        var suma = new Vector3(0, 0, 0);
        var k = 0;
        foreach (var slotAssigment in slotAssigments)
        {
            suma += slotAssigment.character.transform.position;
            k++;
        }

        suma /= k;
        return new Vector3();

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
