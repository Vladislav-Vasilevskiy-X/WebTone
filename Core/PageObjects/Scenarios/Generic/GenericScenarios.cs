using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Core.GeneralUtils;
using Core.GeneralUtils.Constants;
using Core.GeneralUtils.Container;
using Core.GeneralUtils.MSTest;
using Core.PageObjects.Scenarios.Generic.Attributes;
using Core.PageObjects.Scenarios.Generic.Containers;
using Core.PageObjects.Views;
using Core.SeleniumUtils.Core.HtmlElements.Elements;
using Core.SeleniumUtils.Core.HtmlElements.Utils;
using Core.SeleniumUtils.Core.Objects;
using Muyou.LinqToWindows.Extensions;
using Newtonsoft.Json;

namespace Core.PageObjects.Scenarios.Generic
{
	/// <summary>
	///     GenericScenarios class.
	/// </summary>
	public class GenericScenarios : Scenario
	{
		#region PRIVATE

		/// <summary>
		///     Gets the html element blocks list.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <param name="storageListPropertyInfo">The grid content property.</param>
		/// <returns>The List.</returns>
		public IList GetGridBlocksList<TView>(PropertyInfo storageListPropertyInfo) where TView : View
		{
			var gridView = Resolve<TView>();
			var gridBlocksObject = storageListPropertyInfo.GetValue(gridView, null);
			var gridBlocksEnumerable = gridBlocksObject as IEnumerable;
			if (gridBlocksEnumerable == null)
			{
				return new ArrayList();
			}

			var genericParameterType = HtmlElementUtils.GetGenericParameterType(storageListPropertyInfo);
			var listType = typeof(List<>);
			var constructedListType = listType.MakeGenericType(genericParameterType);
			var gridBlocksList = Activator.CreateInstance(constructedListType) as IList;
			if (gridBlocksList == null)
			{
				return new ArrayList();
			}

			foreach (var item in gridBlocksEnumerable)
			{
				gridBlocksList.Add(item);
			}

			if (gridBlocksList.Count == 0)
			{
				return gridBlocksList;
			}

			var skipFlag = storageListPropertyInfo.GetCustomAttribute<PropertyStorageListAttribute>().SkipFlag;
			if (skipFlag == SkipFlag.SkipFirst)
			{
				gridBlocksList.RemoveAt(0);
			}

			return gridBlocksList;
		}

		#endregion

		#region ENTER DATA METHODS

		/// <summary>
		///     Enter model data to the lightbox (or other view with inputs).
		/// </summary>
		/// <typeparam name="TModel">The model type.</typeparam>
		/// <typeparam name="TView">The type of edit view.</typeparam>
		/// <param name="model">The model.</param>
		public void EnterModelData<TView, TModel>(TModel model) where TView : View
		{
			var editView = Resolve<TView>();
			EnterModelValuesToPageObjectProperties(model, editView);
		}

		/// <summary>
		///     Enters the model data to the page object (view or block).
		/// </summary>
		/// <typeparam name="TPageObject">The type of the page object.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="model">The model.</param>
		/// <param name="pageObject">The page object.</param>
		public void EnterModelData<TPageObject, TModel>(TModel model, TPageObject pageObject) where TPageObject : class
		{
			EnterModelValuesToPageObjectProperties(model, pageObject);
		}

		#endregion

		#region PRIVATE METHODS

		/// <summary>
		///     Enters the model values to page object properties.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="model">The model.</param>
		/// <param name="parentPageObject">The parent page object.</param>
		private void EnterModelValuesToPageObjectProperties<TModel>(TModel model, object parentPageObject)
		{
			var matchedViewModelProperties = parentPageObject.GetPublicPropertiesWithAttribute(typeof(ModelPropertyNameAttribute));

			foreach (var matchedViewModelProperty in matchedViewModelProperties)
			{
				EnterModelPropertyValueToPageObject(matchedViewModelProperty, model, parentPageObject);

				if (!matchedViewModelProperty.IsDefined(typeof(MayAcceptConfirmationAttribute)))
				{
				}
			}

			var blockStorageProperties = parentPageObject.GetPublicPropertiesWithAttribute(typeof(PropertyStorageAttribute));
			if (!blockStorageProperties.Any())
			{
				return;
			}

			// recursively enter data into the page object properties
			blockStorageProperties.Select(x => x.GetValue(parentPageObject, null))
				.ForEach(x => EnterModelValuesToPageObjectProperties(model, x));
		}

		/// <summary>
		///     Enters the model property value to page object.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="matchedPageObjectToModelProperty">
		///     The matched page object to model by
		///     <see cref="ModelPropertyNameAttribute" />property.
		/// </param>
		/// <param name="model">The model.</param>
		/// <param name="pageObject">The page object.</param>
		private void EnterModelPropertyValueToPageObject<TModel>(PropertyInfo matchedPageObjectToModelProperty, TModel model,
			object pageObject)
		{
			var modelValue = GetModelValueUsingModelPropertyName(matchedPageObjectToModelProperty, model);
			if (modelValue == null)
			{
				return;
			}

			var fillableWrapper =
				new FillableWrappersContainer().GetWrapperByElementType(matchedPageObjectToModelProperty.PropertyType);
			if (fillableWrapper != null)
			{
				fillableWrapper.EnterModelPropertyValueToPageObject(pageObject, matchedPageObjectToModelProperty, modelValue);
				Browser.WaitAjax();
			}
		}

		#endregion

		#region SELECT METHODS

		/// <summary>
		///     Selects the model by index within current page.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="index">The index of model in the grid.</param>
		/// <returns>The Model.</returns>
		public TModel SelectByIndex<TView, TModel>(int index) where TModel : new() where TView : View
		{
			return SetByIndex<TView, TModel>(index, true);
		}

		/// <summary>
		///     Deselects the model by index within current page.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="index">The index of model in the grid.</param>
		/// <returns>The Model.</returns>
		public TModel DeselectByIndex<TView, TModel>(int index) where TModel : new() where TView : View
		{
			return SetByIndex<TView, TModel>(index, false);
		}

