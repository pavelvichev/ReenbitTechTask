# Project "BlobTask"
- This project is an application developed using Azure cloud resources and the React framework. 
- The project includes a Blob Trigger function that responds to file uploads to Azure Blob Storage and a React web application that allows sending these files via email.

## Functionality
**Blob Trigger**:

- When a file is uploaded to Azure Blob Storage, the Blob Trigger function automatically activates.
  
**React Web Application**:

- Users can upload files through the React web interface.
- When a file is uploaded to the server, the application generates an SAS token for that file.
- The generated SAS token is used to create a temporary and secure link to the file, which can be sent via email.
Server-side validation is implemented:
- Only .docx files are allowed.
- Email validation is performed using a regular expression.

**Sending Files via Email**:

- Users can specify an email address and send the file to that address.
- The included link to the file is protected by an SAS token, ensuring secure access to the file.
  
![image](https://github.com/pavelvichev/ReenbitTechTask/assets/71034124/81043ad3-f664-45af-bd9f-19e27ad231a4)
![image](https://github.com/pavelvichev/ReenbitTechTask/assets/71034124/1c29206d-02d5-473d-9c17-94fbe324f3ae)
![image](https://github.com/pavelvichev/ReenbitTechTask/assets/71034124/074746fe-1004-4cc8-88f5-85e66dfdc855)
![image](https://github.com/pavelvichev/ReenbitTechTask/assets/71034124/7f940707-92c7-45ca-aaf4-321cb05e1140)
![image](https://github.com/pavelvichev/ReenbitTechTask/assets/71034124/4ed775cc-79c4-45b6-bd8f-e8d0c20f6dde)




