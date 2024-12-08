# Artemis Khronos

The main goal of this project is creating artwork images using ChatGPT and Dall-E 3. It uses the present date as a first step and retrieves a historical event on that date, and creates the artwork out this historical event.

## How it works
This software works with the user adding their personal API (OpenAPI) into "Change API" (Nor "Endre API"). To create an artwork image the user presses the button "Make todays image" (Nor "Lag dagens bilde"). The system will then through REST API posts retrieves the propper text and image. When both text and images are made it is stored automatically in a subfolder of the software called Images. The text is stored in a JSON file. The user can copy the deskription of the image through a button called "copy todays text" (Nor "Kopier dagens tekst"). The user has access to the older images through the tab "earlier images" (nor "Tidligere bilder"). Here the button "Retrieve earlier images" ("Hent tidligere bilder") retrieves the data from the JSON file and shows relative filepath in a listbox on the left side of the screen. When the user clicks on a file the text will come up in the middle and the image to the right. It is also possible to copy the text here through the button "copy text" (Nor "Kopier tekst").

## Technical aspects
This system handles several different technical factors like ChatGPT prompting, REST API requests, JSON serialisation and user secrets configuration. 

### REST API
All communication between this system and ChatGPT (gpt-4) and Dall-E 3 has been done through REST API requests (Http requests). The API-key for OpenAI API is stored into secret.json. This is a file and method for storing user secrets in .NET. 

The structure in the API requests for ChatGPT is as following: 
The Chat GPT Endpoint is: https://api.openai.com/v1/chat/completions
While the body has the following elements.
Model: Here the software has used the "gpt-4" model. This can be switched out with other models from ChatGPT.
While the API message consist of an object with role and content as variables. The role is set as "user" and content is the prompt that describes what ChatGPT should produce.
Lastly the request body contains how many tokens ChatGPT should be using on the request.

The structure in the API request for Dall-E looks as following:
The Dall-E endpoint is: https://api.openai.com/v1/images/generations
The body of Dall-E requests are as follows: 
The prompt describes how the image should look like or topic involved in the making of the image.
Next is how many images should be generated
Lastly is the size of the image created. 

According to OpenAI cookbook dated 06.03.2024 they have recently added the possibility to choose model, style, and quality into the REST API body. https://cookbook.openai.com/articles/what_is_new_with_dalle_3

An interesting difference is that Dall-E REST API requests do not contain any token limit compared to ChatGPT. 

### Prompting. 
To produce the prompts in the software a seperate class was created. This class has hardcoded three different lists that helps with the generating the prompt. The class has one list for artstyles, continent and historical period. When generating the prompt for retrieving historical event, the system uses the todays date, with a random continent and historical period. Since Dall-E-2 has a maximum character limit of 1000 characters. We added safeguards both in the maximum tokens and into the prompts. For Historical event the software has a max token of 200 and 350 characters. While the prompt for artwork has a limit of 400 tokens and 700 characters. These are safeguards to  make sure Dall-E receives a complete prompt and to ensure the prompts detail.  This limit is for the modell Dall-E-2. With the new changes in Dall-Es REST API, mentioned above, this limit can be increased to 4000 character that Dall-E-3 uses. 

### Dependencies
To handle different parts of the project, external libraries and framework was used: 
1. Microsoft.Extension.Configuration.UserSecrets. This was used to configure user secrets configuration 
2. Newtonsoft.Json was used to handle JSON serialization and deserialization. Storing and loading data from the JSON file acting as a storage.

### Project classes
Besides the MainWindow class in C# WPF software this class is divided into 4 other classes. Each with a different problem it solves. These classes are as follows: 

dataStructure: 
This class defines the object storing the data in this software. Each instance of this class stores information as all prompts to Chatgpt and Dall-E and the relative filepath to the stored image.

fileHandling: 
This class is responsible for all the file handling in the software. This being finding the fullpath to store the generated image and loading and saving data from the JSON file where the information is stored. This class could have been divided, but because of the scope size it was not deemed neccessary. In a larger project the JSON handling might be in a seperate folder.

gptConnection:
This class handles and creates all the HTTP requests to and from ChatGPT and Dall-E, and stores and retrieves the API-key from user secrets. The handling of API-key can be placed in a seperate class. 

createPrompt:
This class creates the prompts with random artstyles, timeperiod, and continent. These prompts are then used by functions in gptConnection. 

## Other uses for this project or part of the project
Part of this project could be used in other projects such as:
1. Educational tool for art and language learning
2. E-Commerce and marketing
3. API usage monitoring and analytics
4. Data export for machine learning applications
5. Social media automation.
