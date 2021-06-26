using Pada1.BBCore;
using Pada1.BBCore.Framework;
using UnityEngine; // Code attributes

// ConditionBase


namespace Assets.Scrips.Actions
{
    [Condition("Nuevas/CheckMode")]
    [Help("Comprueba si el modo es de ataque o defensa")]
    public class CheckMode : BasePrimitiveAction
    {
        [InParam("equipo")] [Help("Tag de la base")]
        public string equipo; //baseRojo

        [OutParam("modo")] [Help("Modo de la base")]
        public Modo modo;

        public override bool Check()
        {
            var objeto = GameObject.FindGameObjectWithTag(equipo);
            if (objeto != null)
            {
               objeto.GetComponent<>()
            }

            return false;
        }
    } // class IsNightCondition
}