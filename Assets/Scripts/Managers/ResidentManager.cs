using System;
using System.Collections.Generic;
using Library;
using UnityEngine;

public class ResidentManager : MonoBehaviour
{
    public static Dictionary<Guid, House> Residence = new Dictionary<Guid, House>();
    public static List<Guid> AvailableResidence = new List<Guid>();

    public static House GetResidence(Guid id)
    {
        return Residence.GetValueOrDefault(id);
    }

    public static bool IsResidenceAvailable()
    {
        return AvailableResidence.Count != 0;
    }
}
