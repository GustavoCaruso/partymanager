﻿using FluentValidation;
using partymanager.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace partymanager.Domain.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        TModel Add<TModel, TValidator>(TModel inputModel)
        where TValidator : AbstractValidator<TEntity>
        where TModel : class;
        void Delete(int id);
        IEnumerable<TModel> Get<TModel>() where TModel : class;
        TModel GetById<TModel>(int id) where TModel : class;
        TModel Update<TModel, TValidator>(TModel inputModel)
        where TValidator : AbstractValidator<TEntity>
        where TModel : class;

        public IEnumerable<TModel> GetFiltro<TModel>(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
         string includeProperties = null,
         int? take = null)
         where TModel : class;
    }
}
