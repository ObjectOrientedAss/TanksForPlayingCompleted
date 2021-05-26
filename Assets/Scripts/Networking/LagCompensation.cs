using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagCompensation : MonoBehaviour
{
    public struct TankTransform
    {
        public Vector3 TankPosition;
        public Quaternion TankRotation;
        public Quaternion TurretRotation;
        public float TimeStamp;

        public TankTransform(Player player)
        {
            TankPosition = player.transform.position;
            TankRotation = player.Base.rotation;
            TurretRotation = player.Turret.rotation;
            TimeStamp = Time.time;
        }
    }

    //This is the transform data received with the previous update
    public TankTransform PreviousTransform;

    //This is the transform data just received
    public TankTransform LastTransform;

    //This is the transform data with compensating values (correction)
    public TankTransform CompensatedTransform;

    //This is the transform data to apply to a player based on the latest updates and the corrections
    public TankTransform FinalTransform;

    //This is the update frequency with which we are receiving transform data.
    //The highest the number of packet sent (and received) over time, the lowest is the frequency.
    public float Frequency = 1f;

    //The lerp/slerp step amount to use while interpolating positions/rotations for the final transform.
    public float InterpolationStep;

    public bool Compensate;
    public bool Interpolate;

    private Player player;

    public void Init()
    {
        player = GetComponent<Player>();
        PreviousTransform = new TankTransform(player);
        LastTransform = new TankTransform();
        LastTransform = PreviousTransform;

        CompensatedTransform = new TankTransform();
    }

    public void Receive(Vector3 tankPosition, Quaternion tankRotation, Quaternion turretRotation)
    {
        InterpolationStep = 0;
        PreviousTransform = LastTransform;

        LastTransform.TankPosition = tankPosition;
        LastTransform.TankRotation = tankRotation;
        LastTransform.TurretRotation = turretRotation;

        PreviousTransform.TimeStamp = LastTransform.TimeStamp;
        LastTransform.TimeStamp = Time.time;

        float timeElapsed = LastTransform.TimeStamp - PreviousTransform.TimeStamp;

        Frequency = 1f / timeElapsed;

        if (Compensate)
        {          
            CompensatedTransform.TankPosition = (LastTransform.TankPosition - PreviousTransform.TankPosition) * timeElapsed;
            Vector3 tankRot = (LastTransform.TankRotation.eulerAngles - PreviousTransform.TankRotation.eulerAngles) * timeElapsed;
            tankRot.y = 0;
            CompensatedTransform.TankRotation = Quaternion.Euler(tankRot);
            Vector3 turretRot = (LastTransform.TurretRotation.eulerAngles - PreviousTransform.TurretRotation.eulerAngles) * timeElapsed;
            turretRot.y = 0;
            CompensatedTransform.TurretRotation = Quaternion.Euler(turretRot);
        }
        //else
        //{
        //    CompensatedTransform.TankPosition = Vector3.zero;
        //    CompensatedTransform.TankRotation = Quaternion.identity;
        //    CompensatedTransform.TurretRotation = Quaternion.identity;
        //}

        FinalTransform.TankPosition = LastTransform.TankPosition + CompensatedTransform.TankPosition;
        FinalTransform.TankRotation = LastTransform.TankRotation * CompensatedTransform.TankRotation;
        FinalTransform.TurretRotation = LastTransform.TurretRotation * CompensatedTransform.TurretRotation;
    }

    private void Update()
    {
        InterpolationStep += Time.deltaTime * Frequency;

        if (Interpolate)
        {
            FinalTransform.TankPosition = Vector3.Lerp(PreviousTransform.TankPosition + CompensatedTransform.TankPosition,
                LastTransform.TankPosition + CompensatedTransform.TankPosition,
                InterpolationStep);

            FinalTransform.TankRotation = Quaternion.Slerp(PreviousTransform.TankRotation * CompensatedTransform.TankRotation,
                LastTransform.TankRotation * CompensatedTransform.TankRotation,
                InterpolationStep);

            FinalTransform.TurretRotation = Quaternion.Slerp(PreviousTransform.TurretRotation * CompensatedTransform.TurretRotation,
                LastTransform.TurretRotation * CompensatedTransform.TurretRotation,
                InterpolationStep);
        }

        player.transform.position = FinalTransform.TankPosition;
        player.Base.rotation = FinalTransform.TankRotation;
        player.Turret.rotation = FinalTransform.TurretRotation;
    }
}