using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

[SuppressMessage("Minor Code Smell", "S4136:Method overloads should be grouped together")]
public static class PGroongaLinqExtensions
{
    #region TEXT fields

    /// <summary>
    /// This method generates the "&amp;@" match operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text, varchar or jsonb field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/match-v2.html</remarks>
    public static bool PGroongaMatch(this string query, string keyword) => throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;@~" query operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text, varchar or jsonb field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/query-v2.html</remarks>
    public static bool PGroongaQuery(this string query, string keyword) => throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;@*" similar search operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text or varchar field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/similar-search-v2.html</remarks>
    public static bool PGroongaSimilarSearch(this string query, string keyword) => throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;`" script operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text, varchar or jsonb field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/script-v2.html</remarks>
    public static bool PGroongaScriptQuery(this string query, string keyword) => throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;@|" match in operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text or varchar field</param>
    /// <param name="keywords">Keywords to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/match-in-v2.html</remarks>
    public static bool PGroongaMatchIn(this string query, IEnumerable<string> keywords) => throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;@~|" query in operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text or varchar field</param>
    /// <param name="keywords">Keywords to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/query-in-v2.html</remarks>
    public static bool PGroongaQueryIn(this string query, IEnumerable<string> keywords) => throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;^" prefix search operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text or varchar field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/prefix-search-v2.html</remarks>
    public static bool PGroongaPrefixSearch(this string query, string keyword) => throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;^~" prefix rk search operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text or varchar field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/prefix-rk-search-v2.html</remarks>
    public static bool PGroongaPrefixRkSearch(this string query, string keyword) => throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;^|" prefix search in operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text or varchar field</param>
    /// <param name="keywords">Keywords to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/prefix-search-in-v2.html</remarks>
    public static bool PGroongaPrefixSearchIn(this string query, IEnumerable<string> keywords) =>
        throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;^~|" prefix rk search in operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text or varchar field</param>
    /// <param name="keywords">Keywords to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/prefix-rk-search-in-v2.html</remarks>
    public static bool PGroongaPrefixRkSearchIn(this string query, IEnumerable<string> keywords) =>
        throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;~" regex match operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text or varchar field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/regular-expression-v2.html</remarks>
    public static bool PGroongaRegexpMatch(this string query, string keyword) => throw new NotSupportedException();

    #endregion

    #region ARRAY(TEXT) fields

    /// <summary>
    /// This method generates the "&amp;@" match operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text[] field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/match-v2.html</remarks>
    public static bool PGroongaMatch(this IEnumerable<string> query, string keyword) => throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;@~" query operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text[] field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/query-v2.html</remarks>
    public static bool PGroongaQuery(this IEnumerable<string> query, string keyword) => throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;@*" similar search operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text[] field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/similar-search-v2.html</remarks>
    public static bool PGroongaSimilarSearch(this IEnumerable<string> query, string keyword) =>
        throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;`" script operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text[] field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/script-v2.html</remarks>
    public static bool PGroongaScriptQuery(this IEnumerable<string> query, string keyword) => throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;@|" match in operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text[] field</param>
    /// <param name="keywords">Keywords to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/match-in-v2.html</remarks>
    public static bool PGroongaMatchIn(this IEnumerable<string> query, IEnumerable<string> keywords) =>
        throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;@~|" query in operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text[] field</param>
    /// <param name="keywords">Keywords to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/query-in-v2.html</remarks>
    public static bool PGroongaQueryIn(this IEnumerable<string> query, IEnumerable<string> keywords) =>
        throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;^" prefix search operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text[] field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/prefix-search-v2.html</remarks>
    public static bool PGroongaPrefixSearch(this IEnumerable<string> query, string keyword) =>
        throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;^~" prefix rk search operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text[] field</param>
    /// <param name="keyword">Keyword to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/prefix-rk-search-v2.html</remarks>
    public static bool PGroongaPrefixRkSearch(this IEnumerable<string> query, string keyword) =>
        throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;^|" prefix search in operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text[] field</param>
    /// <param name="keywords">Keywords to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/prefix-search-in-v2.html</remarks>
    public static bool PGroongaPrefixSearchIn(this IEnumerable<string> query, IEnumerable<string> keywords) =>
        throw new NotSupportedException();

    /// <summary>
    /// This method generates the "&amp;^~|" prefix rk search in operator
    /// </summary>
    /// <param name="query">A plain search query, should be a text[] field</param>
    /// <param name="keywords">Keywords to search</param>
    /// <remarks>https://pgroonga.github.io/reference/operators/prefix-rk-search-in-v2.html</remarks>
    public static bool PGroongaPrefixRkSearchIn(this IEnumerable<string> query, IEnumerable<string> keywords) =>
        throw new NotSupportedException();

    #endregion
}
