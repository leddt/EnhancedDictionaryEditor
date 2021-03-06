﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;

namespace Sigmund.EnhancedDictionaryEditor.BackOffice.SigmundEnhancedDictionaryEditor
{
    public class DictionnaryKeyTreeNodeRepository
    {
        private ILocalizationService LocalizationService => UmbracoContext.Current.Application.Services.LocalizationService;
        private TreeController TreeController;
        private FormDataCollection EmptyFormData => new FormDataCollection(new List<KeyValuePair<string, string>>(0));
        private const string RootTreeNodeId = "-1";

        public DictionnaryKeyTreeNodeRepository(TreeController treeController)
        {
            TreeController = treeController;
        }

        public TreeNodeCollection GetById(string id)
        {
            IEnumerable<IDictionaryItem> keys;

            if (id == RootTreeNodeId)
            {
                keys = LocalizationService.GetRootDictionaryItems();
            } else
            {
                keys = LocalizationService.GetDictionaryItemChildren(Guid.Parse(id));
            }

            var nodes = new TreeNodeCollection();

            foreach (var dictionnaryKey in keys)
            {
                nodes.Add(CreateTreeNode(dictionnaryKey, id));
            }

            return nodes;
        }

        private TreeNode CreateTreeNode(IDictionaryItem item, string parentId)
        {
            var hasChilds = LocalizationService.GetDictionaryItemChildren(item.Key).Any();
            var node = TreeController.CreateTreeNode(item.Key.ToString(), parentId, EmptyFormData, item.ItemKey, "icon-book-alt", hasChilds);
            node.AdditionalData["key"] = item.ItemKey;

            return node;
        }
    }
}