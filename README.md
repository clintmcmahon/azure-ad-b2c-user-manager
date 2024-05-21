# Azure AD B2C User Management Application

## User Admin Setup & Documentation Guide

This application will allow you to connect to your Azure AD B2C tenant then view all the B2C users then view and edit their properties including extension attributes and custom properties.

This guide outlines the necessary steps and processes to follow in order to successfully set up your account for the User Admin application. If you encounter any issues or need support, please reach out at [clint@parkasoftware.com](mailto:clint@parkasoftware.com).

## Prerequisites

Before you can successfully utilize the User Admin features, ensure that you have the following prerequisites:

- Access to an Azure B2C subscription with administrative rights or the appropriate rights to configure the steps below.
  - Created and access to a valid Azure B2C tenant.
  - Created a new app registration within the tenant for this software.
  - Recorded client id, client secret, and tenant id.
- Basic understanding of Azure B2C user management.

## Registering a New App in Azure B2C Tenant

You will need to register a new app in your Azure B2C tenant for this application to function properly. Perform the following steps:

1. Sign into the Azure portal and navigate to the **Azure AD B2C service**.
2. Select **App registrations** and then click on **New registration**.
3. Provide a name for your application and select the appropriate supported account types.
4. Click on **Register** to create the new app registration.
5. Record the client id of the newly created app. You will need this for the User Admin to access your B2C tenant.

## Creating a Client Secret and Getting the Tenant Id

After registering the app in Azure B2C, follow these steps to obtain the Tenant Id and Client Secret:

1. Note down the Application or Client Id from the newly registered app; this serves as the Client ID for the User Admin app.
2. Navigate into the **Certificates & secrets** section, click **New client secret**. Give it a name and select the expiration time frame. Click **Add** and note down the generated secret value.
3. Obtain the Azure B2C Tenant Id:
   - Click your user name in the upper right corner of the Azure Portal.
   - A drop down will appear; from this drop down select **Switch directory**.
   - In the **Switch directory** window, you will see the list of your tenants below. The Tenant Id is also called the Directory Id.
     ![How to find B2C tenant id](./assets/img/tenantid.png)

## Setting Appropriate API Permissions

Ensure that your newly created app has the following delegated Graph API permissions. These permissions can be set in the **API permissions** section of your app registration in the Azure portal.

1. From your newly created app, select **API permissions**.
2. Click the **Add a permission** button.
3. Within the **Request API permissions --> Microsoft APIs** screen, click the **Microsoft Graph API** button.
4. Then click **Application permissions**.
5. A **Select permissions** section will appear. Search and select the following permissions:
   - **User.ReadWrite.All**: This permission allows the User Admin application to read and write all users’ full profiles.
   - **IdentityFlow.Read.All**: This permission allows the application to read all identity flows.
   - **Applications.Read.All**: This permission allows the application to read the applications in order to find the specific Azure B2C custom properties application. This application is where the custom properties and extension attributes are stored. Without this permission, the User Admin application will not be able to read custom properties or extension attributes.

## Set Client Id, Client Secret, and Tenant Id in the Application

Log into the User Admin app and navigate to the [account settings page](./identity/account/manage/). There you will find three input boxes for you to save your Client Id, Client Secret, and Tenant Id.

The application uses the Client Credentials OAuth flow to connect to the Azure AD B2C tenant to manage your B2C users. You can learn more about the [Client Credentials OAuth flow here](https://auth0.com/docs/get-started/authentication-and-authorization-flow/client-credentials-flow).

After completing these steps, you are now ready to view and modify your Azure B2C users.
