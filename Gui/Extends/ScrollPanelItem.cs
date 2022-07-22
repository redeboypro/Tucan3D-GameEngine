using System;
using OpenTK;
using Tucan3D_GameEngine.Utils;

namespace Tucan3D_GameEngine.Gui.Extends
{
    public class ScrollPanelItem : Text2D
    {
        private ScrollPanel scrollPanel;

        public ScrollPanelItem(string header, ScrollPanel scrollPanel, Font font) : base(font)
        {
            Text = header;
            this.scrollPanel = scrollPanel;
        }

        public void OnClick(Vector2 point)
        {
            FillBackground = false;
            
            if (PointIsInsideBounds(point))
            {
                FillBackground = true;
                scrollPanel.AssignSelectedItem(this);
                scrollPanel.SelectedItemChanged?.Invoke();
            }
        }
    }
}