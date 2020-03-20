// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.Mapping
{
    public interface ICreateFrom<in TFrom, out TTo>
    {
        TTo Create(TFrom from);
    }
}