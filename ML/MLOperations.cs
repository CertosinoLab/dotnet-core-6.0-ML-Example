using Microsoft.ML;
using MLDotnetExample.Common;
using MLDotnetExample.ML.Base;
using MLDotnetExample.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Dynamic;

namespace MLDotnetExample.ML
{
    public class MLOperations : BaseML, IMLOperations
    {
        public string Predict()
        {
            ITransformer mlModel;

            using (var stream = new FileStream(ModelPath(), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mlModel = MlContext.Model.Load(stream, out _);
            }

            if (mlModel == null)
            {
                Console.WriteLine("Failed to load model");

                return "";
            }

            var predictionEngine = MlContext.Model.CreatePredictionEngine<EmploymentHistory, EmploymentHistoryPrediction>(mlModel);

            //var json = File.ReadAllText(inputData);
            var filePath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName
                + "\\MLDotnetExample\\InputData\\employee.json";
            var json = File.ReadAllText(filePath);

            var prediction = predictionEngine.Predict(JsonConvert.DeserializeObject<EmploymentHistory>(json));

            Debug.WriteLine(
                                $"Based on input json:{System.Environment.NewLine}" +
                                $"{json}{System.Environment.NewLine}" +
                                $"The employee is predicted to work {prediction.DurationInMonths:#.##} months");

            return $"The employee is predicted to work {prediction.DurationInMonths:#.##} months";
        }

        public ExpandoObject Train()
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            string trainingFilePath = projectPath + "\\MLDotnetExample\\Datasets\\employeedata.csv";

            if (!File.Exists(trainingFilePath))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFilePath}");
                return null;
            }

            var trainingDataView = MlContext.Data.LoadFromTextFile<EmploymentHistory>(trainingFilePath, ',');

            var dataSplit = MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.4);

            var dataProcessPipeline = MlContext.Transforms.CopyColumns("Label", nameof(EmploymentHistory.DurationInMonths))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.IsMarried)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.BSDegree)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.MSDegree)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.YearsExperience))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.AgeAtHire)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.HasKids)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.WithinMonthOfVesting)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.DeskDecorations)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.LongCommute)))
                .Append(MlContext.Transforms.Concatenate("Features",
                    typeof(EmploymentHistory).ToPropertyList<EmploymentHistory>(nameof(EmploymentHistory.DurationInMonths)))));

            var trainer = MlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath());
            var testSetTransform = trainedModel.Transform(dataSplit.TestSet);

            var modelMetrics = MlContext.Regression.Evaluate(testSetTransform);

            dynamic result = new ExpandoObject();
            result.lossFunction = modelMetrics.LossFunction;
            result.meanAbsoluteError = modelMetrics.MeanAbsoluteError;
            result.meanSquaredError = modelMetrics.MeanSquaredError;
            result.rSquared = modelMetrics.RSquared;
            result.rootMeanSquaredError = modelMetrics.RootMeanSquaredError;

            return result;
        }
    }
}
