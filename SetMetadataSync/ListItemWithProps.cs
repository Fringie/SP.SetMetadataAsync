using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SetMetadataSync
{
    class ListItemWithProps
    {

        public ListItem ListItem { get; set; }
        public string Filename { get; set; }
        public string TitleFrom { get; set; }

        public ListItemWithProps(ListItem listItem, string filename)
        {
            ListItem = listItem;
            Filename = filename;
        }
    }
}
