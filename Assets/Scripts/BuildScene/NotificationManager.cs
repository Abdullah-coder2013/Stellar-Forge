using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private GameObject notificationBoard;
    [SerializeField] private Transform spawnPoint;
    public static Queue<string> notificationQueue = new();
    private static readonly int SlideOut = Animator.StringToHash("SlideOut");

    public static void AddNotificationToQueue(string notification) {
        notificationQueue.Enqueue(notification);
    }


    // Update is called once per frame
    private void Update()
    {
        if (notificationQueue.Count > 0) {
            var notboard = Instantiate(notificationBoard, spawnPoint.transform.position, Quaternion.identity, this.transform);
            StartCoroutine(DestroyNotificationAfterDelay(notboard, 3f));
            notificationBoard.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = notificationQueue.Dequeue();
        }
    }
    
    private IEnumerator DestroyNotificationAfterDelay(GameObject notboard, float delay)
{
    yield return new WaitForSeconds(delay);
    notboard.GetComponent<Animator>().SetBool(SlideOut, true);
    DestroyNotification(notboard);
}

    /// <summary>
    /// Destroys the notification GameObject.
    /// </summary>
    /// <param name="notboard">The notification GameObject to destroy.</param>
    private void DestroyNotification(GameObject notboard) {
        Destroy(notboard);
    }
    
}