		/// <summary>
		///     Sets the model by index within current page.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="index">The index.</param>
		/// <param name="setValue">if set to <c>true</c> then select, otherwise deselect.</param>
		/// <exception cref="System.ArgumentException">The mapping to select not found.</exception>
		/// <returns>The Model.</returns>
		public TModel SetByIndex<TView, TModel>(int index, bool setValue) where TModel : new() where TView : View
		{
			var foundItem = GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>(null, x => false, index).FirstOrDefault();
			if (foundItem == null)
			{
				throw new ArgumentException("The mapping to select not found");
			}

			foundItem.GetSelectableItem.Set(setValue);
			return foundItem.Model;
		}

		/// <summary>
		///     Select single model from the grid by the checkbox marked with <see cref="SelectableCheckBoxAttribute" /> within
		///     current page.
		/// </summary>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="modelToDeselect">The model that should be selected.</param>
		/// <param name="comparer">The comparer of models.</param>
		/// <returns>The selected mapping model.</returns>
		public HtmlElementToBlockMappingModel<TModel> SelectSingleModelInTheGrid<TView, TModel>(TModel modelToDeselect,
			IEqualityComparer<TModel> comparer = null) where TModel : class, new() where TView : View
		{
			var indexesToSelect = SetSeveralModelsInTheGrid<TView, TModel>(new[] {modelToDeselect}, true, comparer);
			return indexesToSelect[0];
		}

		/// <summary>
		///     Select first found by condition model in the grid by the checkbox marked with
		///     <see cref="SelectableCheckBoxAttribute" />
		/// </summary>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="conditionFunction">The model condition to perform selection.</param>
		/// <returns>The HtmlElementToBlockMappingModel.</returns>
		public HtmlElementToBlockMappingModel<TModel> SelectFirstFoundByConditionModelInTheGrid<TView, TModel>(
			Func<TModel, bool> conditionFunction) where TModel : class, new() where TView : View
		{
			var foundItem = GetFirstGridMappingModel<TView, TModel>(conditionFunction);
			if (foundItem == null)
			{
				Console.WriteLine("The object with following properties not found");
				conditionFunction.Target.PrintPublicPropertiesToConsole();
				throw new ArgumentException("The mapping to select not found");
			}

			foundItem.GetSelectableItem.Select();
			return foundItem;
		}

		/// <summary>
		///     Select single model from the grid with paging by the checkbox marked with
		///     <see cref="SelectableCheckBoxAttribute" /> from the grid with paging.
		/// </summary>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="modelToSelect">The model that should be selected.</param>
		/// <param name="matchCondition">The predicate to match models in the grid. If null then TModel.Equals is used.</param>
		/// <returns>The HtmlElementToBlockMappingModel.</returns>
		public HtmlElementToBlockMappingModel<TModel> SelectSingleModelInTheGridWithPaging<TView, TModel>(
			TModel modelToSelect, Func<TModel, bool> matchCondition = null) where TModel : class, new() where TView : View
		{
			var equalsCondition = matchCondition ?? modelToSelect.Equals;
			return SelectFirstFoundByConditionModelInTheGridWithPaging<TView, TModel>(equalsCondition);
		}

		/// <summary>
		///     Select first found by condition model in the grid with paging by the checkbox marked with
		///     <see cref="SelectableCheckBoxAttribute" />
		/// </summary>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="conditionFunction">The model condition to perform selection.</param>
		/// <returns>The HtmlElementToBlockMappingModel.</returns>
		public HtmlElementToBlockMappingModel<TModel> SelectFirstFoundByConditionModelInTheGridWithPaging<TView, TModel>(
			Func<TModel, bool> conditionFunction) where TModel : class, new() where TView : View
		{
			var foundItem =
				GetHtmlElementToBlockMappingsFromTheGridWithPaging<TView, TModel>(conditionFunction, true).FirstOrDefault();
			if (foundItem == null)
			{
				Console.WriteLine("The object with following properties not found");
				conditionFunction.Target.PrintPublicPropertiesToConsole();
				throw new ArgumentException("The mapping to select not found");
			}

			foundItem.GetSelectableItem.Select();
			return foundItem;
		}

		/// <summary>
		///     Deselect single model from the grid by the checkbox marked with <see cref="SelectableCheckBoxAttribute" /> within
		///     current page.
		/// </summary>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="modelToDeselect">The model that should be selected.</param>
		/// <param name="comparer">The comparer of models.</param>
		/// <returns>
		///     The deselected mapping model.
		/// </returns>
		public HtmlElementToBlockMappingModel<TModel> DeselectSingleModelInTheGrid<TView, TModel>(TModel modelToDeselect,
			IEqualityComparer<TModel> comparer = null) where TModel : class, new() where TView : View
		{
			var indexesToSelect = SetSeveralModelsInTheGrid<TView, TModel>(new[] {modelToDeselect}, false, comparer);
			return indexesToSelect[0];
		}

		/// <summary>
		///     Select multiple models from the grid by the checkbox marked with <see cref="SelectableCheckBoxAttribute" /> within
		///     current page.
		/// </summary>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="modelsToSelect">The models that should be selected.</param>
		/// <param name="comparer">The comparer of models.</param>
		/// <returns>The list of selected mapping models.</returns>
		public IList<HtmlElementToBlockMappingModel<TModel>> SelectSeveralModelsInTheGrid<TView, TModel>(
			IList<TModel> modelsToSelect, IEqualityComparer<TModel> comparer = null) where TModel : class, new()
			where TView : View
		{
			return SetSeveralModelsInTheGrid<TView, TModel>(modelsToSelect, true, comparer);
		}

		/// <summary>
		///     Deselect multiple models from the grid by the checkbox marked with <see cref="SelectableCheckBoxAttribute" />
		///     within current page.
		/// </summary>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="modelsToDeselect">The models that should be selected.</param>
		/// <param name="comparer">The comparer of models.</param>
		/// <returns>The list of deselected mapping models.</returns>
		public IList<HtmlElementToBlockMappingModel<TModel>> DeselectSeveralModelsInTheGrid<TView, TModel>(
			IList<TModel> modelsToDeselect, IEqualityComparer<TModel> comparer = null) where TModel : class, new()
			where TView : View
		{
			return SetSeveralModelsInTheGrid<TView, TModel>(modelsToDeselect, false, comparer);
		}

