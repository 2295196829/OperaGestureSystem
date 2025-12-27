using System;

namespace FancyScrollView.Example03
{
    class Context
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
    }
}
