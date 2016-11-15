using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core.GeneralUtils.Container;
using Microsoft.Practices.ObjectBuilder2;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     The tree node.
	/// </summary>
	public class TreeNode : TypifiedElement
	{
		/// <summary>
		///     The node id of the element on the tree.
		/// </summary>
		private readonly string nodeId;

		/// <summary>
		///     The parent element of the current element.
		/// </summary>
		private readonly TreeNode parentElement;

		/// <summary>
		///     Initializes a new instance of the <see cref="TreeNode" /> class.
		/// </summary>
		/// <param name="element">
		///     The element.
		/// </param>
		public TreeNode(IWebElement element)
			: base(element)
		{
			Name = element.Text.Split('\r').First().Trim();
			nodeId = GetNodeId(element);
			parentElement = null;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="TreeNode" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="parent">The parent tree node.</param>
		public TreeNode(IWebElement element, TreeNode parent)
			: base(element)
		{
			Name = element.Text.Split('\r').First().Trim();
			nodeId = GetNodeId(element);
			parentElement = parent;
		}

		/// <summary>
		///     Gets The element on the UI.
		/// </summary>
		private IWebElement UIElement
		{
			get
			{
				if (string.IsNullOrEmpty(nodeId))
				{
					return Browser.WebDriver.FindElement(By.CssSelector("li[type='root']"));
				}

				return
					Browser.WebDriver.FindElements(
						By.CssSelector(string.Format(CultureInfo.InvariantCulture, "li[nodeid='{0}']", nodeId))).FirstOrDefault();
			}
		}

		/// <summary>
		///     Gets the node identifier.
		/// </summary>
		/// <value>
		///     The node identifier.
		/// </value>
		public Guid NodeId
		{
			get { return new Guid(nodeId); }
		}

		/// <summary>
		///     Gets a value indicating whether the TreeNode is expanded or not.
		/// </summary>
		/// <returns>True if it is expanded.</returns>
		/// <value>The is expanded.</value>
		public bool IsExpanded
		{
			get { return UIElement.GetAttribute("class").Contains("open"); }
		}

		/// <summary>
		///     Gets the children.
		/// </summary>
		/// <value>The children.</value>
		public IEnumerable<TreeNode> Children
		{
			get
			{
				Expand();

				Waiter.SpinWait(() => !UIElement.Displayed, TimeSpan.FromMilliseconds(3000), TimeSpan.FromMilliseconds(10));
				var elements = UIElement.FindElements(By.XPath(".//ul//li"));

				return WrapElements(elements);
			}
		}

		/// <summary>
		///     gets the node id. This is for identifying the node even if the tree was refreshed in the browser as this keeps its
		///     value and unique.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The node id.</returns>
		private string GetNodeId(IWebElement element)
		{
			return element.GetAttribute("nodeid");
		}

		/// <summary>
		///     The wrap elements.
		/// </summary>
		/// <param name="elements">
		///     The elements.
		/// </param>
		/// <returns>
		///     List of child nodes.
		/// </returns>
		private IEnumerable<TreeNode> WrapElements(IEnumerable<IWebElement> elements)
		{
			var list = new List<TreeNode>();
			elements.ForEach(x => list.Add(new TreeNode(x, this)));
			return list;
		}

		/// <summary>
		///     Selects the tree node.
		///     If the parent node is collapsed, it expands it as well to be able to select the node.
		/// </summary>
		public void Select()
		{
			if (parentElement != null)
			{
				parentElement.Expand();
			}

			Waiter.SpinWait(() => !UIElement.Displayed, TimeSpan.FromMilliseconds(3000), TimeSpan.FromMilliseconds(10));
			Browser.WaitAjax();
			UIElement.FindElement(By.XPath(".//a")).Click();
		}

		/// <summary>
		///     Selects the child node by name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>Found child node.</returns>
		public TreeNode SelectChildByName(string name)
		{
			if (parentElement != null)
			{
				parentElement.Expand();
			}

			var foundChildNode = Children.First(x => x.Name.Equals(name));
			foundChildNode.Select();
			return foundChildNode;
		}

		/// <summary>
		///     Expands the tree node.
		///     If the parent node is collapsed, it expands it as well to be able to reach the current node.
		/// </summary>
		public void Expand()
		{
			if (parentElement != null)
			{
				parentElement.Expand();
			}

			ChangeNodeState(true);
		}

		/// <summary>
		///     Collapse the tree node.
		/// </summary>
		public void Collapse()
		{
			ChangeNodeState(false);
		}

		/// <summary>
		///     Click on the tree icon, if it is present to expand or collapse the node.
		///     If the node doesn't contain any item and the node was already clicked, the expand icon disappears.
		/// </summary>
		/// <param name="toExpand">To expand.</param>
		private void ChangeNodeState(bool toExpand)
		{
			if (toExpand == !IsExpanded)
			{
				var treeIcon = UIElement.FindElements(By.XPath(".//ins"));
				if (treeIcon.Any())
				{
					Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Tree Node '{0}' was '{1}'", Name,
						toExpand ? "expanded" : "collapsed"));
					Waiter.SpinWait(() => treeIcon.First().Displayed, TimeSpan.FromSeconds(50));
					treeIcon.First().Click();
					Browser.WaitAjax();
				}
			}
		}
	}
}