		/// <summary>
		///     Select multiple models from the grid by the checkbox marked with <see cref="SelectableCheckBoxAttribute" />
		///     It tries to select several models on the same page (if paging), not only the current one.
		///     It works incorrectly in a paged grid with target models on different pages (this is due to limitations of selection
		///     in grids with paging).
		/// </summary>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="modelsToSelect">The models that should be selected.</param>
		/// <param name="setValue">If true, the model should be selected, false - deselected.</param>
		/// <param name="comparer">The comparer of the models.</param>
		/// <returns>Blocks and models collection.</returns>
		public IList<HtmlElementToBlockMappingModel<TModel>> SetSeveralModelsInTheGrid<TView, TModel>(
			IList<TModel> modelsToSelect, bool setValue, IEqualityComparer<TModel> comparer = null) where TModel : class, new()
			where TView : View
		{
			// Build condition function to select only models from the list
			Func<TModel, bool> matchFunction = model => modelsToSelect.Contains(model, comparer);

			// Build process function to stop grid rows enumeration when all models from the list are already found
			// This is based on counting models, so it can work wrong if there are duplicated models in the list or grid
			var modelsCount = modelsToSelect.Count;
			Func<HtmlElementToBlockMappingModel<TModel>, bool> processFunction = model => --modelsCount > 0;

			return SetSeveralModelsInTheGridByCondition<TView, TModel>(matchFunction, processFunction, setValue, false);
		}

		/// <summary>
		///     Deselect models from the grid by the checkbox marked with <see cref="SelectableCheckBoxAttribute" /> by property
		///     name.
		///     For example if you want to deselect the <see cref="ProfileModel" /> instances where Code = "ABCD" on the
		///     <see cref="ProfileView" /> you should call.
		///     <code>
		/// DeselectModelsInTheGridByPropertyValue{ProfileView, ProfileModel}("Code", "ABCD");.
		/// </code>
		/// </summary>
		/// <typeparam name="TView">
		///     >View object which represents the entire grid and contains the <see cref="IList" /> of blocks
		///     of grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="propertyName">The name of the property inside of model.</param>
		/// <param name="propertyValue">The value of the property.</param>
		/// <returns>Indexes of selected/deselcted models.</returns>
		public IList<HtmlElementToBlockMappingModel<TModel>> DeselectModelsInTheGridByPropertyValue<TView, TModel>(
			string propertyName, string propertyValue) where TModel : class, new() where TView : View
		{
			return SetModelsInTheGridByPropertyValue<TView, TModel>(propertyName, propertyValue, false);
		}

		/// <summary>
		///     Select models from the grid by the checkbox marked with <see cref="SelectableCheckBoxAttribute" /> by property
		///     name.
		///     For example if you want to select the <see cref="ProfileModel" /> instances where Code = "ABCD" on the
		///     <see cref="ProfileView" /> you should call.
		///     <code>
		/// SelectModelsInTheGridByPropertyValue{ProfileView, ProfileModel}("Code", "ABCD");.
		/// </code>
		/// </summary>
		/// <typeparam name="TView">
		///     >View object which represents the entire grid and contains the <see cref="IList" /> of blocks
		///     of grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="propertyName">The name of the property inside of model.</param>
		/// <param name="propertyValue">The value of the property.</param>
		/// <returns>Indexes of selected/deselcted models.</returns>
		public IList<HtmlElementToBlockMappingModel<TModel>> SelectModelsInTheGridByPropertyValue<TView, TModel>(
			string propertyName, string propertyValue) where TModel : class, new() where TView : View
		{
			return SetModelsInTheGridByPropertyValue<TView, TModel>(propertyName, propertyValue, true);
		}

		/// <summary>
		///     Select/Deselect models from the grid by the checkbox marked with <see cref="SelectableCheckBoxAttribute" /> by
		///     property name.
		///     For example if you want to select the <see cref="ProfileModel" /> instances where Code = "ABCD" on the
		///     <see cref="ProfileView" /> you should call.
		///     <code>
		/// SetModelsInTheGridByPropertyValue{ProfileView, ProfileModel}("Code", "ABCD", true);.
		/// </code>
		/// </summary>
		/// <typeparam name="TView">
		///     >View object which represents the entire grid and contains the <see cref="IList" /> of blocks
		///     of grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="propertyName">The name of the property inside of model.</param>
		/// <param name="propertyValue">The value of the property.</param>
		/// <param name="setValue">If true, the model should be selected, false - deselected.</param>
		/// <returns>selected/deselcted mappings.</returns>
		public IList<HtmlElementToBlockMappingModel<TModel>> SetModelsInTheGridByPropertyValue<TView, TModel>(
			string propertyName, string propertyValue, bool setValue) where TModel : class, new() where TView : View
		{
			Func<TModel, bool> selectByPropertyValueCondition = model =>
			{
				var foundProperty = typeof(TModel).GetProperty(propertyName);
				return foundProperty.GetValue(model, null).Equals(propertyValue);
			};
			return SetSeveralModelsInTheGridByCondition<TView, TModel>(selectByPropertyValueCondition, null, setValue, true);
		}

		/// <summary>
		///     Sets the model in the grid by found index and condition.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="selectModelFunction">The select model function.</param>
		/// <param name="index">The index.</param>
		/// <param name="setValue">if set to <c>true</c> select, otherwise deselect.</param>
		/// <returns>Block and model.</returns>
		public HtmlElementToBlockMappingModel<TModel> SetModelInTheGridByFoundIndexAndCondition<TView, TModel>(
			Func<TModel, bool> selectModelFunction, int index, bool setValue) where TModel : class, new() where TView : View
		{
			var mappingToSelect =
				GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>(selectModelFunction).ElementAt(index);
			mappingToSelect.GetSelectableItem.Set(setValue);
			return mappingToSelect;
		}

		/// <summary>
		///     Selects the several models in the grid by condition within current page.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="selectModelFunction">The select model function.</param>
		/// <returns>The items to select.</returns>
		/// <exception cref="System.ArgumentException">There are no items to select in the grid.</exception>
		public IList<HtmlElementToBlockMappingModel<TModel>> SelectModelsFromTheCurrentViewByCondition<TView, TModel>(
			Func<TModel, bool> selectModelFunction) where TModel : class, new() where TView : View
		{
			return SetSeveralModelsInTheGridByCondition<TView, TModel>(selectModelFunction, null, true, true);
		}

