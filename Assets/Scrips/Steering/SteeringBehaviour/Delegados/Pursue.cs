using UnityEngine;

public class Pursue : Seek
{
    [SerializeField] [Range(0.0f, 10.0f)] private float maxPrediction;

    public override Steering GetSteering(AgentNPC miAgente)
    {
        // Vamos a  crear un nuevo target en la posicion donde estaria nuestro target
        var direction = target.transform.position - miAgente.transform.position;
        var distance = direction.magnitude;

        // Current Speed
        var speed = miAgente.vVelocidad.magnitude;

        // Si la velocidad es muy pequeña vamos a darle un predicion 
        var predictedSpeed = speed <= distance / maxPrediction
            ? maxPrediction
            : distance / speed;

        // NO Puedo usar el target porque va asignado a otro objecto

        var predictedTargetPosition =
            target.transform.position + target.vVelocidad * predictedSpeed;

        UseCustomDirectionAndRotation(predictedTargetPosition -
                                      miAgente.transform.position);

        return base.GetSteering(miAgente);
    }
}