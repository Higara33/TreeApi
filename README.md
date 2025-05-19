# TreeApi
Task:
Your application has to use code one of the databases: MsSQL, MySQL, PostgreSQL (preferred). You have to use code first migration.

The database model has to be designed for storing the following information:

1. Nodes of in-depended trees. Every node must belong to a single tree. All children nodes must belong to the same tree as their parent node. Every node has a single obligatory field. It is the name. Everything else is optional. If you need anything for designing in-depended trees you can add it as you want.
2. The journal of all exceptions during processing Rest API requests. Every journal record must keep information about: the unique event ID, the timestamp when the event happened, all query and body parameters, and the stack trace of an exception.
Your application should provide Rest API similar (ideally the same) to the existing (check swagger).

Your application should have its own "SecureException" exception class. If during the request processing SecureException or its child exception was thrown, all information about the exception should be stored in the journal and your application should return a response with HTTP status = 500. The response should look like this:

`{"type": "name of exception", "id": "id of event", "data": {"message": "message of exception"}}`

Example:

`{"type": "Secure", "id": "638136064526554554", "data": {"message": "You have to delete all children nodes first"}}`

The full information about all other exception types should be stored in the journal. The only difference is that the response for other exception types should look like:

`{"type": "Exception", "id": "id of event", "data": {"message": "Internal server error ID = id of event"}}`

Example:

`{"type": "Exception", "id": "638136064187111634", "data": {"message": "Internal server error ID = 638136064187111634"}}`

Дополнение:
1) возможно overkill для тестового полноценная аутентификация через JWT. Понимаю что можно было просто принять code, сохранить в таблице и завершить без access/refresh.
2) требовалось логировать query и body параметры - сделал красиво. (колонки для критичных полей и JSONB для всего остального).
3) добавил таблицу NodeClosure (выгода на больших и часто меняющихся деревьях)
Понимаю что при вставке и удалении нужно будет обновлять ее, но как будто "O(1) - запись предок-потомок; O(n) - выборка поддерева без рекурсии" слишком быстро себя окупает.
4) в изначальном задании все эндпоинты были POST - поправил как мне показалось корректнее.
5) ITimeProvider для тестируемого времени. DateTime.UtcNow - один раз были проблемы с ним, теперь всегда делаю так. В .NET 8 есть библиотечное решение https://learn.microsoft.com/en-us/dotnet/api/system.timeprovider?view=net-8.0.
6) не стал добавлять swagger и health-check. делается быстро и просто.

-AuthService: генерация JWT + Refresh-токенов, хранение в БД через EF Core, эндпоинт /api/user/partner/rememberMe.

-JournalService: middleware для перехвата и логирования SecureException и прочих исключений, хранение журналов с JSONB-параметрами в Postgres.

-TreeService: CRUD для деревьев и узлов с уникальным индексом (TreeId, ParentNodeId, Name) и поддержкой Closure Table.