		/// <summary>
		///     Deselects the several models in the grid by condition within current page.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="selectModelFunction">The select model function.</param>
		/// <returns>The items to select.</returns>
		/// <exception cref="System.ArgumentException">There are no items to select in the grid.</exception>
		public IList<HtmlElementToBlockMappingModel<TModel>> DeselectModelsFromTheCurrentViewByCondition<TView, TModel>(
			Func<TModel, bool> selectModelFunction) where TModel : class, new() where TView : View
		{
			return SetSeveralModelsInTheGridByCondition<TView, TModel>(selectModelFunction, null, false, true);
		}

		/// <summary>
		///     Sets the several models in the grid by condition within current page.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="matchFunction">The match function.</param>
		/// <param name="processFunction">
		///     The process function. Returns true if the method should continue looking for models to
		///     set.
		/// </param>
		/// <param name="setValue">if set to <c>true</c> then select, otherwise deselect.</param>
		/// <param name="currentPageOnly">if set to <c>true</c> the methods will not click paging buttons.</param>
		/// <returns>Blocks and models collection.</returns>
		/// <exception cref="System.ArgumentException">There are no items to select in the grid.</exception>
		private IList<HtmlElementToBlockMappingModel<TModel>> SetSeveralModelsInTheGridByCondition<TView, TModel>(
			Func<TModel, bool> matchFunction, Func<HtmlElementToBlockMappingModel<TModel>, bool> processFunction, bool setValue,
			bool currentPageOnly) where TModel : class, new() where TView : View
		{
			processFunction = processFunction ?? (x => true);

			// Build process function to apply selection action and check if we want to stop processing (use processFunction from arguments)
			Func<HtmlElementToBlockMappingModel<TModel>, bool> innerProcessFunction = mappingModel =>
			{
				mappingModel.GetSelectableItem.Set(setValue);
				return processFunction(mappingModel);
			};

			var mappingsToSelect = ProcessGridRows<TView, TModel>(matchFunction, innerProcessFunction, currentPageOnly);

			if (mappingsToSelect.Count == 0)
			{
				throw new ArgumentException("There are no items to select in the grid");
			}

			return mappingsToSelect;
		}

		#endregion

		#region GET METHODS

		/// <summary>
		///     Gets the HTML element to block mappings from current view.
		/// </summary>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="filter">The filter function for TModel.</param>
		/// <param name="process">The process function for HtmlElementToBlockMappingModel.</param>
		/// <param name="skipCount">The skip count.</param>
		/// <returns>Blocks and models collection.</returns>
		public IEnumerable<HtmlElementToBlockMappingModel<TModel>> GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>
			(Func<TModel, bool> filter = null, Func<HtmlElementToBlockMappingModel<TModel>, bool> process = null,
				int skipCount = 0) where TModel : new() where TView : View
		{
			filter = filter ?? (x => true);
			process = process ?? (x => true);
			var gridView = Resolve<TView>();
			var propertyStorageListContent = GetPropertyStorageListContent(gridView);
			var gridContentList = GetGridBlocksList<TView>(propertyStorageListContent);

			var genericParameterType = HtmlElementUtils.GetGenericParameterType(propertyStorageListContent);
			var matchedViewModelProperties =
				genericParameterType.GetPublicPropertiesWithAttribute(typeof(ModelPropertyNameAttribute));

			var processFlag = true;
			var index = 0;

			foreach (var item in gridContentList.Cast<object>())
			{
				// Skip several first items
				if (index >= skipCount)
				{
					var model = BuildModelFromPageObject<TModel>(item, matchedViewModelProperties);

					// Filter the model
					if (filter(model))
					{
						var blockMappingModel = new HtmlElementToBlockMappingModel<TModel>
						{
							Index = index + skipCount,
							HtmlElementBlock = item,
							Model = model
						};

						// Process model and set stop flag (processingFlag = false) if necessary
						processFlag = processFlag && process(blockMappingModel);
						yield return blockMappingModel;

						if (!processFlag)
						{
							yield break;
						}
					}
				}

				index++;
			}
		}

		/// <summary>
		///     Get the list of entities from the entire grid with paging.
		/// </summary>
		/// <typeparam name="TModel">Type of the model.</typeparam>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <returns>The result list of retrieved models.</returns>
		public IList<TModel> GetModelsList<TView, TModel>() where TModel : new() where TView : View
		{
			return GetHtmlElementToBlockMappings<TView, TModel>().Select(m => m.Model).ToList();
		}

		/// <summary>
		///     Get the list of entities from the current view.
		/// </summary>
		/// <typeparam name="TModel">Type of the model.</typeparam>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute <see cref="PropertyStorageListAttribute" />
		/// </typeparam>
		/// <returns>The result list of retrieved models.</returns>
		public IList<TModel> GetModelsListFromCurrentView<TView, TModel>() where TModel : new() where TView : View
		{
			return GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>().Select(m => m.Model).ToList();
		}

		/// <summary>
		///     Gets the items from the grid.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="compareModelFunction">The compare model function.</param>
		/// <param name="breakIfConditionMet">
		///     if set to <c>true</c> then do not navigate on the next grid page and return first
		///     found item on the page.
		/// </param>
		/// <param name="processFunction">The process function.</param>
		/// <returns>Blocks and models collection.</returns>
		public IList<HtmlElementToBlockMappingModel<TModel>> GetHtmlElementToBlockMappingsFromTheGridWithPaging<TView, TModel>
			(Func<TModel, bool> compareModelFunction, bool breakIfConditionMet,
				Func<HtmlElementToBlockMappingModel<TModel>, bool> processFunction = null) where TModel : new() where TView : View
		{
			var processFlag = true;
			Func<HtmlElementToBlockMappingModel<TModel>, bool> innerProcessFunction = x =>
			{
				var isProcessingInProgress = !breakIfConditionMet && (processFunction == null || processFunction(x));
				processFlag = processFlag && isProcessingInProgress;
				return processFlag;
			};

			var gridScenarios = Resolve<GridScenarios>();
			var foundItems = new List<HtmlElementToBlockMappingModel<TModel>>();

			gridScenarios.NavigateToFirstPage();

			while (true)
			{
				Browser.WaitAjax();
				foundItems.AddRange(GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>(compareModelFunction,
					innerProcessFunction));

				// Processing is stopped by processing function or it's the last page
				if (!processFlag || gridScenarios.IsPaginateTextButtonDisabled(PagingItem.Next))
				{
					break;
				}

				gridScenarios.NavigateToItem(PagingItem.Next);
			}

			return foundItems;
		}

