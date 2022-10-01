using System;
using System.Collections;
using System.Collections.Generic;
using AchromaticDev.Util.Pooling;
using DG.Tweening;
using UnityEngine;

namespace AchromaticDev.Util.Notification
{
    [Serializable]
    public class NotificationSettings
    {
        [Header("Notification Settings")]
        public int MaxNotifications = 5;
        public float DisplayDuration = 3f;

        [Header("Animation Settings")]
        public float AnimationDuration = 1.5f;
        public Ease InEase = Ease.InBack;
        public Ease OutEase = Ease.OutBack;

        [Header("Layout Settings")]
        public float SpaceBetween = 50f;
        public Vector2 NotificationSize = new Vector2(500, 100);
    }


    [ExecuteAlways, RequireComponent(typeof(NotificationFactory))]
    public class NotificationManager : MonoSingleton<NotificationManager>
    {
        public NotificationSettings Settings;

        [SerializeField] private GameObject NotificationPrefab;
        [SerializeField] private RectTransform NotificationContainer;

        private LinkedList<NotificationElement> NotificationQueue = new LinkedList<NotificationElement>();
        private NotificationFactory NotificationFactory;

        private void Awake()
        {
            NotificationFactory = GetComponent<NotificationFactory>();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                NotificationPrefab.GetComponent<RectTransform>().sizeDelta = Settings.NotificationSize;
                var rectTransform = NotificationContainer.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(Settings.NotificationSize.x, Settings.NotificationSize.y * Settings.MaxNotifications + Settings.SpaceBetween * (Settings.MaxNotifications - 1));
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ShowNotification("Test Notification");
                }
            }
        }

        public void ShowNotification(string message)
        {
            if (NotificationQueue.Count >= Settings.MaxNotifications)
            {
                NotificationQueue.First.Value.Index = -1;
                NotificationQueue.RemoveFirst();

                foreach (var notification in NotificationQueue)
                {
                    notification.Index--;
                }
            }

            var notificationObject = PoolManager.Instantiate(NotificationPrefab, NotificationContainer);
            notificationObject
                .GetComponent<NotificationElement>()
                .Initialize(
                    message,
                    NotificationQueue
                        .AddLast(
                        notificationObject
                            .GetComponent<NotificationElement>()
                    ),
                    NotificationQueue.Count - 1
                )
                .Show();
        }
    }
}
