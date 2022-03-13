## Setup procedure for MongoDB in Docker
1. Install and run with:
docker run -d  --name backend_mongodb  -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=secret
2. Connect to MongoDB with:
mongodb://mongoadmin:secret@localhost:27017

## Deployment description for "Home Strategies" API

1. Create the project in Visul Studio -> Create -> projecthomestrategies_api
2. Connect to VM with FTP client
3. Stop the API service with "sudo systemctl stop homestrategiesapi
4. Copy / replace content of "publish" folder to /root/homestrategies/apirelease
5. Start the API service with "sudo systemctl start homestrategiesapi
6. Check if the API service is running with "sudo systemctl status homestrategiesapi".

## Deployment description for "Home Strategies" database

1. Open MySQL workbench
2. Connect to the Development DB docker instance
3. Under Server -> Data Export
4. Select schema "homestrategies
5. Select the changed or newly created tables
6. Select "Dump structure only
7. Select "Export to self-contained file
8. Select "Include Create Schema
9. Start export
10. Execute SQL statements on the live environment 

Translated with www.DeepL.com/Translator (free version)