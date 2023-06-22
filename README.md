# Cultural Events


## API Setup

- Add default connection to your database in _[appsettings](EventsAPI/appsettings.json)_.

    _Notice the project uses SQL, in case of setting up another database it's necessary to also modify *[Program](EventsAPI/Program.cs)*._

- Create database using EF Core.
    ```
    cd EventsAPI
    dotnet ef database update
    ```

- Run EventsAPI and add info.