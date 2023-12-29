using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class UICanvasPopup : UICanvas
    {
        public Button btnBG;

        protected override void Awake()
        {
            base.Awake();
            btnBG.onClick.AddListener(OnClickBG);
        }

        public void OnClickBG()
        {
            Show(false);
        }
    }

}