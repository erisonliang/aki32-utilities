using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI;
public class MLNetHandler
{

    // ★★★★★★★★★★★★★★★ props

    public IEstimator<ITransformer> PipeLineHead { get; set; } = new EstimatorChain<ITransformer>();
    public MLContext Context { get; set; }


    // ★★★★★★★★★★★★★★★ methods

    public void ConnectNode<TTrans>(IEstimator<TTrans> estimator, TransformerScope scope = TransformerScope.Everything) where TTrans : class, ITransformer
        => PipeLineHead = PipeLineHead.Append(estimator, scope);

    public void ConnectCheckPoint()
        => PipeLineHead.AppendCacheCheckpoint(Context);


    // ★★★★★★★★★★★★★★★ 

}
