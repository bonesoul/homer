﻿/*ISC License

Copyright (c) 2019, Daan Verstraten, daanverstraten@hotmail.com

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.*/

using System;
using uuid.net.Classes.UUID;

namespace uuid.net.Interfaces.IUUID_Generator {
    /// <summary>The interface responsible for determing how a <see cref="UUID"/> generator should behave</summary>
    public interface IUUIDGenerator {

        /// <summary>Gets the version of the UUID generator</summary>
        Int32 Version { get; }

        /// <summary>Gets the variant of the UUID generator</summary>
        Int32 Variant { get; }

        /// <summary>Gets if this <see cref="IUUIDGenerator"/> needs context to generate <see cref="UUID"/>s</summary>
        Boolean NeedContext { get; }

        /// <summary>Gets what type this <see cref="IUUIDGenerator"/> needs to generate a <see cref="UUID"/></summary>
        Type ContextType { get; }

        /// <summary>Returns a new <see cref="UUID"/></summary>
        /// <param name="Context">The context needed to generate this UUID</param>
        /// <returns>Returns a new <see cref="UUID"/></returns>
        UUID Generate(Object Context = default);

        /// <summary>Returns a new collection of <see cref="UUID"/></summary>
        /// <param name="Count">The amount of UUID to generate</param>
        /// <param name="Context">The context needed to generate this UUIDs</param>
        /// <returns>Returns a new collection of <see cref="UUID"/></returns>
        UUID[] Generate(Int32 Count, Object[] Context = default);
    }
}