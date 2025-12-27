using Leap.Unity;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


namespace FancyScrollView.Example03
{
    class Cell : FancyCell<ItemData, Context>
    {
        [SerializeField] Animator animator = default;
        [SerializeField] Text message = default;
        [SerializeField] Text messageLarge = default;
        [SerializeField] Image image = default;
        [SerializeField] public Sprite gesture0;
        [SerializeField] public Sprite gestureIntro0;
        [SerializeField] public Sprite gesture1;
        [SerializeField] public Sprite gestureIntro1;
        [SerializeField] public Sprite gesture2;
        [SerializeField] public Sprite gestureIntro2;
        [SerializeField] public Sprite gesture3;
        [SerializeField] public Sprite gestureIntro3;
        [SerializeField] public Image imageLarge = default;
        [SerializeField] Button button = default;

        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }

        void Start()
        {
            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        }


        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.Message;
            messageLarge.text = Index.ToString();
            var selected = Context.SelectedIndex == Index;
            ChangeImageSpire(Index);
            imageLarge.color = image.color = selected
                ? new Color32(221, 215, 199, 180)
                : new Color32(255, 255, 255, 20);
            ChangePageSpire(Index);
        }

        public override void UpdatePosition(float position)
        {
            currentPosition = position;

            if (animator.isActiveAndEnabled)
            {
                animator.Play(AnimatorHash.Scroll, -1, position);
            }

            animator.speed = 0;
        }

        // 当 GameObject 变为非激活状态时，Animator 会被重置
        // 因此，需要保留当前位置，并在 OnEnable 时机重新设置当前位置
        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);

        private void ChangeImageSpire(int Index)
        {
            if (Index == 0)
            {
                image.sprite = gesture0;
            }
            if (Index == 1)
            {
                image.sprite = gesture1;
            }
            if (Index == 2)
            {
                image.sprite = gesture2;
            }
            if (Index == 3)
            {
                image.sprite = gesture3;
            }
        }

        private void ChangePageSpire(int Index)
        {
            if (Index == 0)
            {
                imageLarge.sprite = gestureIntro0;
            }
            if (Index == 1)
            {
                imageLarge.sprite = gestureIntro1;
            }
            if (Index == 2)
            {
                imageLarge.sprite = gestureIntro2;
            }
            if (Index == 3)
            {
                imageLarge.sprite = gestureIntro3;
            }
        }




    }
}
