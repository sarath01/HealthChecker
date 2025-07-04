// Copyright (c) 2025 Sarath Konda
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using CoreML;
using Vision;
using UIKit;
using Foundation;
using System;
using System.Linq;

namespace Animatix.iOS
{
    public static class HealthImageAnalyzer
    {
        public static void AnalyzeHealthImage(UIImage image, Action<string>? onComplete = null)
        {
            // Load the compiled CoreML model
            var model = tryLoadModel("HealthcareClassifier");
            if (model == null)
            {
                onComplete?.Invoke("Model load failed.");
                return;
            }

            var vnModel = VNCoreMLModel.FromMLModel(model, out var error);
            if (error != null || vnModel == null)
            {
                onComplete?.Invoke("VNModel creation failed.");
                return;
            }

            var request = new VNCoreMLRequest(vnModel, (req, err) =>
            {
                var results = req.GetResults<VNClassificationObservation>();
                if (results == null || results.Length == 0)
                {
                    onComplete?.Invoke("No results.");
                    return;
                }

                var result = results.FirstOrDefault();

                if (result != null)
                {
                    onComplete?.Invoke($"Detected: {result.Identifier} ({result.Confidence:P0})");
                }
                else
                {
                    onComplete?.Invoke("Could not classify.");
                }
            });

            var handler = new VNImageRequestHandler(image.CGImage, new VNImageOptions());
            if (handler != null)
            {
                onComplete?.Invoke("Image handler failed.");
                return;
            }

            handler.Perform(new[] { request }, out var performError);
            if (performError != null)
            {
                onComplete?.Invoke("Analysis failed.");
            }
        }

        private static MLModel? tryLoadModel(string modelName)
        {
            try
            {
                var url = NSBundle.MainBundle.GetUrlForResource(modelName, "mlmodelc");
                if (url != null)
                {
                    return MLModel.Create(url, out var error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Model load exception: {ex.Message}");
            }

            return null;
        }
    }
}
