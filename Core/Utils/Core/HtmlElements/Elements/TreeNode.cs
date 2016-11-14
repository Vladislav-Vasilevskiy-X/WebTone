
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Container;

using Microsoft.Practices.ObjectBuilder2;

using OpenQA.Selenium;

namespace Core.HtmlElements.Elements
{
	/// <summary>
	/// The tree node.
	/// </summary>
	public class TreeNode : TypifiedElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TreeNode"/> class.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		public TreeNode(IWebElement element)
			: base(element)
		{
			this.Name = element.Text.Split('\r').First().Trim();
			this.nodeId = this.GetNodeId(element);
			this.parentElement = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeNode"/> class.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="parent">The parent tree node.</param>
		public TreeNode(IWebElement element, TreeNode parent)
			: base(element)
		{
			this.Name = element.Text.Split('\r').First().Trim();
			this.nodeId = this.GetNodeId(element);
			this.parentElement = parent;
		}

		/// <summary>
		/// The node id of the element on the tree.
		/// </summary>
		private readonly string nodeId;

		/// <summary>
		/// The parent element of the current element.
		/// </summary>
		private readonly TreeNode parentElement;

		/// <summary>
		/// Gets The element on the UI.
		/// </summary>
		private IWebElement UIElement
		{
			get
			{
				if (string.IsNullOrEmpty(this.nodeId))
				{
					return this.Browser.WebDriver.FindElement(By.CssSelector("li[type='root']"));
				}

				return this.Browser.WebDriver.FindElements(By.CssSelector(string.Format(CultureInfo.InvariantCulture, "li[nodeid='{0}']", this.nodeId))).FirstOrDefault();
			}
		}

		/// <summary>
		/// Gets the node identifier.
		/// </summary>
		/// <value>
		/// The node identifier.
		/// </value>
		public Guid NodeId
		{
			get
			{
				return new Guid(this.nodeId);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the TreeNode is expanded or not.
		/// </summary>
		/// <returns>True if it is expanded.</returns>
		/// <value>The is expanded.</value>
		public bool IsExpanded
		{
			get
			{
				return this.UIElement.GetAttribute("class").Contains("open");
			}
		}

		/// <summary>
		/// Gets the children.
		/// </summary>
		/// <value>The children.</value>
		public IEnumerable<TreeNode> Children
		{
			get
			{
				this.Expand();

				Waiter.SpinWait(() => !this.UIElement.Displayed, TimeSpan.FromMilliseconds(3000), TimeSpan.FromMilliseconds(10));
				var elements = this.UIElement.FindElements(By.XPath(".//ul//li"));

				return this.WrapElements(elements);
			}
		}

		/// <summary>
		/// gets the node id. This is for identifying the node even if the tree was refreshed in the browser as this keeps its value and unique.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The node id.</returns>
		private string GetNodeId(IWebElement element)
		{
			return element.GetAttribute("nodeid");
		}

		/// <summary>
		/// The wrap elements.
		/// </summary>
		/// <param name="elements">
		/// The elements.
		/// </param>
		/// <returns>
		/// List of child nodes.
		/// </returns>
		private IEnumerable<TreeNode> WrapElements(IEnumerable<IWebElement> elements)
		{
			var list = new List<TreeNode>();
			elements.ForEach(x => list.Add(new TreeNode(x, this)));
			return list;
		}

		/// <summary>
		/// Selects the tree node.
		/// If the parent node is collapsed, it expands it as well to be able to select the node.
		/// </summary>
		public void Select()
		{
			if (this.parentElement != null)
			{
				this.parentElement.Expand();
			}

			Waiter.SpinWait(() => !this.UIElement.Displayed, TimeSpan.FromMilliseconds(3000), TimeSpan.FromMilliseconds(10));
			this.Browser.WaitAjax();
			this.UIElement.FindElement(By.XPath(".//a")).Click();
		}

		/// <summary>
		/// Selects the child node by name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>Found child node.</returns>
		public TreeNode SelectChildByName(string name)
		{
			if (this.parentElement != null)
			{
				this.parentElement.Expand();
			}

			var foundChildNode = this.Children.First(x => x.Name.Equals(name));
			foundChildNode.Select();
			return foundChildNode;
		}

		/// <summary>
		/// Expands the tree node.
		/// If the parent node is collapsed, it expands it as well to be able to reach the current node.
		/// </summary>
		public void Expand()
		{
			if (this.parentElement != null)
			{
				this.parentElement.Expand();
			}

			this.ChangeNodeState(true);
		}

		/// <summary>
		/// Collapse the tree node.
		/// </summary>
		public void Collapse()
		{
			this.ChangeNodeState(false);
		}

		/// <summary>
		/// Click on the tree icon, if it is present to expand or collapse the node.
		/// If the node doesn't contain any item and the node was already clicked, the expand icon disappears.
		/// </summary>
		/// <param name="toExpand">To expand.</param>
		private void ChangeNodeState(bool toExpand)
		{
			if (toExpand == !this.IsExpanded)
			{
				var treeIcon = this.UIElement.FindElements(By.XPath(".//ins"));
				if (treeIcon.Any())
				{
					Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Tree Node '{0}' was '{1}'", this.Name, toExpand ? "expanded" : "collapsed"));
					Waiter.SpinWait(() => treeIcon.First().Displayed, TimeSpan.FromSeconds(50));
					treeIcon.First().Click();
					this.Browser.WaitAjax();
				}
			}
		}
	}
}