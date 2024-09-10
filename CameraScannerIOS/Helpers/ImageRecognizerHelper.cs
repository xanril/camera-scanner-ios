using System.Text;
using CameraScannerIOS.Models;
using Vision;

namespace CameraScannerIOS.Helpers;

public static class ImageRecognizerHelper
{
    private static string[] IGNORE_LIST = new[]
    {
        "CHINABANK", "BC", "YEARS", "PRIME", "Visa", "MasterCard", "Mastercard", "American Express", "Discover"
    };
    
    public static CardDetails ValidateScanImage(UIImage image)
    {
        var cgImage = image?.CGImage;
        if (cgImage == null)
        {
            return null;
        }
        
        var recognizedTextList = new List<string>();
        var scannedItems = new List<ScannedItem>();

        var textRecognitionRequest = new VNRecognizeTextRequest((request, error) =>
        {
            var results = request?.GetResults<VNRecognizedTextObservation>();
            if (results == null || results.Length == 0)
            {
                return;
            }
        
            foreach (var result in results)
            {
                var candidate = result.TopCandidates(1).FirstOrDefault();
                if (candidate == null)
                {
                    continue;
                }
                recognizedTextList.Add(candidate.String);
            }

            scannedItems = getScannedItemsInfo(results, image);
            // parseResultsV2(scannedItems);
        });
        
        textRecognitionRequest.RecognitionLevel = VNRequestTextRecognitionLevel.Accurate;
        textRecognitionRequest.UsesLanguageCorrection = true;
        textRecognitionRequest.CustomWords = IGNORE_LIST;
        
        var handler = new VNImageRequestHandler(cgImage, new VNImageOptions());
        try
        {
            handler.Perform(new []{ textRecognitionRequest }, out var error);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        Console.WriteLine("Validation Completed!");
        foreach (var item in recognizedTextList)
        {
            Console.WriteLine("Recognized Text: " + item);
        }
           
        // return parseResults(recognizedTextList);
        return parseResultsV2(scannedItems);
    }
    
    private static List<ScannedItem> getScannedItemsInfo(VNRecognizedTextObservation[] observations, UIImage image)
    {
        var scannedItems = new List<ScannedItem>();
        foreach (var observation in observations)
        {
            var scanItem = new ScannedItem();
            
            // Find the top observation.
            var candidate = observation.TopCandidates(1).FirstOrDefault();
            if (candidate == null)
            {
                continue;
            }

            // Find the bounding-box observation for the string range.
            var stringRange = new NSRange(0, candidate.String.Length);
            VNRectangleObservation boxObservation = null;
            try
            {
                boxObservation = candidate.GetBoundingBox(stringRange, out var nsError);
            }
            catch (Exception)
            {
                continue;
            }

            
            
            // Get the normalized CGRect value.
            var boundingBox = boxObservation?.BoundingBox ?? CGRect.Empty;

            // Convert the rectangle from normalized coordinates to image coordinates.
            var imageWidth = (int)image.Size.Width;
            var imageHeight = (int)image.Size.Height;
            var rect = new CGRect(
                boundingBox.X * imageWidth,
                (1 - boundingBox.Y) * imageHeight - boundingBox.Height * imageHeight,
                boundingBox.Width * imageWidth,
                boundingBox.Height * imageHeight
            );
            
            scanItem.Text = candidate.String;
            scanItem.Bounds = rect;
            
            scannedItems.Add(scanItem);
        }

        return scannedItems;
    }
    
    private static CardDetails parseResults(List<string> recognizedTextList)
    {
        var cardDetails = new CardDetails();
        
        // credit card number
        var fixedCreditCardInitialNumbers = new[] { '4', '5', '3', '6' };
        var creditCardNumber = recognizedTextList.FirstOrDefault(text => text.Length >= 14 && fixedCreditCardInitialNumbers.Contains(text.First()));
        cardDetails.CreditCardNumber = creditCardNumber;
        Console.WriteLine("Credit Card Number: " + creditCardNumber);
        
        // Expiry Date
        var expiryDateString = recognizedTextList.FirstOrDefault(text => text.Length > 4 && text.Contains("/"));
        var expiryDateChars = expiryDateString?.Where(c => char.IsDigit(c) || c == '/');
        var expiryDate = expiryDateChars?.Aggregate("", (current, c) => current + c);
        Console.WriteLine("Expiry Date: " + expiryDate);
        
        var wordsToAvoid = new List<string>();

        if (creditCardNumber?.ToString() != null)
        {
            wordsToAvoid.Add(creditCardNumber);
        }

        if (expiryDate?.ToString() != null)
        {
            wordsToAvoid.Add(expiryDate?.ToString());
        }
        
        wordsToAvoid.AddRange(IGNORE_LIST);
        wordsToAvoid.AddRange(IGNORE_LIST.Select(x => x.ToLower()));
        wordsToAvoid.AddRange(IGNORE_LIST.Select(x => x.ToUpper()));
        
        // Name
        var name = recognizedTextList.LastOrDefault(text => !wordsToAvoid.Contains(text));
        cardDetails.Name = name;
        Console.WriteLine("Name: " + name);

        return cardDetails;
    }

    private static CardDetails parseResultsV2(List<ScannedItem> scannedItems)
    {
        var cardDetails = new CardDetails();
        
        // look for credit card number
        cardDetails.CreditCardNumber = parseCreditCardNumber(scannedItems);

        return cardDetails;
    }

    private static string parseCreditCardNumber(List<ScannedItem> scannedItems)
    {
        var fixedCreditCardInitialNumbers = new[] { '4', '5', '3', '6' };
        
        // filter scannedItems with text that contain numbers only
        var filteredItems = scannedItems.Where(x => 
            x.Text.All(c => char.IsDigit(c) || char.IsWhiteSpace(c)) 
            && x.Text.Any(char.IsDigit)
            && x.Text.Length >= 4).ToList();
        
        var candidateNumbers = filteredItems.Where(x => fixedCreditCardInitialNumbers.Contains(x.Text.First())).ToList();
        
        // get the item with the lowest X value
        var initialNumber = candidateNumbers.OrderBy(x => x.Bounds.X).FirstOrDefault();
        
        // group items that are close to each other
        var groupedItems = new List<ScannedItem>();
        for (var i = 0; i < filteredItems.Count; i++)
        {
            var currentItem = filteredItems[i];
            if (currentItem == initialNumber)
            {
                continue;
            }
            var distance = Math.Abs(currentItem.Bounds.Y - initialNumber.Bounds.Y);
            if (distance < 30)
            {
                groupedItems.Add(currentItem);
            }
        }
        
        // combine the initial number and grouped items
        var combinedItems = new List<ScannedItem> { initialNumber };
        combinedItems.AddRange(groupedItems);
        
        // sort the combined items by X value
        var sortedItems = combinedItems.OrderBy(x => x.Bounds.X).ToList();
        
        // combine the text
        var creditCardNumber = "";
        foreach (var item in sortedItems)
        {
            creditCardNumber += item.Text;
        }
        
        // remove spaces
        creditCardNumber = creditCardNumber.Replace(" ", "");
        
        // group the text by 4 characters split by space
        var formattedNumber = new StringBuilder();
        for (int i = 0; i < creditCardNumber.Length; i++)
        {
            if (i > 0 && i % 4 == 0)
            {
                formattedNumber.Append(" ");
            }
            formattedNumber.Append(creditCardNumber[i]);
        }
        
        return formattedNumber.ToString();
    }
}