// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.Mapping
{
    public interface IMapper<in TFrom, in TTo>
    {
        void Map(TFrom from, TTo to);
    }
}