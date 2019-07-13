using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SetMetadataSync
{
    class Engine
    {
        private ClientContext _ctx;

        public Engine()
        {
            _ctx = Helper.GetWebCtx();
        }

        public void Start()
        {
            if (_ctx == null) return;
            using (_ctx)
            {
                List copyFromList = _ctx.Web.GetListByTitle(App.Default.CopyFromLibTitle);
                List copyToList = _ctx.Web.GetListByTitle(App.Default.CopyToLibTitle);

                _ctx.Load(copyFromList);
                _ctx.Load(copyToList);
                _ctx.ExecuteQueryRetry();
                var copyFromItemsWithDuplicates = GetListItemsWithNames(LoadListItems(copyFromList));
                var copyFromWithoutDuplicates = RemoveDuplicateListItems(copyFromItemsWithDuplicates);
                var i = 0;
                var p = new List<List>();
                Parallel.ForEach(copyFromWithoutDuplicates, listItem =>
                {
                    using (var ctx = Helper.CreateDuplicateNewClient(_ctx))
                    {
                        var list = ctx.Web.Lists.GetByTitle(App.Default.CopyToLibTitle);
                        ctx.Load(list, l => l.Title);
                        ctx.ExecuteQuery();
                        p.Add(list);
                        i++;
                        Console.Out.WriteLineAsync($"List {list.Title}/{i}");
                    }
                });
                //await _ctx.ExecuteQueryAsync();
                foreach (var list in p)
                {
                    i++;
                    Console.WriteLine($"{list.Title}/{i}");
                }
                //copyFromList.get*/
            }
        }

        private List<ListItem> LoadListItems(List spList)
        {
            List<ListItem> itemsList = new List<ListItem>();
            CamlQuery camlQuery = new CamlQuery
            {
                ViewXml = @"<View Scope='Recursive'>
                              <Query>
                              </Query><RowLimit>5000</RowLimit>
                           </View>"
            };
            ListItemCollectionPosition position = null;
            do
            {
                Console.WriteLine($"[Document library] Loading files from {spList.Title}");
                camlQuery.ListItemCollectionPosition = position;
                var listItems = spList.GetItems(camlQuery);
                _ctx.Load(listItems);
                _ctx.ExecuteQueryRetry();
                itemsList.AddRange(listItems);
                position = listItems.ListItemCollectionPosition;
            } while (position != null);

            return itemsList;
        }

        private List<ListItemWithProps> GetListItemsWithNames(List<ListItem> listItemsNoName)
        {
            List<ListItemWithProps> listItemsWithName = new List<ListItemWithProps>();
            foreach (var listItem in listItemsNoName)
            {
                var toAdd = new ListItemWithProps(listItem, listItem["FileLeafRef"] as string);
                listItemsWithName.Add(toAdd);
                Console.WriteLine($"[List item] Loaded {toAdd.Filename}");
            }
            return listItemsWithName;
        }

        private List<ListItemWithProps> RemoveDuplicateListItems(List<ListItemWithProps> list)
        {
            return list.GroupBy(li => li.Filename).Select(li => li.First()).ToList();
        }
    }
    

    
}
