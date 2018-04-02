using System.Collections.Generic;
using System.Web.Mvc;

namespace SpfInventaire.Web.Helpers.Interfaces
{
    public interface ISelectListHelper
    {
        SelectList AddFirstItemSelectList(SelectList origList, object selectedValue, string firstItemText);
        SelectList CreateSelectList(Dictionary<string, string> dictionaryItems, string seletedValue = null);
    }
}