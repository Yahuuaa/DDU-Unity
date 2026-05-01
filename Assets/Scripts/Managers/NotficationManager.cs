using System.Collections.Generic;
using Library;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public static List<Notification> messages = new List<Notification>();

    public GameObject template;
    public static NotificationManager instance;

    void Start()
    {
        instance = this;
    }
    
    void Update()
    {
        foreach (Notification notification in new List<Notification>(messages))
        {
            notification.DecreaseTimer(1f);
        }
    }
    
    public static void createMessage(string title, string message, Color color)
    {
        foreach (Notification notification in messages)
        {
            notification.MoveModel();
        }
        messages.Add(new Notification(title, message, instance.template, color));
    }
    
}
