using System;
using System.Collections.Generic;
using Library;
using UnityEngine;

public class WorkManager : MonoBehaviour
{
    public static Dictionary<Guid, Factory> Workplaces = new Dictionary<Guid, Factory>();
    public static List<Guid> AvailableWorkplaces = new List<Guid>();

    public static Factory GetWorkplace(Guid id)
    {
        return Workplaces.GetValueOrDefault(id);
    }
}