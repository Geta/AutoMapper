// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.Mapping
{
    public abstract class Mapper<TFrom, TTo> : IMapper<TFrom, TTo>, ICreateFrom<TFrom, TTo>
        where TTo : new()
    {
        public virtual TTo Create(TFrom from)
        {
            var to = new TTo();
            Map(from, to);
            return to;
        }

        public abstract void Map(TFrom from, TTo to);
    }
}