using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppContainer.Contract
{
    public interface IViewModel
    {
        /// <summary>
        /// 根据ViewModel创建View
        /// </summary>
        /// <returns></returns>
        Control CreateView();
    }
}
