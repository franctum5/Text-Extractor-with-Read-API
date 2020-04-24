# Text Extractor with Read API

The script uses the Microsoft Read API to extract all text based content from PDF / PNG / BMP /JPEG / TIFF files.

You can either use the code or the Precompiled version.

## Precompiled

1. Download the TxtExtractWithReadAPI.zip from the Precompiled App folder
2. Extract the files on your local machine
3. Open the settings.json file and fill it with your own information
  a. inputFolder: the path to the folder on your local machine the contains the input files
  b. outputFolder: the path to the folder on your local machine where the .txt will be created
  c. CognitiveServicesEndpoint: the endpoint of your Cognitive Services resource in Azure (e.g. https://westeurope.api.cognitive.microsoft.com)
  d. CognitiveServicesKey: the key of your Cognitive Services resource in Azure
4. Save and close the settings.json
5. Launch the TxtExtractWithReadApi.exe file and wait for the output to be created in the output folder

## Code
In the TxtExtractWithReadAPI you can find a .Net Core Console application. 
