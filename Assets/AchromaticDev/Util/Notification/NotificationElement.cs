using System.Collections;
using System.Collections.Generic;
using AchromaticDev.Util.Pooling;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace AchromaticDev.Util.Notification
{
    public class NotificationElement : MonoBehaviour
    {
        [SerializeField] private Text messageText;

        private int _index = -1;

        private RectTransform _rectTransform;
        private LinkedListNode<NotificationElement> _node;
        private NotificationManager _manager;

        internal int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                MoveToIndex();
            }
        }

        public NotificationElement Initialize(string message, LinkedListNode<NotificationElement> node, int index)
        {
            _index = index;
            _node = node;

            messageText.text = message;
            _manager = NotificationManager.Instance;
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.sizeDelta = _manager.Settings.NotificationSize;
            _rectTransform.anchoredPosition = new Vector2(0,
                -_manager.Settings.NotificationSize.y * index - _manager.Settings.SpaceBetween * index);
            return this;
        }

        public NotificationElement Show()
        {
            gameObject.SetActive(true);
            _rectTransform.DOAnchorPosX(-_rectTransform.sizeDelta.x, _manager.Settings.AnimationDuration)
                .SetEase(_manager.Settings.InEase);
            StartCoroutine(HideAfterDelay());
            return this;
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(_manager.Settings.DisplayDuration);
            Hide();
        }

        public NotificationElement Hide()
        {
            _rectTransform.DOAnchorPosX(0, _manager.Settings.AnimationDuration)
                .SetEase(_manager.Settings.OutEase);
            StartCoroutine(DestroyAfterDelay(_manager.Settings.AnimationDuration));
            return this;
        }

        private IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy();
        }

        public NotificationElement Destroy()
        {
            DOTween.Kill(_rectTransform);
            PoolManager.Destroy(gameObject);
            for (var node = _node.Next; node != null; node = node.Next)
            {
                node.Value.Index--;
            }

            _node.List?.Remove(_node);
            return this;
        }

        private void MoveToIndex()
        {
            var notificationSettings = NotificationManager.Instance.Settings;
            _rectTransform
                .DOAnchorPosY(
                    -notificationSettings.NotificationSize.y * Index - notificationSettings.SpaceBetween * Index,
                    _manager.Settings.AnimationDuration).SetEase(Ease.OutBack);
        }
    }
}