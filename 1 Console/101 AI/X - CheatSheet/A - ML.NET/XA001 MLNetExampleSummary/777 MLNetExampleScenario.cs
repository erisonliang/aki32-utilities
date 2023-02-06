

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public enum MLNetExampleScenario
{
    // A    Binary Classification
    A001_Sentiment_Analysis,
    A002_Spam_Detection,
    A003_CreditCardFraudDetection,
    A004_HeartDiseasePrediction,
    A777_Auto,

    // B    Multi-class classification
    B001_IssuesClassification,
    B002_IrisFlowersClassification,
    B003_MNIST,
    B777_Auto,

    // C    Recommendation
    C001_ProductRecommendation,
    C002_MovieRecommender_MatrixFactorization,
    C003_MovieRecommender_FieldAwareFactorizationMachines,
    C777_Auto,

    // D    Regression
    D001_PricePrediction,
    D002_SalesForecasting_Regression,
    D003_DemandPrediction,
    D777_Auto,

    // E    Time Series Forecasting
    E001_SalesForecasting_TimeSeries,

    // F    Anomaly Detection
    F001_SalesSpikeDetection,
    F002_PowerAnomalyDetection,
    F003_CreditCardFraudDetection,

    // G    Clustering
    G001_CustomerSegmentation,
    G002_IrisFlowersClustering,

    // H    Ranking
    H001_RankSearchEngineResults,

    // I    Computer Vision
    I001_ImageClassificationTraining_HighLevelAPI,
    I002_ImageClassificationPredictions_PretrainedTensorFlowModelScoring,
    I003_ImageClassificationTraining_TensorFlowFeaturizerEstimator,
    I004_ObjectDetection_ONNXModelScoring,

    // J    Cross Cutting Scenarios
    J001_ScalableModelOnWebAPI,
    J002_ScalableModelOnRazorWebApp,
    J003_ScalableModelOnAzureFunctions,
    J004_ScalableModelOnBlazorWebApp,
    J005_LargeDatasets,
    J006_LoadingDataWithDatabaseLoader,
    J007_LoadingDataWithLoadFromEnumerable,
    J008_ModelExplainability,
    J009_ExportToONNX,

    // K    Advanced Experiment
    K777_Auto,

}