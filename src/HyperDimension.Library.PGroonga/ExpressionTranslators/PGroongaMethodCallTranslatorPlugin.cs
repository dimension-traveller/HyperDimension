using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace HyperDimension.Library.PGroonga.ExpressionTranslators;

public class PGroongaMethodCallTranslatorPlugin : IMethodCallTranslatorPlugin
{
    public IEnumerable<IMethodCallTranslator> Translators { get; }

    public PGroongaMethodCallTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory, IRelationalTypeMappingSource typeMappingSource)
    {
        Translators = new IMethodCallTranslator[]
        {
            new PGroongaMethodCallTranslator((SqlExpressionFactory)sqlExpressionFactory, typeMappingSource),
        };
    }
}
