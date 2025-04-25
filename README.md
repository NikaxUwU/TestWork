# **Инструкция по установке и запуску**

1. Клонировать репозиторий:
   git clone https://github.com/NikaxUwU/TestWork.git

2. Перейти в папку проекта:
   cd TestWork/TestWork

3. Создать пустую БД в PostgreSQL.

4. Настроить:
   - В appsettings.json указать данные подключения к PostgreSQL.
   - В config.json указать порт хоста API.

5. Восстановить зависимости:
   dotnet restore

6. Применить миграции:
   dotnet ef database update

7. Собрать проект:
   dotnet build

8. Запустить приложение:
   dotnet run