		/// <summary>
		///     Gets the items from the grid.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="compareModelFunction">The compare model function.</param>
		/// <param name="breakIfConditionMet">
		///     if set to <c>true</c> then do not scroll the page and return first found item on the
		///     page.
		/// </param>
		/// <returns>Blocks and models collection.</returns>
		public IList<HtmlElementToBlockMappingModel<TModel>> GetHtmlElementToBlockMappingsFromTheGridWithScrolling
			<TView, TModel>(Func<TModel, bool> compareModelFunction, bool breakIfConditionMet) where TModel : new()
			where TView : BaseGridView
		{
			var scrollScenarios = Resolve<ScrollScenarios>();
			IList<HtmlElementToBlockMappingModel<TModel>> foundItems = new List<HtmlElementToBlockMappingModel<TModel>>();
			Browser.WaitAjax();
			while (scrollScenarios.IsScrollingAvailable())
			{
				var gridMappings = GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>();
				foreach (var htmlElementToBlockMappingModel in gridMappings)
				{
					if (compareModelFunction(htmlElementToBlockMappingModel.Model))
					{
						foundItems.Add(htmlElementToBlockMappingModel);
					}
				}

				if (foundItems.Count > 0 && breakIfConditionMet)
				{
					return foundItems;
				}

				scrollScenarios.ScrollAllPageDown(1);
				Browser.WaitAjax();
			}

			var lastPageMappings = GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>();
			foreach (var htmlElementToBlockMappingModel in lastPageMappings)
			{
				if (compareModelFunction(htmlElementToBlockMappingModel.Model))
				{
					foundItems.Add(htmlElementToBlockMappingModel);
				}
			}

			return foundItems;
		}

		/// <summary>
		///     Gets the HTML element to block mappings, works with both grids with/without paging.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <returns>Blocks and models collection.</returns>
		public IEnumerable<HtmlElementToBlockMappingModel<TModel>> GetHtmlElementToBlockMappings<TView, TModel>()
			where TView : View where TModel : new()
		{
			return !Resolve<GridScenarios>().IsPagingPresent
				? GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>()
				: GetHtmlElementToBlockMappingsFromTheGridWithPaging<TView, TModel>(model => true, false);
		}

		/// <summary>
		///     Gets the mapping models from the grid scrolling or paging the grid.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="filter">The filter function.</param>
		/// <param name="breakIfFirstFound">if set to <c>true</c> the first found item is returned and the processing is stopped.</param>
		/// <returns>Mapping models from the grid.</returns>
		private IEnumerable<HtmlElementToBlockMappingModel<TModel>> GetInnerGridMappingModels<TView, TModel>(
			Func<TModel, bool> filter, bool breakIfFirstFound) where TView : View where TModel : new()
		{
			filter = filter ?? (x => true);

			var gridScenarios = Resolve<GridScenarios>();
			if (gridScenarios.IsPagingPresent)
			{
				Logger.WriteLine("Get models from grid with paging");
				return GetHtmlElementToBlockMappingsFromTheGridWithPaging<TView, TModel>(filter, breakIfFirstFound);
			}

			Func<HtmlElementToBlockMappingModel<TModel>, bool> process = null;
			if (breakIfFirstFound)
			{
				// This will select the only record causing underlying selector to stop processing (returns false for the first found model)
				process = x => false;
			}

			var scrollScenarios = Resolve<ScrollScenarios>();
			if (scrollScenarios.IsScrollingAvailable())
			{
				Logger.WriteLine("Get models from grid with scrolling");
				scrollScenarios.ScrollAllPageDown();
				return GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>(filter, process);
			}

			Logger.WriteLine("Get models from one page grid");
			return GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>(filter, process);
		}

		/// <summary>
		///     Processes the grid rows going or scrolling through all pages.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="filter">The filter function.</param>
		/// <param name="processFunction">The process function. If returns false then further rows should not be processed.</param>
		/// <param name="currentPageOnly">if set to <c>true</c> the methods will not click paging buttons.</param>
		/// <returns>
		///     The list of items from the grid, filtered by the filter before processFunction stopped processing.
		/// </returns>
		private IList<HtmlElementToBlockMappingModel<TModel>> ProcessGridRows<TView, TModel>(Func<TModel, bool> filter,
			Func<HtmlElementToBlockMappingModel<TModel>, bool> processFunction, bool currentPageOnly) where TView : View
			where TModel : new()
		{
			filter = filter ?? (x => true);

			var gridScenarios = Resolve<GridScenarios>();
			if (gridScenarios.IsPagingPresent)
			{
				if (currentPageOnly)
				{
					Logger.WriteLine("Get models from grid with paging (current page only)");
					return GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>(filter, processFunction).ToList();
				}

				Logger.WriteLine("Get models from grid with paging");
				return GetHtmlElementToBlockMappingsFromTheGridWithPaging<TView, TModel>(filter, false, processFunction).ToList();
			}

			var scrollScenarios = Resolve<ScrollScenarios>();
			if (scrollScenarios.IsScrollingAvailable())
			{
				Logger.WriteLine("Get models from grid with scrolling");
				scrollScenarios.ScrollAllPageDown();
				return GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>(filter, processFunction).ToList();
			}

			Logger.WriteLine("Get models from one page grid");
			return GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>(filter, processFunction).ToList();
		}

