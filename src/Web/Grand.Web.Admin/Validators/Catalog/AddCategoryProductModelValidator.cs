﻿using FluentValidation;
using Grand.Infrastructure;
using Grand.Infrastructure.Validators;
using Grand.Business.Core.Interfaces.Catalog.Categories;
using Grand.Business.Core.Interfaces.Common.Localization;
using Grand.Web.Admin.Extensions;
using Grand.Web.Admin.Models.Catalog;

namespace Grand.Web.Admin.Validators.Catalog
{
    public class AddCategoryProductModelValidator : BaseGrandValidator<CategoryModel.AddCategoryProductModel>
    {
        public AddCategoryProductModelValidator(
            IEnumerable<IValidatorConsumer<CategoryModel.AddCategoryProductModel>> validators,
            ITranslationService translationService, ICategoryService categoryService, IWorkContext workContext)
            : base(validators)
        {
            if (!string.IsNullOrEmpty(workContext.CurrentCustomer.StaffStoreId))
            {
                RuleFor(x => x).MustAsync(async (x, y, context) =>
                {
                    var category = await categoryService.GetCategoryById(x.CategoryId);
                    if (category != null)
                        if (!category.AccessToEntityByStore(workContext.CurrentCustomer.StaffStoreId))
                            return false;

                    return true;
                }).WithMessage(translationService.GetResource("Admin.Catalog.Categories.Permisions"));
            }
        }
    }
}