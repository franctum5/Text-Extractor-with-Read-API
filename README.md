# Text Extractor with Read API

The script uses the [Microsoft Read API](https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/concept-recognizing-text#read-api) to extract all text based content from PDF / PNG / BMP /JPEG / TIFF files.

You can either use the Code or the Precompiled version.

## Precompiled

1. Download the **TxtExtractWithReadAPI.zip** from the **Precompiled App** folder
2. Extract the files on your local machine
3. Open the **settings.json** file and fill it with your own information
  1. *inputFolder*: the path to the folder on your local machine the contains the input files (if your using "\" in your path remember to escape it. E.g. C:\\Users\\xyz)
  2. *outputFolder*: the path to the folder on your local machine where the .txt will be created (if your using "\" in your path remember to escape it. E.g. C:\\Users\\xyz)
  3. *CognitiveServicesEndpoint*: the endpoint of your Cognitive Services resource in Azure (e.g. https://westeurope.api.cognitive.microsoft.com)
  4. *CognitiveServicesKey*: the key of your Cognitive Services resource in Azure
4. Save and close the **settings.json**
5. Launch the **TxtExtractWithReadApi.exe** file and wait for the output to be created in the output folder

## Code
In the TxtExtractWithReadAPI you can find the code of the .Net Core Console application. 