		/// <summary>
		///     Gets the mapping models from the grid going or scrolling through all pages.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="filter">The filter function.</param>
		/// <returns>The list of items from the grid, filtered by the filter.</returns>
		public IList<HtmlElementToBlockMappingModel<TModel>> GetGridMappingModels<TView, TModel>(
			Func<TModel, bool> filter = null) where TView : View where TModel : new()
		{
			return GetInnerGridMappingModels<TView, TModel>(filter, false).ToList();
		}

		/// <summary>
		///     Gets the first mapping model, which matches the filter condition, from the grid going or scrolling through all
		///     pages.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="filter">The filter function.</param>
		/// <returns>The first item from the grid that matches the filter condition.</returns>
		public HtmlElementToBlockMappingModel<TModel> GetFirstGridMappingModel<TView, TModel>(Func<TModel, bool> filter = null)
			where TView : View where TModel : new()
		{
			return GetInnerGridMappingModels<TView, TModel>(filter, true).FirstOrDefault();
		}

		/// <summary>
		///     Gets the models from the grid going or scrolling through all pages.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="filter">The filter function.</param>
		/// <returns>The list of items from the grid, filtered by the filter.</returns>
		public IList<TModel> GetGridModels<TView, TModel>(Func<TModel, bool> filter = null) where TView : View
			where TModel : new()
		{
			return GetInnerGridMappingModels<TView, TModel>(filter, false).Select(x => x.Model).ToList();
		}

		/// <summary>
		///     Gets the first model, which matches the filter condition, from the grid going or scrolling through all pages.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="filter">The filter function.</param>
		/// <returns>The first item from the grid that matches the filter condition.</returns>
		public TModel GetFirstGridModel<TView, TModel>(Func<TModel, bool> filter = null) where TView : View
			where TModel : new()
		{
			return GetInnerGridMappingModels<TView, TModel>(filter, true).Select(x => x.Model).FirstOrDefault();
		}

		/// <summary>
		///     Makes the grid row visible scrolling or paging the grid.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="match">The predicate for the row to make visible.</param>
		/// <returns>True if the row is made visible, False otherwise (e.g. there is no matching row).</returns>
		public bool MakeGridRowVisible<TView, TModel>(Func<TModel, bool> match = null) where TView : View where TModel : new()
		{
			return GetFirstGridMappingModel<TView, TModel>(match) != null;
		}

		/// <summary>
		///     Makes the grid row visible scrolling or paging the grid.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="model">The model of the row to make visible.</param>
		/// <returns>True if the row is made visible, False otherwise (e.g. there is no matching row).</returns>
		public bool MakeGridRowVisible<TView, TModel>(TModel model) where TView : BaseGridView where TModel : new()
		{
			return GetFirstGridMappingModel<TView, TModel>(x => model.Equals(x)) != null;
		}

		#endregion

		#region VERIFY METHODS

		/// <summary>
		///     Verifies the default sorting order.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		public void VerifyDefaultSortingOrder<TView, TModel>() where TModel : new() where TView : View
		{
			var existingModels = GetHtmlElementToBlockMappingsFromCurrentView<TView, TModel>().Select(x => x.Model).ToList();
			Verify.IsTrue(existingModels.Count > 0,
				"There is no item on the list. Thus, the default sorting order verification can not be performed.");
			VerifyDefaultSortingOrder(existingModels);
		}

		/// <summary>
		///     Verifies the default sorting order. The default sorting order is defined by the property or properties marked with
		///     attribute <see cref="DefaultSortingColumnAttribute" />.
		///     A maximum of 2 columns can be specified for the sorting and they will be applied in the order the properties apear
		///     in the model.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="existingModels">The existing models.</param>
		/// <exception cref="System.ArgumentException">Add DefaultSortingColumnAttribute for one property of the model.</exception>
		public void VerifyDefaultSortingOrder<TModel>(IList<TModel> existingModels) where TModel : new()
		{
			IList<TModel> sortedModels;

			var getDefaultSortColumnProperties =
				typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(x => Attribute.IsDefined(x, typeof(DefaultSortingColumnAttribute)))
					.ToList();

			if (!(getDefaultSortColumnProperties.Count > 0))
			{
				throw new ArgumentException("Add DefaultSortingColumnAttribute for one property of the model");
			}

			var defaultSortColumnProperty = getDefaultSortColumnProperties.First();
			var sortOrderFromAttribute = defaultSortColumnProperty.GetCustomAttribute<DefaultSortingColumnAttribute>().SortOrder;

			switch (getDefaultSortColumnProperties.Count)
			{
				case 1:

					// Sort by One Column
					sortedModels = sortOrderFromAttribute == SortOrder.Ascending
						? existingModels.OrderBy(x => defaultSortColumnProperty.GetValue(x, null)).ToList()
						: existingModels.OrderByDescending(x => defaultSortColumnProperty.GetValue(x, null)).ToList();
					break;
				case 2:
				{
					// Sort by Two Columns
					var defaultSecondSortColumnProperty = getDefaultSortColumnProperties[1];
					var secondSortOrderFromAttribute =
						defaultSecondSortColumnProperty.GetCustomAttribute<DefaultSortingColumnAttribute>().SortOrder;

					if (sortOrderFromAttribute == SortOrder.Ascending)
					{
						sortedModels = secondSortOrderFromAttribute == SortOrder.Ascending
							? existingModels.OrderBy(x => defaultSortColumnProperty.GetValue(x, null))
								.ThenBy(x => defaultSecondSortColumnProperty.GetValue(x, null))
								.ToList()
							: existingModels.OrderBy(x => defaultSecondSortColumnProperty.GetValue(x, null))
								.ThenByDescending(x => defaultSecondSortColumnProperty.GetValue(x, null))
								.ToList();
					}
					else
					{
						sortedModels = secondSortOrderFromAttribute == SortOrder.Ascending
							? existingModels.OrderByDescending(x => defaultSortColumnProperty.GetValue(x, null))
								.ThenBy(x => defaultSecondSortColumnProperty.GetValue(x, null))
								.ToList()
							: existingModels.OrderByDescending(x => defaultSecondSortColumnProperty.GetValue(x, null))
								.ThenByDescending(x => defaultSecondSortColumnProperty.GetValue(x, null))
								.ToList();
					}
				}

					break;
				default:
					throw new NotImplementedException("A Maximum of 2 Sorting Columns are currently implemented.");
			}

			Verify.AreCollectionsEqual(sortedModels, existingModels, false);
		}

