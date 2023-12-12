using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace HyperDimension.Library.PGroonga.ExpressionTranslators;

public class PGroongaMethodCallTranslator : IMethodCallTranslator
{
    private readonly SqlExpressionFactory _sqlExpressionFactory;
    private readonly RelationalTypeMapping _boolMapping;

    public PGroongaMethodCallTranslator(SqlExpressionFactory sqlExpressionFactory,
        IRelationalTypeMappingSource typeMappingSource)
    {
        ArgumentNullException.ThrowIfNull(typeMappingSource);

        _sqlExpressionFactory = sqlExpressionFactory;
        _boolMapping = typeMappingSource.FindMapping(typeof(bool))!;
    }

    private static readonly Dictionary<string, string> SqlNameByMethodName = new Dictionary<string, string>
    {
        [nameof(PGroongaDbFunctionsExtensions.PGroongaCommand)] = "pgroonga_command",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaCommandEscapeValue)] = "pgroonga_command_escape_value",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaEscape)] = "pgroonga_escape",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaFlush)] = "pgroonga_flush",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaHighlightHtml)] = "pgroonga_highlight_html",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaIsWritable)] = "pgroonga_is_writable",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaMatchPositionsByte)] = "pgroonga_match_positions_byte",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaMatchPositionsCharacter)] = "pgroonga_match_positions_character",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaNormalize)] = "pgroonga_normalize",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaQueryEscape)] = "pgroonga_query_escape",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaQueryExpand)] = "pgroonga_query_expand",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaQueryExtractKeywords)] = "pgroonga_query_extract_keywords",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaSetWritable)] = "pgroonga_set_writable",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaScore)] = "pgroonga_score",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaSnippetHtml)] = "pgroonga_snippet_html",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaTableName)] = "pgroonga_table_name",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaWalApply)] = "pgroonga_wal_apply",
        [nameof(PGroongaDbFunctionsExtensions.PGroongaWalTruncate)] = "pgroonga_wal_truncate"
    };


    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        ArgumentNullException.ThrowIfNull(method);

        if (method.DeclaringType != typeof(PGroongaDbFunctionsExtensions) &&
            method.DeclaringType != typeof(PGroongaLinqExtensions))
        {
            return null;
        }

        if (!SqlNameByMethodName.TryGetValue(method.Name, out var sqlFunctionName))
        {
            return TryTranslateOperator(method, arguments);
        }

        if (sqlFunctionName != "pgroonga_score")
        {
            return _sqlExpressionFactory.Function(sqlFunctionName, arguments.Skip(1), true, Array.Empty<bool>(),
                method.ReturnType);
        }

        // hack for pgroonga_score
        return _sqlExpressionFactory.Function(sqlFunctionName, new[]
        {
            _sqlExpressionFactory.Fragment("tableoid"),
            _sqlExpressionFactory.Fragment("ctid")
        }, true, new bool[2], method.ReturnType);
    }

    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
    private PgUnknownBinaryExpression? TryTranslateOperator(MemberInfo method, IReadOnlyList<SqlExpression> arguments)
    {
        if (method.DeclaringType != typeof(PGroongaLinqExtensions))
        {
            return null;
        }

        return method.Name switch
        {
            nameof(PGroongaLinqExtensions.PGroongaMatch) => BoolReturningOnTwoQueries("&@"),
            nameof(PGroongaLinqExtensions.PGroongaQuery) => BoolReturningOnTwoQueries("&@~"),
            nameof(PGroongaLinqExtensions.PGroongaSimilarSearch) => BoolReturningOnTwoQueries("&@*"),
            nameof(PGroongaLinqExtensions.PGroongaScriptQuery) => BoolReturningOnTwoQueries("&`"),
            nameof(PGroongaLinqExtensions.PGroongaMatchIn) => BoolReturningOnTwoQueries("&@|"),
            nameof(PGroongaLinqExtensions.PGroongaQueryIn) => BoolReturningOnTwoQueries("&@~|"),
            nameof(PGroongaLinqExtensions.PGroongaPrefixSearch) => BoolReturningOnTwoQueries("&^"),
            nameof(PGroongaLinqExtensions.PGroongaPrefixRkSearch) => BoolReturningOnTwoQueries("&^~"),
            nameof(PGroongaLinqExtensions.PGroongaPrefixSearchIn) => BoolReturningOnTwoQueries("&^|"),
            nameof(PGroongaLinqExtensions.PGroongaPrefixRkSearchIn) => BoolReturningOnTwoQueries("&^~|"),
            nameof(PGroongaLinqExtensions.PGroongaRegexpMatch) => BoolReturningOnTwoQueries("&~"),
            _ => null
        };

        PgUnknownBinaryExpression BoolReturningOnTwoQueries(string @operator)
        {
            return new PgUnknownBinaryExpression(
                _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[0]),
                _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[1]),
                @operator,
                _boolMapping.ClrType,
                _boolMapping
            );
        }
    }
}
