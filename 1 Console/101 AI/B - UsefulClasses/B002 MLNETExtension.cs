using Microsoft.ML;
using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI;
public class MLNetHandler
{

    // ★★★★★★★★★★★★★★★ props

    public IEstimator<ITransformer> PipeLine { get; set; } = new EstimatorChain<ITransformer>();


    // ★★★★★★★★★★★★★★★ methods

    public void ConnectNode<TTrans>(IEstimator<TTrans> estimator, TransformerScope scope = TransformerScope.Everything) where TTrans : class, ITransformer
        => PipeLine = PipeLine.Append(estimator, scope);

    public void ConnectCheckPoint(MLContext context)
        => PipeLine.AppendCacheCheckpoint(context);


    // ★★★★★★★★★★★★★★★ 

}
