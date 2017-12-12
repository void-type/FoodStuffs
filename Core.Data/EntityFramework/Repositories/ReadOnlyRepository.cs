﻿using Core.Model.Services.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Core.Data.EntityFramework.Repositories
{
    public class ReadOnlyRepository<TInterface, TDbEntity> : DatabaseService, IReadOnlyRepository<TInterface>
        where TDbEntity : class, TInterface, new() where TInterface : class

    {
        public virtual IQueryable<TInterface> Stored => Context.Set<TDbEntity>();

        public ReadOnlyRepository(DbContext context) : base(context)
        {
        }
    }
}