		/// <summary>
		///     Verify several models not displayed (e.g. not added) in the grid.
		///     Note the <see cref="TModel" /> should either extend <see cref="IEquatable{TModel}" /> with overrided methods
		///     <see cref="IEquatable{TModel}.Equals(TModel)" />, <see cref="object.Equals(object)" /> and
		///     <see cref="object.GetHashCode" /> like in <see cref="FunctionModel" />.
		/// </summary>
		/// <typeparam name="TModel">Type of the model.</typeparam>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute.
		/// </typeparam>
		/// <param name="modelsToVerify">The list of the models to verify.</param>
		/// <param name="comparer">Equality comparer.</param>
		public void VerifySeveralModelsNotDisplayedInTheGrid<TView, TModel>(IList<TModel> modelsToVerify,
			IEqualityComparer<TModel> comparer = null) where TModel : new() where TView : View
		{
			var filterDisplayedModels =
				new Func<IList<TModel>, IList<TModel>>(allModels => modelsToVerify.Intersect(allModels, comparer).ToList());
			const string ErrorMessage = "ERROR: Models displayed in the grid: {0}";
			BaseVerifySeveralModelsInTheGrid<TView, TModel>(filterDisplayedModels, ErrorMessage);
		}

		/// <summary>
		///     Verify single model not displayed (e.g. not added) in the grid.
		///     Note the <see cref="TModel" /> should extend <see cref="IEquatable{TModel}" /> with overrided methods
		///     <see cref="IEquatable{TModel}.Equals(TModel)" />, <see cref="object.Equals(object)" /> and
		///     <see cref="object.GetHashCode" /> like in <see cref="FunctionModel" />.
		/// </summary>
		/// <typeparam name="TModel">Type of the model.</typeparam>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute.
		/// </typeparam>
		/// <param name="modelToVerify">The model to verify.</param>
		public void VerifySingleModelNotDisplayedInTheGrid<TView, TModel>(TModel modelToVerify) where TModel : new()
			where TView : View
		{
			BaseVerifySingleModelsInTheGrid<TView, TModel>(modelToVerify, false);
		}

		/// <summary>
		///     Verify several models displayed (e.g. added) in the grid.
		///     Note the <see cref="TModel" /> should either extend <see cref="IEquatable{TModel}" /> with overrided methods
		///     <see cref="IEquatable{TModel}.Equals(TModel)" />, <see cref="object.Equals(object)" /> and
		///     <see cref="object.GetHashCode" /> like in <see cref="FunctionModel" />.
		/// </summary>
		/// <typeparam name="TModel">Type of the model.</typeparam>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute.
		/// </typeparam>
		/// <param name="modelsToVerify">The list of the models to verify.</param>
		/// <param name="comparer">Equality Comparer.</param>
		public void VerifySeveralModelsDisplayedInTheGrid<TView, TModel>(IList<TModel> modelsToVerify,
			IEqualityComparer<TModel> comparer = null) where TModel : new() where TView : View
		{
			var filterNotDisplayedModels =
				new Func<IList<TModel>, IList<TModel>>(allModels => modelsToVerify.Except(allModels, comparer).ToList());
			const string ErrorMessage = "ERROR: Models not displayed in the grid: {0}";
			BaseVerifySeveralModelsInTheGrid<TView, TModel>(filterNotDisplayedModels, ErrorMessage);
		}

		/// <summary>
		///     Verify single model displayed (e.g. added) in the grid.
		///     Note the <see cref="TModel" /> should either extend <see cref="IEquatable{TModel}" /> with overrided methods
		///     <see cref="IEquatable{TModel}.Equals(TModel)" />, <see cref="object.Equals(object)" /> and
		///     <see cref="object.GetHashCode" /> like in <see cref="FunctionModel" />.
		/// </summary>
		/// <typeparam name="TModel">Type of the model.</typeparam>
		/// <typeparam name="TView">
		///     View object which represents the entire grid and contains the <see cref="IList" /> of blocks of
		///     grid items marked with attribute.
		/// </typeparam>
		/// <param name="modelToVerify">The model to verify.</param>
		public void VerifySingleModelDisplayedInTheGrid<TView, TModel>(TModel modelToVerify) where TModel : new()
			where TView : View
		{
			BaseVerifySingleModelsInTheGrid<TView, TModel>(modelToVerify, true);
		}

		/// <summary>
		///     Base verify for models in the grid.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="filterErrorModels">The filter error models action.</param>
		/// <param name="errorMessage">The error message.</param>
		private void BaseVerifySeveralModelsInTheGrid<TView, TModel>(Func<IList<TModel>, IList<TModel>> filterErrorModels,
			string errorMessage) where TModel : new() where TView : View
		{
			IList<TModel> actuallyExistingModels = GetGridModels<TView, TModel>().ToList();
			var errorModelsArray = filterErrorModels(actuallyExistingModels).ToList();
			VerifyModelsAndPrintMessage(errorModelsArray, errorMessage);
		}

		/// <summary>
		///     Bases the verify single models in the grid.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="modelToVerify">The model to verify.</param>
		/// <param name="checkIsModelPresent">
		///     if set to <c>true</c> then check is model displayed in th grid, otherwise - check the
		///     model not displayed in the grid.
		/// </param>
		private void BaseVerifySingleModelsInTheGrid<TView, TModel>(TModel modelToVerify, bool checkIsModelPresent)
			where TModel : new() where TView : View
		{
			string errorMessage = null;
			var model = GetFirstGridMappingModel<TView, TModel>(m => modelToVerify.Equals(m));

			// Check if the model is found in the grid
			if (checkIsModelPresent && model == null)
			{
				errorMessage = "ERROR: Models not displayed in the grid: {0}";
			}
			else if (!checkIsModelPresent && model != null)
			{
				// Check if the model is not found
				errorMessage = "ERROR: Models displayed in the grid: {0}";
			}

			if (errorMessage != null)
			{
				VerifyModelsAndPrintMessage(new List<TModel> {modelToVerify}, errorMessage);
			}
		}

