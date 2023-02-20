

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public enum MLNetExampleScenario
{
    // A    Binary Classification
    A001_BinaryClassification_SentimentAnalysis,
    A002_BinaryClassification_SpamDetection,
    A003_BinaryClassification_CreditCardFraudDetection,
    A004_BinaryClassification_HeartDiseasePrediction,
    A777_BinaryClassification_Auto_SentimentAnalysis,

    // B    Multi-class classification
    B001_MultiClassClassification_IssuesClassification,
    B002_MultiClassClassification_IrisFlowersClassification,
    B003_MultiClassClassification_MNIST,
    B777_MultiClassClassification_Auto_MNIST,

    // C    Recommendation
    C001_Recommendation_ProductRecommender,
    C002_Recommendation_MovieRecommender_MatrixFactorization,
    C003_Recommendation_MovieRecommender_FieldAwareFactorizationMachines,
    C777_Auto_Recommendation,

    // D    Regression
    D001_Regression_TaxiFarePrediction,
    D002_Regression_SalesForecasting,
    D003_Regression_DemandPrediction,
    D777_Regression_Auto_TaxiFarePrediction,

    // E    Time Series Forecasting
    E001_TimeSeriesForecasting_SalesForecasting,

    // F    Anomaly Detection
    F001_AnomalyDetection_SalesSpikeDetection_DetectIidSpike,
    F001_AnomalyDetection_SalesSpikeDetection_DetectIidChangePoint,
    F002_AnomalyDetection_PowerAnomalyDetection,
    F003_AnomalyDetection_CreditCardFraudDetection,

    // G    Clustering
    G001_Clustering_CustomerSegmentation,
    G002_Clustering_IrisFlowerClustering,

    // H    Ranking
    H001_Ranking_RankSearchEngineResults,
    H777_Ranking_Auto_RankSearchEngineResults,

    // I    Computer Vision
    I001_ComputerVision_ImageClassificationTraining_HighLevelAPI,
    I002_ComputerVision_ImageClassificationPredictions_PretrainedTensorFlowModelScoring,
    I003_ComputerVision_ImageClassificationTraining_TensorFlowFeaturizerEstimator,
    I004_ComputerVision_ObjectDetection_ImportONNXModel_TinyYoloV2_08,
    I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV2_09,
    I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV3_10,

    // J    Cross Cutting Scenarios
    J001_CrossCuttingScenarios_ScalableModelOnWebAPI,
    J002_CrossCuttingScenarios_ScalableModelOnRazorWebApp,
    J003_CrossCuttingScenarios_ScalableModelOnAzureFunctions,
    J004_CrossCuttingScenarios_ScalableModelOnBlazorWebApp,
    J005_CrossCuttingScenarios_LargeDatasets,
    J006_CrossCuttingScenarios_LoadingDataWithDatabaseLoader,
    J007_CrossCuttingScenarios_LoadingDataWithLoadFromEnumerable,
    J008_CrossCuttingScenarios_ModelExplainability,
    J009_CrossCuttingScenarios_ExportToONNX,

    // K    Advanced Experiment
    K777_Auto,

    // ignore
    Z999_Ignore,

}