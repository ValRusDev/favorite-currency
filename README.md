# FavoriteCurrency

Тестовое задание: микросервисное приложение для работы с избранными валютами и их курсами.

## Архитектура

Решение состоит из нескольких сервисов:

* **UserService**

  * регистрация / логин пользователя
  * управление избранными валютами

* **FinanceService**

  * получение курсов валют
  * запрашивает у UserService избранные валюты пользователя

* **CurrencyUpdater.Worker**

  * фоновый сервис
  * загружает курсы валют с ЦБ РФ
  * обновляет таблицу `finance.currencies`

* **ApiGateway (YARP)**

  * единая точка входа
  * проксирует запросы к сервисам

---

## Технологии

* .NET 8
* ASP.NET Core
* Entity Framework Core (PostgreSQL)
* MediatR (CQRS)
* YARP (API Gateway)
* xUnit + Moq (unit-тесты)

---

## База данных

PostgreSQL

---

## Аутентификация

* JWT Bearer
* Токен выдается в `UserService`
* Используется в `FinanceService`

---

## Порты

| Сервис         | URL                    |
| -------------- | ---------------------- |
| API Gateway    | https://localhost:7002 |
| UserService    | https://localhost:7194 |
| FinanceService | https://localhost:7160 |

---

## Запуск проекта

### 1. Установить зависимости

* .NET 8 SDK
* PostgreSQL

---

### 2. Настроить строку подключения

В `appsettings.Development.json` сервисов:

```json
"ConnectionStrings": {
  "Postgres": "Host=localhost;Port=55432;Database=favoritecurrency;Username=postgres;Password=postgres"
}
```

---

### 3. Применить миграции

Запустить `FinanceService` — миграции применятся автоматически
(или через `dotnet ef database update`)

---

### 4. Запустить сервисы

Запустить одновременно:

* `FavoriteCurrency.ApiGateway`
* `FavoriteCurrency.UserService.API`
* `FavoriteCurrency.FinanceService.API`
* `FavoriteCurrency.CurrencyUpdater.Worker`

---

## Как работает система

1. Worker загружает XML с ЦБ РФ:

   ```
   http://www.cbr.ru/scripts/XML_daily.asp
   ```

2. Парсит валюты и обновляет таблицу:

   ```
   finance.currencies
   ```

3. Пользователь:

   * регистрируется
   * логинится
   * получает JWT

4. Добавляет валюты в избранное

5. Запрашивает курсы через:

   ```
   GET /api/finance/rates
   ```

6. FinanceService:

   * получает избранные валюты из UserService
   * возвращает курсы только по ним

---

## Работа через API Gateway

Все запросы выполняются через:

```
https://localhost:7002
```

---

### Регистрация

```
POST /api/users/register
```

```json
{
  "userName": "ValRus",
  "password": "Qwerty123!"
}
```

---

### Логин

```
POST /api/users/login
```

Ответ:

```json
{
  "accessToken": "...",
  "expiresAtUtc": "..."
}
```

---

### Добавление в избранное

```
POST /api/users/favorites
Authorization: Bearer <token>
```

```json
{
  "currencyCode": "USD"
}
```

---

### Получение курсов

```
GET /api/finance/rates
Authorization: Bearer <token>
```

---

## Тесты

Запуск:

```bash
dotnet test
```

Покрыты:

* UserService (auth + favorites)
* FinanceService (получение курсов)

---

## Требования

* Visual Studio 2022
* .NET 8 SDK
* PostgreSQL

---