		/// <summary>
		///     Verifies the models and print message.
		/// </summary>
		/// <typeparam name="TModel">The type of the T model.</typeparam>
		/// <param name="errorModelsArray">The error models array.</param>
		/// <param name="errorMessage">The error message.</param>
		private void VerifyModelsAndPrintMessage<TModel>(IList<TModel> errorModelsArray, string errorMessage)
		{
			var errorModelsArrayAsJson = errorModelsArray.Count > 0
				? MSTestReportUtils.ReplaceBrackets(JsonConvert.SerializeObject(errorModelsArray))
				: string.Empty;
			Verify.IsFalse(errorModelsArrayAsJson.Any(),
				string.Format(CultureInfo.InvariantCulture, errorMessage, errorModelsArrayAsJson));
		}

		/// <summary>
		///     Verify that <see cref="TextInput" /> objects marked with attribute <see cref="DefaultValueAttribute" /> are empty.
		/// </summary>
		/// <typeparam name="TView">The view with inputs.</typeparam>
		public void VerifyDefaultValues<TView>() where TView : View
		{
			var view = Resolve<TView>();
			var inErrorNonEmptyInputs =
				view.GetPropertyValuesWithFilteredAttribute<TextInput, DefaultValueAttribute>(x => x.Flag == DefaultValueFlag.Empty)
					.Where(x => !string.IsNullOrEmpty(x.Text))
					.ToArray();
			Verify.IsFalse(inErrorNonEmptyInputs.Any(),
				"ERROR: The following inputs are not empty: " +
				string.Join(", ",
					inErrorNonEmptyInputs.Select(x => x.Name)
						.Zip(inErrorNonEmptyInputs.Select(x => x.Text), (name, text) => name + ": " + text)));

			var inErrorSelectedCheckboxes =
				view.GetPropertyValuesWithFilteredAttribute<CheckBox, DefaultValueAttribute>(
					x => x.Flag == DefaultValueFlag.Empty || x.Flag == DefaultValueFlag.NotSelected).Where(x => x.Selected).ToArray();
			Verify.IsFalse(inErrorSelectedCheckboxes.Any(),
				"ERROR: The following checkboxes are selected: " + string.Join(", ", inErrorSelectedCheckboxes.Select(x => x.Name)));

			var inErrorNotSelectedCheckboxes =
				view.GetPropertyValuesWithFilteredAttribute<CheckBox, DefaultValueAttribute>(
					x => x.Flag == DefaultValueFlag.Selected).Where(x => !x.Selected).ToArray();
			Verify.IsFalse(inErrorNotSelectedCheckboxes.Any(),
				"ERROR: The following checkboxes are not selected: " +
				string.Join(", ", inErrorNotSelectedCheckboxes.Select(x => x.Name)));
		}

		#endregion

		#region COMMON PRIVATE METHODS

		/// <summary>
		///     Gets the name of the model value using model property.
		/// </summary>
		/// <param name="matchedViewModelProperty">The matched view model property.</param>
		/// <param name="model">The model.</param>
		/// <returns>The object.</returns>
		private object GetModelValueUsingModelPropertyName(PropertyInfo matchedViewModelProperty, object model)
		{
			var modelPropertyNameFromAttribute = matchedViewModelProperty.GetCustomAttribute<ModelPropertyNameAttribute>().Value;
			var modelProperty = model.GetType().GetProperty(modelPropertyNameFromAttribute);
			if (modelProperty == null)
			{
				throw new ArgumentException("Model " + model.GetType().Name + "not contains the property " +
				                            modelPropertyNameFromAttribute + ". Correct the value of ModelPropertyNameAttribute");
			}

			return modelProperty.GetValue(model, null);
		}

		/// <summary>
		///     Gets the content of the property storage list.
		/// </summary>
		/// <param name="gridView">The grid view.</param>
		/// <returns>The object.</returns>
		public PropertyInfo GetPropertyStorageListContent(object gridView)
		{
			var gridContentProperties =
				gridView.GetType()
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(x => Attribute.IsDefined(x, typeof(PropertyStorageListAttribute)))
					.ToList();

			if (gridContentProperties.Count != 1)
			{
				throw new ArgumentException("Define only one item as " + typeof(PropertyStorageListAttribute).Name + " for the view");
			}

			return gridContentProperties.First();
		}

		/// <summary>
		///     Builds the model entity from page object (html element or view).
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="pageObject">The page object.</param>
		/// <param name="matchedViewModelProperties">The matched view model properties.</param>
		/// <returns>The Model.</returns>
		/// <exception cref="System.ArgumentException">
		///     The property  + modelPropertyNameAttributeValue +  not existsts in the model
		///     typeof(TModel).FullName.
		/// </exception>
		private TModel BuildModelFromPageObject<TModel>(object pageObject,
			IEnumerable<PropertyInfo> matchedViewModelProperties) where TModel : new()
		{
			var constructedModel = new TModel();
			var readableWrappersContainer = new ReadableWrappersContainer();
			foreach (var matchedViewModelProperty in matchedViewModelProperties)
			{
				var modelPropertyNameAttributeValue =
					matchedViewModelProperty.GetCustomAttribute<ModelPropertyNameAttribute>().Value;
				var modelProperty =
					constructedModel.GetType()
						.GetProperties()
						.FirstOrDefault(property => property.Name == modelPropertyNameAttributeValue);
				if (modelProperty == null)
				{
					throw new ArgumentException("The property " + modelPropertyNameAttributeValue + " not existsts in the model" +
					                            typeof(TModel).FullName);
				}

				var readableWraper = readableWrappersContainer.GetWrapperByElementType(matchedViewModelProperty.PropertyType);
				readableWraper.ReadPageObjectPropertyToModelProperty(pageObject, matchedViewModelProperty, constructedModel,
					modelProperty);

				// Process trim attribute
				if (!matchedViewModelProperty.IsDefined(typeof(TrimAttribute)))
				{
					continue;
				}

				var value = modelProperty.GetValue(constructedModel) as string;
				if (value != null)
				{
					modelProperty.SetValue(constructedModel,
						Convert.ChangeType(value.Trim(), modelProperty.PropertyType, CultureInfo.InvariantCulture), null);
				}
			}

			return constructedModel;
		}

		#endregion
	}
}