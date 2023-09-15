# Project "BlobTask"
This project is an application developed using Azure cloud resources and the React framework. The project includes a Blob Trigger function that responds to file uploads to Azure Blob Storage and a React web application that allows sending these files via email.

## Functionality
**Blob Trigger**:

- When a file is uploaded to Azure Blob Storage, the Blob Trigger function automatically activates.

## React Web Application:

- Users can upload files through the React web interface.
- When a file is uploaded to the server, the application generates an SAS token for that file.
- The generated SAS token is used to create a temporary and secure link to the file, which can be sent via email.
  
### Server-side validation is implemented:
- Only .docx files are allowed.
- Email validation is performed using a regular expression.
  
## Sending Files via Email:
- Users can specify an email address and send the file to that address.
- The included link to the file is protected by an SAS token, ensuring secure access to the file.
- Feel free to expand on this README with additional details, instructions, and contact information as needed.

# Scrins

![image](https://github.com/pavelvichev/ReenbitTechTask/assets/71034124/dc35253f-d243-4608-805c-5476d3c43b25)
![image](https://github.com/pavelvichev/ReenbitTechTask/assets/71034124/20a05da4-c168-455d-8f98-16e50bf1569c)
![image](https://github.com/pavelvichev/ReenbitTechTask/assets/71034124/7bff3825-e3cb-4c35-b946-887ea4c026b5)
![image](https://github.com/pavelvichev/ReenbitTechTask/assets/71034124/ebf066a6-4d55-48ae-881b-c62ee8f809c0)
![image](https://github.com/pavelvichev/ReenbitTechTask/assets/71034124/fb227852-7a63-4ec7-bf72-4d20f503ffb2)
![image](https://github.com/pavelvichev/ReenbitTechTask/assets/71034124/99f4b53a-49a4-4e62-ac89-a2c009d3a64d)






