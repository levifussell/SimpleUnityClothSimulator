using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartStaticCollisionConstraint : MonoBehaviour
{
    [SerializeField]
    private CollisionConstraint collisionConstraint;

    [SerializeField]
    private CollisionConstraint processCollisionContraint;

    private List<PointMass> processedPoints;

    [SerializeField]
    private int updateRateInSteps = 20;
    private int stepCount = 0;

    [SerializeField]
    private int initSettleUpdateSteps = 100;

    void FindAndStoreNearbyPoints(PointMass[] allPoints, bool reset=true)
    {
        if(processedPoints == null)
            processedPoints = new List<PointMass>();
        else if(reset)
            processedPoints.Clear();
        foreach(PointMass p in allPoints)
        {
            Vector3 position = p.position;
            if(processCollisionContraint.ApplyConstraint(ref position))
            {
                if(reset || !processedPoints.Contains(p))
                    processedPoints.Add(p);
            }
        }
    }

    public void ApplyConstraintToProcessedPoints(PointMass[] allPoints)
    {
        bool initReset = stepCount <= initSettleUpdateSteps;
        if(processedPoints == null || (updateRateInSteps != -1 && stepCount % updateRateInSteps == 0) || initReset)
            FindAndStoreNearbyPoints(allPoints, !initReset);

        collisionConstraint.ApplyConstraint(processedPoints);
        stepCount++;
    }
    // public void ApplyConstraintToProcessedPoints()
    // {
    //     collisionConstraint.ApplyConstraint(processedPoints);
    // }

    public void Draw(Color cColor, Color pColor)
    {
        if(collisionConstraint != null)
            collisionConstraint.Draw(cColor);
        if(processCollisionContraint != null)
            processCollisionContraint.Draw(pColor);
    }
}
