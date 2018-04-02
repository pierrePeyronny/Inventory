using SpfInventaire.Web.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpfInventaire.Web.Helpers
{

    public class SelectListHelper : ISelectListHelper
    {
        public SelectList AddFirstItemSelectList(SelectList origList, object selectedValue, string firstItemText)
        {
            List<SelectListItem> newList = origList.ToList();
            SelectListItem firstItem = new SelectListItem();
            firstItem.Text = firstItemText;
            firstItem.Value = "0";
            newList.Insert(0, firstItem);

            return new SelectList(newList, "Value", "Text", selectedValue);
        }

        public SelectList CreateSelectList(Dictionary<string, string> dictionaryItems, string seletedValue = null)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            SelectListItem itemlist = new SelectListItem();
            int index = 0;

            foreach (KeyValuePair<string, string> item in dictionaryItems)
            {
                itemlist = new SelectListItem();
                itemlist.Value = item.Key;
                itemlist.Text = item.Value;

                list.Insert(index, itemlist);
                index++;
            }

            SelectList selectListCreated = new SelectList(list, "Value", "Text", seletedValue);
            return selectListCreated;
        }


    }
}